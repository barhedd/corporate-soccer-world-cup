using CorporateSoccerWorldCup.Api.Commons.Correlation;
using OpenTelemetry.Trace;
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
        // 1Obtener o generar CorrelationId
        var correlationId =
            context.Request.Headers[CorrelationConstants.CorrelationIdHeader]
                .FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        // Obtener TraceId real si existe Activity (OpenTelemetry)
        var activity = Activity.Current;

        var traceId =
            activity?.TraceId.ToString()
            ?? context.TraceIdentifier;

        // Guardar en HttpContext
        context.Items[CorrelationConstants.CorrelationIdItemKey] = correlationId;
        context.Items[CorrelationConstants.TraceIdItemKey] = traceId;

        // Devolver CorrelationId al cliente
        context.Response.Headers[CorrelationConstants.CorrelationIdHeader] = correlationId;

        // Enlazar CorrelationId al span actual
        if (activity is not null)
        {
            activity.SetTag("correlation.id", correlationId);
            activity.SetTag("trace.id", traceId);
        }

        using (_logger.BeginScope(new Dictionary<string, object>
        {
            [CorrelationConstants.CorrelationIdLogProperty] = correlationId,
            [CorrelationConstants.TraceIdLogProperty] = traceId
        }))
        {
            try
            {
                await _next(context);

                _logger.LogInformation(
                    "HTTP {Method} {Path} responded {StatusCode}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode);
            }
            catch (Exception ex)
            {
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                activity?.AddException(ex);

                _logger.LogError(ex,
                    "Unhandled exception for HTTP {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                throw;
            }
        }
    }
}