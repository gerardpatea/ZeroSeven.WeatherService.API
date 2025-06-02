using RestSharp;
using System.Linq.Expressions;

namespace ZeroSeven.TripService.Client
{
    public class ZeroSevenTripServiceClient : IZeroSevenTripServiceClient
    {
        #region Members

        private readonly IRestClient _restClient;

        #endregion

        #region Constructor

        public ZeroSevenTripServiceClient(IRestClient restClient
            )
        {
            _restClient = restClient;
        }

        #endregion

        #region IZeroSevenTripServiceClient

        /// <summary>
        /// Gets the next day trip details
        /// </summary>
        /// <returns>Details of the next days trip</returns>
        public async Task<NextDayTripResponse> GetNextDayTrip()
        {
            var request = new RestRequest("/getnextdaytrip");

            var result = await _restClient.ExecuteGetAsync<NextDayTripResponse>(request);

            return result.Data;
        }

        /// <summary>
        /// Post the details of the days trips and recieve a message back
        /// </summary>
        /// <param name="tellMeTheWeatherRequest"></param>
        /// <returns>Message regarding the trips details</returns>
        public async Task<TellMeTheWeatherResponse> TellMeTheWeather(TellMeTheWeatherRequest tellMeTheWeatherRequest)
        {
            var request = new RestRequest("/tellmetheweather/{tripid}")
                .AddUrlSegment("tripid", tellMeTheWeatherRequest.TripId)
                .AddJsonBody(new
                {
                    precisCode = tellMeTheWeatherRequest.PreciseCode,
                    minTemperature = tellMeTheWeatherRequest.MinTemperature,
                    maxTemperature = tellMeTheWeatherRequest.MaxTemperature
                });

            var result = await _restClient.ExecutePostAsync<TellMeTheWeatherResponse>(request);

            return result.Data;
        }

        #endregion
    }
}
