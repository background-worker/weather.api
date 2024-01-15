using Contracts;
using ExceptionHandling;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using System.Net;
using WeatherManager.Models;

namespace WeatherManager
{
    public class WeatherService : IWeatherService
    {
        private HttpClient _httpClient;
        private IConfiguration _configuration;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetWeatherDescriptionAsync(string country, string city)
        {
            if(string.IsNullOrWhiteSpace(country) || string.IsNullOrWhiteSpace(city))
                ErrorHandler.ResolveError(HttpStatusCode.BadRequest);

            var apiUrl = GetQueryUrl(country, city);

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.StatusCode != HttpStatusCode.OK)
                ErrorHandler.ResolveError(response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();

            var description = ReadDescription(content);

            if (string.IsNullOrWhiteSpace(description))
                ErrorHandler.ResolveError(HttpStatusCode.NotFound);

            return description;
        }

        private string GetQueryUrl(string country, string city)
        {
            var url = _configuration.GetValue<string>(WeatherHelper.WeatherServiceUrl);
            var appId = _configuration.GetValue<string>(WeatherHelper.WeatherApiKey);

            return $"{url}?q={country},{city}&appid={appId}";
        }

        private string ReadDescription(string content)
        {
            var jsonResponse = JsonConvert.DeserializeObject<GetWeatherResponse>(content);

            return jsonResponse?.Weather.FirstOrDefault().Description;
        }
    }
}