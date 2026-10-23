namespace DeliveryService;

public class ExpressDelivery : IDeliveryMethod
{
    public decimal CalculateCost(double totalWeight)
    {
        return 120m + (decimal)totalWeight * 25m;
    }
}