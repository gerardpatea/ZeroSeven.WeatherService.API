namespace ZeroSeven.WillyWeather.Client
{
    public interface IWillyWeatherClient
    {
        Task<GetWeatherForecastResponse> GetWeatherForecast(GetWeatherForecastRequest getWeatherForecastRequest);
    }
}
