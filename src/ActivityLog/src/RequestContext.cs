namespace Solster.AspNetCore.ActivityLog;

public class RequestContext : IRequestContext
{
    public string Actor { get; set; } = "public";
    public string? ActorIp { get; set; }
    public string? UserAgent { get; set; }
}

