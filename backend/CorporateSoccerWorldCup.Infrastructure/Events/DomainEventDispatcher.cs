using CorporateSoccerWorldCup.Application.Abstractions.Messaging;
using CorporateSoccerWorldCup.Domain.Entities.Common;
using Microsoft.Extensions.Logging;

namespace CorporateSoccerWorldCup.Infrastructure.Events;

public sealed class DomainEventDispatcher(
    ILogger<DomainEventDispatcher> logger) : IDomainEventDispatcher
{
    private readonly ILogger<DomainEventDispatcher> _logger = logger;

    public Task DispatchAsync(
        IEnumerable<DomainEvent> events,
        CancellationToken ct)
    {
        foreach (var domainEvent in events)
        {
            _logger.LogInformation(
                "Domain event dispatched {@DomainEvent}",
                domainEvent);
        }

        return Task.CompletedTask;
    }
}