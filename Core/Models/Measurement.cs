namespace TempFukt.Core.Models
{
    public enum LocationType
    {
        Vardagsrum,
        Balkong,
        Ute,
        Inne
    }

    public class Measurement
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public LocationType Location { get; set; }

        public double Temperature { get; set; }

        public double Humidity { get; set; }
    }
}