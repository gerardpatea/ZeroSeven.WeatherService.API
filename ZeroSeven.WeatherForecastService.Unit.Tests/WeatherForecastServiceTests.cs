using ZeroSeven.WeatherForecastService;
using NSubstitute;
using ZeroSeven.TripService.Client;
using ZeroSeven.WillyWeather.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute.Extensions;
using ZeroSeven.TripService.Client.Models;
using ZeroSeven.WillyWeather.Client.Models;

namespace ZeroSeven.WeatherForecastService.Unit.Tests
{
    [TestClass]
    public sealed class WeatherForecastServiceTests
    {

        #region Members

        private IWeatherForecastService _weatherForecastService; 
        private TripService.Client.IZeroSevenTripServiceClient _zeroSevenTripServiceClient; 
        private IWillyWeatherClient _willyWeatherClient; 
        private ILogger<ZeroSeven.WeatherForecastService.WeatherForecastService> _logger;

        #endregion

        #region Setup

        [TestInitialize]
        public void Setup()
        {
            _zeroSevenTripServiceClient = Substitute.For<IZeroSevenTripServiceClient>();
            _willyWeatherClient = Substitute.For<IWillyWeatherClient>();
            _logger = Substitute.For<ILogger<ZeroSeven.WeatherForecastService.WeatherForecastService>>();

            _weatherForecastService = new ZeroSeven.WeatherForecastService.WeatherForecastService(
                _zeroSevenTripServiceClient,
                _willyWeatherClient,
                _logger
                );
        }

        #endregion

        #region Tests

        [TestMethod]
        public async Task GetWeatherForecast_WhenValidInputs_ReturnsSuccessMessage()
        {
            //Arrange
            var today = DateTime.Now.Date;
            var todayString = today.ToString("yyyy-MM-dd");
            var locationId = 123;
            var expectedResultMessage = "Success";

            var nextDayTripResponse = new NextDayTripResponse
            {
                City = "Hobart",
                Date = DateTime.Now.Date,
                TripId = Guid.NewGuid(),
                WillyWeatherAPIKey = "APIKey",
                WillyWeatherId = locationId
            };

            _zeroSevenTripServiceClient.GetNextDayTrip()
                .Returns(nextDayTripResponse);

            var willyWeatherResponse = new WillyWeather.Client.Models.GetWeatherForecastResponse
            {
                Forecasts = new Forecasts
                {
                    Weather = new Weather
                    {
                        Days = new List<Day>
                        {
                            new Day
                            {
                                DateTime = todayString,
                                Entries = new List<Entry>
                                {
                                    new Entry
                                    {
                                        DateTime = todayString,
                                        PrecisCode = "mostly-fine",
                                        Precis = "Mostly sunny",
                                        PrecisOverlayCode = "",
                                        Night = false,
                                        Min = 22,
                                        Max = 32
                                    }
                                }
                            }
                        }
                    }
                },

                Location = new Location
                {
                    Id = locationId,
                    Name = "Hobart"
                }
            };

            _willyWeatherClient.GetWeatherForecast(Arg.Any<GetWeatherForecastRequest>())
                .Returns(willyWeatherResponse);

            var tellMeTheWeathResponse = new TellMeTheWeatherResponse
            {
                Message = expectedResultMessage
            };

            _zeroSevenTripServiceClient.TellMeTheWeather(Arg.Any<TellMeTheWeatherRequest>())
                .Returns(tellMeTheWeathResponse);

            //Act
            var result = await _weatherForecastService.GetWeatherForecast();

            //Assert
            Assert.AreEqual(expectedResultMessage, result.Message);
        }

        #endregion

    }
}
