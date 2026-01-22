using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.Extensions.Http;
using PollyLearn;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
string uristring = builder.Configuration["ApiSettings:Weather"] ?? "";

#region POLICIES

    // Retry Policy
    var retryPolicy = HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(
            3,
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            (outcome, timespan, retryAttempt, context) =>
            {
                Console.WriteLine($"Retry {retryAttempt} after {timespan.TotalSeconds}s");
            });

    // Timeout Policy
    var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(5);

    // Circuit Breaker Policy
    var circuitBreakerPolicy = HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 3,
            durationOfBreak: TimeSpan.FromSeconds(30),
            onBreak: (result, timespan) =>
            {
                Console.WriteLine("Circuit OPEN");
            },
            onReset: () =>
            {
                Console.WriteLine("Circuit CLOSED");
            });

    // Fallback Policy
    var fallbackPolicy = Policy<HttpResponseMessage>
        .Handle<Exception>()
        .OrResult(r => !r.IsSuccessStatusCode)
        .FallbackAsync(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("[{\"summary\":\"Fallback data\"}]")
        });

    // Bulkhead Policy
    var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(
        maxParallelization: 2,
        maxQueuingActions: 2,
        onBulkheadRejectedAsync: context =>
        {
            Console.WriteLine("Bulkhead limit reached");
            return Task.CompletedTask;
        });

#endregion







// 2. Register a Named HttpClient with the Policy
builder.Services.AddHttpClient("MyResilientClient", client =>
{
    client.BaseAddress = new Uri(uristring);
})
.AddPolicyHandler(fallbackPolicy)
.AddPolicyHandler(bulkheadPolicy)
.AddPolicyHandler(retryPolicy)
.AddPolicyHandler(timeoutPolicy)
.AddPolicyHandler(circuitBreakerPolicy);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(2000));
}