using CorporateSoccerWorldCup.Application.Common.Results;

namespace CorporateSoccerWorldCup.Application.Common.Abstractions.Events;

public interface IQueryHandler<TQuery, TResult>
{
    Task<Result<TResult>> Handle(
        TQuery query,
        CancellationToken cancellationToken);
}
