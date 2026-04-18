namespace Solster.AspNetCore.ActivityLog;

/// <summary>
/// Configures which log EventIds are captured as ActivityLogEntry records.
/// Call RegisterEvent for each EventId your application emits via IActivityService.RecordEvent.
/// </summary>
public sealed class ActivityLogOptions
{
    public int MinEventId { get; set; } = 9000;
    public int MaxEventId { get; set; } = 9999;

    internal Dictionary<int, (short Category, short Action, string ResourceType)> EventMap { get; } = new();

    public ActivityLogOptions RegisterEvent(int eventId, short category, short action, string resourceType = "")
    {
        EventMap[eventId] = (category, action, resourceType);
        return this;
    }
}

