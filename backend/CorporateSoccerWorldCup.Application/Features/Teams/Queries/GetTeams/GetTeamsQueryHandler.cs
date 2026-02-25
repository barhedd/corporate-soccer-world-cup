using CorporateSoccerWorldCup.Application.Features.Teams.DataTransferObjects;
using CorporateSoccerWorldCup.Application.Features.Teams.Interfaces;
using CorporateSoccerWorldCup.Application.Interfaces;

namespace CorporateSoccerWorldCup.Application.Features.Teams.Queries.GetTeams;

public class GetTeamsQueryHandler(
    ITeamReadRepository teamReadRepository) : IQueryHandler<GetTeamsQuery, IEnumerable<TeamDto>>
{
    private readonly ITeamReadRepository _teamReadRepository = teamReadRepository;

    public async Task<IEnumerable<TeamDto>> Handle(GetTeamsQuery query, CancellationToken cancellationToken)
    {
        return await _teamReadRepository.GetAllAsync(cancellationToken);
    }
}
