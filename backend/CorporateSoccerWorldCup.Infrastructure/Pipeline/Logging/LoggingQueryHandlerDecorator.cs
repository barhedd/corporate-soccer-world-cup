using CorporateSoccerWorldCup.Application.Abstractions.Events;
using CorporateSoccerWorldCup.Application.Common.Results;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CorporateSoccerWorldCup.Infrastructure.Pipeline.Logging;

public class LoggingQueryHandlerDecorator<TQuery, TResponse>(
    IQueryHandler<TQuery, TResponse> inner,
    ILogger<LoggingQueryHandlerDecorator<TQuery, TResponse>> logger)
    : IQueryHandler<TQuery, TResponse>
{
    private readonly IQueryHandler<TQuery, TResponse> _inner = inner;
    private readonly ILogger<LoggingQueryHandlerDecorator<TQuery, TResponse>> _logger = logger;

    public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
    {
        var queryName = typeof(TQuery).Name;
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("Executing query {QueryName}", queryName);

        try
        {
            var result = await _inner.Handle(query, cancellationToken);

            stopwatch.Stop();

            if (result is Result r && r.Failure)
            {
                _logger.LogWarning(
                    "Query {QueryName} failed with code {ErrorCode} and message {ErrorMessage} in {ElapsedMs}ms",
                    queryName,
                    r.ErrorCode,
                    r.ErrorMessage,
                    stopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogInformation(
                    "Query {QueryName} executed successfully in {ElapsedMs}ms",
                    queryName,
                    stopwatch.ElapsedMilliseconds);
            }

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(
                ex,
                "Query {QueryName} threw exception after {ElapsedMs}ms",
                queryName,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}