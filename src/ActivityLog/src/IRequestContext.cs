namespace Solster.AspNetCore.ActivityLog;

public interface IRequestContext
{
    string Actor { get; set; }
    string? ActorIp { get; set; }
    string? UserAgent { get; set; }
}

