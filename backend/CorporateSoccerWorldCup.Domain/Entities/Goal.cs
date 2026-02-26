using CorporateSoccerWorldCup.Domain.Entities.Common;

namespace CorporateSoccerWorldCup.Domain.Entities;

public class Goal : BaseEntity
{
    public Guid MatchId { get; set; }
    public Guid PlayerId { get; set; }
    public int Minute { get; set; }
    public Match Match { get; set; } = null!;
    public Player Player { get; set; } = null!;
}
