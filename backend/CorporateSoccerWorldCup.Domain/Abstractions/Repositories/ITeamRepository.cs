using CorporateSoccerWorldCup.Domain.Entities.Teams;

namespace CorporateSoccerWorldCup.Domain.Abstractions.Repositories;

public interface ITeamRepository
{
    Task<bool> ExistByNameAsync(string name, CancellationToken cancellationToken);
    Task AddAsync(Team team, CancellationToken cancellationToken);
    Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    void Delete(Team team);   
}
