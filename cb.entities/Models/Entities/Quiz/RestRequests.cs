using System;
using System.Collections.Generic;
using System.Text;

namespace Elsa.Entities.Quiz
{
    public class StartSessionRequest
    {
        public string QuizId { get; set; }
    }

    public class JoinSessionRequest
    {
        public string UserId { get; set; }
        public string SessionId { get; set; }
    }

    public class SubmitSessionRequest
    {
        public string UserId { get; set; }
        public string SessionId { get; set; }

        public TimeSpan Duration { get; set; }

        public Quiz Quiz { get; set; }
    }

    public class SubmitSessionResponse
    {
        public string UserId { get; set; }
        public string SessionId { get; set; }

        public Quiz Quiz { get; set; }
    }
}
