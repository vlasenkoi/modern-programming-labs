using DeliveryService;

Console.WriteLine("=== Modern C# Delivery Service LINQ Demo (Lab 04 - Variant 5) ===\n");

// 1. Генерація даних та заповнення Repository[cite: 20]
var (orderRepo, courierRepo, customerRepo) = TestDataGenerator.GenerateData();
var orders = orderRepo.GetAll();

Console.WriteLine($"Total orders in system: {orders.Count}\n");

// Запит 1: Активні доставки (Created, CourierAssigned, InTransit)[cite: 20]
var activeDeliveries = orders
    .Where(o => o.Status == DeliveryStatus.Created ||
                o.Status == DeliveryStatus.CourierAssigned ||
                o.Status == DeliveryStatus.InTransit)
    .ToList();
Console.WriteLine($"1. Active Deliveries Count: {activeDeliveries.Count}");

// Запит 2: Топ-5 найважчих замовлень (OrderByDescending + Take)[cite: 20]
var top5HeavyOrders = orders
    .OrderByDescending(o => o.GetTotalWeight())
    .Take(5);
Console.WriteLine("\n2. Top-5 Heaviest Delivery Orders:");
foreach (var o in top5HeavyOrders)
{
    Console.WriteLine($"   - Order {o.Id.ToString()[..8]}... | Weight: {o.GetTotalWeight():F1} kg | Status: {o.Status}");
}

// Запит 3: Загальна вага всіх посилок (SelectMany + Sum)[cite: 20]
double totalParcelsWeight = orders
    .SelectMany(o => o.Parcels)
    .Sum(p => p.Weight);
Console.WriteLine($"\n3. Total Weight of All Parcels: {totalParcelsWeight:F1} kg");

// Запит 4: Середня вартість доставки (Average)[cite: 20]
decimal averageCost = orders.Average(o => o.GetTotalCost());
Console.WriteLine($"4. Average Delivery Cost: {averageCost:F2} UAH");

// Запит 5: Кількість доставок для кожного кур'єра (GroupBy)[cite: 20]
var deliveriesPerCourier = orders
    .Where(o => o.AssignedCourier != null)
    .GroupBy(o => o.AssignedCourier!.Name)
    .Select(g => new { CourierName = g.Key, Count = g.Count() });
Console.WriteLine("\n5. Deliveries per Courier:");
foreach (var item in deliveriesPerCourier)
{
    Console.WriteLine($"   - {item.CourierName}: {item.Count} orders");
}

// Запит 6: Усі посилки через SelectMany[cite: 20]
var allParcels = orders.SelectMany(o => o.Parcels).ToList();
Console.WriteLine($"\n6. Total Parcels Extracted via SelectMany: {allParcels.Count}");

// Запит 7: Топ-3 кур'єри за кількістю виконаних доставок[cite: 20]
var top3Couriers = orders
    .Where(o => o.Status == DeliveryStatus.Delivered && o.AssignedCourier != null)
    .GroupBy(o => o.AssignedCourier!)
    .OrderByDescending(g => g.Count())
    .Take(3)
    .Select(g => new { Courier = g.Key, CompletedCount = g.Count() });
Console.WriteLine("\n7. Top-3 Couriers by Completed Deliveries:");
foreach (var item in top3Couriers)
{
    Console.WriteLine($"   - {item.Courier.Name}: {item.CompletedCount} completed");
}

// Запит 8: Клієнти з більше ніж 3 замовленнями[cite: 20]
var frequentCustomers = orders
    .GroupBy(o => o.Customer)
    .Where(g => g.Count() > 3)
    .Select(g => new { CustomerName = g.Key.Name, OrderCount = g.Count() });
Console.WriteLine("\n8. Customers with more than 3 deliveries:");
foreach (var c in frequentCustomers)
{
    Console.WriteLine($"   - {c.CustomerName} ({c.OrderCount} orders)");
}

// Запит 9: Перевірка через Any(), чи є посилка > 20 кг[cite: 20]
bool hasHeavyParcel = orders
    .SelectMany(o => o.Parcels)
    .Any(p => p.Weight > 20.0);
Console.WriteLine($"\n9. Is there any parcel weighing > 20 kg? Answer: {hasHeavyParcel}");

// Запит 10: Створення Dictionary<CourierId, CompletedCount> (ToDictionary)[cite: 20]
var courierStatsDictionary = courierRepo.GetAll()
    .ToDictionary(
        courier => courier.Id,
        courier => orders.Count(o => o.AssignedCourier?.Id == courier.Id && o.Status == DeliveryStatus.Delivered)
    );
Console.WriteLine("\n10. Courier Statistics Dictionary (CourierId -> Completed Deliveries):");
foreach (var (courierId, count) in courierStatsDictionary)
{
    Console.WriteLine($"   - Courier ID {courierId.ToString()[..8]}... -> {count} completed deliveries");
}