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

        html.Should().Contain("Hello, world!");
    }

    [Fact]
    public async Task RenderAsync_TypedComponent_RendersModelData()
    {
        var sp = BuildServiceProvider();
        var renderer = new HtmlRenderer(sp);

        var html = await renderer.RenderAsync<GreetingComponent, GreetingModel>(new GreetingModel("Alice"));

        html.Should().Contain("Alice");
    }

    [Fact]
    public async Task RenderAsync_TypedComponent_DoesNotContainOtherName()
    {
        var sp = BuildServiceProvider();
        var renderer = new HtmlRenderer(sp);

        var html = await renderer.RenderAsync<GreetingComponent, GreetingModel>(new GreetingModel("Bob"));

        html.Should().NotContain("Alice");
        html.Should().Contain("Bob");
    }
}
