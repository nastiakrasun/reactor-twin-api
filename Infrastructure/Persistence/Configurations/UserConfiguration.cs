using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReactorTwinAPI.Domain.Entities;

namespace ReactorTwinAPI.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(u => u.Username)
                .IsUnique();

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.IsSuperUser)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(u => u.CanCreateReactor)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasMany(u => u.ReactorTwins)
                .WithOne(rt => rt.Owner)
                .HasForeignKey(rt => rt.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
