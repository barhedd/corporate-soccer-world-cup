using CorporateSoccerWorldCup.Api.Commons.Contracts;

namespace CorporateSoccerWorldCup.Api.Controllers.Players.Contracts;

public sealed class GetPlayersRequest : PagedQueryRequest
{
    // Filters
    public string? Name { get; init; }
    public DateTimeOffset? Birthday { get; init; }
    public Guid? TeamId { get; init; }
    public Guid? StatusId { get; init; }
    public int? SanctionedMatchesRemaining { get; init; }
}
