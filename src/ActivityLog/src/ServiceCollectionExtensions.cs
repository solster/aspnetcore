using System.Threading.Channels;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Solster.AspNetCore.ActivityLog;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all store-agnostic activity logging services:
    /// Channel + ActivityChannelProcessor, ActivityLoggerProvider (as ILoggerProvider),
    /// IActivityService, IRequestContext, IActivityQueryService.
    /// Use the configure delegate to register your application's EventId mappings via ActivityLogOptions.
    /// You must also register an IActivityStore implementation — for example call
    /// services.AddEfCoreActivityStore(...) from Solster.AspNetCore.ActivityLog.EntityFrameworkCore.
    /// </summary>
    public static IServiceCollection AddActivityLogging(
        this IServiceCollection services,
        Action<ActivityLogOptions>? configure = null)
    {
        services.Configure(configure ?? (_ => { }));

        var channel = Channel.CreateUnbounded<ActivityLogEntry>(new UnboundedChannelOptions
        {
            SingleReader = true
        });

        services.AddSingleton(channel.Reader);
        services.AddSingleton(channel.Writer);

        // Registered as ILoggerProvider so the logging framework picks it up automatically via DI
        services.AddSingleton<ILoggerProvider, ActivityLoggerProvider>();

        services.AddHostedService<ActivityChannelProcessor>();

        services.AddScoped<IRequestContext, RequestContext>();
        services.AddScoped<IActivityService, ActivityService>();
        services.AddScoped<IActivityQueryService, ActivityQueryService>();

        return services;
    }

    /// <summary>
    /// Adds the RequestContextMiddleware to the pipeline.
    /// Must be called after UseForwardedHeaders() and UseAuthentication().
    /// </summary>
    public static IApplicationBuilder UseRequestContext(this IApplicationBuilder app) =>
        app.UseMiddleware<RequestContextMiddleware>();
}
