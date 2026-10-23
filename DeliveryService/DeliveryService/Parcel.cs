namespace DeliveryService;

public class Parcel
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Description { get; }
    public double Weight { get; }

    public Parcel(string description, double weight)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Parcel description cannot be empty.", nameof(description));

        if (weight <= 0)
            throw new ArgumentOutOfRangeException(nameof(weight), "Weight must be greater than zero.");

        Description = description;
        Weight = weight;
    }
}