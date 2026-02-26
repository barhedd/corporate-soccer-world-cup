using CorporateSoccerWorldCup.Domain.Entities.Common;
using CorporateSoccerWorldCup.Domain.Entities.Teams.Events;

namespace CorporateSoccerWorldCup.Domain.Entities.Teams;

public class Team : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public ICollection<Player> Players { get; set; } = [];
    public ICollection<TournamentTeam> TournamentTeams { get; set; } = [];
    public ICollection<Match> LocalMatches { get; set; } = [];
    public ICollection<Match> GuestMatches { get; set; } = [];

    private Team() { }

    public static Team Create(string name, string imageUrl)
    {
        var team = new Team
        {
            Id = Guid.NewGuid(),
            Name = name,
            ImageUrl = imageUrl
        };

        team.AddDomainEvent(
            new TeamCreatedEvent(team.Id, team.Name, team.ImageUrl));

        return team;
    }
}
