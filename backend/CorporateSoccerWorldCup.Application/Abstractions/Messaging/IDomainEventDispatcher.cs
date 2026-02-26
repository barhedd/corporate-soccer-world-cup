using CorporateSoccerWorldCup.Domain.Entities.Common;

namespace CorporateSoccerWorldCup.Application.Abstractions.Messaging;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<DomainEvent> events, CancellationToken ct);
}
