using CorporateSoccerWorldCup.Domain.Abstractions.Repositories;
using CorporateSoccerWorldCup.Domain.Entities.PlayerStatuses;
using CorporateSoccerWorldCup.Infrastructure.Contexts;

namespace CorporateSoccerWorldCup.Infrastructure.Persistence.Repositories;

public class PlayerStatusRepository(
    CorporateSoccerWorldCupContext dbContext) : IPlayerStatusRepository
{
    private readonly CorporateSoccerWorldCupContext _dbContext = dbContext;

    public async Task<PlayerStatus?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _dbContext.PlayerStatuses.FindAsync([id], cancellationToken);
}
