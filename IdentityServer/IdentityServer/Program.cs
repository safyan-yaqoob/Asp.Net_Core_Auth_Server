using IdentityServer.Data;
using IdentityServer.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.RegisterServices(builder.Configuration);
 
var app = builder.Build();

app.ConfigurePipeline();
app.Run();

using var serviceScope = app.Services.CreateScope();
var seeders = serviceScope.ServiceProvider.GetServices<IdentityDataSeeder>();

foreach (var seeder in seeders)
{
    await seeder.SeedAllAsync();
}