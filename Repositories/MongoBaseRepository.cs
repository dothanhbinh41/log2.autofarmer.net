using Autolike.Options;
using LogJson.AutoFarmer.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LogJson.AutoFarmer.Repositories
{
    public interface IMongoRepository<T>
    {
        IMongoCollection<T> Collection { get; }

    }


    public class MongoBaseRepository<T> : IMongoRepository<T>
    {
        public IMongoCollection<T> Collection => _collection;
        private readonly IMongoCollection<T> _collection;
        public MongoBaseRepository(IAutolikeMongoClient client, IOptions<MongoOptions> options)
        {
            var database = client.GetDatabase(options.Value.DatabaseName);
            var name = GetCollectionName();
            _collection = database.GetCollection<T>(name, new MongoCollectionSettings { });
        }

        protected virtual string GetCollectionName()
        {
            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(typeof(T));
            var collectionName = attrs.FirstOrDefault(d => d.GetType() == typeof(CollectionNameAttribute));
            if (collectionName == null)
            {
                return typeof(T).Name;
            }
            return (collectionName as CollectionNameAttribute).Name;
        }
    }
}
