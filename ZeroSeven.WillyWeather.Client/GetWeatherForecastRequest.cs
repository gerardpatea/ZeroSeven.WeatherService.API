namespace ZeroSeven.WillyWeather.Client
{
    public class GetWeatherForecastRequest
    {
        public string ApiKey { get; set; }
        public int LocationId { get; set; }
        public DateTime Date { get; set; }
    }
}
