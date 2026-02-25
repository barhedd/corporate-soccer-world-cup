namespace CorporateSoccerWorldCup.Application.Features.Teams.DataTransferObjects;

public record TeamDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string ImageUrl {  get; init; } = string.Empty;
}
