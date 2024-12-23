using System;

namespace Elsa.Entities.Quiz
{
    public class ParticipantRecord
    {
        public string UserId { get; set; }
        public int Score { get; set; }
        public TimeSpan Duration { get; set; }

    }
}
