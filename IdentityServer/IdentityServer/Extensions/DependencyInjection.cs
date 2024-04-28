using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using IdentityServer.Data;
using IdentityServer.Data.Repository;
using IdentityServer.Models;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace IdentityServer.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                options.UseOpenIddict();
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
            {
                o.SignIn.RequireConfirmedAccount = !true;
                o.Password = new PasswordOptions
                {
                    RequireDigit = false,
                    RequiredLength = 1,
                    RequiredUniqueChars = 0,
                    RequireLowercase = false,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false
                };
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                            .UseDbContext<ApplicationDbContext>();
                })
                .AddServer(options =>
                {
                    options.SetAuthorizationEndpointUris("connect/authorize")
                            .SetLogoutEndpointUris("connect/logout")
                            .SetTokenEndpointUris("connect/token");

                    options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

                    options.AllowAuthorizationCodeFlow();

                    options.AddDevelopmentEncryptionCertificate()
                            .AddDevelopmentSigningCertificate();

                    options.UseAspNetCore()
                            .EnableAuthorizationEndpointPassthrough()
                            .EnableLogoutEndpointPassthrough()
                            .EnableTokenEndpointPassthrough();
                }).AddClient(options =>
                {
                    options.AllowAuthorizationCodeFlow();
                    options.AddDevelopmentEncryptionCertificate()
                           .AddDevelopmentSigningCertificate();

                    options.UseAspNetCore()
                           .EnableRedirectionEndpointPassthrough();

                    options.UseSystemNetHttp();

                    // Register the Google integration.
                    options.UseWebProviders().AddGoogle(options =>
                        {
                            options.SetClientId("client_id")
                                      .SetClientSecret("client_secrets")
                                      .SetRedirectUri("/signin-google")
                                      .SetProviderDisplayName("Sign In With Google")
                                      .AddScopes("email profile");
                        });
                }).AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });

            services.AddControllers();
            services.AddRazorPages();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(c =>
                {
                    c.LoginPath = "/Identity/Account/Login";
                });

            services.AddTransient<AuthorizationService>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("https://localhost:7002")
                        .AllowAnyHeader();

                    policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader();
                });
            });

            services.AddTransient<ClientAppRepository>();
            services.AddTransient<ScopesRepository>();
            services.AddScoped<IdentityDataSeeder>();

            return services;
        }
    }
}