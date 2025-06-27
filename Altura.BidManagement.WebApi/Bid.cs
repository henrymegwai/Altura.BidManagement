using SQLite;
namespace Altura.BidManagement.WebApi
{
    public class Bid
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string State { get; set; } = "Draft"; // Default state
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
