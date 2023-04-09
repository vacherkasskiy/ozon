using MediatR;
using Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;
using Route256.Week5.Homework.PriceCalculator.Bll.Services.Interfaces;

namespace Route256.Week5.Homework.PriceCalculator.Bll.Commands;

public record ClearHistoryCommand(
        long UserId,
        long[] CalculationIds)
    : IRequest<Unit>;

public class ClearHistoryCommandHandler
    : IRequestHandler<ClearHistoryCommand, Unit>
{
    private readonly ICalculationService _calculationService;

    public ClearHistoryCommandHandler(
        ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }

    public async Task<Unit> Handle(
        ClearHistoryCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _calculationService.CheckCalculationsExistence(
                request.CalculationIds,
                cancellationToken))
            throw new OneOrManyCalculationsNotFoundException();

        var wrongCalculationIds = await _calculationService.CheckUserAccess(
            request.UserId,
            request.CalculationIds,
            cancellationToken);

        if (request.CalculationIds.Length > 0
            && wrongCalculationIds.Length > 0)
            throw new OneOrManyCalculationsBelongsToAnotherUserException(wrongCalculationIds);

        var goodIds = await _calculationService.GetGoodIds(
            request.CalculationIds,
            cancellationToken);
        _calculationService.CascadeDelete(
            goodIds,
            cancellationToken);
        _calculationService.DeleteWithIdsAndUserId(
            request.UserId,
            request.CalculationIds,
            cancellationToken);

        return Unit.Value;
    }
}