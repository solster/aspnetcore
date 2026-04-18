using AwesomeAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Solster.AspNetCore.ActivityLog.UnitTests;

public class ActivityServiceTests
{
    private readonly IRequestContext _ctx = Substitute.For<IRequestContext>();
    private readonly ILogger<ActivityService> _logger = Substitute.For<ILogger<ActivityService>>();
    private readonly ActivityService _sut;

    public ActivityServiceTests()
    {
        _ctx.Actor.Returns("officer@police.uk");
        _ctx.ActorIp.Returns("10.0.0.1");
        _ctx.UserAgent.Returns("Mozilla/5.0");
        _sut = new ActivityService(_logger, _ctx);
    }

    [Fact]
    public void RecordEvent_DoesNotThrow()
    {
        var act = () => _sut.RecordEvent(9001, "sub-1", "Ticket 42");
        act.Should().NotThrow();
    }

    [Fact]
    public void RecordEvent_ReadsActorFromRequestContext()
    {
        _sut.RecordEvent(9001, "sub-1", "Ticket 42");
        _ = _ctx.Received().Actor;
    }

    [Fact]
    public void RecordEvent_ReadsActorIpFromRequestContext()
    {
        _sut.RecordEvent(9001, "sub-1", "Ticket 42");
        _ = _ctx.Received().ActorIp;
    }

    [Fact]
    public void RecordEvent_ReadsUserAgentFromRequestContext()
    {
        _sut.RecordEvent(9001, "sub-1", "Ticket 42");
        _ = _ctx.Received().UserAgent;
    }
}
