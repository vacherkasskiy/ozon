using Moq;
using Route256.Week5.Homework.PriceCalculator.Bll.Queries;
using Route256.Week5.Homework.PriceCalculator.Bll.Services.Interfaces;

namespace Route256.Week5.Homework.PriceCalculator.UnitTests.Stubs;

public class GetCalculationHistoryHandlerStub : GetCalculationHistoryQueryHandler
{
    public GetCalculationHistoryHandlerStub(
        Mock<ICalculationService> calculationService)
        : base(
            calculationService.Object)
    {
        CalculationService = calculationService;
    }

    public Mock<ICalculationService> CalculationService { get; }

    public void VerifyNoOtherCalls()
    {
        CalculationService.VerifyNoOtherCalls();
    }
}