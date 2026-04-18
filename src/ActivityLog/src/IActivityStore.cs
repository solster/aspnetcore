namespace Solster.AspNetCore.ActivityLog;

/// <summary>
/// Append-only store contract for activity log entries.
/// Implement this interface to bring your own persistence back-end.
/// Register your implementation with AddActivityLogging() then call
/// services.AddSingleton/Scoped&lt;IActivityStore, YourStore&gt;().
/// </summary>
public interface IActivityStore
{
    /// <summary>
    /// Appends a new entry to the activity log. Implementations must never
    /// update or delete existing rows — the log is immutable by design.
    /// </summary>
    Task AppendAsync(ActivityLogEntry entry, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ActivityLogEntry>> GetGlobalActivityAsync(
        int take = 100,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ActivityLogEntry>> GetActivityForResourceAsync(
        string resourceType,
        string resourceId,
        int take = 100,
        CancellationToken cancellationToken = default);
}

