using System;
using System.Collections.Generic;
using System.Text;

namespace Elsa.Entities
{
    public class SocketMessageType
    {
        public const string Quiz_Session_SessionStarted = "Quiz.Session.SessionStarted";
        public const string Quiz_Session_UserJoined = "Quiz.Session.UserJoined";
        public const string Quiz_Session_SessionSubmitted = "Quiz.Session.SessionSubmitted";
        public const string Quiz_Leaders_ScoreRecored = "Quiz.Leaders.UserScoreRecored";
    }

    public class SocketMessage
    {
        public string Type { get; set; }            //  consts of SocketMessageType
        public string SessionId { get; set; }
        public string QuizId { get; set; }
        public string QuizName { get; set; }        // Name of quiz that is used with a session by session Id
        public string UserId { get; set; }
        public string FullName { get; set; }      // Name of User = Last name + First name
        public int Score { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
