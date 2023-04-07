using MediatR;
using Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;
using Route256.Week5.Homework.PriceCalculator.Bll.Models;
using Route256.Week5.Homework.PriceCalculator.Bll.Services.Interfaces;

namespace Route256.Week5.Homework.PriceCalculator.Bll.Queries;

public record ClearHistoryQuery(
        long UserId,
        long[] CalculationIds) 
    : IRequest<ClearHistoryQueryResult>;

public class ClearHistoryQueryHandler
    : IRequestHandler<ClearHistoryQuery, ClearHistoryQueryResult>
{
    private readonly ICalculationService _calculationService;

    public ClearHistoryQueryHandler(
        ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }
    
    public async Task<ClearHistoryQueryResult> Handle(
        ClearHistoryQuery request, 
        CancellationToken cancellationToken)
    {
        var userIds = await _calculationService.GetUserIds(request.CalculationIds, cancellationToken);
        var enumerable = userIds.ToArray();

        if (enumerable.ToList().Any(x => x != request.UserId))
        {
            throw new OneOrManyCalculationsBelongsToAnotherUserException();
        }

        if (request.CalculationIds.Length != enumerable.ToList().Count())
        {
            throw new OneOrManyCalculationsNotFoundException();
        }

        if (request.CalculationIds.Length == 0)
        {
            var deletedRowsAmount = await _calculationService.DeleteAllWithUserId(request.UserId, cancellationToken);
            return new ClearHistoryQueryResult(deletedRowsAmount);
        }
        else
        {
            var deletedRowsAmount = await _calculationService.DeleteWithIds(request.CalculationIds, cancellationToken);
            return new ClearHistoryQueryResult(deletedRowsAmount);
        }
    }
}