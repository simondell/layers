var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

builder.Services.AddHttpClient("numbers", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services__Numbers__BaseUrl"] ?? "http://localhost:5001");
});

builder.Services.AddHttpClient("esme_squalor", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services__EsmeSqualor__BaseUrl"] ?? "http://localhost:5002");
});

var app = builder.Build();

app.MapGet("/result", async (int count, IHttpClientFactory httpClientFactory) =>
{
    var numbers = httpClientFactory.CreateClient("numbers");
    var esme = httpClientFactory.CreateClient("esme_squalor");

    var numberTasks = Enumerable.Range(0, count)
        .Select(_ => numbers.GetFromJsonAsync<NumberResponse>("/number"));

    var numberResponses = await Task.WhenAll(numberTasks);

    var verdictTasks = numberResponses
        .Select(r => esme.GetFromJsonAsync<VerdictResponse>($"/verdict?number={r!.Value}"));

    var verdicts = await Task.WhenAll(verdictTasks);

    var total = numberResponses
        .Zip(verdicts, (n, v) => (number: n!.Value, verdict: v!.Verdict))
        .Where(x => x.verdict == "in")
        .Sum(x => x.number);

    return Results.Ok(new { total });
});

app.Run();

record NumberResponse(int Value);
record VerdictResponse(string Verdict);
