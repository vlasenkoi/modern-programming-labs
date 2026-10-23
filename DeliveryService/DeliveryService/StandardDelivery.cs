namespace DeliveryService;

public class StandardDelivery : IDeliveryMethod
{
    public decimal CalculateCost(double totalWeight)
    {
        return 50m + (decimal)totalWeight * 10m;
    }
}