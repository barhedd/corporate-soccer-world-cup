namespace CorporateSoccerWorldCup.Application.Features.Teams.Commands.DeleteTeam;

public record DeleteTeamCommand
{
    public Guid Id { get; init; }
}
