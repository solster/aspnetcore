using Microsoft.EntityFrameworkCore;

namespace Solster.AspNetCore.ActivityLog.EntityFrameworkCore;

/// <summary>
/// EF Core + Npgsql implementation of IActivityStore.
/// Appends entries to ActivityDbContext and delegates queries to it.
/// This is an append-only log — no updates or deletes are ever issued.
/// </summary>
public class EfCoreActivityStore(ActivityDbContext db) : IActivityStore
{
    public async Task AppendAsync(ActivityLogEntry entry, CancellationToken cancellationToken = default)
    {
        db.ActivityLogs.Add(entry);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ActivityLogEntry>> GetGlobalActivityAsync(
        int take = 100,
        CancellationToken cancellationToken = default) =>
        await db.ActivityLogs
            .OrderByDescending(e => e.Timestamp)
            .Take(take)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<ActivityLogEntry>> GetActivityForResourceAsync(
        string resourceType,
        string resourceId,
        int take = 100,
        CancellationToken cancellationToken = default) =>
        await db.ActivityLogs
            .Where(e => e.ResourceType == resourceType && e.ResourceId == resourceId)
            .OrderByDescending(e => e.Timestamp)
            .Take(take)
            .ToListAsync(cancellationToken);
}

