using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Solster.AspNetCore.ActivityLog;

/// <summary>
/// Runs after UseForwardedHeaders so RemoteIpAddress already reflects the real client IP.
/// Populates IRequestContext from the current HTTP request; the scoped instance is then
/// available throughout the request (and persisted to the Blazor circuit via Routes.razor).
/// </summary>
public class RequestContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext, IRequestContext requestContext)
    {
        requestContext.Actor = httpContext.User.FindFirstValue(ClaimTypes.Email) ?? "public";
        requestContext.ActorIp = httpContext.Connection.RemoteIpAddress?.ToString();

        var ua = httpContext.Request.Headers.UserAgent.ToString();
        requestContext.UserAgent = string.IsNullOrWhiteSpace(ua) ? null : ua;

        await next(httpContext);
    }
}

