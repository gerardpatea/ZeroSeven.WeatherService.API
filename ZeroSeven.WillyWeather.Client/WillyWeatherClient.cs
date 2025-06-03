using Microsoft.Extensions.Caching.Memory;
using Polly;
using RestSharp;
using ZeroSeven.WillyWeather.Client.Models;

namespace ZeroSeven.WillyWeather.Client
{

    public class WillyWeatherClient : IWillyWeatherClient
    {
        #region Members

        private readonly IRestClient _restClient;
        private readonly IMemoryCache _memoryCache;
        private const string GET_WEATHER_DATE_FORMAT = "yyyy-MM-dd";
        private const int _RETRY_COUNT = 3;
        private TimeSpan _RETRY_INTERVAL = TimeSpan.FromSeconds(1);

        #endregion

        #region Constructor

        public WillyWeatherClient(IRestClient restClient, IMemoryCache memoryCache)
        {
            _restClient = restClient;
            _memoryCache = memoryCache;
        }

        #endregion

        #region IWillyWeatherClient

        /// <summary>
        /// Gets the weather forecast for a given day and locationId
        /// </summary>
        /// <param name="getWeatherForecastRequest"></param>
        /// <returns>Weather forecast summary</returns>
        public async Task<GetWeatherForecastResponse> GetWeatherForecast(GetWeatherForecastRequest getWeatherForecastRequest)
        {
            var cacheKey = $"GetWeatherForecast:{getWeatherForecastRequest.LocationId}:{getWeatherForecastRequest.Date.ToString("yyyy-MM-dd")}";

            //Lets check our cache first to save on API calls

            if (!_memoryCache.TryGetValue(cacheKey, out GetWeatherForecastResponse item))
            {
                var date = getWeatherForecastRequest.Date.ToString(GET_WEATHER_DATE_FORMAT);

                var payload = "{\"startDate\":\"#date\",\"forecasts\":[\"weather\"],\"days\":1}"
                    .Replace("#date", date);

                var request = new RestRequest("{apikey}/locations/{locationid}/weather.json")
                    .AddUrlSegment("apikey", getWeatherForecastRequest.ApiKey)
                    .AddUrlSegment("locationid", getWeatherForecastRequest.LocationId)
                    .AddHeader("Content-Type", "application/json")
                    .AddHeader("x-payload", payload);

                var result = await Policy
                    .HandleResult<RestResponse<GetWeatherForecastResponse>>(x => !x.IsSuccessful)
                    .WaitAndRetryAsync(_RETRY_COUNT, count => _RETRY_INTERVAL)
                    .ExecuteAsync(async () =>
                    {
                        return await _restClient.ExecuteGetAsync<GetWeatherForecastResponse>(request);
                    });

                //Expire the cache if it doesnt get touched within 5minutes
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                if (result != null && result.IsSuccessful)
                {
                    item = _memoryCache.Set(cacheKey, result.Data, cacheOptions);
                }
            }

            return item;

        }

        #endregion
    }
}
