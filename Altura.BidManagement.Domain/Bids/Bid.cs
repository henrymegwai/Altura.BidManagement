using SQLite;

namespace Altura.BidManagement.Domain.Bids;

public class Bid(
    Guid id,
    string title,
    decimal amount,
    string state,
    DateTime createdAt)
{
    [PrimaryKey] public Guid Id { get; set; } = id;
    public string Title { get; set; } = title;
    public decimal Amount { get; set; } = amount;
    public string State { get; set; } = state;
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}