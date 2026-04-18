namespace Solster.AspNetCore.ActivityLog;

public class ActivityLogEntry
{
    public Guid Id { get; init; }
    public DateTime Timestamp { get; init; }
    public short Category { get; init; }
    public short Action { get; init; }
    public string? ResourceType { get; init; }
    public string? ResourceId { get; init; }
    public string? ResourceLabel { get; init; }
    public string Actor { get; init; } = default!;
    public string? ActorIp { get; init; }
    public string? UserAgent { get; init; }
    public Guid? CorrelationId { get; init; }
}

