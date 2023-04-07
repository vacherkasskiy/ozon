using FluentValidation;
using Route256.Week5.Homework.PriceCalculator.Api.Requests.V1;

namespace Route256.Week5.Homework.PriceCalculator.Api.Validators.V1;

public class ClearHistoryValidator : AbstractValidator<ClearHistoryRequest>
{
    public ClearHistoryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
    }
}