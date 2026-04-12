using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Solster.AspNetCore.Components;

public static class TemplateResolverServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Registers <see cref="IHtmlRenderer"/> for resolving template names and rendering them to HTML strings.
        /// Optionally configures <see cref="TemplateResolverOptions"/> and registers <see cref="ITemplateResolver"/>.
        /// </summary>
        public IServiceCollection AddTemplateResolver(Action<TemplateResolverOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.TryAddSingleton<ITemplateResolver, TemplateResolver>();
            return services;
        }
        
        public IServiceCollection AddTemplateResolver(Assembly assembly)
            => services.AddTemplateResolver(options => options.TemplateProviders.Add(new AssemblyTemplateProvider(assembly)));
    }
}
