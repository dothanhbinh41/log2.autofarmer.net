using Autolike.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver; 

namespace LogJson.AutoFarmer.Repositories
{
    public interface IAutolikeMongoClient : IMongoClient
    {
    }

    public class AutolikeMongoClient : MongoClient, IAutolikeMongoClient
    {
        public AutolikeMongoClient(IOptions<MongoOptions> options) : base(options.Value.ConnectionString)
        {

        }
    }
}
