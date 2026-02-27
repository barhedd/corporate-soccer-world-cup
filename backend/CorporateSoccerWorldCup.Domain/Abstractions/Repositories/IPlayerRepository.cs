using CorporateSoccerWorldCup.Domain.Entities;

namespace CorporateSoccerWorldCup.Domain.Abstractions.Repositories;

public interface IPlayerRepository
{
    Task AddAsync(Player team, CancellationToken cancellationToken);
}
