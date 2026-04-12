using System.Reflection;
using Solster.AspNetCore.Components.Tests.Components;

namespace Solster.AspNetCore.Components.Tests;

public sealed class AssemblyTemplateProviderTests
{
    [Fact]
    public void GetTemplates_WhenAssemblyGetTypesThrowsReflectionTypeLoadException_ReturnsNonNullTypes()
    {
        var assembly = new ThrowingAssembly([
            typeof(GreetingComponent),
            null,
            typeof(SimpleComponent)
        ]);
        var provider = new AssemblyTemplateProvider(assembly);

        var templates = provider.GetTemplates();

        templates.Should().Contain(typeof(GreetingComponent));
        templates.Should().Contain(typeof(SimpleComponent));
        templates.Should().HaveCount(2);
    }

    private sealed class ThrowingAssembly(Type?[] types) : Assembly
    {
        public override Type[] GetTypes() =>
            throw new ReflectionTypeLoadException(types, [new TypeLoadException("test")]);

        public override String FullName => "ThrowingAssembly";
    }
}

