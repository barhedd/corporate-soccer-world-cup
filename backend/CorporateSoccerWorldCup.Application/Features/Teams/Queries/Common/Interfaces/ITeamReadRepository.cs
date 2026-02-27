using CorporateSoccerWorldCup.Application.Common.Pagination;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.Common.Dtos;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.GetTeams;

namespace CorporateSoccerWorldCup.Application.Features.Teams.Queries.Common.Interfaces;

public interface ITeamReadRepository
{
    Task<PagedResult<TeamResponseDto>> GetPagedAsync(GetTeamsQuery query, CancellationToken cancellationToken);
    Task<TeamResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
