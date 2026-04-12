namespace Solster.AspNetCore.Components;

/// <summary>
/// Options for HTML rendering, including template type resolution and JSON model binding.
/// </summary>
public class TemplateResolverOptions
{
	public IList<ITemplateProvider> TemplateProviders { get; } = [];
}

