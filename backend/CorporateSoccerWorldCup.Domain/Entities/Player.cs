using CorporateSoccerWorldCup.Domain.Entities.Common;
using CorporateSoccerWorldCup.Domain.Entities.PlayerStatuses;
using CorporateSoccerWorldCup.Domain.Entities.Teams;

namespace CorporateSoccerWorldCup.Domain.Entities;

public class Player : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset Birthday { get; set; }
    public Guid TeamId { get; set; }
    public Guid StatusId { get; set; }
    public Team Team { get; set; } = null!;
    public PlayerStatus Status { get; set; } = null!;
    public int? SanctionedMatchesRemaining { get; set; }
    public ICollection<Goal> Goals { get; set; } = [];
}
