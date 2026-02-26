using CorporateSoccerWorldCup.Domain.Entities.Common;
using CorporateSoccerWorldCup.Domain.Entities.Teams;

namespace CorporateSoccerWorldCup.Domain.Entities;

public class Player : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset Birthday { get; set; }
    public Guid TeamId { get; set; }
    public Team Team { get; set; } = null!;
    public ICollection<Goal> Goals { get; set; } = [];
}
