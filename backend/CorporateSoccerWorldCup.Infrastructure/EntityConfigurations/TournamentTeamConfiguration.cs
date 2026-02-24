using CorporateSoccerWorldCup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CorporateSoccerWorldCup.Infrastructure.EntityConfigurations;

public class TournamentTeamConfiguration : IEntityTypeConfiguration<TournamentTeam>
{
    public void Configure(EntityTypeBuilder<TournamentTeam> builder)
    {
        builder.ToTable("TournamentTeams");

        builder.HasKey(tt => new { tt.TournamentId, tt.TeamId });

        builder.HasOne(tt => tt.Tournament)
            .WithMany(t => t.TournamentTeams)
            .HasForeignKey(tt => tt.TournamentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(tt => tt.Team)
            .WithMany(e => e.TournamentTeams)
            .HasForeignKey(tt => tt.TeamId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
