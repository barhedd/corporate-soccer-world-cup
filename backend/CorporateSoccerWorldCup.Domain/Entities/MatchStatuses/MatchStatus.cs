using CorporateSoccerWorldCup.Domain.Entities.Common;

namespace CorporateSoccerWorldCup.Domain.Entities.MatchStatuses;

public class MatchStatus : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public MatchStatusEnum Status { get; set; }
    public ICollection<Match> Matches { get; set; } = [];
}
