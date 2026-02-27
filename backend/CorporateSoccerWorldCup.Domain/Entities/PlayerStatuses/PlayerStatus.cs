using CorporateSoccerWorldCup.Domain.Entities.Common;

namespace CorporateSoccerWorldCup.Domain.Entities.PlayerStatuses;

public class PlayerStatus : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public PlayerStatusEnum Status { get; set; }
    public ICollection<Player> Players { get; set; } = [];
}
