using CorporateSoccerWorldCup.Application.Common.Abstractions.Events;
using CorporateSoccerWorldCup.Application.Common.Errors;
using CorporateSoccerWorldCup.Application.Common.Results;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.Common.Dtos;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.Common.Interfaces;

namespace CorporateSoccerWorldCup.Application.Features.Teams.Queries.GetTeamById;

public class GetTeamByIdQueryHandler(
    ITeamReadRepository teamReadRepository) : IQueryHandler<GetTeamByIdQuery, TeamResponseDto>
{
    private readonly ITeamReadRepository _teamReadRepository = teamReadRepository;

    public async Task<Result<TeamResponseDto>> Handle(GetTeamByIdQuery query, CancellationToken cancellationToken)
    {
        var team = await _teamReadRepository.GetByIdAsync(query.Id, cancellationToken);

        if (team is null)
            return Result<TeamResponseDto>.Fail(
                "Team not found",
                ErrorCodes.NotFound);

        return Result<TeamResponseDto>.Ok(team);
    }
}
