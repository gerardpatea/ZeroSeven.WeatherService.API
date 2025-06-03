using ZeroSeven.WillyWeather.Client.Models;

namespace ZeroSeven.WillyWeather.Client
{
    public interface IWillyWeatherClient
    {
        Task<GetWeatherForecastResponse> GetWeatherForecast(GetWeatherForecastRequest getWeatherForecastRequest);
    }
}
