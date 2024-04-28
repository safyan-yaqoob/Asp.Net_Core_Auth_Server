using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("DefaultConnection")["ConnectionString"];
            services.AddDbContext<ApplicationDbContext>(o =>
            {
                o.UseNpgsql(connectionString);
            });

            services.AddScoped<IDataSeeder, DataSeeder>();

            return services;
        }
    }
}
