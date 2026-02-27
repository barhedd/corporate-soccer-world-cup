namespace CorporateSoccerWorldCup.Api.Controllers.Teams.Contracts;

public record CreateTeamRequest
{
    public string Name { get; init; } = string.Empty;
    public string ImageUrl {  get; init; } = string.Empty;
}
