using core.Domain.Models;
using MongoDB.Driver;


namespace core.Data
{
    public interface IMongoDBContext
    {
        IMongoDatabase GetDatabase();
        IMongoCollection<User> Users { get; }

        IMongoCollection<Product> Products { get; }


    }
}
