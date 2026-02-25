using CorporateSoccerWorldCup.Application.Features.Teams.DataTransferObjects;

namespace CorporateSoccerWorldCup.Application.Features.Teams.Interfaces;

public interface ITeamReadRepository
{
    Task<IEnumerable<TeamDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<TeamDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
