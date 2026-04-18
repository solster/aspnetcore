using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Solster.AspNetCore.ActivityLog;

/// <summary>
/// Custom ILoggerProvider that intercepts activity log events within the configured EventId range
/// and writes typed ActivityLogEntry objects to the Channel for async persistence.
/// All other log events are ignored — they continue to App Insights / console as normal.
/// </summary>
public sealed class ActivityLoggerProvider(
    ChannelWriter<ActivityLogEntry> writer,
    IOptions<ActivityLogOptions> options) : ILoggerProvider
{
    private readonly ActivityLogOptions _options = options.Value;

    public ILogger CreateLogger(string categoryName) => new ActivityLogger(writer, _options);

    public void Dispose() { }

    private sealed class ActivityLogger(ChannelWriter<ActivityLogEntry> writer, ActivityLogOptions options) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (eventId.Id < options.MinEventId || eventId.Id > options.MaxEventId)
            {
                return;
            }

            if (!options.EventMap.TryGetValue(eventId.Id, out var mapping))
            {
                return;
            }

            var props = state as IEnumerable<KeyValuePair<string, object?>> ?? [];
            var dict = props.ToDictionary(p => p.Key, p => p.Value);

            var entry = new ActivityLogEntry
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Category = mapping.Category,
                Action = mapping.Action,
                ResourceType = mapping.ResourceType.Length > 0 ? mapping.ResourceType : null,
                ResourceId = dict.GetValueOrDefault("ResourceId")?.ToString(),
                ResourceLabel = dict.GetValueOrDefault("ResourceLabel")?.ToString(),
                Actor = dict.GetValueOrDefault("Actor")?.ToString() ?? "public",
                ActorIp = dict.GetValueOrDefault("ActorIp")?.ToString(),
                UserAgent = dict.GetValueOrDefault("UserAgent")?.ToString(),
            };

            writer.TryWrite(entry);
        }
    }
}
