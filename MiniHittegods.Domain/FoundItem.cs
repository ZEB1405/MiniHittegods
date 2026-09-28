namespace MiniHittegods.Domain;

public class FoundItem
{
    public FoundItemStatus Status { get; private set; } = FoundItemStatus.Available;
    public string Title { get; }
    public string? Description { get;}
    public string? Category { get; }
    public string FoundLocation { get; }
    public DateTime FoundAtUtc { get; }
    public Guid Id { get; } = Guid.NewGuid();
    public string? ClaimedBy { get; private set; }
    public DateTime? ClaimedAtUtc { get; private set; }
    public DateTime? ReturnedAtUtc { get; private set; }
    public void Claim(string claimedBy)
    {
        if (Status != FoundItemStatus.Available)
        {
            throw new InvalidOperationException("Item is already claimed.");
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

    public FoundItem(string title, string foundLocation)
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
    }
}