namespace ZeroSeven.TripService.Client.Models
{
    public class TellMeTheWeatherRequest
    {
        public Guid TripId { get; set; }
        public int MinTemperature { get; set; }
        public int MaxTemperature { get; set; }
        public string PreciseCode { get; set; }

    }
}
