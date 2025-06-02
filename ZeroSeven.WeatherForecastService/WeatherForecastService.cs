using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ZeroSeven.TripService.Client;
using ZeroSeven.WillyWeather.Client;

namespace ZeroSeven.WeatherForecastService
{
    public class WeatherForecastService : IWeatherForecastService
    {
        #region Members

        private readonly IZeroSevenTripServiceClient _zeroSevenTripServiceClient;
        private readonly IWillyWeatherClient _willyWeatherClient;
        private readonly ILogger<WeatherForecastService> _logger;

        #endregion

        #region Constructor

        public WeatherForecastService(IZeroSevenTripServiceClient zeroSevenTripServiceClient,
            IWillyWeatherClient willyWeatherClient,
            ILogger<WeatherForecastService> logger
            )
        {
            _zeroSevenTripServiceClient = zeroSevenTripServiceClient;
            _willyWeatherClient = willyWeatherClient;
            _logger = logger;
        }

        #endregion

        #region IWeatherForecastService

        /// <summary>
        /// Gets the weather forecast details given a random trip details from ZeroSeven
        /// </summary>
        /// <returns></returns>
        public async Task<GetWeatherForecastResponse> GetWeatherForecast()
        {
            try
            {
                _logger.LogInformation("Get our next day trip details from ZeroSeven");
                //get the next day trip from zero seven
                var nextDayTripResponse = await _zeroSevenTripServiceClient.GetNextDayTrip();

                //using those details we'll get the weather deatils from willy API

                var locationId = nextDayTripResponse.WillyWeatherId;
                var date = nextDayTripResponse.Date.ToString("yyyy-MM-dd");

                _logger.LogInformation("Get our forecast details from Willy Weather for LocationId: {0}, Date: {1}",
                    locationId,
                    date
                    );

                var willyWeatherForecastResponse = await _willyWeatherClient.GetWeatherForecast(new GetWeatherForecastRequest
                {
                    LocationId = locationId,
                    ApiKey = nextDayTripResponse.WillyWeatherAPIKey,
                    Date = nextDayTripResponse.Date
                });

                //get our forecast for the day from the response - assuming its the first item in the lists for now
                var willyWeatherDayForecast = willyWeatherForecastResponse.Forecasts?.Weather?.Days?.FirstOrDefault()?.Entries.FirstOrDefault();

                _logger.LogInformation("Calling ZeroSeven's TellMeTheWeather");

                var tellMeTheWeatherResponse = await _zeroSevenTripServiceClient.TellMeTheWeather(new TellMeTheWeatherRequest
                {
                    TripId = nextDayTripResponse.TripId,
                    MaxTemperature = willyWeatherDayForecast.Max,
                    MinTemperature = willyWeatherDayForecast.Min,
                    PreciseCode = willyWeatherDayForecast.PrecisCode
                });

                return new GetWeatherForecastResponse { Message = tellMeTheWeatherResponse.Message };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred in WeatherForecastService");

                throw;
            }

        }

        #endregion
    }
}
