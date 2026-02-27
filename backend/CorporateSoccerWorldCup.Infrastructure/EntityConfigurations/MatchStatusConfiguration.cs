using CorporateSoccerWorldCup.Domain.Entities.MatchStatuses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CorporateSoccerWorldCup.Infrastructure.EntityConfigurations;

public class MatchStatusConfiguration : IEntityTypeConfiguration<MatchStatus>
{
    public void Configure(EntityTypeBuilder<MatchStatus> builder)
    {
        builder.ToTable("MatchStatuses");

        builder.HasKey(ms => ms.Id);

        builder.Property(ms => ms.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ms => ms.Status)
            .IsRequired();

        builder.HasMany(ms => ms.Matches)
            .WithOne(m => m.Status) 
            .HasForeignKey(m => m.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
