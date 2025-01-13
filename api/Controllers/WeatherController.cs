using Microsoft.AspNetCore.Mvc;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        // GET: api/weather
        [HttpGet]
        public IActionResult GetWeather()
        {
            return Ok(new { Temperature = 25, Condition = "Sunny" });
        }

        // POST: api/weather
        [HttpPost]
        public IActionResult AddWeather([FromBody] WeatherViewModel weather)
        {
            return Created("", weather);
        }
    }
}
