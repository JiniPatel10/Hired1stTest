using MongoDB.Driver;


namespace twoladder.Core.Data
{
    public interface IMongoDBContext
    {
        IMongoDatabase GetDatabase();
    //    IMongoCollection<User> Users { get; }



    }
}
