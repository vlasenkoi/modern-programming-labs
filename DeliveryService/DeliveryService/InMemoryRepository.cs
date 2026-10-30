namespace DeliveryService;

public class InMemoryRepository<T>
{
    private readonly List<T> _items = [];

    public void Add(T item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));
        _items.Add(item);
    }

    public bool Remove(T item)
    {
        return _items.Remove(item);
    }

    public IReadOnlyCollection<T> GetAll()
    {
        return _items.AsReadOnly();
    }
}