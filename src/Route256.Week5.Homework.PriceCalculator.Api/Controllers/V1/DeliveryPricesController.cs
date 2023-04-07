using MediatR;
using Microsoft.AspNetCore.Mvc;
using Route256.Week5.Homework.PriceCalculator.Api.Requests.V1;
using Route256.Week5.Homework.PriceCalculator.Api.Responses.V1;
using Route256.Week5.Homework.PriceCalculator.Bll.Commands;
using Route256.Week5.Homework.PriceCalculator.Bll.Models;
using Route256.Week5.Homework.PriceCalculator.Bll.Queries;
using Route256.Week5.Homework.PriceCalculator.Dal.Repositories.Interfaces;

namespace Route256.Week5.Homework.PriceCalculator.Api.Controllers.V1;

[ApiController]
[Route("/v1/delivery-prices")]
public class DeliveryPricesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICalculationRepository _repository;

    public DeliveryPricesController(
        IMediator mediator,
        ICalculationRepository repository)
    {
        _mediator = mediator;
        _repository = repository;
    }
    
    /// <summary>
    /// Метод расчета стоимости доставки на основе объема товаров
    /// или веса товара. Окончательная стоимость принимается как наибольшая
    /// </summary>
    /// <returns></returns>
    [HttpPost("calculate")]
    public async Task<CalculateResponse> Calculate(
        CalculateRequest request,
        CancellationToken ct)
    {
        var command = new CalculateDeliveryPriceCommand(
            request.UserId,
            request.Goods
                .Select(x => new GoodModel(
                    x.Height,
                    x.Length,
                    x.Width,
                    x.Weight))
                .ToArray());
        var result = await _mediator.Send(command, ct);
        
        return new CalculateResponse(
            result.CalculationId,
            result.Price);
    }
    
    
    /// <summary>
    /// Метод получения истории вычисления
    /// </summary>
    /// <returns></returns>
    [HttpPost("get-history")]
    public async Task<GetHistoryResponse[]> History(
        GetHistoryRequest request,
        CancellationToken ct)
    {
        var query = new GetCalculationHistoryQuery(
            request.UserId,
            request.Take,
            request.Skip);
        var result = await _mediator.Send(query, ct);

        return result.Items
            .Select(x => new GetHistoryResponse(
                new GetHistoryResponse.CargoResponse(
                    x.Volume,
                    x.Weight,
                    x.GoodIds),
                x.Price))
            .ToArray();
    }

    [HttpPost("clear-history")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> ClearHistory(
        ClearHistoryRequest request,
        CancellationToken ct)
    {
        _repository.DeleteWithIds(request.CalculationIds, ct);
        return StatusCode(200);
    }
}