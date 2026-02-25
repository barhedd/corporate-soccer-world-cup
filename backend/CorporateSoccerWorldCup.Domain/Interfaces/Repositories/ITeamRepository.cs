using CorporateSoccerWorldCup.Domain.Entities;

namespace CorporateSoccerWorldCup.Domain.Interfaces.Repositories;

public interface ITeamRepository
{
    Task AddAsync(Team team, CancellationToken cancellationToken);
}
