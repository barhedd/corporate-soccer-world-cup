using CorporateSoccerWorldCup.Domain.Entities.Common;
using CorporateSoccerWorldCup.Domain.Entities.Teams;

namespace CorporateSoccerWorldCup.Domain.Entities;

public class TournamentTeam : BaseEntity
{
    public Guid TournamentId { get; set; }
    public Guid TeamId { get; set; }
    public Tournament Tournament { get; set; } = null!;
    public Team Team { get; set; } = null!;
}
