// ==================== INVOICE ====================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RailBook.Domain.Entities;

public class Invoice
{
    public int Id { get; set; }
    public int BookingId { get; set; } // FK to Booking
    public int TotalAmount { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedById { get; set; }
    public DateTime? ModifiedAt { get; set; }

    // Navigation properties
    public virtual Booking Booking { get; set; } = null!;
    public virtual InvoiceDetails? InvoiceDetails { get; set; }  // ✓ ADDED: Navigation property for bi-directional access

    public class Configuration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> entity)
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.BookingId).IsRequired();
            entity.Property(i => i.TotalAmount).IsRequired();

            entity.Property(i => i.CreatedBy).IsRequired();
            entity.Property(i => i.CreatedAt).IsRequired();
            entity.Property(i => i.ModifiedById).IsRequired(false);
            entity.Property(i => i.ModifiedAt).IsRequired(false);

            // Relationship: Invoice belongs to Booking (one-to-one)
            // Configure this here because Invoice has the BookingId FK
            entity.HasOne(i => i.Booking)
                  .WithOne(b => b.Invoice)
                  .HasForeignKey<Invoice>(i => i.BookingId)
                  .OnDelete(DeleteBehavior.Restrict);

            // ✓ NO configuration for Invoice ↔ InvoiceDetails here!
            // That relationship is configured in InvoiceDetails.cs (it has the FK)

            // Audit relationships
            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(i => i.CreatedBy)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(i => i.ModifiedById)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}