namespace Solster.AspNetCore.ActivityLog;

/// <summary>
/// Read-only queries delegated to the configured IActivityStore.
/// Queries never leave the activity store — no joins to domain data.
/// </summary>
public class ActivityQueryService(IActivityStore store) : IActivityQueryService
{
    public Task<IReadOnlyList<ActivityLogEntry>> GetGlobalActivityAsync(
        int take = 100,
        CancellationToken cancellationToken = default) =>
        store.GetGlobalActivityAsync(take, cancellationToken);

    public Task<IReadOnlyList<ActivityLogEntry>> GetActivityForResourceAsync(
        string resourceType,
        string resourceId,
        int take = 100,
        CancellationToken cancellationToken = default) =>
        store.GetActivityForResourceAsync(resourceType, resourceId, take, cancellationToken);
}

