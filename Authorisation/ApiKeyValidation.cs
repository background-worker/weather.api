using Contracts;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorisation
{
    public class ApiKeyValidation : IApiKeyValidation
    {
        private readonly IConfiguration _configuration;
        public ApiKeyValidation(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool IsApiKeyValid(string userApiKey)
        {
            if (string.IsNullOrWhiteSpace(userApiKey))
                return false;

            var apiKeyCollection = _configuration.GetSection(AuthorisationHelper.ApiKeyCollection);
            if (apiKeyCollection == null || !GetApiKeyCollection(apiKeyCollection).Where(x => x == userApiKey).Any())
                return false;

            return true;
        }

        private static IEnumerable<string> GetApiKeyCollection(IConfigurationSection apiKeyCollection)
        {
            return new List<string>
            {
                apiKeyCollection.GetValue<string>(AuthorisationHelper.ApiKey1),
                apiKeyCollection.GetValue<string>(AuthorisationHelper.ApiKey2),
                apiKeyCollection.GetValue<string>(AuthorisationHelper.ApiKey3),
                apiKeyCollection.GetValue<string>(AuthorisationHelper.ApiKey4),
                apiKeyCollection.GetValue<string>(AuthorisationHelper.ApiKey5),
            };
        }
    }
}
