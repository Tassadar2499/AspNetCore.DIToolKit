using AspNetCore.DIToolKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AspNetCore.DIToolKit.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void ConfigureServicesByLifeTimeCycle_RegistersExpectedLifetimes()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.ConfigureServicesByLifeTimeCycle(configuration);

        var singletonDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ISingletonTest));
        var scopedDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IScopedTest));
        var transientDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ITransientTest));

        Assert.NotNull(singletonDescriptor);
        Assert.NotNull(scopedDescriptor);
        Assert.NotNull(transientDescriptor);

        Assert.Equal(ServiceLifetime.Singleton, singletonDescriptor!.Lifetime);
        Assert.Equal(ServiceLifetime.Scoped, scopedDescriptor!.Lifetime);
        Assert.Equal(ServiceLifetime.Transient, transientDescriptor!.Lifetime);
    }

    public interface ISingletonTest { }
    public class SingletonTest : ISingletonTest, LifeTime.ISingleton { }

    public interface IScopedTest { }
    public class ScopedTest : IScopedTest, LifeTime.IScoped { }

    public interface ITransientTest { }
    public class TransientTest : ITransientTest, LifeTime.ITransient { }
}
