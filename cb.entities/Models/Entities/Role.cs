using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CP.Api.Models
{
  public class Role
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; }
  }
}