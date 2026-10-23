namespace DeliveryService;

public class DeliveryOrder
{
    private readonly List<Parcel> _parcels = [];

    public Guid Id { get; } = Guid.NewGuid();
    public Customer Customer { get; }
    public Courier? AssignedCourier { get; private set; }
    public DeliveryStatus Status { get; private set; } = DeliveryStatus.Created;
    public IDeliveryMethod DeliveryMethod { get; }

    public IReadOnlyCollection<Parcel> Parcels => _parcels.AsReadOnly();

    public DeliveryOrder(Customer customer, IDeliveryMethod deliveryMethod)
    {
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        DeliveryMethod = deliveryMethod ?? throw new ArgumentNullException(nameof(deliveryMethod));
    }

    public void AddParcel(Parcel parcel)
    {
        if (Status != DeliveryStatus.Created)
            throw new InvalidOperationException("Cannot add parcels to an order that is in progress or completed.");

        _parcels.Add(parcel ?? throw new ArgumentNullException(nameof(parcel)));
    }

    public void AssignCourier(Courier courier)
    {
        if (Status == DeliveryStatus.Cancelled)
            throw new InvalidOperationException("Cannot assign courier to a cancelled order.");

        if (Status == DeliveryStatus.Completed)
            throw new InvalidOperationException("Cannot assign courier to a completed order.");

        AssignedCourier = courier ?? throw new ArgumentNullException(nameof(courier));
    }

    public void StartDelivery()
    {
        if (Status != DeliveryStatus.Created)
            throw new InvalidOperationException("Delivery can only start from Created state.");

        if (AssignedCourier == null)
            throw new InvalidOperationException("Cannot start delivery without an assigned courier.");

        if (_parcels.Count == 0)
            throw new InvalidOperationException("Cannot start delivery without parcels.");

        Status = DeliveryStatus.InTransit;
    }

    public void CompleteDelivery()
    {
        if (Status != DeliveryStatus.InTransit)
            throw new InvalidOperationException("Only orders in transit can be completed.");

        Status = DeliveryStatus.Completed;
    }

    public void Cancel()
    {
        if (Status == DeliveryStatus.Completed)
            throw new InvalidOperationException("Completed delivery cannot be cancelled.");

        Status = DeliveryStatus.Cancelled;
    }

    public double GetTotalWeight() => _parcels.Sum(p => p.Weight);

    public decimal GetTotalCost() => DeliveryMethod.CalculateCost(GetTotalWeight());
}