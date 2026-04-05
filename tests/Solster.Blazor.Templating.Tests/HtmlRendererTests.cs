using Microsoft.Extensions.DependencyInjection;
using Solster.Blazor.Templating.Tests.Components;

namespace Solster.Blazor.Templating.Tests;

public sealed class HtmlRendererTests
{
    private static IServiceProvider BuildServiceProvider() =>
        new ServiceCollection().BuildServiceProvider();

    [Fact]
    public async Task RenderAsync_ParameterlessComponent_ReturnsExpectedHtml()
    {
        var sp = BuildServiceProvider();
        var renderer = new HtmlRenderer(sp);

        var html = await renderer.RenderAsync<SimpleComponent>();

        Assert.Contains("Hello, world!", html);
    }

    [Fact]
    public async Task RenderAsync_TypedComponent_RendersModelData()
    {
        var sp = BuildServiceProvider();
        var renderer = new HtmlRenderer(sp);

        var html = await renderer.RenderAsync<GreetingComponent, GreetingModel>(new GreetingModel("Alice"));

        Assert.Contains("Alice", html);
    }

    [Fact]
    public async Task RenderAsync_TypedComponent_DoesNotContainOtherName()
    {
        var sp = BuildServiceProvider();
        var renderer = new HtmlRenderer(sp);

        var html = await renderer.RenderAsync<GreetingComponent, GreetingModel>(new GreetingModel("Bob"));

        Assert.DoesNotContain("Alice", html);
        Assert.Contains("Bob", html);
    }
}
