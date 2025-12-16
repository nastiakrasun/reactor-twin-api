namespace ReactorTwinAPI.Features.ReactorTwins.Dtos
{
    public class CreateReactorTwinDto
    {
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
    }
}
