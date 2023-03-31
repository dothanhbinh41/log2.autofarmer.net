using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LogJson.AutoFarmer.Models
{
    public class Entity
    {
        [BsonElement("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
