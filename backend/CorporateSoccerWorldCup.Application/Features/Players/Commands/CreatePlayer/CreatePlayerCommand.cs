namespace CorporateSoccerWorldCup.Application.Features.Players.Commands.CreatePlayer;

public record CreatePlayerCommand
{
    public string Name { get; init; } = string.Empty;
    public DateTimeOffset Birthday { get; init; }
    public Guid TeamId { get; init; }
    public Guid StatusId { get; init; }
    public int? SanctionedMatchesRemaining { get; init; }
}
