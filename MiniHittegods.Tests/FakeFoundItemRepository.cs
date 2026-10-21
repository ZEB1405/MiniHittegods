using MiniHittegods.Domain;
using MiniHittegods.Domain.Interfaces;

namespace MiniHittegods.Tests;

public class FakeFoundItemRepository : IFoundItemRepository
{
    private readonly List<FoundItem> _items = [];

    public Task AddAsync(FoundItem item)
    {
        _items.Add(item);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(FoundItem item)
    {
        _items.Remove(item);
        return Task.CompletedTask;
    }

    public Task<FoundItem?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_items.FirstOrDefault(item => item.Id == id));
    }

    public bool UpdateCalled { get; private set; }
    public Task UpdateAsync(FoundItem item)
    {
        var existingItem = _items.FirstOrDefault(i => i.Id == item.Id);
        if (existingItem is not null)
        {
            _items.Remove(existingItem);
            _items.Add(item);
        }
        UpdateCalled = true;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<FoundItem>> GetAllAsync(
    FoundItemStatus? status = null,
    string? category = null,
    string? q = null)
    {
        IEnumerable<FoundItem> query = _items;

        if (status.HasValue)
        {
            query = query.Where(item => item.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(item =>
                item.Category?.Equals(
                    category,
                    StringComparison.OrdinalIgnoreCase) == true);
        }

        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(item =>
                item.Title.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                item.Description?.Contains(
                    q,
                    StringComparison.OrdinalIgnoreCase) == true);
        }

        return Task.FromResult<IReadOnlyList<FoundItem>>(query.ToList());
    }
}