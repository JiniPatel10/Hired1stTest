using System;
using MongoDB.Driver;


namespace twoladder.Core.Data
{
    public class MongoDBContext : IMongoDBContext
    {
        // These are set in the Startup.cs class of the project, from the configuration files
        public static string ConnString { get; set; } = string.Empty;
        public static string Database { get; set; } = string.Empty;

        private IMongoDatabase _currentDatabase = null;

        public MongoDBContext()
        {
            try
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(ConnString));
                settings.WaitQueueSize = 5000;
                settings.MaxConnectionPoolSize = 500;
                var mongoClient = new MongoClient(settings);
                if (mongoClient != null)
                    _currentDatabase = mongoClient.GetDatabase(Database);
            }
            catch (Exception ex)
            {
                throw new Exception("Error connecting to the Mongo Database", ex);
            }
        }
        public IMongoDatabase GetDatabase()
        {
            return _currentDatabase;
        }

        // Define the name of the collections in MongoDB. Alphabetically
        public static class CollectionNames
        {
            public const string AUTOINCREMENTS = "autoincrements";
            public const string USER = "user";
            
        }

		// Enumerate all collections handle by the MongoDBContext of the application
		//public IMongoCollection<AutoIncrement> AutoIncrements
  //      {
  //          get { return _currentDatabase.GetCollection<AutoIncrement>(MongoDBContext.CollectionNames.AUTOINCREMENTS); }
  //      }
  //      public IMongoCollection<User> Users
  //      {
  //          get { return _currentDatabase.GetCollection<User>(MongoDBContext.CollectionNames.USER); }
  //      }
        
    }
}
