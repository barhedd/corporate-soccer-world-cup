using CorporateSoccerWorldCup.Domain.Entities.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CorporateSoccerWorldCup.Infrastructure.EntityConfigurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(t => t.ImageUrl)
            .HasMaxLength(500);

        builder.HasIndex(t => t.Name)
            .IsUnique();

        // Relationships
        builder.HasMany(t => t.Players)
            .WithOne(p => p.Team)
            .HasForeignKey(p => p.TeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.LocalMatches)
            .WithOne(m => m.LocalTeam)
            .HasForeignKey(m => m.LocalTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.GuestMatches)
            .WithOne(m => m.GuestTeam)
            .HasForeignKey(m => m.GuestTeamId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
