namespace MiniHittegods.Domain.Interfaces;


public interface IFoundItemRepository
{
    Task<FoundItem?> GetByIdAsync(Guid id);
    Task AddAsync(FoundItem item);
    Task UpdateAsync(FoundItem item);
    Task DeleteAsync(FoundItem item);
    Task<IReadOnlyList<FoundItem>> GetAllAsync();
}