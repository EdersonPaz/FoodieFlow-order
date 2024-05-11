using FoodieFlow.GestaoPedido.Core.Interfaces.Repository;
using FoodieFlow.GestaoPedido.Core.Interfaces.Service;
using FoodieFlow.GestaoPedido.Core.Services;
using FoodieFlow.GestaoPedido.Infra.Repository.Context;
using Microsoft.EntityFrameworkCore;

using Scrutor;

namespace FoodieFlow.GestaoPedido.Config
{
    public static class InjectionConfig
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services, WebApplicationBuilder builder, IConfiguration configuration)
        {

            builder.Services.Scan(scan => scan.FromApplicationDependencies()
            .AddClasses(classes => classes.AssignableTo(typeof(IBaseRepository<>)))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IBaseService<>)))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithScopedLifetime()
            .AddClasses(classes => classes.InNamespaces("FoodieFlow.GestaoPedido.Core.Services"))
                .AsMatchingInterface()
                .WithScopedLifetime()
            .AddClasses(classes => classes.InNamespaces("FoodieFlow.GestaoPedido.Infra.Repository"))
                .AsMatchingInterface()
                .WithScopedLifetime()
             .AddClasses(classes => classes.InNamespaces("FoodieFlow.GestaoPedido.Infra.Adapter"))
                .AsMatchingInterface()
            .WithScopedLifetime()
            );

            services.AddDbContext<PostgreSqlContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}

