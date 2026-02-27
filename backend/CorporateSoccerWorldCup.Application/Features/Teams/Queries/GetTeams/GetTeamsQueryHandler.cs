using CorporateSoccerWorldCup.Application.Abstractions.Events;
using CorporateSoccerWorldCup.Application.Common.Pagination;
using CorporateSoccerWorldCup.Application.Common.Results;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.Common.Dtos;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.Common.Interfaces;

namespace CorporateSoccerWorldCup.Application.Features.Teams.Queries.GetTeams;

public class GetTeamsQueryHandler(
    ITeamReadRepository teamReadRepository) : IQueryHandler<GetTeamsQuery, PagedResult<TeamResponseDto>>
{
    private readonly ITeamReadRepository _teamReadRepository = teamReadRepository;

    public async Task<Result<PagedResult<TeamResponseDto>>> Handle(GetTeamsQuery query, CancellationToken cancellationToken)
    {
        var pagedResult = await _teamReadRepository.GetPagedAsync(query, cancellationToken);

        return Result<PagedResult<TeamResponseDto>>.Ok(pagedResult);
    }
}
