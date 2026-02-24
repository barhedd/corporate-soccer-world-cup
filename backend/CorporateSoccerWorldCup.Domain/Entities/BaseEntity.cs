namespace CorporateSoccerWorldCup.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset? EditedDate { get; set; }
    public string? EditedBy { get; set; }
    public DateTimeOffset? DeletedDate { get; set; }
    public string? DeletedBy { get; set; }
}
