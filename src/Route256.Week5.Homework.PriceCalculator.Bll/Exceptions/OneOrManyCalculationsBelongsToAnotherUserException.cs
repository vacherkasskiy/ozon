using System.Runtime.Serialization;

namespace Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;

public class OneOrManyCalculationsBelongsToAnotherUserException : Exception
{
    public long[] CalculationIds;
    public OneOrManyCalculationsBelongsToAnotherUserException(long[] calculationIds)
    {
        CalculationIds = calculationIds;
    }
}