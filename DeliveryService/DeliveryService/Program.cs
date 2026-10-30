using DeliveryService;

Console.WriteLine("=== Modern C# Delivery Service Demo (Lab 03 - Variant 5) ===\n");

var address1 = new DeliveryAddress("Poltava", "Sobornosty St", "15", "Apt 42");
var address2 = new DeliveryAddress("Poltava", "Sobornosty St", "15", "Apt 42");

Console.WriteLine($"Address Record Value Equality: {address1 == address2}");

var customer = new Customer
{
    Name = "Egor Vlasenko",
    Phone = "+380991234567",
    Email = "egor@example.com",
    PrimaryAddress = address1
};

var courier = new Courier { Name = "Alex Delivery" };

var smallParcel = new Parcel("Phone Charger", 0.3) { Description = "Phone Charger" };
var mediumParcel = new Parcel("Gaming Laptop", 4.5) { Description = "Gaming Laptop" };
var heavyParcel = new Parcel("Washing Machine", 15.0) { Description = "Washing Machine" };

Console.WriteLine($"\n--- Parcel Categories (Relational Pattern) ---");
Console.WriteLine($"{smallParcel.Description} ({smallParcel.Weight} kg): {smallParcel.GetParcelCategory()}");
Console.WriteLine($"{mediumParcel.Description} ({mediumParcel.Weight} kg): {mediumParcel.GetParcelCategory()}");
Console.WriteLine($"{heavyParcel.Description} ({heavyParcel.Weight} kg): {heavyParcel.GetParcelCategory()}");

IDeliveryMethod expressMethod = new ExpressDelivery();
var expressOrder = new DeliveryOrder(customer, expressMethod, address1);

expressOrder.AddParcel(mediumParcel);
expressOrder.AssignCourier(courier);

Console.WriteLine($"\n--- Delivery Priority (Property Pattern) ---");
Console.WriteLine($"Status before transit: {expressOrder.GetDeliveryPriorityDescription()}");

expressOrder.StartDelivery();
Console.WriteLine($"Status after transit start: {expressOrder.GetDeliveryPriorityDescription()}");

expressOrder.CompleteDelivery();
Console.WriteLine($"Final Status: {expressOrder.GetDeliveryPriorityDescription()}");