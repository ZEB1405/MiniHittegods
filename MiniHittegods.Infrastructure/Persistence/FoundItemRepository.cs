
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
    public async Task<IReadOnlyList<FoundItem>> GetAllAsync(
    FoundItemStatus? status = null,
    string? category = null,
    string? q = null)
    {
        var query = _context.FoundItems.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(item => item.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(item =>
                item.Category != null &&
                item.Category.ToLower() == category.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(q))
        {
            var searchTerm = q.ToLower();

            query = query.Where(item =>
                item.Title.ToLower().Contains(searchTerm) ||
                (item.Description != null &&
                item.Description.ToLower().Contains(searchTerm)));
        }

        return await query.ToListAsync();
    }
}
