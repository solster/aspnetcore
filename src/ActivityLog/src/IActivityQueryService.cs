namespace Solster.AspNetCore.ActivityLog;

public interface IActivityQueryService
{
    Task<IReadOnlyList<ActivityLogEntry>> GetGlobalActivityAsync(
        int take = 100,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ActivityLogEntry>> GetActivityForResourceAsync(
        string resourceType,
        string resourceId,
        int take = 100,
        CancellationToken cancellationToken = default);
}

