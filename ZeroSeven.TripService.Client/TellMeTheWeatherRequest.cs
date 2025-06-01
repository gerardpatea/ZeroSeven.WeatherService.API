namespace ZeroSeven.TripService.Client
{
    public class TellMeTheWeatherRequest
    {
        public Guid TripId { get; set; }
        public int MinTemperature { get; set; }
        public int MaxTemperature { get; set; }
        public string PreciseCode { get; set; }

    }
}
