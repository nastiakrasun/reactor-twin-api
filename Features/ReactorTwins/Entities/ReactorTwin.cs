namespace ReactorTwinAPI.Domain.Entities
{
    public class ReactorTwin
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string SerialNumber { get; set; } = null!;
        public string Version { get; set; } = "1.0";
        public string Status { get; set; } = "Active";
        public string ReactorType { get; set; } = null!;
        public double ThermalOutputMW { get; set; }
        public double ElectricalOutputMW { get; set; }
        public string FuelType { get; set; } = null!;
        public double CoreTemperature { get; set; }
        public double PressureLevel { get; set; }
        public string CoolingSystemType { get; set; } = null!;
        public double CurrentTemperature { get; set; }
        public double CurrentPressure { get; set; }
        public double CurrentPowerOutput { get; set; }
        public double RadiationLevel { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public Guid OwnerId { get; set; }
        public ReactorTwinAPI.Domain.Entities.User? Owner { get; set; }
    }
}
