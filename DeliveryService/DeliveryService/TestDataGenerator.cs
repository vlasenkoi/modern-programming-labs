namespace DeliveryService;

public static class TestDataGenerator
{
    public static (InMemoryRepository<DeliveryOrder> OrderRepo, InMemoryRepository<Courier> CourierRepo, InMemoryRepository<Customer> CustomerRepo) GenerateData()
    {
        var orderRepo = new InMemoryRepository<DeliveryOrder>();
        var courierRepo = new InMemoryRepository<Courier>();
        var customerRepo = new InMemoryRepository<Customer>();

        // 1. Створюємо кур'єрів
        var couriers = new List<Courier>
        {
            new() { Name = "Alex Express" },
            new() { Name = "Dmytro Speed" },
            new() { Name = "Oleg Fast" },
            new() { Name = "Ihor Logistics" },
            new() { Name = "Anna Courier" }
        };
        foreach (var c in couriers) courierRepo.Add(c);

        // 2. Створюємо клієнтів
        var customers = new List<Customer>
        {
            new() { Name = "Egor Vlasenko", Phone = "+380991112233", Email = "egor@test.com" },
            new() { Name = "Ivan Franko", Phone = "+380502223344", Email = "ivan@test.com" },
            new() { Name = "Taras Shevchenko", Phone = "+380673334455", Email = "taras@test.com" },
            new() { Name = "Lesya Ukrainka", Phone = "+380634445566", Email = "lesya@test.com" },
            new() { Name = "Mikhailo Hrushevsky", Phone = "+380955556677", Email = "misha@test.com" }
        };
        foreach (var cust in customers) customerRepo.Add(cust);

        var address = new DeliveryAddress("Poltava", "Sobornosty St", "15", "Apt 10");
        var random = new Random(42); // Фіксований seed для стабільності даних

        string[] parcelDescriptions = ["Laptop", "Documents", "Phone", "Washing Machine", "TV", "Books", "Clothing", "Tools", "Microwave", "Monitor"];

        // 3. Генеруємо 20 замовлень
        for (int i = 0; i < 20; i++)
        {
            var customer = customers[i % customers.Count];
            IDeliveryMethod method = (i % 2 == 0) ? new StandardDelivery() : new ExpressDelivery();
            var order = new DeliveryOrder(customer, method, address);

            // Додаємо від 1 до 3 посилок у замовлення
            int parcelCount = random.Next(1, 4);
            for (int j = 0; j < parcelCount; j++)
            {
                string desc = parcelDescriptions[random.Next(parcelDescriptions.Length)];
                // Спеціально робимо одну посилку > 20 кг для перевірки Any()
                double weight = (i == 7 && j == 0) ? 22.5 : Math.Round(random.NextDouble() * 15 + 0.5, 1);
                order.AddParcel(new Parcel(desc, weight) { Description = desc });
            }

            // Назначаємо кур'єра та міняємо статус
            var courier = couriers[i % couriers.Count];
            order.AssignCourier(courier);

            if (i % 3 == 0)
            {
                order.StartDelivery();
                order.CompleteDelivery(); // Виконана доставка
            }
            else if (i % 3 == 1)
            {
                order.StartDelivery(); // В дорозі (InTransit)
            }

            orderRepo.Add(order);
        }

        return (orderRepo, courierRepo, customerRepo);
    }
}