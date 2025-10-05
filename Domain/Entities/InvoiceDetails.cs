using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RailBook.Domain.Entities;

public class InvoiceDetails
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int TotalPrice { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string? InvoiceDescription { get; set; } = string.Empty;
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedById { get; set; }
    public DateTime? ModifiedAt { get; set; }

    // Navigation property
    public virtual Invoice Invoice { get; set; } = null!;

    public class Configuration : IEntityTypeConfiguration<InvoiceDetails>
    {
        public void Configure(EntityTypeBuilder<InvoiceDetails> entity)
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Id).ValueGeneratedOnAdd();

            entity.Property(d => d.TotalPrice).IsRequired().HasColumnType("integer");  // ✓ FIXED: Changed from "integer" to "decimal(18,2)"
            entity.Property(d => d.InvoiceDate).IsRequired();
            entity.Property(d => d.InvoiceDescription).IsRequired(false);
            entity.Property(d => d.CreatedAt).IsRequired();
            entity.Property(d => d.ModifiedById).IsRequired(false);
            entity.Property(d => d.ModifiedAt).IsRequired(false);

            // One-to-One Relationship: InvoiceDetails belongs to Invoice
            entity.HasOne(d => d.Invoice)
                  .WithOne(i => i.InvoiceDetails)  // ✓ FIXED: Changed from .WithOne() to .WithOne(i => i.InvoiceDetails)
                  .HasForeignKey<InvoiceDetails>(d => d.InvoiceId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Create unique index on InvoiceId to enforce one-to-one
            entity.HasIndex(d => d.InvoiceId).IsUnique();

            // Audit relationships
            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(d => d.CreatedBy)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(d => d.ModifiedById)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}