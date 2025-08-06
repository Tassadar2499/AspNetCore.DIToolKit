using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AspNetCore.DIToolKit.Tests;

public interface ITransientService {}
public class TransientService : ITransientService, LifeTime.ITransient {}

public interface IScopedService {}
public class ScopedService : IScopedService, LifeTime.IScoped {}

public interface ISingletonService {}
public class SingletonService : ISingletonService, LifeTime.ISingleton {}

public class ServiceRegistrationTests
{
    [Fact]
    public void ConfigureServicesByLifeTimeCycle_RegistersServicesWithCorrectLifetime()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();
        services.ConfigureServicesByLifeTimeCycle(configuration);

        var provider = services.BuildServiceProvider();

        var transient1 = provider.GetService<ITransientService>();
        var transient2 = provider.GetService<ITransientService>();
        Assert.NotNull(transient1);
        Assert.NotNull(transient2);
        Assert.NotSame(transient1, transient2);

        using var scope1 = provider.CreateScope();
        using var scope2 = provider.CreateScope();
        var scoped1 = scope1.ServiceProvider.GetService<IScopedService>();
        var scoped2 = scope1.ServiceProvider.GetService<IScopedService>();
        var scoped3 = scope2.ServiceProvider.GetService<IScopedService>();
        Assert.NotNull(scoped1);
        Assert.Same(scoped1, scoped2);
        Assert.NotSame(scoped1, scoped3);

        var singleton1 = provider.GetService<ISingletonService>();
        var singleton2 = provider.GetService<ISingletonService>();
        Assert.NotNull(singleton1);
        Assert.Same(singleton1, singleton2);
    }
}
