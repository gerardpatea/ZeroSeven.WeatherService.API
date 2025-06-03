using ZeroSeven.WeatherForecastService.Models;

namespace ZeroSeven.WeatherForecastService
{
    public interface IWeatherForecastService
    {
        public Task<GetWeatherForecastResponse> GetWeatherForecast();
    }
}
