using CorporateSoccerWorldCup.Domain.Entities;
using CorporateSoccerWorldCup.Domain.Interfaces.Repositories;
using CorporateSoccerWorldCup.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CorporateSoccerWorldCup.Infrastructure.Persistence.Repositories;

public class TeamRepository(CorporateSoccerWorldCupContext dbContext) : ITeamRepository
{
    private readonly CorporateSoccerWorldCupContext _dbContext = dbContext;

    public async Task<bool> ExistByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.Teams
            .AnyAsync(team => team.Name == name);
    }

    public async Task AddAsync(Team team, CancellationToken cancellationToken)
    {
        await _dbContext.Teams.AddAsync(team, cancellationToken);
    }
}
