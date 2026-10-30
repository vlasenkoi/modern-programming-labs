namespace DeliveryService;

public class Customer
{
    public Guid Id { get; } = Guid.NewGuid();

    public required string Name { get; init; }
    public required string Phone { get; init; }

    public string? Email { get; init; }
    public DeliveryAddress? PrimaryAddress { get; set; }

    public Customer() { }

    public Customer(string name, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be empty.", nameof(phone));

        Name = name;
        Phone = phone;
    }
}