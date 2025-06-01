using Microsoft.Extensions.Caching.Memory;
using RestSharp;
using RestSharp.Authenticators;
using ZeroSeven.TripService.Client;
using ZeroSeven.WillyWeather.Client;
using ZeroSevent.WeatherForecastService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IZeroSevenTripServiceClient>(x =>
{
    var baseUrl = builder.Configuration.GetValue<string>("ZeroSevenTripServiceBaseUrl");
    var username = builder.Configuration.GetValue<string>("ZeroSevenTripServiceUsername");
    var password = builder.Configuration.GetValue<string>("ZeroSevenTripServicePassword");

    var options = new RestClientOptions(baseUrl)
    {
        Authenticator = new HttpBasicAuthenticator(username, password)
    };

    var restSharpClient = new RestClient(options);

    return new ZeroSevenTripServiceClient(restSharpClient);
});

builder.Services.AddScoped<IWillyWeatherClient>(x =>
{
    var baseUrl = builder.Configuration.GetValue<string>("WillyWeatherBaseUrl");

    var restSharpClient = new RestClient(baseUrl);

    var memoryCache = x.GetService<IMemoryCache>();

    return new WillyWeatherClient(restSharpClient, memoryCache);
});

builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
