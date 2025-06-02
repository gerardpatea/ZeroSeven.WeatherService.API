using Microsoft.Extensions.Caching.Memory;
using RestSharp;

namespace ZeroSeven.WillyWeather.Client
{

    public class WillyWeatherClient : IWillyWeatherClient
    {
        #region Members

        private readonly IRestClient _restClient;
        private readonly IMemoryCache _memoryCache;
        private const string GET_WEATHER_DATE_FORMAT = "yyyy-MM-dd";

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

                var result = await _restClient.ExecuteGetAsync<GetWeatherForecastResponse>(request);

                //Expire the cache if it doesnt get touched within 5minutes
                
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                item = _memoryCache.Set(cacheKey, result.Data, cacheOptions);
            }

            return item;

        }

        #endregion
    }
}
