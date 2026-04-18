namespace Solster.AspNetCore.ActivityLog;

/// <summary>
/// Serialisable snapshot of IRequestContext, persisted from SSR to interactive
/// circuit via PersistentComponentState.
/// </summary>
public record RequestContextData(string Actor, string? ActorIp, string? UserAgent);

