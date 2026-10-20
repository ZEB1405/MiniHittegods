using MiniHittegods.Domain;
using MiniHittegods.Domain.Interfaces;

namespace MiniHittegods.Application;

public class FoundItemService
{
    private readonly IFoundItemRepository _repository;
    public FoundItemService(IFoundItemRepository repository)
    {
        _repository = repository;
    }
    public async Task DeleteItemAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
        {
            throw new ArgumentException("Item not found.");
        }

        if (item.Status is not FoundItemStatus.Available)
        {
            throw new InvalidOperationException("Claimed item cannot be deleted.");
        }

        await _repository.DeleteAsync(item);
    }
    public async Task ClaimItemAsync(Guid id, string claimer)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
        {
            throw new ArgumentException("Item not found.");
        }

        if (item.Status is not FoundItemStatus.Available)
        {
            throw new InvalidOperationException("Item is not available for claiming.");
        }

        item.Claim(claimer);
        await _repository.UpdateAsync(item);
    }

    public async Task ReturnItemAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
        {
            throw new ArgumentException("Item not found.");
        }
        
        if (item.Status is not FoundItemStatus.Claimed)
        {
            throw new InvalidOperationException("Item is not claimed.");
        }

        item.Return();
        await _repository.UpdateAsync(item);
    }

    public async Task<FoundItem> GetItemByIdAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
        {
            throw new ArgumentException("Item not found.");
        }
        return item;
    }

    public async Task<FoundItem> CreateItemAsync(
    string title,
    string foundLocation,
    string? description = null,
    string? category = null)
    {
        var item = new FoundItem(title, foundLocation, description, category);
        await _repository.AddAsync(item);
        return item;
    }

    public async Task<IReadOnlyList<FoundItem>> GetAllItemsAsync()
    {
        return await _repository.GetAllAsync();
    }
}