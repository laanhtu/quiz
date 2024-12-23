using Elsa.Entities.Quiz;
using System;
using System.Threading.Tasks;


namespace Elsa.Repository.Interfaces
{
    public class SessionEventArgs : EventArgs
    {
        public string SessionId { get; set; }
    }
    public class SessionStartedEventArgs : SessionEventArgs
    {
        public string QuizId { get; set; }
        public string QuizName { get; set; } 
    }
    public class UserJoinedEventArgs : SessionEventArgs
    {
        public string UserId { get; set; }
    }

    public class SessionSubmittedEventArgs : SessionEventArgs
    { 
        public string UserId { get; set; }
        public int Score { get; set; }
        public TimeSpan Duration { get; set; }
    }

    public delegate void SessionStartedEventHandler(object sender, SessionStartedEventArgs e);

    public delegate void UserJoinedEventHandler(object sender, UserJoinedEventArgs e);

    public delegate void SessionSubmittedEventHandler(object sender, SessionSubmittedEventArgs e);

    public interface IQuizSessionsService
    {
        event SessionStartedEventHandler SessionStarted;
        event UserJoinedEventHandler UserJoined;
        event SessionSubmittedEventHandler SessionSubmitted;

        /// <summary>
        /// Start a new Quiz session
        /// </summary>
        /// <param name="newSession"></param>
        /// <returns></returns>
        Task<QuizSession> Start(QuizSession newSession);
        /// <summary>
        /// Get Quiz session by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QuizSession> GetSession(string id);

        /// <summary>
        /// Join a Quiz session by id
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        Task<QuizSession> Join(string sessionId, ParticipantRecord participant);

        /// <summary>
        /// Submit result of quiz for a session by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        Task<ParticipantRecord> Submit(string id, ParticipantRecord participant);
    }

    public interface IQuizSessionsServiceTransient : IQuizSessionsService { }
    public interface IQuizSessionsServiceScoped : IQuizSessionsService { }
    public interface IQuizSessionsServiceSingleton : IQuizSessionsService { }
}