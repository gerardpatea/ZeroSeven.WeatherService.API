namespace ZeroSeven.TripService.Client
{
    public interface IZeroSevenTripServiceClient
    {
        Task<NextDayTripResponse> GetNextDayTrip();
        Task<TellMeTheWeatherResponse> TellMeTheWeather(TellMeTheWeatherRequest tellMeTheWeatherRequest);

    }
}
