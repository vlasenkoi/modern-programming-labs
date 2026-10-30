namespace DeliveryService;

public record DeliveryAddress(
    string City,
    string Street,
    string Building,
    string? Apartment = null
);