namespace Route256.Week5.Homework.TestingInfrastructure.Creators;

public static class Create
{
    private static long _counter = DateTime.UtcNow.Ticks;
    private static readonly Random StaticRandom = new();

    public static long RandomId()
    {
        return Interlocked.Increment(ref _counter);
    }

    public static double RandomDouble()
    {
        return StaticRandom.NextDouble();
    }

    public static decimal RandomDecimal()
    {
        return (decimal) StaticRandom.NextDouble();
    }
}