using DeliveryService;

Console.WriteLine("=== Delivery Service Demo (Lab 02 - Variant 5) ===\n");

var customer = new Customer("Egor Vlasenko", "Poltava, Sobornosty St 15");
var courier = new Courier("Alex Delivery");

IDeliveryMethod standard = new StandardDelivery();
IDeliveryMethod express = new ExpressDelivery();

var order1 = new DeliveryOrder(customer, standard);
order1.AddParcel(new Parcel("Laptop", 2.5));
order1.AddParcel(new Parcel("Documents", 0.3));

Console.WriteLine($"Order 1 (Standard): Total Weight = {order1.GetTotalWeight()} kg | Cost = {order1.GetTotalCost()} UAH");

var order2 = new DeliveryOrder(customer, express);
order2.AddParcel(new Parcel("Box", 5.0));

Console.WriteLine($"Order 2 (Express): Total Weight = {order2.GetTotalWeight()} kg | Cost = {order2.GetTotalCost()} UAH\n");

order1.AssignCourier(courier);
order1.StartDelivery();
Console.WriteLine($"Order 1 Status: {order1.Status}");

order1.CompleteDelivery();
Console.WriteLine($"Order 1 Status: {order1.Status}");

try
{
    Console.WriteLine("\nAttempting to cancel completed delivery...");
    order1.Cancel();
}
catch (Exception ex)
{
    Console.WriteLine($"[Caught expected error]: {ex.Message}");
}