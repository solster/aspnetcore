using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Solster.AspNetCore.ActivityLog;

/// <summary>
/// Drains the Channel and appends each ActivityLogEntry to the activity store.
/// Runs in the background so the request thread is never blocked by store writes.
/// </summary>
public class ActivityChannelProcessor(
    ChannelReader<ActivityLogEntry> reader,
    IServiceScopeFactory scopeFactory,
    ILogger<ActivityChannelProcessor> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var entry in reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var store = scope.ServiceProvider.GetRequiredService<IActivityStore>();
                await store.AppendAsync(entry, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.FailedToPersistEntry(ex, entry.Id);
            }
        }
    }
}
