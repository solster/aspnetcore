using Microsoft.AspNetCore.Components;
using Solster.Blazor.Templating;

namespace Solster.Blazor.Templating.PreMailer;

/// <summary>
/// Decorates <see cref="IHtmlRenderer"/> to inline CSS using PreMailer.Net.
/// </summary>
public sealed class PreMailerHtmlRenderer(IHtmlRenderer inner, Uri cssBaseUri) : IHtmlRenderer
{
    public async Task<String> RenderAsync<TComponent, TModel>(TModel model, bool inlineCss = true)
        where TComponent : IHtmlTemplate<TModel>
    {
        var html = await inner.RenderAsync<TComponent, TModel>(model, inlineCss: false);
        return inlineCss ? InlineCss(html) : html;
    }

    public async Task<String> RenderAsync<TComponent>(bool inlineCss = true)
        where TComponent : IComponent
    {
        var html = await inner.RenderAsync<TComponent>(inlineCss: false);
        return inlineCss ? InlineCss(html) : html;
    }

    private String InlineCss(String html)
    {
        var result = new global::PreMailer.Net.PreMailer(html, cssBaseUri).MoveCssInline(removeComments: true);
        return result.Html;
    }
}
