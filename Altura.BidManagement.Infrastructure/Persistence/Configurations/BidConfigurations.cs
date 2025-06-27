using Altura.BidManagement.Domain.Bids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Altura.BidManagement.Infrastructure.Persistence.Configurations;

public class BidConfigurations : IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd();

        builder.Property(b => b.Amount)
            .IsRequired().HasPrecision(18, 2);
        
        builder.Property(b => b.Title)
            .IsRequired().HasMaxLength(100);
        
        builder.Property(b => b.State)
            .IsRequired().HasMaxLength(10);

        builder.Property(b => b.CreatedAt)
            .IsRequired();

        builder.Property(b => b.UpdatedAt)
            .IsRequired(false);
    }
}