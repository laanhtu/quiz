using MongoDB.Bson;
using System.Collections.Generic;
using System.Text;

namespace Elsa.Entities.Quiz
{
    public class QuizSession : MongoEntity
    {
        public string QuizId { get; set; }
        public List<ParticipantRecord> ParticipantRecords { get; set; } = new List<ParticipantRecord>();
    }
}
