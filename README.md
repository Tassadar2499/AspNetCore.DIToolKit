# AspNetCore.DIToolKit
Simple injection of ASP.NET Core services. Just implement one of selected interfaces: `LifeTime.ISingleton`, `LifeTime.IScoped` or `LifeTime.ITransient`.

## Example

Create your service by implementing one of the lifetime interfaces:

```csharp
public interface IGreetingService
{
    string Greet();
}

public class GreetingService : IGreetingService, LifeTime.ITransient
{
    public string Greet() => "Hello from transient service!";
}
```

And register all services automatically:

```csharp
builder.Services.ConfigureServicesByLifeTimeCycle(builder.Configuration);
```

