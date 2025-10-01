using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RailBook.Core.Domain.Entities
{
    // ==================== USER ====================
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // No navigation properties needed for audit-only relationships

        public class Configuration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> entity)
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedOnAdd();

                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Password).IsRequired();

                entity.Property(u => u.CreatedBy).IsRequired(false);
                entity.Property(u => u.CreatedAt).IsRequired();
                entity.Property(u => u.ModifiedById).IsRequired(false);
                entity.Property(u => u.ModifiedAt).IsRequired(false);

                // Self-referencing relationships
                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(u => u.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(u => u.ModifiedById)
                      .OnDelete(DeleteBehavior.Restrict);
            }
        }
    }


}
