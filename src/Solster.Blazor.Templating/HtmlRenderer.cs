using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Renderer = Microsoft.AspNetCore.Components.Web.HtmlRenderer;

namespace Solster.Blazor.Templating;

public sealed class HtmlRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, Uri? cssBaseUri = null) : IHtmlRenderer
{
    public Task<String> RenderAsync<TComponent, TModel>(TModel model, Boolean inlineCss = false)
        where TComponent : IHtmlTemplate<TModel>
        => RenderAsync(typeof(TComponent), model, inlineCss);

    public Task<String> RenderAsync<TModel>(Type componentType, TModel model, Boolean inlineCss = false)
    {
        ValidateTemplateComponentType<TModel>(componentType);

        var parameters = new Dictionary<String, Object?>
        {
            [nameof(IHtmlTemplate<>.Model)] = model
        };

        return RenderComponentAsync(componentType, ParameterView.FromDictionary(parameters), inlineCss);
    }

    public Task<String> RenderAsync<TComponent>(Dictionary<String, Object?> parameters, Boolean inlineCss = false)
        where TComponent : IComponent
        => RenderAsync(typeof(TComponent), parameters, inlineCss);

    public Task<String> RenderAsync(Type componentType, Dictionary<String, Object?> parameters, Boolean inlineCss = false)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        ValidateComponentType(componentType);

        return RenderComponentAsync(componentType, ParameterView.FromDictionary(parameters), inlineCss);
    }

    public Task<String> RenderAsync<TComponent>(Boolean inlineCss = false)
        where TComponent : IComponent
        => RenderAsync(typeof(TComponent), inlineCss);

    public Task<String> RenderAsync(Type componentType, Boolean inlineCss = false)
    {
        ValidateComponentType(componentType);

        return RenderComponentAsync(componentType, ParameterView.Empty, inlineCss);
    }

    private async Task<String> RenderComponentAsync(Type componentType, ParameterView parameters, Boolean inlineCss)
    {
        await using var renderer = new Renderer(serviceProvider, loggerFactory);

        var html = await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var output = await renderer.RenderComponentAsync(componentType, parameters);
            return output.ToHtmlString();
        });

        return inlineCss && cssBaseUri is not null ? InlineCss(html) : html;
    }

    private static void ValidateComponentType(Type componentType)
    {
        ArgumentNullException.ThrowIfNull(componentType);

        if (!typeof(IComponent).IsAssignableFrom(componentType))
        {
            throw new ArgumentException($"The component type '{componentType}' must implement {nameof(IComponent)}.", nameof(componentType));
        }
    }

    private static void ValidateTemplateComponentType<TModel>(Type componentType)
    {
        ValidateComponentType(componentType);

        var templateInterface = componentType
            .GetInterfaces()
            .FirstOrDefault(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IHtmlTemplate<>) &&
                i.GenericTypeArguments[0] == typeof(TModel));

        if (templateInterface is null)
        {
            throw new ArgumentException(
                $"The component type '{componentType}' must implement {typeof(IHtmlTemplate<TModel>)}.",
                nameof(componentType));
        }
    }

    private String InlineCss(String html)
    {
        var result = new PreMailer.Net.PreMailer(html, cssBaseUri).MoveCssInline(removeComments: true);
        return result.Html;
    }
}
