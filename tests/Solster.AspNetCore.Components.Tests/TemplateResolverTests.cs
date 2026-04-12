using Microsoft.Extensions.DependencyInjection;
using Solster.AspNetCore.Components.Tests.Components;

namespace Solster.AspNetCore.Components.Tests;

public sealed class TemplateResolverTests
{
    private static ITemplateResolver BuildResolver() =>
        new ServiceCollection()
            .AddTemplateResolver(typeof(GreetingComponent).Assembly)
            .BuildServiceProvider()
            .GetRequiredService<ITemplateResolver>();

    [Fact]
    public void Resolve_KnownTemplateName_ReturnsCorrectType()
    {
        var resolver = BuildResolver();

        var type = resolver.Resolve("GreetingComponent");

        type.Should().Be(typeof(GreetingComponent));
    }

    [Fact]
    public void Resolve_KnownTemplateName_IsCaseInsensitive()
    {
        var resolver = BuildResolver();

        var type = resolver.Resolve("greetingcomponent");

        type.Should().Be(typeof(GreetingComponent));
    }

    [Fact]
    public void Resolve_UnknownTemplateName_ThrowsTemplateResolveException()
    {
        var resolver = BuildResolver();

        var act = () => resolver.Resolve("NonExistentTemplate");

        act.Should().Throw<TemplateResolverException>();
    }

    [Fact]
    public void Resolve_UnknownTemplateName_ExceptionMessageContainsTemplateName()
    {
        var resolver = BuildResolver();

        var act = () => resolver.Resolve("NonExistentTemplate");

        act.Should().Throw<TemplateResolverException>()
            .WithMessage("*NonExistentTemplate*");
    }

    [Fact]
    public void Resolve_UnknownTemplateName_ExceptionMessageListsAvailableTemplates()
    {
        var resolver = BuildResolver();

        var act = () => resolver.Resolve("NonExistentTemplate");

        act.Should().Throw<TemplateResolverException>()
            .WithMessage("*GreetingComponent*");
    }
}
