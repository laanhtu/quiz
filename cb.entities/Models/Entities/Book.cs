using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CP.Api.Models
{
  public class Book
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }

    [BsonElement("RenderText")]
    public string RenderText { get; set; }
    public string Keywords { get; set; }

    /*
     * User Id that create this book
     */
    public string CreatedBy { get; set; }

    public int PrintCount { get; set; }
    public int ViewCount { get; set; }

  }
}