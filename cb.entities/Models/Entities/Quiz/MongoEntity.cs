using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Elsa.Entities.Quiz
{
    public class MongoEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
