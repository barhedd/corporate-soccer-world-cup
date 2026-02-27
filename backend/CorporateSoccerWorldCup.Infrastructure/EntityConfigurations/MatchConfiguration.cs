using CorporateSoccerWorldCup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CorporateSoccerWorldCup.Infrastructure.EntityConfigurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("Matches");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Date)
            .IsRequired();

        builder.Property(m => m.LocalGoals);
        builder.Property(m => m.GuestGoals);

        builder.HasOne(m => m.Tournament)
            .WithMany(t => t.Matches)
            .HasForeignKey(p => p.TournamentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.LocalTeam)
            .WithMany(e => e.LocalMatches)
            .HasForeignKey(p => p.LocalTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.GuestTeam)
            .WithMany(e => e.GuestMatches)
            .HasForeignKey(p => p.GuestTeamId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
