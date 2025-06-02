
using RestSharp;

namespace ZeroSeven.WeatherForecastService.Client
{
    public class ZeroSevenWeatherForecastClient : IWeatherForecastService
    {
        #region Members

        private readonly IRestClient _restClient;

        #endregion

        #region Constructor

        public ZeroSevenWeatherForecastClient(IRestClient restSharp)
        {
            _restClient = restSharp;
        }

        #endregion

        #region IWeatherForecastService

        public async Task<GetWeatherForecastResponse> GetWeatherForecast()
        {
            var request = new RestRequest("/weatherforecast");

            var result = await _restClient.ExecuteGetAsync<GetWeatherForecastResponse>(request);

            return result.Data;
        }

        #endregion

    }
}
