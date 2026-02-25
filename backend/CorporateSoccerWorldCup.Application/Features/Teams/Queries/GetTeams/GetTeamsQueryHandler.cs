using CorporateSoccerWorldCup.Application.Common.Errors;
using CorporateSoccerWorldCup.Application.Common.Interfaces;
using CorporateSoccerWorldCup.Application.Common.Results;
using CorporateSoccerWorldCup.Application.Features.Teams.DataTransferObjects;
using CorporateSoccerWorldCup.Application.Features.Teams.Interfaces;

namespace CorporateSoccerWorldCup.Application.Features.Teams.Queries.GetTeams;

public class GetTeamsQueryHandler(
    ITeamReadRepository teamReadRepository) : IQueryHandler<GetTeamsQuery, IEnumerable<TeamDto>>
{
    private readonly ITeamReadRepository _teamReadRepository = teamReadRepository;

    public async Task<Result<IEnumerable<TeamDto>>> Handle(GetTeamsQuery query, CancellationToken cancellationToken)
    {
        var teams = await _teamReadRepository.GetAllAsync(cancellationToken);

        if (!teams.Any())
            return Result<IEnumerable<TeamDto>>.Fail(
                "Teams not found",
                ErrorCodes.NotFound);

        return Result<IEnumerable<TeamDto>>.Ok(teams);
    }
}
