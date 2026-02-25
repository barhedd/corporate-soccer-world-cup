namespace CorporateSoccerWorldCup.Application.Interfaces;

public interface ICommandHandler<TCommand, TResult>
{
    Task<TResult> Handle(
        TCommand command,
        CancellationToken cancellationToken);
}
