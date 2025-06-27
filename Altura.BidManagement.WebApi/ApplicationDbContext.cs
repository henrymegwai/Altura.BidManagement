using SQLite;

namespace Altura.BidManagement.WebApi
{
    public class ApplicationDbContext
    {
        private readonly SQLiteConnection _connection;

        public ApplicationDbContext()
        {
            var dbPath = Path.Combine(Environment.CurrentDirectory, "Altura.BidManagement.db");
            _connection = new SQLiteConnection(dbPath);
            _connection.CreateTable<Bid>();
        }

        public List<Bid> Bids => _connection.Table<Bid>().ToList();

        public void AddBid(Bid bid) => _connection.Insert(bid);
        public Bid GetBid(Guid id) => _connection.Find<Bid>(id);
        public void UpdateBid(Bid bid) => _connection.Update(bid);
        public void DeleteBid(Bid bid) => _connection.Delete(bid);
    }
}
