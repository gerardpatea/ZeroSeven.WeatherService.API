namespace ZeroSeven.WeatherService
{
    public interface IWeatherService
    {
        Task<string> GetWeatherForecast();
    }
}
