using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Solster.AspNetCore.Components.Tests.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Solster.AspNetCore.Components.Tests;

public sealed class PreMailerHtmlRendererTests
{
    private static IServiceProvider BuildServiceProvider() =>
        new ServiceCollection().BuildServiceProvider();

    [Fact]
    public async Task RenderAsync_TypedComponent_InlinesCssWhenInlineCssTrue()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var html = await renderer.RenderAsync<StyledComponent, CssModel>(new CssModel("My Title"), inlineCss: true);

        html.Should().Contain("My Title");
        // CSS inlined — styles are applied as inline attributes on elements
        html.Should().Contain("style=");
    }

    [Fact]
    public async Task RenderAsync_ComponentTypeAndModel_InlinesCssWhenInlineCssTrue()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var html = await renderer.RenderAsync(typeof(StyledComponent), new CssModel("Typed Title"), inlineCss: true);

        html.Should().Contain("Typed Title");
        html.Should().Contain("style=");
    }

    [Fact]
    public async Task RenderAsync_TypedComponent_SkipsCssWhenInlineCssFalse()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var html = await renderer.RenderAsync<StyledComponent, CssModel>(new CssModel("My Title"), inlineCss: false);

        html.Should().Contain("My Title");
        // CSS not inlined, so no inline style attribute on elements
        html.Should().NotContain("<h1 style=");
    }

    [Fact]
    public async Task RenderAsync_ParameterlessComponent_DoesNotInlineCssByDefault()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var html = await renderer.RenderAsync<SimpleComponent>();

        html.Should().Contain("Hello, world!");
    }

    [Fact]
    public async Task RenderAsync_ParameterlessComponent_SkipsCssWhenInlineCssFalse()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var html = await renderer.RenderAsync<SimpleComponent>(inlineCss: false);

        html.Should().Contain("Hello, world!");
    }

    [Fact]
    public async Task RenderAsync_DictionaryOverload_WithNullParameters_ThrowsArgumentNullException()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        Dictionary<String, Object?>? parameters = null;
        var act = () => renderer.RenderAsync<StyledComponent>(parameters!, inlineCss: true);

        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("parameters");
    }
}
