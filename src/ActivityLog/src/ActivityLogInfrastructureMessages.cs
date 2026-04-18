using Microsoft.Extensions.Logging;

namespace Solster.AspNetCore.ActivityLog;

/// <summary>
/// Source-generated log methods for ActivityLog infrastructure errors.
/// </summary>
internal static partial class ActivityLogInfrastructureMessages
{
    [LoggerMessage(
        EventId = 2001,
        Level = LogLevel.Error,
        Message = "Failed to persist activity log entry {EntryId}")]
    public static partial void FailedToPersistEntry(this ILogger logger, Exception ex, Guid entryId);
}

