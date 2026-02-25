using CorporateSoccerWorldCup.Application.Common.Pagination;

namespace CorporateSoccerWorldCup.Application.Features.Teams.Queries.GetTeams;

public sealed class GetTeamsQuery : PagedQuery
{
    // Filters
    public string? Name { get; init; }
}
