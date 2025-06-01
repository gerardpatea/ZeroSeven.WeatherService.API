namespace ZeroSeven.WillyWeather.Client
{
    public class GetWeatherForecastResponse
    {
        public Location Location { get; set; }
        public Forecasts Forecasts { get; set; }
    }
}
