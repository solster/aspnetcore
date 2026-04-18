using System.Net;
using System.Security.Claims;
using AwesomeAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Solster.AspNetCore.ActivityLog.UnitTests;

public class RequestContextMiddlewareTests
{
    private static (RequestContextMiddleware Middleware, IRequestContext Ctx, bool NextCalled) Build(
        HttpContext httpContext)
    {
        var ctx = Substitute.For<IRequestContext>();
        var nextCalled = false;
        var middleware = new RequestContextMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });

        // invoke and capture side effects via closure
        // We'll test by inspecting ctx after InvokeAsync
        return (middleware, ctx, nextCalled);
    }

    private static DefaultHttpContext AuthenticatedContext(string email)
    {
        var httpCtx = new DefaultHttpContext();
        var identity = new ClaimsIdentity(
            [new Claim(ClaimTypes.Email, email)],
            "TestAuth");
        httpCtx.User = new ClaimsPrincipal(identity);
        return httpCtx;
    }

    [Fact]
    public async Task InvokeAsync_AuthenticatedUser_SetsActorToEmail()
    {
        var httpCtx = AuthenticatedContext("officer@police.uk");
        var ctx = Substitute.For<IRequestContext>();
        var middleware = new RequestContextMiddleware(_ => Task.CompletedTask);

        await middleware.InvokeAsync(httpCtx, ctx);

        ctx.Actor.Should().Be("officer@police.uk");
    }

    [Fact]
    public async Task InvokeAsync_AnonymousUser_SetsActorToPublic()
    {
        var httpCtx = new DefaultHttpContext(); // no identity
        var ctx = Substitute.For<IRequestContext>();
        var middleware = new RequestContextMiddleware(_ => Task.CompletedTask);

        await middleware.InvokeAsync(httpCtx, ctx);

        ctx.Actor.Should().Be("public");
    }

    [Fact]
    public async Task InvokeAsync_WithRemoteIp_SetsActorIp()
    {
        var httpCtx = new DefaultHttpContext();
        httpCtx.Connection.RemoteIpAddress = IPAddress.Parse("10.0.0.5");
        var ctx = Substitute.For<IRequestContext>();
        var middleware = new RequestContextMiddleware(_ => Task.CompletedTask);

        await middleware.InvokeAsync(httpCtx, ctx);

        ctx.ActorIp.Should().Be("10.0.0.5");
    }

    [Fact]
    public async Task InvokeAsync_WithUserAgent_SetsUserAgent()
    {
        var httpCtx = new DefaultHttpContext();
        httpCtx.Request.Headers.UserAgent = "Mozilla/5.0";
        var ctx = Substitute.For<IRequestContext>();
        var middleware = new RequestContextMiddleware(_ => Task.CompletedTask);

        await middleware.InvokeAsync(httpCtx, ctx);

        ctx.UserAgent.Should().Be("Mozilla/5.0");
    }

    [Fact]
    public async Task InvokeAsync_EmptyUserAgent_SetsUserAgentToNull()
    {
        var httpCtx = new DefaultHttpContext(); // no UA header
        var ctx = Substitute.For<IRequestContext>();
        var middleware = new RequestContextMiddleware(_ => Task.CompletedTask);

        await middleware.InvokeAsync(httpCtx, ctx);

        ctx.UserAgent.Should().BeNull();
    }

    [Fact]
    public async Task InvokeAsync_AlwaysCallsNext()
    {
        var httpCtx = new DefaultHttpContext();
        var ctx = Substitute.For<IRequestContext>();
        var nextCalled = false;
        var middleware = new RequestContextMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });

        await middleware.InvokeAsync(httpCtx, ctx);

        nextCalled.Should().BeTrue();
    }
}

