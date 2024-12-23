using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elsa.Entities.Quiz
{
    public class QuizLeader
    {
        /// <summary>
        /// User ID refers to User
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int Score { get; set; }  // Total scores that user earns

        /// <summary>
        /// Total time that user used to get score
        /// </summary>
        public TimeSpan TotalTime { get; set; }

        /// <summary>
        /// Number of quizs that user joined.
        /// </summary>
        public int NumberOfQuiz { get; set; }

        [BsonIgnore]
        public string FullName { get; set; }
    }
}
