using Authorisation;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherApp.Extensions;
using WeatherManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddControllers().AddApplicationPart(typeof(WeatherApp.Presentation.AssemblyReference).Assembly);
builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidation>();
builder.Services.ConfigureRateLimitingOptions();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.ConfigureExceptionHandler();

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseMiddleware<ApiKeyMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseRateLimiter();
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
