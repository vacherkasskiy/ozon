namespace Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;

public class OneOrManyCalculationsBelongsToAnotherUserException : Exception
{
    public OneOrManyCalculationsBelongsToAnotherUserException()
    {
    }

    public OneOrManyCalculationsBelongsToAnotherUserException(string? message) : base(message)
    {
    }
}