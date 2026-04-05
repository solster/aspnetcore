using Microsoft.AspNetCore.Components;

namespace Solster.Blazor.Templating;

/// <summary>
/// Renders a Blazor component to an HTML string.
/// </summary>
public interface IHtmlRenderer
{
    /// <summary>
    /// Renders <typeparamref name="TComponent"/> with the given <paramref name="model"/> to an HTML string.
    /// </summary>
    /// <param name="inlineCss">
    /// When <see langword="true"/> (the default), CSS is inlined into the HTML if the renderer supports it.
    /// Pass <see langword="false"/> to skip CSS inlining for this call.
    /// </param>
    Task<String> RenderAsync<TComponent, TModel>(TModel model, bool inlineCss = true)
        where TComponent : IHtmlTemplate<TModel>;

    /// <summary>
    /// Renders <typeparamref name="TComponent"/> with no model (for parameter-less templates).
    /// </summary>
    /// <param name="inlineCss">
    /// When <see langword="true"/> (the default), CSS is inlined into the HTML if the renderer supports it.
    /// Pass <see langword="false"/> to skip CSS inlining for this call.
    /// </param>
    Task<String> RenderAsync<TComponent>(bool inlineCss = true)
        where TComponent : IComponent;
}
