using Microsoft.Extensions.DependencyInjection;
using Solster.Blazor.Templating;

namespace Solster.Blazor.Templating.PreMailer;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IHtmlRenderer"/> with PreMailer.Net CSS inlining.
    /// CSS inlining can be disabled per-call by passing <c>inlineCss: false</c> to <see cref="IHtmlRenderer.RenderAsync{TComponent, TModel}"/>.
    /// </summary>
    public static IServiceCollection AddHtmlRenderer(
        this IServiceCollection services,
        Uri cssBaseUri)
    {
        services.AddSingleton<IHtmlRenderer>(sp =>
            new PreMailerHtmlRenderer(new HtmlRenderer(sp), cssBaseUri));
        return services;
    }
}
