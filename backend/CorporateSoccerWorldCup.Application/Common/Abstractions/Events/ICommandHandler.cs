using CorporateSoccerWorldCup.Application.Common.Results;

namespace CorporateSoccerWorldCup.Application.Common.Abstractions.Events;

public interface ICommandHandler<TCommand>
{
    Task<Result> Handle(
        TCommand command,
        CancellationToken cancellationToken);
}

public interface ICommandHandler<TCommand, TResult>
{
    Task<Result<TResult>> Handle(
        TCommand command,
        CancellationToken cancellationToken);
}
