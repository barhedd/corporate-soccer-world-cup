namespace CorporateSoccerWorldCup.Application.Features.Teams.Commands.CreateTeam;

public record CreateTeamCommand
{
    public string Name { get; init; } = string.Empty;
    public string ImageUrl {  get; init; } = string.Empty;
}
