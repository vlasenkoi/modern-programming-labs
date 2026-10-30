namespace DeliveryService;

public class Parcel
{
    public Guid Id { get; } = Guid.NewGuid();
    public required string Description { get; init; }
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

    public string GetParcelCategory() => Weight switch
    {
        <= 2.0 => "Small Parcel (<= 2 kg)",
        <= 10.0 => "Medium Parcel (<= 10 kg)",
        _ => "Heavy Parcel (> 10 kg)"
    };
}