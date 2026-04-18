namespace Solster.AspNetCore.ActivityLog;

public interface IActivityService
{
    void RecordEvent(int eventId, string resourceId, string label);
}
