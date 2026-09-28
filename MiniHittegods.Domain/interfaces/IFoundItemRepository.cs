//Application service needs to call this in order to remove an item
namespace MiniHittegods.Domain.interfaces;


public interface IFoundItemRepository
{
    Task<FoundItem?> GetByIdAsync(Guid id);
    Task AddAsync(FoundItem item);
    Task UpdateAsync(FoundItem item);
    Task DeleteAsync(FoundItem item);
}