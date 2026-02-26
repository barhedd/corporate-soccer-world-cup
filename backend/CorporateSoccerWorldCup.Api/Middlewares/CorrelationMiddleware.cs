using CorporateSoccerWorldCup.Api.Commons.Correlation;
using System.Diagnostics;

namespace CorporateSoccerWorldCup.Api.Middlewares;

public class CorrelationMiddleware(
    RequestDelegate next,
    ILogger<CorrelationMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<CorrelationMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        var correlationId =
            context.Request.Headers[CorrelationConstants.CorrelationIdHeader]
                .FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        // TraceId estándar si existe Activity
        var traceId =
            Activity.Current?.TraceId.ToString()
            ?? context.TraceIdentifier;

        // Guardar en HttpContext
        context.Items[CorrelationConstants.CorrelationIdItemKey] = correlationId;
        context.Items[CorrelationConstants.TraceIdItemKey] = traceId;

        // Devolver header al cliente
        context.Response.Headers[CorrelationConstants.CorrelationIdHeader] = correlationId;

        using (_logger.BeginScope(new Dictionary<string, object>
        {
            [CorrelationConstants.CorrelationIdLogProperty] = correlationId,
            [CorrelationConstants.TraceIdLogProperty] = traceId
        }))
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await _next(context);

                stopwatch.Stop();

                _logger.LogInformation(
                    "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(ex,
                    "Unhandled exception for HTTP {Method} {Path} after {ElapsedMs}ms",
                    context.Request.Method,
                    context.Request.Path,
                    stopwatch.ElapsedMilliseconds);

                throw;
            }
        }
    }
}
