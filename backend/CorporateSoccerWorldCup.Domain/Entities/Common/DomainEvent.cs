namespace CorporateSoccerWorldCup.Domain.Entities.Common;

public abstract record DomainEvent(
    Guid Id,
    DateTime OccurredOn);
