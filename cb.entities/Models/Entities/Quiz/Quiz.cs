using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;


namespace Elsa.Entities.Quiz
{
    public class Quiz : MongoEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// When IsStarted is true, this quiz is associated with a session
        /// </summary>
        public bool IsStarted { get; set; }

        public List<QuizItem> Questions { get; set; } = new List<QuizItem>();
    }
}
