using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace Solster.AspNetCore.Components;

/// <summary>
/// Resolves template names to Blazor component types by scanning the configured <see cref="TemplateResolverOptions.TemplateProviders"/>.
/// </summary>
internal sealed class TemplateResolver : ITemplateResolver
{
    private readonly IReadOnlyDictionary<String, Type> _templates;

    /// <summary>
    /// Initializes a new instance of <see cref="TemplateResolver"/> using the assembly specified in <paramref name="options"/>.
    /// </summary>
    public TemplateResolver(IOptions<TemplateResolverOptions> options)
    {
        _templates = options.Value.TemplateProviders.SelectMany(x => x.GetTemplates())
            .Where(t => t is { IsAbstract: false, IsInterface: false } &&
                        typeof(IComponent).IsAssignableFrom(t))
            .ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);
    }


    /// <inheritdoc />
    public Type Resolve(String templateName)
    {
        if (_templates.TryGetValue(templateName, out var type))
        {
            return type;
        }

        throw new TemplateResolverException(
            templateName,
            $"No template found for '{templateName}'. Available: {String.Join(", ", _templates.Keys)}");
    }
}
