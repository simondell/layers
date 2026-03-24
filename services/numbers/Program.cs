var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

app.MapGet("/number", () =>
{
    var value = Random.Shared.Next(1, 101);
    return Results.Ok(new { value });
});

app.Run();
