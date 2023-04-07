namespace Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;

public class OneOrManyCalculationsNotFoundException : Exception
{
    public OneOrManyCalculationsNotFoundException()
    {
    }

    public OneOrManyCalculationsNotFoundException(string? message) : base(message)
    {
    }
}