using System.Diagnostics;
using System.Text;
using CreditService.Common.DTO;
using CreditService.Common.System;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using university.Server.Exception.ExceptionsModels;

namespace WebAuth.Services;

public class MetricMiddleware
{  
    private readonly RequestDelegate _next;
    private readonly HttpClient _httpClient;

    public MetricMiddleware(RequestDelegate next,IOptions<HttpClient> httpClient)
    {
        _next = next;
        _httpClient = httpClient.Value;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("DeviceId", out var deviceId))
        {
            throw new IncorrectDataException("не передан deviceId");
        }
        var startTime = DateTimeOffset.UtcNow;
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        await _next(context);
        stopwatch.Stop();
        var endTime = DateTimeOffset.UtcNow;
        var metric = new MetricDto
        {
            ActionName = "authServer",
            Code = context.Response.StatusCode.ToString(),
            Message =" ", 
            DeviceId= deviceId.ToString(),
            StartTime =  startTime,
            EndTime = endTime
        };
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiConstants.MetricBaseUrl);
        requestMessage.Headers.Add("traceparent", Guid.NewGuid().ToString());
        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(metric), Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error sending metric");
        }
        
    }

}