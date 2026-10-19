namespace MiniHittegods.Api.Models;

public class CreateItemRequest
{
    public string Title { get; set; } = string.Empty;
    public string FoundLocation { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
}