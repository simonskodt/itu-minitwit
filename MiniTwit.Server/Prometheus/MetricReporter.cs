using Prometheus;

namespace MiniTwit.Server.Prometheus;

public class MetricReporter
{
    private readonly ILogger<MetricReporter> _logger;
    private readonly Counter _requestCounter;
    private readonly Histogram _responseTimeHistogram;

    public MetricReporter(ILogger<MetricReporter> logger)
    {
        _logger = logger;

        _requestCounter = Metrics.CreateCounter("minitwit_total_requests", "The total number of requests serviced by the MiniTwit API.", new CounterConfiguration
        {
            ExemplarBehavior = ExemplarBehavior.NoExemplars()
        });
        _responseTimeHistogram = Metrics.CreateHistogram("minitwit_request_duration_seconds", "The duration in seconds between the response to a request to the Minitwit API.", new HistogramConfiguration
        {
            Buckets = Histogram.ExponentialBuckets(0.01, 2, 10),
            LabelNames = new[] { "status_code", "method" }
        });
    }

    public void RegisterRequest()
    {
        _requestCounter.Inc();
    }

    public void RegisterResponseTime(int statusCode, string method, TimeSpan elapsed)
    {
        _responseTimeHistogram.Labels(statusCode.ToString(), method).Observe(elapsed.TotalSeconds);
    }
}