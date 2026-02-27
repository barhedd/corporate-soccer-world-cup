namespace CorporateSoccerWorldCup.Application.Features.Teams.Commands.UpdateTeam;

public record UpdateTeamCommand
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string ImageUrl {  get; init; } = string.Empty; 
}
