using Microsoft.Extensions.DependencyInjection;
using Solster.Blazor.Templating.PreMailer;
using Solster.Blazor.Templating.Tests.Components;

namespace Solster.Blazor.Templating.Tests;

public sealed class PreMailerHtmlRendererTests
{
    private static readonly Uri DummyBaseUri = new("https://example.com/");

    private static IServiceProvider BuildServiceProvider() =>
        new ServiceCollection().BuildServiceProvider();

    [Fact]
    public async Task RenderAsync_TypedComponent_InlinesCss()
    {
        var sp = BuildServiceProvider();
        var inner = new HtmlRenderer(sp);
        var renderer = new PreMailerHtmlRenderer(inner, DummyBaseUri);

        var html = await renderer.RenderAsync<StyledComponent, CssModel>(new CssModel("My Title"));

        Assert.Contains("My Title", html);
        // After inlining, styles are applied as inline attributes on elements
        Assert.Contains("style=", html);
    }

    [Fact]
    public async Task RenderAsync_ParameterlessComponent_ReturnsExpectedHtml()
    {
        var sp = BuildServiceProvider();
        var inner = new HtmlRenderer(sp);
        var renderer = new PreMailerHtmlRenderer(inner, DummyBaseUri);

        var html = await renderer.RenderAsync<SimpleComponent>();

        Assert.Contains("Hello, world!", html);
    }

    [Fact]
    public void AddHtmlRenderer_WithInlineCssTrue_RegistersPreMailerRenderer()
    {
        var services = new ServiceCollection();
        services.AddHtmlRenderer(DummyBaseUri, inlineCss: true);
        var sp = services.BuildServiceProvider();

        var renderer = sp.GetRequiredService<IHtmlRenderer>();

        Assert.IsType<PreMailerHtmlRenderer>(renderer);
    }

    [Fact]
    public void AddHtmlRenderer_WithInlineCssFalse_RegistersPlainRenderer()
    {
        var services = new ServiceCollection();
        services.AddHtmlRenderer(DummyBaseUri, inlineCss: false);
        var sp = services.BuildServiceProvider();

        var renderer = sp.GetRequiredService<IHtmlRenderer>();

        Assert.IsType<HtmlRenderer>(renderer);
    }

    [Fact]
    public void AddHtmlRenderer_DefaultInlineCss_RegistersPreMailerRenderer()
    {
        var services = new ServiceCollection();
        services.AddHtmlRenderer(DummyBaseUri);
        var sp = services.BuildServiceProvider();

        var renderer = sp.GetRequiredService<IHtmlRenderer>();

        Assert.IsType<PreMailerHtmlRenderer>(renderer);
    }
}
