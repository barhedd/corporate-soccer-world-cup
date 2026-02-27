namespace CorporateSoccerWorldCup.Application.Features.Players.Queries.Common.Dtos;

public record PlayerResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateTimeOffset Birthday { get; init; }
    public string TeamName { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public int? SanctionedMatchesRemaining { get; init; }
}
