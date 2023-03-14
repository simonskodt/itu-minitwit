using System.Diagnostics;

namespace MiniTwit.Server.Prometheus;

public class ResponseMetricMiddleware
{
    private readonly RequestDelegate _request;

    public ResponseMetricMiddleware(RequestDelegate request)
    {
        _request = request;
    }

    public async Task Invoke(HttpContext context, MetricReporter reporter)
    {
        // Ignore metrics endpoint
        var path = context.Request.Path.Value;
        if (path == "/metrics")
        {
            await _request.Invoke(context);
            return;
        }

        var sw = Stopwatch.StartNew();

        try
        {
            await _request.Invoke(context);
        }
        finally
        {
            sw.Stop();
            reporter.RegisterRequest();
            reporter.RegisterResponseTime(context.Response.StatusCode, context.Request.Method, context.GetEndpoint()!.DisplayName!, sw.Elapsed);
        }
    }
}
