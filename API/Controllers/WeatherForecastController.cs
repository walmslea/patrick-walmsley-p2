using Microsoft.AspNetCore.Mvc;
using Domain;
using Persistence;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    private readonly DataContext _context;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;

    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
    [HttpPost]
    public ActionResult<WeatherForecast> Create(){
        Console.WriteLine($"Database path: {_context:DbPath}");
        Console.WriteLine("Insert a new WeatherForecast");

        var forecast = new WeatherForecast(){
            Date = new DateTime(),
            TemperatureC = 75,
            Summary = "Warm"
        };
        _context.WeatherForecasts.Add(forecast);
        var success = _context.SaveChanges() > 0;

        if (success){
            return forecast;
        }

        throw new Exception("error creating WeatherForecast");
    }
}
