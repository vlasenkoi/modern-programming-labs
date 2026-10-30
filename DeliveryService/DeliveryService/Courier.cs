namespace DeliveryService;

public class Courier
{
    public Guid Id { get; } = Guid.NewGuid();
    public required string Name { get; init; }

    public Courier() { }

    public Courier(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Courier name cannot be empty.", nameof(name));

        Name = name;
    }
}