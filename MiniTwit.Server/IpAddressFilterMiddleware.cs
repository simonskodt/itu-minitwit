using System.Net;

namespace MiniTwit.Server;

public class IpAddressFilterMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IList<IPAddress> _allowedIpAddresses;
    private readonly ILogger<IpAddressFilterMiddleware> _logger;

    public IpAddressFilterMiddleware(RequestDelegate next, IList<string> allowedIpRanges, ILogger<IpAddressFilterMiddleware> logger)
    {
        _allowedIpAddresses = allowedIpRanges.Select(x => IPAddress.Parse(x)).ToList();
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        IPAddress clientIpAddress = context.Connection.RemoteIpAddress!;

        if (!IsIpAllowed(clientIpAddress, context))
        {
            context.Response.StatusCode = 403; // Forbidden
            _logger.LogCritical($"Unknown client IP address: {clientIpAddress} is trying to call backend endpoints");
            await context.Response.WriteAsync("The client IP address is not allowed.");
            return;
        }

        await _next(context);
    }

    public bool IsIpAllowed(IPAddress ipAddress, HttpContext context)
    {
        if (ipAddress == null)
        {
            return true;
        }

        return IPAddress.IsLoopback(context.Connection.LocalIpAddress!) || _allowedIpAddresses.Contains(ipAddress);
    }
}
