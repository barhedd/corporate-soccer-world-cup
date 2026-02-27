using CorporateSoccerWorldCup.Domain.Entities.PlayerStatuses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CorporateSoccerWorldCup.Infrastructure.EntityConfigurations;

public class PlayerStatusConfiguration : IEntityTypeConfiguration<PlayerStatus>
{
    public void Configure(EntityTypeBuilder<PlayerStatus> builder)
    {
        builder.ToTable("PlayerStatuses");

        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ps => ps.Status)
            .IsRequired();

        builder.HasMany(ps => ps.Players)
            .WithOne(p => p.Status)
            .HasForeignKey(p => p.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
