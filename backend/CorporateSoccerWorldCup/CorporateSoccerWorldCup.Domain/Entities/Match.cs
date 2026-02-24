namespace CorporateSoccerWorldCup.Domain.Entities;

public class Match : BaseEntity
{
    public Guid TournamentId { get; set; }
    public Guid LocalTeamId { get; set; }
    public Guid GuestTeamId { get; set; }
    public int LocalGoals { get; set; }
    public int GuestGoals { get; set; }
    public DateTimeOffset Date {  get; set; }
    public Tournament Tournament { get; set; } = null!;
    public Team LocalTeam { get; set; } = null!;
    public Team GuestTeam { get; set; } = null!;
    public ICollection<Goal> Goals { get; set; } = [];
}
