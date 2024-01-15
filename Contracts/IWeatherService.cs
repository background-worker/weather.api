namespace Contracts
{
    public interface IWeatherService
    {
        Task<string> GetWeatherDescriptionAsync(string country, string city);
    }
}