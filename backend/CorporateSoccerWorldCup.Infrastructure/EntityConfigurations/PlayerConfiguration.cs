using CorporateSoccerWorldCup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CorporateSoccerWorldCup.Infrastructure.EntityConfigurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("Players");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.Birthday)
            .IsRequired();

        builder.Property(p => p.SanctionedMatchesRemaining);

        builder.HasOne(p => p.Team)
            .WithMany(t => t.Players)
            .HasForeignKey(p => p.TeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Goals)
            .WithOne(g => g.Player)
            .HasForeignKey(g => g.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
