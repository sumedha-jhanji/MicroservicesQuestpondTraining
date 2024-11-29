// See https://aka.ms/new-console-template for more information
using Polly;
using PollyResiliency;

Console.WriteLine("Hello, World!");

//defining policy using polly

////Retry Pattern
Console.WriteLine("Retry Pattern");
var policyRetryPattern = Policy
                    .Handle<Exception>()
                    // if we want to handle specifix exceptions thn we can write as
                    // .Handle<DividebyZeroException>()
                    //.Or<AggregateException>()
                    .WaitAndRetry(5, x => TimeSpan.FromSeconds(5));// it will try for 5 time sand after every try it will wait for 5 seconds
policyRetryPattern.Execute(() => PollyExample.SimulateOperation());

////Circuit breaker pattern
Console.WriteLine("Circuit breaker pattern\n");
var policyCircuitBreakerPattern = Policy
                    .Handle<Exception>()
                    .CircuitBreaker(3, TimeSpan.FromSeconds(10)); // first parameter is exceptions allowed before breaking, second parameter is duration of pause

for (int i = 0; i < 10; i++)
{
    try
    {
        Console.WriteLine("\nRetry " + i);
        policyCircuitBreakerPattern.Execute(() => PollyExample.SimulateOperation());
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

////Fallback Pattern
Console.WriteLine("Fallback Pattern");
var policyFallbackPattern = Policy
                    .Handle<Exception>()
                    .Fallback(() => PollyExample.SimulateOperationFallback());
policyFallbackPattern.Execute(() => PollyExample.SimulateOperation());


//for http calls, how to use polly for Request messages
var httpRequest = new HttpClient();
var policyBuilderRequest = Policy
                    .Handle<HttpRequestException>()
                    .WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromSeconds(1),// first it will retry after 1 sec, then after 3 secs, 9 seconds.
                        TimeSpan.FromSeconds(3),
                        TimeSpan.FromSeconds(9),
                        TimeSpan.FromSeconds(10),
                    },
                    (exception, retrycount) => // to track somedata if retry happens (calllback function)
                    {
                        Console.WriteLine($"{exception.Message}. " + $" Retry Count = {retrycount}");
                    });

await policyBuilderRequest.ExecuteAsync(() => httpRequest.GetAsync("https://localhost:7113/api/Values"));// http calls are async in nature

//for http calls, how to use polly for Response messages
var httpResponse = new HttpClient();
var policyBuilderResponse = Policy
                    .HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode) // if status code is not 200
                    .WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromSeconds(1),// first it will retry after 1 sec, then after 3 secs, 9 seconds.
                        TimeSpan.FromSeconds(3),
                        TimeSpan.FromSeconds(9),
                        TimeSpan.FromSeconds(10),
                    },
                    (result, timeSpan, retryCount, context) => // to track somedata if retry happens (calllback function)
                    {
                        Console.WriteLine($"Failed Request with {result.Result.StatusCode}.");
                    });

var response = await policyBuilderResponse.ExecuteAsync(() => httpResponse.GetAsync("https://localhost:7113/api/Values"));// http calls are async in nature
if (response.IsSuccessStatusCode)
    Console.WriteLine("success");
else
    Console.WriteLine("failure");


Console.ReadLine();
