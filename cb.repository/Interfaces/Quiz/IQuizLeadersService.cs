using Elsa.Entities.Quiz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Elsa.Repository.Interfaces
{
    public class ScoreRecoredEventArgs : SessionEventArgs
    {
        public string UserId { get; set; }
        public int Score { get; set; }
        public TimeSpan Duration { get; set; }
    }

    public delegate void ScoreRecordedEventHandler(object sender, ScoreRecoredEventArgs e);
    /// <summary>
    /// Constract for opearations on Quiz Leader entity
    /// </summary>
    public interface IQuizLeadersService
    {
        /// <summary>
        /// Every time score of a leader has been recorded, this event will be raised.
        /// </summary>
        event ScoreRecordedEventHandler ScoreRecorded;
        /// <summary>
        /// List all leaders of quiz
        /// </summary>
        /// <returns></returns>
        Task<List<QuizLeader>> List();

        /// <summary>
        /// Record a quiz leader record
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        Task<QuizLeader> Record(ParticipantRecord record);
    }

    public interface IQuizLeadersServiceTransient : IQuizLeadersService { }
    public interface IQuizLeadersServiceScoped : IQuizLeadersService { }
    public interface IQuizLeadersServiceSingleton : IQuizLeadersService { }

}