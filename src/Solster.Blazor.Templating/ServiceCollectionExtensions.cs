using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Solster.Blazor.Templating;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IHtmlRenderer"/> for rendering Blazor components to HTML strings.
    /// </summary>
    public static IServiceCollection AddHtmlRenderer(this IServiceCollection services)
    {
        services.AddLogging();
        services.AddSingleton<IHtmlRenderer, HtmlRenderer>();
        return services;
    }

    /// <summary>
    /// Registers <see cref="IHtmlRenderer"/> with PreMailer.Net CSS inlining support.
    /// CSS inlining is opt-in per render call via the <c>inlineCss</c> parameter on <see cref="IHtmlRenderer.RenderAsync{TComponent,TModel}"/>.
    /// </summary>
    public static IServiceCollection AddHtmlRenderer(this IServiceCollection services, Uri cssBaseUri)
    {
        services.AddLogging();
        services.AddSingleton<IHtmlRenderer>(sp =>
            new HtmlRenderer(sp, sp.GetRequiredService<ILoggerFactory>(), cssBaseUri));
        return services;
    }
}
