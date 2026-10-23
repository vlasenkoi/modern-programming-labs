namespace DeliveryService;

public class Customer
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }
    public string Address { get; }

    public Customer(string name, string address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be empty.", nameof(address));

        Name = name;
        Address = address;
    }
}