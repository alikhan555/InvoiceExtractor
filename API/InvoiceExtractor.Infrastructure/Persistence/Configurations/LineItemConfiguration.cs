using InvoiceExtractor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceExtractor.Infrastructure.Persistence.Configurations;

public class LineItemConfiguration : IEntityTypeConfiguration<LineItem>
{
    public void Configure(EntityTypeBuilder<LineItem> builder)
    {
        builder.ToTable("line_items");
        builder.HasKey(li => li.Id);

        builder.Property(li => li.Description).HasMaxLength(1024);

        builder.Property(li => li.Quantity).HasColumnType("numeric(18,4)");
        builder.Property(li => li.UnitPrice).HasColumnType("numeric(18,4)");
        builder.Property(li => li.LineAmount).HasColumnType("numeric(18,2)");

        builder.HasIndex(li => li.InvoiceId);
    }
}
