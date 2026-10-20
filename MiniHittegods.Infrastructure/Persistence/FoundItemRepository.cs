
using Microsoft.EntityFrameworkCore;
using MiniHittegods.Domain;
using MiniHittegods.Domain.Interfaces;

namespace MiniHittegods.Infrastructure.Persistence;

public class FoundItemRepository : IFoundItemRepository
{
    private readonly FoundItemDbContext _context;

    public FoundItemRepository(FoundItemDbContext context)
    {
        _context = context;
    }
    public async Task<FoundItem?> GetByIdAsync(Guid id)
    {
        return await _context.FoundItems.FindAsync(id);
    }
    public async Task AddAsync(FoundItem item)
    {
        await _context.FoundItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(FoundItem item)
    {
        _context.FoundItems.Update(item);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(FoundItem item)
    {
        _context.FoundItems.Remove(item);
        await _context.SaveChangesAsync();
    }
    public async Task<IReadOnlyList<FoundItem>> GetAllAsync()
    {
        return await _context.FoundItems.ToListAsync();
    }
}
