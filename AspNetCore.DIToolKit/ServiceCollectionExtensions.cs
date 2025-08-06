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
        var assemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => !a.IsDynamic)
            .Where(a => !a.FullName.StartsWith("System", StringComparison.Ordinal) &&
                        !a.FullName.StartsWith("Microsoft", StringComparison.Ordinal));

        scan.FromAssemblies(assemblies)
            .AddClasses(z => z.AssignableTo<LifeTime.ITransient>())
            .AsImplementedInterfaces()
            .WithTransientLifetime();

        scan.FromAssemblies(assemblies)
            .AddClasses(z => z.AssignableTo<LifeTime.IScoped>())
            .AsImplementedInterfaces()
            .WithScopedLifetime();

        scan.FromAssemblies(assemblies)
            .AddClasses(z => z.AssignableTo<LifeTime.ISingleton>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime();
    }
}
