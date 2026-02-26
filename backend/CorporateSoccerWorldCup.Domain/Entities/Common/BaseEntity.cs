using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateSoccerWorldCup.Domain.Entities.Common;

public abstract class BaseEntity
{
    [NotMapped]
    private readonly List<DomainEvent> _domainEvents = [];

    public Guid Id { get; protected set; }

    public bool IsDeleted { get; protected set; }

    public DateTimeOffset CreatedDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    public DateTimeOffset? EditedDate { get; set; }
    public string? EditedBy { get; set; }

    public DateTimeOffset? DeletedDate { get; set; }
    public string? DeletedBy { get; set; }

    public void SetCreated(string createdBy, DateTimeOffset now)
    {
        CreatedDate = now;
        CreatedBy = createdBy;
    }

    public void SetEdited(string editedBy, DateTimeOffset now)
    {
        EditedDate = now;
        EditedBy = editedBy;
    }

    public void MarkAsDeleted(string deletedBy, DateTimeOffset now)
    {
        IsDeleted = true;
        DeletedDate = now;
        DeletedBy = deletedBy;
    }

    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents
        => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(DomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}
