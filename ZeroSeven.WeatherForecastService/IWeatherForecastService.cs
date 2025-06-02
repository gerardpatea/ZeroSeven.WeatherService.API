namespace ZeroSevent.WeatherForecastService
{
    public interface IWeatherForecastService
    {
        public Task<GetWeatherForecastResponse> GetWeatherForecast();
    }
}
