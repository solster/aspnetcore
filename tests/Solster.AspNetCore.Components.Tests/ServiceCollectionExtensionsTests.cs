using Microsoft.Extensions.DependencyInjection;
using Solster.AspNetCore.Components.Tests.Components;

namespace Solster.AspNetCore.Components.Tests;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddTemplateResolver_WithConfigureAction_RegistersITemplateResolver()
    {
        var services = new ServiceCollection();
        services.AddTemplateResolver(options =>
            options.TemplateProviders.Add(new AssemblyTemplateProvider(typeof(GreetingComponent).Assembly)));
        var sp = services.BuildServiceProvider();

        var resolver = sp.GetRequiredService<ITemplateResolver>();
        var type = resolver.Resolve("GreetingComponent");

        type.Should().Be(typeof(GreetingComponent));
    }

    [Fact]
    public void AddTemplateResolver_WithAssembly_RegistersITemplateResolver()
    {
        var services = new ServiceCollection();
        services.AddTemplateResolver(typeof(GreetingComponent).Assembly);
        var sp = services.BuildServiceProvider();

        var resolver = sp.GetRequiredService<ITemplateResolver>();
        var type = resolver.Resolve("GreetingComponent");

        type.Should().Be(typeof(GreetingComponent));
    }

    [Fact]
    public void AddTemplateResolver_ReturnsSameInstance_WhenResolvedTwice()
    {
        var services = new ServiceCollection();
        services.AddTemplateResolver(typeof(GreetingComponent).Assembly);
        var sp = services.BuildServiceProvider();

        var resolver1 = sp.GetRequiredService<ITemplateResolver>();
        var resolver2 = sp.GetRequiredService<ITemplateResolver>();

        resolver1.Should().BeSameAs(resolver2);
    }
}
