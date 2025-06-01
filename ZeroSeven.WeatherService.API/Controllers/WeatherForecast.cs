using Microsoft.AspNetCore.Mvc;
using ZeroSeven.TripService.Client;
using ZeroSevent.WeatherForecastService;

namespace ZeroSeven.WeatherService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    #region Members

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastService _weatherForecastService;

    #endregion

    #region Constructor

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
        IWeatherForecastService weatherForecastService
        )
    {
        _logger = logger;
        _weatherForecastService = weatherForecastService;
    }

    #endregion

    #region API

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var response = await _weatherForecastService.GetWeatherForecast();

            return new OkObjectResult(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem("Something went wrong");
        }
    }

    #endregion
}
