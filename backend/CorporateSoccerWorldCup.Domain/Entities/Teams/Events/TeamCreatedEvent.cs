using CorporateSoccerWorldCup.Domain.Entities.Common;

namespace CorporateSoccerWorldCup.Domain.Entities.Teams.Events;

public sealed record TeamCreatedEvent(
    Guid TeamId,
    string Name,
    string imageUrl)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
