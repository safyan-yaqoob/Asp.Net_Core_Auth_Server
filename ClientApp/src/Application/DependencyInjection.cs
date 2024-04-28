using Application.PipelineBehaviors;
using Domain.Abstraction;
using Domain.Abstraction.Repositories;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddMediatR(o =>
            {
                o.RegisterServicesFromAssembly(AssemblyReference.Assembly);
            });

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddValidatorsFromAssembly(AssemblyReference.Assembly);

            return services;
        }
    }
}
