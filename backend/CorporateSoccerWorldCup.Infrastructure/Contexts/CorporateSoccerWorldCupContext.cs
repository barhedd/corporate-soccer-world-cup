using CorporateSoccerWorldCup.Application.Common.Abstractions.Messaging;
using CorporateSoccerWorldCup.Domain.Entities;
using CorporateSoccerWorldCup.Domain.Entities.Common;
using CorporateSoccerWorldCup.Domain.Entities.MatchStatuses;
using CorporateSoccerWorldCup.Domain.Entities.PlayerStatuses;
using CorporateSoccerWorldCup.Domain.Entities.Teams;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace CorporateSoccerWorldCup.Infrastructure.Contexts;

public class CorporateSoccerWorldCupContext(
    DbContextOptions<CorporateSoccerWorldCupContext> options,
    IDomainEventDispatcher dispatcher) : DbContext(options)
{
    private readonly IDomainEventDispatcher _dispatcher = dispatcher;

    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<TournamentTeam> TournamentTeams { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<MatchStatus> MatchStatuses { get; set; }
    public DbSet<PlayerStatus> PlayerStatuses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CorporateSoccerWorldCupContext).Assembly);

        // Aplicar filtro global de Soft Delete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(GetIsDeletedRestriction(entityType.ClrType));
            }
        }
    }

    private static LambdaExpression GetIsDeletedRestriction(Type type)
    {
        var parameter = Expression.Parameter(type, "e");
        var prop = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
        var condition = Expression.Equal(prop, Expression.Constant(false));
        return Expression.Lambda(condition, parameter);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;

        var entries = ChangeTracker
            .Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.SetCreated("SYSTEM", now);
                    break;

                case EntityState.Modified:
                    entry.Entity.SetEdited("SYSTEM", now);
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.MarkAsDeleted("SYSTEM", now);
                    break;
            }
        }

        var domainEntities = ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        if (domainEvents.Count != 0)
        {
            await _dispatcher.DispatchAsync(domainEvents, cancellationToken);

            foreach (var entity in domainEntities)
            {
                entity.Entity.ClearDomainEvents();
            }
        }

        return result;
    }
}
