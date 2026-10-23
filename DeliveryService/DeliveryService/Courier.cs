namespace DeliveryService;

public class Courier
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }

    public Courier(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Courier name cannot be empty.", nameof(name));

        Name = name;
    }
}