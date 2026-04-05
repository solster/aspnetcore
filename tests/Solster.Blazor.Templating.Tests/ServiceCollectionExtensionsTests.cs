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

        renderer.Should().BeOfType<HtmlRenderer>();
    }

    [Fact]
    public void AddHtmlRenderer_ReturnsSameInstance_WhenResolvedTwice()
    {
        var services = new ServiceCollection();
        services.AddHtmlRenderer();
        var sp = services.BuildServiceProvider();

        var renderer1 = sp.GetRequiredService<IHtmlRenderer>();
        var renderer2 = sp.GetRequiredService<IHtmlRenderer>();

        renderer1.Should().BeSameAs(renderer2);
    }
}
