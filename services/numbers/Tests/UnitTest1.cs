using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Numbers.Tests;

public class NumbersTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Number_endpoint_returns_value_between_1_and_100()
    {
        var response = await _client.GetAsync("/number");
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(body);
        var value = doc.RootElement.GetProperty("value").GetInt32();

        Assert.InRange(value, 1, 100);
    }
}
