using InvoiceExtractor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceExtractor.Infrastructure.Persistence.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("invoices");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.VendorName).HasMaxLength(512);
        builder.Property(i => i.InvoiceNumber).HasMaxLength(128);
        builder.Property(i => i.Currency).HasMaxLength(16);

        // Date-only fields — stored as `date` so DateTimeKind is irrelevant.
        builder.Property(i => i.InvoiceDate).HasColumnType("date");
        builder.Property(i => i.DueDate).HasColumnType("date");

        builder.Property(i => i.Subtotal).HasColumnType("numeric(18,2)");
        builder.Property(i => i.Tax).HasColumnType("numeric(18,2)");
        builder.Property(i => i.Total).HasColumnType("numeric(18,2)");

        builder.Property(i => i.OriginalFilePath).HasMaxLength(1024).IsRequired();
        builder.Property(i => i.OriginalFileName).HasMaxLength(512).IsRequired();

        builder.Property(i => i.CreatedAt).HasColumnType("timestamp with time zone");

        builder.HasMany(i => i.LineItems)
            .WithOne(li => li.Invoice!)
            .HasForeignKey(li => li.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
