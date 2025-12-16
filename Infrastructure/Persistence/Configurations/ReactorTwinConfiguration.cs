using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReactorTwinAPI.Domain.Entities;

namespace ReactorTwinAPI.Infrastructure.Persistence.Configurations
{
    public class ReactorTwinConfiguration : IEntityTypeConfiguration<ReactorTwin>
    {
        public void Configure(EntityTypeBuilder<ReactorTwin> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Model)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.SerialNumber)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Version)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ReactorType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.FuelType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.CoolingSystemType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.ThermalOutputMW);
            builder.Property(x => x.ElectricalOutputMW);
            builder.Property(x => x.CoreTemperature);
            builder.Property(x => x.PressureLevel);
            builder.Property(x => x.CurrentTemperature);
            builder.Property(x => x.CurrentPressure);
            builder.Property(x => x.CurrentPowerOutput);
            builder.Property(x => x.RadiationLevel);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.HasOne(r => r.Owner)
                .WithMany(u => u.ReactorTwins)
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
