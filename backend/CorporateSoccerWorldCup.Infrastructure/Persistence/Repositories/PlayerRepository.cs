using CorporateSoccerWorldCup.Domain.Abstractions.Repositories;
using CorporateSoccerWorldCup.Domain.Entities;
using CorporateSoccerWorldCup.Infrastructure.Contexts;

namespace CorporateSoccerWorldCup.Infrastructure.Persistence.Repositories;

public class PlayerRepository(
    CorporateSoccerWorldCupContext dbContext) : IPlayerRepository
{
    private readonly CorporateSoccerWorldCupContext _dbContext = dbContext;

    public async Task AddAsync(Player team, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(team, cancellationToken);
    }
}
