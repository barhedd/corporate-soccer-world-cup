using CorporateSoccerWorldCup.Domain.Entities.PlayerStatuses;

namespace CorporateSoccerWorldCup.Domain.Abstractions.Repositories;

public interface IPlayerStatusRepository
{
    Task<PlayerStatus?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
