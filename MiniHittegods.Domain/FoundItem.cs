namespace MiniHittegods.Domain;

public class FoundItem
{
    public FoundItemStatus Status { get; private set; } = FoundItemStatus.Available;
    public string Title { get; private set; } = null!;
    public string? Description { get; private set;}
    public string? Category { get; private set;}
    public string FoundLocation { get; private set; } = null!;
    public DateTime FoundAtUtc { get; private set; }
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string? ClaimedBy { get; private set; }
    public DateTime? ClaimedAtUtc { get; private set; }
    public DateTime? ReturnedAtUtc { get; private set; }
    private FoundItem() { }
    public void Claim(string claimedBy)
    {
        if (Status != FoundItemStatus.Available)
        {
            throw new InvalidOperationException("Item is not available for claiming.");
        }
        ClaimedBy = claimedBy;
        ClaimedAtUtc = DateTime.UtcNow;
        Status = FoundItemStatus.Claimed;
    }
    public void Return()
    {
        if (Status != FoundItemStatus.Claimed)
        {
            throw new InvalidOperationException("Item is not claimed.");
        }
        ReturnedAtUtc = DateTime.UtcNow;
        Status = FoundItemStatus.Returned;
    }

    public FoundItem(string title, string foundLocation, string? description = null, string? category = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be empty or whitespace.");
        }
        if (title.Length > 80)
        {
            throw new ArgumentException("Title cannot be more than 80 characters.");
        }

        if(string.IsNullOrWhiteSpace(foundLocation))
        {
            throw new ArgumentException("Found location cannot be empty or whitespace.");
        }

        Title = title;
        FoundLocation = foundLocation;
        FoundAtUtc = DateTime.UtcNow;
        Description = description;
        Category = category;
    }
}