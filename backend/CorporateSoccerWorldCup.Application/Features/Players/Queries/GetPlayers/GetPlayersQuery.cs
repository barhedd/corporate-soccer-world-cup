using CorporateSoccerWorldCup.Application.Common.Pagination;

namespace CorporateSoccerWorldCup.Application.Features.Players.Queries.GetPlayers;

public sealed class GetPlayersQuery : PagedQuery
{
    // Filters
    public string? Name { get; init; }
    public DateTimeOffset? Birthday { get; init; }
    public Guid? TeamId { get; init; }
    public Guid? StatusId { get; init; }  
    public int? SanctionedMatchesRemaining { get; init; }
}
