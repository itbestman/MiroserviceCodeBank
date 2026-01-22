using Microsoft.AspNetCore.Mvc;
using PollyLearn.Extensions;

namespace PollyLearn.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly HttpClient _httpClient;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyResilientClient");
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            var response = await _httpClient.GetAsync("WeatherForecast");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.ReadContentAs<IEnumerable<WeatherForecast>>();
                return Ok(content);

            }

            return StatusCode((int)response.StatusCode, "Service is currently unavailable.");

            //var response = await _httpClient.GetAsync("http://localhost:5056/WeatherForecast");
            //return await response.ReadContentAs<IEnumerable<WeatherForecast>>();
        }
    }
}
