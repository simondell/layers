namespace Numbers.Tests;

public class NumbersTests
{
    [Fact]
    public void Number_is_between_1_and_100()
    {
        var value = Random.Shared.Next(1, 101);
        Assert.InRange(value, 1, 100);
    }
}
