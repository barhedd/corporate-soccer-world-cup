using CorporateSoccerWorldCup.Application.Common.Results;

namespace CorporateSoccerWorldCup.Application.Common.Interfaces;

public interface ICommandHandler<TCommand, TResult>
{
    Task<Result<TResult>> Handle(
        TCommand command,
        CancellationToken cancellationToken);
}
