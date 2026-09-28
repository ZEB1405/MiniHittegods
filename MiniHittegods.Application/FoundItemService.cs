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
}