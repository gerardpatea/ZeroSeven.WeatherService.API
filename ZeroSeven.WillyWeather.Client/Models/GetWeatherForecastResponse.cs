namespace ZeroSeven.WillyWeather.Client.Models
{
    public class GetWeatherForecastResponse
    {
        public Location Location { get; set; }
        public Forecasts Forecasts { get; set; }
    }
}
