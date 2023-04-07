namespace Route256.Week5.Homework.PriceCalculator.Api.Requests.V1;

public record ClearHistoryRequest(int UserId, int[] CalculationIds);