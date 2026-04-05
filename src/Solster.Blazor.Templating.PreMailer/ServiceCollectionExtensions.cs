using Microsoft.Extensions.DependencyInjection;
using Solster.Blazor.Templating;

namespace Solster.Blazor.Templating.PreMailer;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IHtmlRenderer"/> with optional PreMailer.Net CSS inlining.
    /// </summary>
    public static IServiceCollection AddHtmlRenderer(
        this IServiceCollection services,
        Uri cssBaseUri,
        bool inlineCss = true)
    {
        services.AddSingleton<IHtmlRenderer>(sp =>
            inlineCss
                ? new PreMailerHtmlRenderer(new HtmlRenderer(sp), cssBaseUri)
                : new HtmlRenderer(sp));
        return services;
    }
}
