using Microsoft.EntityFrameworkCore;
using MiniHittegods.Domain;

namespace MiniHittegods.Infrastructure.Persistence;
public class FoundItemDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FoundItem>()
            .HasKey(item => item.Id);

        base.OnModelCreating(modelBuilder);
    }
    public DbSet<FoundItem> FoundItems => Set<FoundItem>();

    public FoundItemDbContext(DbContextOptions<FoundItemDbContext> options) : base(options)
    {
        
    }
}