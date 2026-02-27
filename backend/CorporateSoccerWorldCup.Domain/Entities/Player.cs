using CorporateSoccerWorldCup.Domain.Entities.Common;
using CorporateSoccerWorldCup.Domain.Entities.PlayerStatuses;
using CorporateSoccerWorldCup.Domain.Entities.Teams;
using System.Text.Json.Serialization;

namespace CorporateSoccerWorldCup.Domain.Entities;

public class Player : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset Birthday { get; set; }
    public Guid TeamId { get; set; }
    public Guid StatusId { get; set; }
    public int? SanctionedMatchesRemaining { get; set; }
    public Team Team { get; set; } = null!;
    public PlayerStatus Status { get; set; } = null!;
    public ICollection<Goal> Goals { get; set; } = [];

    [JsonConstructor]
    private Player() { }

    public static Player Create(
        string name, 
        DateTimeOffset birthday,
        Guid teamId,
        Guid statusId,
        int? SanctionedMatchesRemaining)
    {
        var player = new Player
        {
            Name = name,
            Birthday = birthday,
            TeamId = teamId,
            StatusId = statusId,
            SanctionedMatchesRemaining = SanctionedMatchesRemaining
        };

        return player;
    }
}
