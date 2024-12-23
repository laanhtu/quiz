using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CP.Api.Models
{
  public class Category
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }

    public string ParentId { get; set; }

    [BsonIgnore]
    public int Weight { get; set; }
  }
}