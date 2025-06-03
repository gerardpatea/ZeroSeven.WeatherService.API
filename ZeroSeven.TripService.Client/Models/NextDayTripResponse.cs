namespace ZeroSeven.TripService.Client.Models
{
    public class NextDayTripResponse
    {
        public Guid TripId { get; set; }
        public DateTime Date { get; set; }
        public string City { get; set; }
        public int WillyWeatherId { get; set; }
        public string WillyWeatherAPIKey { get; set; }
    }
}
