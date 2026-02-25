using CorporateSoccerWorldCup.Domain.Entities;

namespace CorporateSoccerWorldCup.Domain.Interfaces.Repositories;

public interface ITeamRepository
{
    Task<bool> ExistByNameAsync(string name, CancellationToken cancellationToken);
    Task AddAsync(Team team, CancellationToken cancellationToken);
}
