using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Contracts;
using WeatherManager;

namespace WeatherApp.Presentation.Controllers
{
    [Route("api/weather")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        /// <summary>
        /// Gets weather desciption of a city 
        /// </summary>
        /// <param name="country">Country</param>
        /// <param name="city">City</param>
        /// <returns>Weather description</returns>
        [HttpGet]
        public async Task<IActionResult> GetWeatherDescription(string country, string city)
        {
           var weatherDescription = await _weatherService.GetWeatherDescriptionAsync(country, city);
            return Ok(weatherDescription);
        }

        [HttpOptions]
        public IActionResult GetWeatherOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS");
            return Ok();
        }
    }

}
