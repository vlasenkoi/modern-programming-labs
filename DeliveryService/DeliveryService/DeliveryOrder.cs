using System.Diagnostics.Metrics;

namespace DeliveryService;

public class DeliveryOrder
{
    private readonly List<Parcel> _parcels = [];

    public Guid Id { get; } = Guid.NewGuid();
    public Customer Customer { get; }
    public Courier? AssignedCourier { get; private set; }
    public DeliveryStatus Status { get; private set; } = DeliveryStatus.Created;
    public IDeliveryMethod DeliveryMethod { get; }
    public DeliveryAddress DestinationAddress { get; }

    public IReadOnlyCollection<Parcel> Parcels => _parcels.AsReadOnly();

    public DeliveryOrder(Customer customer, IDeliveryMethod deliveryMethod, DeliveryAddress destinationAddress)
    {
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        DeliveryMethod = deliveryMethod ?? throw new ArgumentNullException(nameof(deliveryMethod));
        DestinationAddress = destinationAddress ?? throw new ArgumentNullException(nameof(destinationAddress));
    }

    public void AddParcel(Parcel parcel)
    {
        if (Status != DeliveryStatus.Created)
            throw new InvalidOperationException("Cannot add parcels to an active or completed order.");

        _parcels.Add(parcel ?? throw new ArgumentNullException(nameof(parcel)));
    }

    public void AssignCourier(Courier courier)
    {
        if (Status == DeliveryStatus.Cancelled || Status == DeliveryStatus.Delivered)
            throw new InvalidOperationException("Cannot assign courier to a completed or cancelled order.");

        AssignedCourier = courier ?? throw new ArgumentNullException(nameof(courier));
        Status = DeliveryStatus.CourierAssigned;
    }

    public void StartDelivery()
    {
        if (Status != DeliveryStatus.CourierAssigned)
            throw new InvalidOperationException("Delivery can only start after a courier is assigned.");

        if (_parcels.Count == 0)
            throw new InvalidOperationException("Cannot start delivery without parcels.");

        Status = DeliveryStatus.InTransit;
    }

    public void CompleteDelivery()
    {
        if (Status != DeliveryStatus.InTransit)
            throw new InvalidOperationException("Only orders in transit can be delivered.");

        Status = DeliveryStatus.Delivered;
    }

    public void Cancel()
    {
        if (Status == DeliveryStatus.Delivered)
            throw new InvalidOperationException("Delivered order cannot be cancelled.");

        Status = DeliveryStatus.Cancelled;
    }

    public double GetTotalWeight() => _parcels.Sum(p => p.Weight);

    public decimal GetTotalCost() => DeliveryMethod.CalculateCost(GetTotalWeight());

    public string GetDeliveryPriorityDescription() => (Status, DeliveryMethod) switch
    {
        (DeliveryStatus.InTransit, ExpressDelivery) => "Priority Delivery (Express in Transit)",
        (DeliveryStatus.InTransit, StandardDelivery) => "Standard Delivery in Transit",
        (DeliveryStatus.Delivered, _) => "Completed Delivery",
        (DeliveryStatus.Cancelled, _) => "Cancelled Delivery",
        _ => "Pending / Processing"
    };
}
