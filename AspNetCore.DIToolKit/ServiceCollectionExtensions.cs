using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace AspNetCore.DIToolKit;

public static class ServiceCollectionExtensions
{
    public static void ConfigureServicesByLifeTimeCycle(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Scan(ConfigureByAssemblySelector);
    }

    private static void ConfigureByAssemblySelector(IAssemblySelector scan)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        scan.FromAssemblies(assemblies)
            .AddClasses(z => z.AssignableTo<LifeTimeInterfaces.ITransientLifeTime>())
            .AsImplementedInterfaces()
            .WithTransientLifetime()
            .AddClasses(z => z.AssignableTo<LifeTimeInterfaces.IScopedLifeTime>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(z => z.AssignableTo<LifeTimeInterfaces.ISingletonLifeTime>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime();
    }
}