using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CP.Api.Models
{
    [BsonIgnoreExtraElements]
    public class Element
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string CategoryId { get; set; }
        public string Name { get; set; }

        [BsonElement("RenderText")]
        public string RenderText { get; set; }
        public string Tags { get; set; }

        public int Dowloads { get; set; }

        public int Views { get; set; }

        public int Likes { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<string> ImageFiles { get; set; }
    }
}