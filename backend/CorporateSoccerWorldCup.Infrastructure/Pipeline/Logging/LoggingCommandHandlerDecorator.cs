using CorporateSoccerWorldCup.Application.Abstractions.Events;
using CorporateSoccerWorldCup.Application.Common.Results;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CorporateSoccerWorldCup.Infrastructure.Pipeline.Logging;

public class LoggingCommandHandlerDecorator<TCommand, TResponse>(
    ICommandHandler<TCommand, TResponse> inner,
    ILogger<LoggingCommandHandlerDecorator<TCommand, TResponse>> logger)
    : ICommandHandler<TCommand, TResponse>
{
    private readonly ICommandHandler<TCommand, TResponse> _inner = inner;
    private readonly ILogger<LoggingCommandHandlerDecorator<TCommand, TResponse>> _logger = logger;

    public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
    {
        var commandName = typeof(TCommand).Name;
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("Executing command {CommandName}", commandName);

        try
        {
            var result = await _inner.Handle(command, cancellationToken);

            stopwatch.Stop();

            if (result is Result r && r.Failure)
            {
                _logger.LogWarning(
                    "Command {CommandName} failed with code {ErrorCode} and message {ErrorMessage} in {ElapsedMs}ms",
                    commandName,
                    r.ErrorCode,
                    r.ErrorMessage,
                    stopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogInformation(
                    "Command {CommandName} executed successfully in {ElapsedMs}ms",
                    commandName,
                    stopwatch.ElapsedMilliseconds);
            }

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(
                ex,
                "Command {CommandName} threw exception after {ElapsedMs}ms",
                commandName,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}