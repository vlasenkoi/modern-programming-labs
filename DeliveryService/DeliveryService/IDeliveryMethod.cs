namespace DeliveryService;

public interface IDeliveryMethod
{
    decimal CalculateCost(double totalWeight);
}