using Dapper;
using Microsoft.Extensions.Options;
using Route256.Week5.Homework.PriceCalculator.Dal.Entities;
using Route256.Week5.Homework.PriceCalculator.Dal.Models;
using Route256.Week5.Homework.PriceCalculator.Dal.Repositories.Interfaces;
using Route256.Week5.Homework.PriceCalculator.Dal.Settings;

namespace Route256.Week5.Homework.PriceCalculator.Dal.Repositories;

public class CalculationRepository : BaseRepository, ICalculationRepository
{
    public CalculationRepository(
        IOptions<DalOptions> dalSettings) : base(dalSettings.Value)
    {
    }

    public async Task<long[]> Add(
        CalculationEntityV1[] entityV1,
        CancellationToken token)
    {
        const string sqlQuery = @"
insert into calculations (user_id, good_ids, total_volume, total_weight, price, at)
select user_id, good_ids, total_volume, total_weight, price, at
  from UNNEST(@Calculations)
returning id;
";

        var sqlQueryParams = new
        {
            Calculations = entityV1
        };

        await using var connection = await GetAndOpenConnection();
        var ids = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));

        return ids
            .ToArray();
    }

    public async Task<CalculationEntityV1[]> Query(
        CalculationHistoryQueryModel query,
        CancellationToken token)
    {
        const string sqlQuery = @"
select id
     , user_id
     , good_ids
     , total_volume
     , total_weight
     , price
     , at
  from calculations
 where user_id = @UserId
 order by at desc
 limit @Limit offset @Offset
";

        var sqlQueryParams = new
        {
            query.UserId,
            query.Limit,
            query.Offset
        };

        await using var connection = await GetAndOpenConnection();
        var calculations = await connection.QueryAsync<CalculationEntityV1>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));

        return calculations
            .ToArray();
    }

    public async Task<bool> CheckCalculationsExistence(
        long[] calculationIds,
        CancellationToken token)
    {
        const string sqlQuery = @"
select id from calculations
where id = any(@CalculationIds)
";
        var sqlQueryParams = new
        {
            CalculationIds = calculationIds
        };

        await using var connection = await GetAndOpenConnection();
        var calculationIdsArray = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));

        return calculationIdsArray.ToArray().Length == calculationIds.Length;
    }

    public async Task<long[]> CheckUserAccess(
        long userId,
        long[] calculationIds,
        CancellationToken token)
    {
        const string sqlQuery = @"
select id from calculations
where id = any(@CalculationIds)
and user_id != @UserId
";
        var sqlQueryParams = new
        {
            CalculationIds = calculationIds,
            UserId = userId
        };

        await using var connection = await GetAndOpenConnection();
        var wrongCalculationIds = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));

        return wrongCalculationIds.ToArray();
    }

    public async void DeleteWithIdsAndUserId(
        long userId,
        long[] calculationIds,
        CancellationToken token)
    {
        var sqlQuery = @"
delete from calculations
where user_id = @UserId
";
        if (calculationIds.Length > 0) sqlQuery += "\nand id = any(@CalculationIds)";
        var sqlQueryParams = new
        {
            CalculationIds = calculationIds,
            UserId = userId
        };

        await using var connection = await GetAndOpenConnection();
        var result = await connection.QueryAsync<string>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));
    }
}