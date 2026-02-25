using CorporateSoccerWorldCup.Domain.Entities;
using CorporateSoccerWorldCup.Domain.Interfaces.Repositories;
using CorporateSoccerWorldCup.Infrastructure.Contexts;

namespace CorporateSoccerWorldCup.Infrastructure.Persistence.Repositories;

public class TeamRepository(CorporateSoccerWorldCupContext dbContext) : ITeamRepository
{
    private readonly CorporateSoccerWorldCupContext _dbContext = dbContext;

    public async Task AddAsync(Team team, CancellationToken cancellationToken)
    {
        await _dbContext.Teams.AddAsync(team, cancellationToken);
    }
}
