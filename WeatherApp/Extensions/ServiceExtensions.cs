using Authorisation;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading.RateLimiting;

namespace WeatherApp.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {
            });

        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            IApiKeyValidation apiKeyValidation = services.BuildServiceProvider().GetRequiredService<IApiKeyValidation>();

            services.AddRateLimiter(opt =>
            {
                opt.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    if (context.Request.Query.ContainsKey(AuthorisationHelper.ApiKeyQueryParameter))
                    {
                        var userApiKey = context.Request.Query[AuthorisationHelper.ApiKeyQueryParameter].ToString();
                        if (!string.IsNullOrWhiteSpace(userApiKey) && apiKeyValidation.IsApiKeyValid(userApiKey))
                        {
                            return RateLimitPartition.GetFixedWindowLimiter(userApiKey, partition => new FixedWindowRateLimiterOptions
                            {
                                AutoReplenishment = true,
                                PermitLimit = 5,
                                QueueLimit = 0,
                                Window = TimeSpan.FromHours(1)
                            });
                        }
                    }

                    return RateLimitPartition.GetFixedWindowLimiter("Anonymous", partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 0,
                        QueueLimit = 0,
                        Window = TimeSpan.FromHours(1)
                    });
                });
                              

                opt.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                        await context.HttpContext.Response
                        .WriteAsync($"Only 5 requests are allowed per hour. Please try again after { retryAfter.TotalMinutes} minute(s).", token);
                    else
                        await context.HttpContext.Response
                        .WriteAsync("Only 5 requests are allowed per hour. Please try again later.", token);
                };
            });
        }
    }
}
