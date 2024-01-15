using System.Net.Http;
using System.Net;
using Contracts;
using Microsoft.AspNetCore.Http;
using ExceptionHandling;

namespace Authorisation
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApiKeyValidation _apiKeyValidation;
        public ApiKeyMiddleware(RequestDelegate next, IApiKeyValidation apiKeyValidation)
        {
            _next = next;
            _apiKeyValidation = apiKeyValidation;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Query.ContainsKey(AuthorisationHelper.ApiKeyQueryParameter))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ErrorHandler.ResolveError(HttpStatusCode.BadRequest);
            }    

            var userApiKey = context.Request.Query[AuthorisationHelper.ApiKeyQueryParameter].ToString();
            
            if (string.IsNullOrWhiteSpace(userApiKey) || !_apiKeyValidation.IsApiKeyValid(userApiKey))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                ErrorHandler.ResolveError(HttpStatusCode.Unauthorized);
            }
            await _next(context);
        }
    }
}