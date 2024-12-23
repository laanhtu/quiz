using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Elsa.Entities.Quiz
{
    public class QuizItem
    {
        public string Question { get; set; }
        public List<string> Choices { get; set; } = new List<string>();
        public int CorrectChoice { get; set; }
        [BsonIgnore]
        public int AnswerChoice { get; set; } = -1;
    }
}
