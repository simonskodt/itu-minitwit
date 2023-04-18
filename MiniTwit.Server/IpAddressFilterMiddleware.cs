
using NetTools;
using System.Net;

namespace MiniTwit.Server;

public class IpAddressFilterMiddleware
{

    private readonly RequestDelegate _next;
    private readonly List<IPAddressRange> _allowedIpRanges;
    private readonly ILogger<IpAddressFilterMiddleware> _logger;


    public IpAddressFilterMiddleware(RequestDelegate next, List<string> allowedIpRanges, ILogger<IpAddressFilterMiddleware> logger)
    {
        _next = next;
        _allowedIpRanges = allowedIpRanges.ConvertAll(x => IPAddressRange.Parse(x));
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        IPAddress clientIpAddress = context.Connection.RemoteIpAddress!;
/* 
        if (clientIpAddress == null)
        {
            context.Response.StatusCode = 400; // Bad Request
            _logger.LogError("Unable to determine the client IP address");
            await context.Response.WriteAsync("Unable to determine the client IP address.");
            return;
        }
*/

        if (!IsIpAllowed(clientIpAddress, context))
        {
            context.Response.StatusCode = 403; // Forbidden
            _logger.LogCritical($"Unknown client IP address: {clientIpAddress} is trying to call backend endpoints");
            await context.Response.WriteAsync("The client IP address is not allowed.");
            return;
        }

        await _next.Invoke(context);
    }

    public bool IsIpAllowed(IPAddress ipAddress, HttpContext context)
    {
        if (ipAddress == null){
            return true;
        }
        bool isAllowed = false;

        foreach (IPAddressRange range in _allowedIpRanges)
        {
            if (IPAddress.IsLoopback(context.Connection.LocalIpAddress!) || range.Contains(ipAddress))
            {
                isAllowed = true;
                break;
            }
        }

        if (!isAllowed)
        {
            return false;
        }
        return isAllowed;
    }
}