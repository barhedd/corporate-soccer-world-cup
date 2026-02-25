using CorporateSoccerWorldCup.Application.Common.Pagination;
using CorporateSoccerWorldCup.Application.Features.Teams.DataTransferObjects;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.GetTeams;

namespace CorporateSoccerWorldCup.Application.Features.Teams.Interfaces;

public interface ITeamReadRepository
{
    Task<PagedResult<TeamDto>> GetPagedAsync(GetTeamsQuery query, CancellationToken cancellationToken);
    Task<TeamDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
