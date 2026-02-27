using CorporateSoccerWorldCup.Domain.Abstractions.Repositories;
using CorporateSoccerWorldCup.Domain.Entities.Teams;
using CorporateSoccerWorldCup.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CorporateSoccerWorldCup.Infrastructure.Persistence.Repositories;

public class TeamRepository(CorporateSoccerWorldCupContext dbContext) : ITeamRepository
{
    private readonly CorporateSoccerWorldCupContext _dbContext = dbContext;

    public async Task<bool> ExistByNameAsync(string name, CancellationToken cancellationToken) =>
        await _dbContext.Teams
            .AnyAsync(team => team.Name == name, cancellationToken);

    public async Task AddAsync(Team team, CancellationToken cancellationToken) =>
        await _dbContext.Teams.AddAsync(team, cancellationToken);

    public async Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _dbContext.Teams.FindAsync([id], cancellationToken);

    public void Delete(Team team) =>
        _dbContext.Teams.Remove(team);
}
