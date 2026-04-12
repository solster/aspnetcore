using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Solster.AspNetCore.Components.Tests.Components;

namespace Solster.AspNetCore.Components.Tests;

public sealed class HtmlRendererTests
{
    private static IServiceProvider BuildServiceProvider() =>
        new ServiceCollection().BuildServiceProvider();

    private sealed class NotAComponent;

    [Fact]
    public async Task RenderAsync_ParameterlessComponent_ReturnsExpectedHtml()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var html = await renderer.RenderAsync<SimpleComponent>();

        html.Should().Contain("Hello, world!");
    }

    [Fact]
    public async Task RenderAsync_TypedComponent_RendersModelData()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var html = await renderer.RenderAsync<GreetingComponent, GreetingModel>(new GreetingModel("Alice"));

        html.Should().Contain("Alice");
    }

    [Fact]
    public async Task RenderAsync_ComponentTypeAndModel_RendersModelData()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var html = await renderer.RenderAsync(typeof(GreetingComponent), new GreetingModel("Diana"));

        html.Should().Contain("Diana");
    }

    [Fact]
    public async Task RenderAsync_DictionaryParameters_RendersModelData()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var parameters = new Dictionary<String, Object?> { ["Model"] = new GreetingModel("Charlie") };
        var html = await renderer.RenderAsync<GreetingComponent>(parameters);

        html.Should().Contain("Charlie");
    }

    [Fact]
    public async Task RenderAsync_ComponentTypeAndDictionaryParameters_RendersModelData()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var parameters = new Dictionary<String, Object?> { ["Model"] = new GreetingModel("Eve") };
        var html = await renderer.RenderAsync(typeof(GreetingComponent), parameters);

        html.Should().Contain("Eve");
    }

    [Fact]
    public async Task RenderAsync_ComponentTypeParameterlessComponent_ReturnsExpectedHtml()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var html = await renderer.RenderAsync(typeof(SimpleComponent));

        html.Should().Contain("Hello, world!");
    }

    [Fact]
    public async Task RenderAsync_TypedComponent_DoesNotContainOtherName()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var html = await renderer.RenderAsync<GreetingComponent, GreetingModel>(new GreetingModel("Bob"));

        html.Should().NotContain("Alice");
        html.Should().Contain("Bob");
    }

    [Fact]
    public async Task RenderAsync_ModelOverload_WithNonComponentType_ThrowsArgumentException()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var act = () => renderer.RenderAsync(typeof(NotAComponent), new GreetingModel("Alice"));

        await act.Should().ThrowAsync<ArgumentException>()
            .WithParameterName("componentType");
    }

    [Fact]
    public async Task RenderAsync_DictionaryOverload_WithNonComponentType_ThrowsArgumentException()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var act = () => renderer.RenderAsync(typeof(NotAComponent), new Dictionary<String, Object?>());

        await act.Should().ThrowAsync<ArgumentException>()
            .WithParameterName("componentType");
    }

    [Fact]
    public async Task RenderAsync_ParameterlessOverload_WithNonComponentType_ThrowsArgumentException()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var act = () => renderer.RenderAsync(typeof(NotAComponent));

        await act.Should().ThrowAsync<ArgumentException>()
            .WithParameterName("componentType");
    }

    [Fact]
    public async Task RenderAsync_DictionaryOverload_WithNullParameters_ThrowsArgumentNullException()
    {
        var sp = BuildServiceProvider();
        await using var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        Dictionary<String, Object?>? parameters = null;
        var act = () => renderer.RenderAsync(typeof(GreetingComponent), parameters!);

        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("parameters");
    }
}
