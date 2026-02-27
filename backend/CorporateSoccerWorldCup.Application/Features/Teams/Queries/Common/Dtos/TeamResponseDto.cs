namespace CorporateSoccerWorldCup.Application.Features.Teams.Queries.Common.Dtos;

public record TeamResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string ImageUrl {  get; init; } = string.Empty;
}
