using Route256.Week5.Homework.PriceCalculator.Dal.Entities;
using Route256.Week5.Homework.PriceCalculator.Dal.Models;

namespace Route256.Week5.Homework.PriceCalculator.Dal.Repositories.Interfaces;

public interface ICalculationRepository : IDbRepository
{
    Task<long[]> Add(
        CalculationEntityV1[] entityV1, 
        CancellationToken token);

    Task<CalculationEntityV1[]> Query(
        CalculationHistoryQueryModel query,
        CancellationToken token);

    Task<IEnumerable<int>> GetUserIds(
        int[] calculationIds,
        CancellationToken token);
    
    void DeleteWithIds(
        int[] calculationIds,
        CancellationToken token);

    void DeleteAllWithUserId(
        int userId,
        CancellationToken token);
}