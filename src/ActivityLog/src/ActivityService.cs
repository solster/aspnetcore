using System.Collections;
using Microsoft.Extensions.Logging;

namespace Solster.AspNetCore.ActivityLog;

/// <summary>
/// Scoped service that records activity events.
/// Actor, IP and browser are resolved automatically from IRequestContext.
/// Call sites supply the EventId, resource id, and label.
/// </summary>
public class ActivityService(ILogger<ActivityService> logger, IRequestContext requestContext) : IActivityService
{
    public void RecordEvent(int eventId, string resourceId, string label) =>
        logger.Log(
            LogLevel.Information,
            new EventId(eventId),
            new ActivityEventState(resourceId, label, requestContext.Actor, requestContext.ActorIp, requestContext.UserAgent),
            null,
            static (s, _) => s.ToString());
}

internal sealed class ActivityEventState(
    string resourceId,
    string label,
    string actor,
    string? actorIp,
    string? userAgent) : IEnumerable<KeyValuePair<string, object?>>
{
    public override string ToString() => $"Activity recorded: {resourceId} — {label}";

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        yield return new KeyValuePair<string, object?>("ResourceId", resourceId);
        yield return new KeyValuePair<string, object?>("ResourceLabel", label);
        yield return new KeyValuePair<string, object?>("Actor", actor);
        yield return new KeyValuePair<string, object?>("ActorIp", actorIp);
        yield return new KeyValuePair<string, object?>("UserAgent", userAgent);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
