namespace CorporateSoccerWorldCup.Domain.Entities;

public class Tournament : BaseEntity
{
    public string Name { get; set; } = null!;
    public DateTimeOffset StartDate { get; set; }
    public ICollection<TournamentTeam> TournamentTeams { get; set; } = [];
    public ICollection<Match> Matches { get; set; } = [];
}
