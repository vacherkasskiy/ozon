using Dapper;
using Microsoft.Extensions.Options;
using Route256.Week5.Homework.PriceCalculator.Dal.Entities;
using Route256.Week5.Homework.PriceCalculator.Dal.Repositories.Interfaces;
using Route256.Week5.Homework.PriceCalculator.Dal.Settings;

namespace Route256.Week5.Homework.PriceCalculator.Dal.Repositories;

public class GoodsRepository : BaseRepository, IGoodsRepository
{
    public GoodsRepository(
        IOptions<DalOptions> dalSettings) : base(dalSettings.Value)
    {
    }

    public async Task<long[]> Add(
        GoodEntityV1[] goods,
        CancellationToken token)
    {
        const string sqlQuery = @"
insert into goods (user_id, width, height, length, weight) 
select user_id, width, height, length, weight
  from UNNEST(@Goods)
returning id;
";

        var sqlQueryParams = new
        {
            Goods = goods
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

    public async Task<GoodEntityV1[]> Query(
        long userId,
        CancellationToken token)
    {
        const string sqlQuery = @"
select id
     , user_id
     , width
     , height
     , length
     , weight
  from goods
 where user_id = @UserId;
";

        var sqlQueryParams = new
        {
            UserId = userId
        };

        await using var connection = await GetAndOpenConnection();
        var goods = await connection.QueryAsync<GoodEntityV1>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));

        return goods
            .ToArray();
    }

    public async Task<long[]> GetGoodIds(
        long[] calculationIds,
        CancellationToken token)
    {
        const string sqlQuery = @"
select unnest(good_ids)
from calculations
where id = any(@CalculationIds)
";

        var sqlQueryParams = new
        {
            CalculationIds = calculationIds
        };

        await using var connection = await GetAndOpenConnection();
        var goodIds = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));

        return goodIds
            .ToArray();
    }

    public async void CascadeDelete(
        long[] goodIds,
        CancellationToken token)
    {
        const string sqlQuery = @"
delete from goods
where id = any(@GoodIds)
";

        var sqlQueryParams = new
        {
            GoodIds = goodIds
        };

        await using var connection = await GetAndOpenConnection();
        await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));
    }
}