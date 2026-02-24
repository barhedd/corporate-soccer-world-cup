using CorporateSoccerWorldCup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CorporateSoccerWorldCup.Infrastructure.EntityConfigurations;

public class GoalConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.ToTable("Goals");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Minute)
            .IsRequired();

        builder.HasOne(g => g.Match)
            .WithMany(m => m.Goals)
            .HasForeignKey(g => g.MatchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(g => g.Player)
            .WithMany(p => p.Goals)
            .HasForeignKey(g => g.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
