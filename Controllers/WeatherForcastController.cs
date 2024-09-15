using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForcastController : ControllerBase
    {
        private readonly string[] _summaries = new[]
        {
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching",
        };

        [HttpGet("", Name = "GetWeatherForcast")]
        public IEnumerable<WeatherForecast> GetFiveDayForcast()
        {
            var forecast = Enumerable
                .Range(1, 5)
                .Select(index => new WeatherForecast(
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    _summaries[Random.Shared.Next(_summaries.Length)]
                ))
                .ToArray();
            return forecast;
        }
    }

    public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
