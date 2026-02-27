using CorporateSoccerWorldCup.Api.Commons.Contracts;

namespace CorporateSoccerWorldCup.Api.Controllers.Teams.Contracts;

public sealed class GetTeamsRequest : PagedQueryRequest
{
    // filtros
    public string? Name { get; init; }
}
