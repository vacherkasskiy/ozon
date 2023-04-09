using Route256.Week5.Homework.PriceCalculator.Dal.Entities;

namespace Route256.Week5.Homework.PriceCalculator.Dal.Repositories.Interfaces;

public interface IGoodsRepository : IDbRepository
{
    Task<long[]> Add(
        GoodEntityV1[] goods,
        CancellationToken token);

    Task<GoodEntityV1[]> Query(
        long userId,
        CancellationToken token);

    Task<long[]> GetGoodIds(
        long[] calculationIds,
        CancellationToken token);

    void CascadeDelete(
        long[] goodIds,
        CancellationToken token);
}