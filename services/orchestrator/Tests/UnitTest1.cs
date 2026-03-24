using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace Orchestrator.Tests;

public class OrchestratorTests
{
    private static WebApplicationFactory<Program> BuildFactory(
        Func<HttpRequestMessage, HttpResponseMessage> numbersHandler,
        Func<HttpRequestMessage, HttpResponseMessage> esmeHandler)
    {
        return new WebApplicationFactory<Program>().WithWebHostBuilder(host =>
        {
            host.ConfigureServices(services =>
            {
                services.ConfigureAll<HttpClientFactoryOptions>(options =>
                {
                    options.HttpMessageHandlerBuilderActions.Add(b =>
                    {
                        b.PrimaryHandler = b.Name switch
                        {
                            "numbers" => new StubHandler(numbersHandler),
                            "esme_squalor" => new StubHandler(esmeHandler),
                            _ => b.PrimaryHandler
                        };
                    });
                });
            });
        });
    }

    [Fact]
    public async Task Returns_sum_of_in_numbers()
    {
        // numbers returns 50, esme says "in" → total = 50
        var factory = BuildFactory(
            _ => Json(new { value = 50 }),
            _ => Json(new { verdict = "in" }));

        var client = factory.CreateClient();
        var response = await client.GetAsync("/result?count=1");
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(body);
        Assert.Equal(50, doc.RootElement.GetProperty("total").GetInt32());
    }

    [Fact]
    public async Task Returns_zero_when_all_out()
    {
        // numbers returns 10, esme says "out" → total = 0
        var factory = BuildFactory(
            _ => Json(new { value = 10 }),
            _ => Json(new { verdict = "out" }));

        var client = factory.CreateClient();
        var response = await client.GetAsync("/result?count=3");
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(body);
        Assert.Equal(0, doc.RootElement.GetProperty("total").GetInt32());
    }

    [Fact]
    public async Task Calls_numbers_and_esme_count_times()
    {
        var numbersCalls = 0;
        var esmeCalls = 0;

        var factory = BuildFactory(
            _ => { numbersCalls++; return Json(new { value = 99 }); },
            _ => { esmeCalls++; return Json(new { verdict = "in" }); });

        var client = factory.CreateClient();
        await client.GetAsync("/result?count=3");

        Assert.Equal(3, numbersCalls);
        Assert.Equal(3, esmeCalls);
    }

    private static HttpResponseMessage Json(object body) =>
        new(HttpStatusCode.OK)
        {
            Content = new StringContent(
                JsonSerializer.Serialize(body),
                System.Text.Encoding.UTF8,
                "application/json")
        };
}

class StubHandler(Func<HttpRequestMessage, HttpResponseMessage> handler) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken) =>
        Task.FromResult(handler(request));
}
