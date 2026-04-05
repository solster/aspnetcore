using Microsoft.Extensions.DependencyInjection;

namespace Solster.Blazor.Templating.Tests;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddHtmlRenderer_RegistersIHtmlRendererAsSingleton()
    {
        var services = new ServiceCollection();
        services.AddHtmlRenderer();
        var sp = services.BuildServiceProvider();

        var renderer = sp.GetRequiredService<IHtmlRenderer>();

        Assert.IsType<HtmlRenderer>(renderer);
    }

    [Fact]
    public void AddHtmlRenderer_ReturnsSameInstance_WhenResolvedTwice()
    {
        var services = new ServiceCollection();
        services.AddHtmlRenderer();
        var sp = services.BuildServiceProvider();

        var renderer1 = sp.GetRequiredService<IHtmlRenderer>();
        var renderer2 = sp.GetRequiredService<IHtmlRenderer>();

        Assert.Same(renderer1, renderer2);
    }
}
