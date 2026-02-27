using CorporateSoccerWorldCup.Application.Common.Abstractions.Events;
using CorporateSoccerWorldCup.Application.Common.Results;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CorporateSoccerWorldCup.Infrastructure.Pipelines.Logging;

public class LoggingQueryHandlerDecorator<TQuery, TResponse>(
    IQueryHandler<TQuery, TResponse> inner,
    ILogger<LoggingQueryHandlerDecorator<TQuery, TResponse>> logger)
    : IQueryHandler<TQuery, TResponse>
{
    private static readonly ActivitySource ActivitySource =
        new("CorporateSoccerWorldCup.Application");

    private readonly IQueryHandler<TQuery, TResponse> _inner = inner;
    private readonly ILogger<LoggingQueryHandlerDecorator<TQuery, TResponse>> _logger = logger;

    public async Task<Result<TResponse>> Handle(
        TQuery query,
        CancellationToken cancellationToken)
    {
        var queryName = typeof(TQuery).Name;

        using var activity = ActivitySource.StartActivity(
            queryName,
            ActivityKind.Internal);

        if (activity is not null)
        {
            activity.SetTag("handler.type", "query");
            activity.SetTag("handler.name", queryName);
            activity.SetTag("request.type", typeof(TQuery).FullName);

            var correlationId = Activity.Current?.GetTagItem("correlation.id")?.ToString();
            if (!string.IsNullOrWhiteSpace(correlationId))
            {
                activity.SetTag("correlation.id", correlationId);
            }
        }

        _logger.LogInformation("Executing query {QueryName}", queryName);

        try
        {
            var result = await _inner.Handle(query, cancellationToken);

            if (result is Result r && r.Failure)
            {
                activity?.SetStatus(ActivityStatusCode.Error, r.ErrorMessage);
                activity?.SetTag("error.code", r.ErrorCode);
                activity?.SetTag("error.message", r.ErrorMessage);

                _logger.LogWarning(
                    "Query {QueryName} failed with code {ErrorCode} and message {ErrorMessage}",
                    queryName,
                    r.ErrorCode,
                    r.ErrorMessage);
            }
            else
            {
                activity?.SetStatus(ActivityStatusCode.Ok);

                _logger.LogInformation(
                    "Query {QueryName} executed successfully",
                    queryName);
            }

            return result;
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.AddException(ex);

            _logger.LogError(
                ex,
                "Query {QueryName} threw exception",
                queryName);

            throw;
        }
    }
}