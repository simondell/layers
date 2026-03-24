var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

app.MapGet("/verdict", (int number) =>
{
    var dayScore = DayScore(DateTime.UtcNow.DayOfWeek);
    var verdict = number > dayScore ? "in" : "out";
    return Results.Ok(new { verdict });
});

app.Run();

static int DayScore(DayOfWeek day) => day switch
{
    DayOfWeek.Monday    => 42,
    DayOfWeek.Tuesday   => 46,
    DayOfWeek.Wednesday => 32,
    DayOfWeek.Thursday  => 49,
    DayOfWeek.Friday    => 33,
    DayOfWeek.Saturday  => 40,
    DayOfWeek.Sunday    => 54,
    _ => throw new ArgumentOutOfRangeException(nameof(day))
};
