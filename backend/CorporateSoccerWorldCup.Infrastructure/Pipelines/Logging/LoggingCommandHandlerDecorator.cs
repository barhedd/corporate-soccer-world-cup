using CorporateSoccerWorldCup.Application.Abstractions.Events;
using CorporateSoccerWorldCup.Application.Common.Results;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CorporateSoccerWorldCup.Infrastructure.Pipelines.Logging;
public class LoggingCommandHandlerDecorator<TCommand, TResponse>(
    ICommandHandler<TCommand, TResponse> inner,
    ILogger<LoggingCommandHandlerDecorator<TCommand, TResponse>> logger)
    : ICommandHandler<TCommand, TResponse>
{
    private static readonly ActivitySource ActivitySource =
        new("CorporateSoccerWorldCup.Application");

    private readonly ICommandHandler<TCommand, TResponse> _inner = inner;
    private readonly ILogger<LoggingCommandHandlerDecorator<TCommand, TResponse>> _logger = logger;

    public async Task<Result<TResponse>> Handle(
        TCommand command,
        CancellationToken cancellationToken)
    {
        var commandName = typeof(TCommand).Name;

        using var activity = ActivitySource.StartActivity(
            commandName,
            ActivityKind.Internal);

        if (activity is not null)
        {
            activity.SetTag("handler.type", "command");
            activity.SetTag("handler.name", commandName);
            activity.SetTag("request.type", typeof(TCommand).FullName);

            var correlationId = Activity.Current?.GetTagItem("correlation.id")?.ToString();
            if (!string.IsNullOrWhiteSpace(correlationId))
            {
                activity.SetTag("correlation.id", correlationId);
            }
        }

        _logger.LogInformation("Executing command {CommandName}", commandName);

        try
        {
            var result = await _inner.Handle(command, cancellationToken);

            if (result is Result r && r.Failure)
            {
                activity?.SetStatus(ActivityStatusCode.Error, r.ErrorMessage);
                activity?.SetTag("error.code", r.ErrorCode);
                activity?.SetTag("error.message", r.ErrorMessage);

                _logger.LogWarning(
                    "Command {CommandName} failed with code {ErrorCode} and message {ErrorMessage}",
                    commandName,
                    r.ErrorCode,
                    r.ErrorMessage);
            }
            else
            {
                activity?.SetStatus(ActivityStatusCode.Ok);

                _logger.LogInformation(
                    "Command {CommandName} executed successfully",
                    commandName);
            }

            return result;
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.AddException(ex);

            _logger.LogError(
                ex,
                "Command {CommandName} threw exception",
                commandName);

            throw;
        }
    }
}