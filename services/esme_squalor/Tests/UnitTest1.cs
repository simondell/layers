using Microsoft.AspNetCore.Mvc.Testing;

namespace EsmeSqualor.Tests;

public class VerdictTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Number_99_is_always_in()
    {
        var response = await _client.GetAsync("/verdict?number=99");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"in\"", body);
    }

    [Fact]
    public async Task Number_1_is_always_out()
    {
        var response = await _client.GetAsync("/verdict?number=1");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"out\"", body);
    }
}
