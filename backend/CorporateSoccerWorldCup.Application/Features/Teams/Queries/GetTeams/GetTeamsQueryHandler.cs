using CorporateSoccerWorldCup.Application.Abstractions.Events;
using CorporateSoccerWorldCup.Application.Common.Errors;
using CorporateSoccerWorldCup.Application.Common.Pagination;
using CorporateSoccerWorldCup.Application.Common.Results;
using CorporateSoccerWorldCup.Application.Features.Teams.DataTransferObjects;
using CorporateSoccerWorldCup.Application.Features.Teams.Interfaces;

namespace CorporateSoccerWorldCup.Application.Features.Teams.Queries.GetTeams;

public class GetTeamsQueryHandler(
    ITeamReadRepository teamReadRepository) : IQueryHandler<GetTeamsQuery, PagedResult<TeamDto>>
{
    private readonly ITeamReadRepository _teamReadRepository = teamReadRepository;

    public async Task<Result<PagedResult<TeamDto>>> Handle(GetTeamsQuery query, CancellationToken cancellationToken)
    {
        var pagedResult = await _teamReadRepository.GetPagedAsync(query, cancellationToken);

        return Result<PagedResult<TeamDto>>.Ok(pagedResult);
    }
}
