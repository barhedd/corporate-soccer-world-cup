namespace CorporateSoccerWorldCup.Domain.Entities;

public class Team : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public ICollection<Player> Players { get; set; } = [];
    public ICollection<TournamentTeam> TournamentTeams { get; set; } = [];
    public ICollection<Match> LocalMatches { get; set; } = [];
    public ICollection<Match> GuestMatches { get; set; } = [];
}
