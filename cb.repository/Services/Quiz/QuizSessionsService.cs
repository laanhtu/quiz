using System.Threading.Tasks;
using Elsa.Repository.Interfaces;
using Elsa.Entities.Quiz;
using MongoDB.Driver;
using System.Linq;
using System;
using Elsa.Api.Helpers;
using MongoDB.Bson;

namespace Elsa.Repository.Services
{
    public class QuizNotFoundException : Exception
    {
        public QuizNotFoundException(string message) : base(message) { }
    }

    public class SessionNotFoundException : Exception
    {
        public SessionNotFoundException(string message) : base(message) { }
    }

    public class QuizSessionsService : IQuizSessionsServiceTransient, IQuizSessionsServiceScoped, IQuizSessionsServiceSingleton
    {
        private readonly IMongoCollection<QuizSession> _quizSessions;
        private readonly IQuizzesService _quizzesService;

        public QuizSessionsService(IDatabaseSettings dbSettings, IQuizzesService quizzesService)
        {
            var client = new MongoClient(dbSettings.ConnectionString);
            var database = client.GetDatabase(dbSettings.DatabaseName);

            _quizSessions = database.GetCollection<QuizSession>(dbSettings.QuizSessionsCollectionName);

            _quizzesService = quizzesService;
        }

        public event UserJoinedEventHandler UserJoined;
        public event SessionStartedEventHandler SessionStarted;
        public event SessionSubmittedEventHandler SessionSubmitted;

        /// <summary>
        /// Start a new Quiz session
        /// </summary>
        /// <param name="newSession"></param>
        /// <returns></returns>
        public async Task<QuizSession> Start(QuizSession newSession)
        {
            if (newSession == null)
            {
                throw new ArgumentNullException(nameof(newSession));
            }

            if (ObjectId.TryParse(newSession.QuizId, out _))
            {
                var quiz = await _quizzesService.GetById(newSession.QuizId);

                if (quiz != null)
                {
                    await _quizSessions.InsertOneAsync(newSession);

                    if (SessionStarted != null)
                    {
                        SessionStarted(this, new SessionStartedEventArgs { QuizId = newSession.QuizId, SessionId = newSession.Id, QuizName = quiz.Name });
                    }

                    return newSession;
                }
                else
                {
                    throw new QuizNotFoundException($"Quiz with id {newSession.QuizId} not found.");
                }
            }

            throw new ArgumentNullException(nameof(newSession.QuizId));

        }

        /// <summary>
        /// Get Quiz session by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QuizSession> GetSession(string id)
        {
            if (ObjectId.TryParse(id, out _))
            {
                var cursor = await _quizSessions.FindAsync(x => x.Id == id);

                return await cursor.FirstOrDefaultAsync();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Join a Quiz session by id
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        public async Task<QuizSession> Join(string sessionId, ParticipantRecord participant)
        {
            if (participant == null)
                throw new ArgumentNullException(nameof(participant));

            var cursor = await _quizSessions.FindAsync(x => x.Id == sessionId);

            var session = await cursor.FirstOrDefaultAsync();

            if (session == null)
            {
                throw new SessionNotFoundException($"Session with Id {sessionId} is not existed.");
            }
            else
            {
                if (session.ParticipantRecords == null)
                {
                    session.ParticipantRecords = new System.Collections.Generic.List<ParticipantRecord> { participant };
                }
                else
                {
                    var filter = Builders<QuizSession>.Filter.Eq(e => e.Id, sessionId);

                    var update = Builders<QuizSession>.Update.Push<ParticipantRecord>(e => e.ParticipantRecords, participant);

                    var joinedSession = await _quizSessions.FindOneAndUpdateAsync(filter, update);

                    if (UserJoined != null)
                    {
                        UserJoined(this, new UserJoinedEventArgs { UserId = participant.UserId, SessionId = session.Id });
                    }

                    return joinedSession;
                }

                return session;
            }
        }

        /// <summary>
        /// Submit result of quiz for a session by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        public async Task<ParticipantRecord> Submit(string id, ParticipantRecord participant)
        {
            if (participant == null)
                throw new ArgumentNullException(nameof(participant));

            var cursor = await _quizSessions.FindAsync(x => x.Id == id);

            var session = await cursor.FirstOrDefaultAsync();

            if (session == null)
            {
                throw new InvalidOperationException($"Quiz session {id} is not found");
            }

            if (session.ParticipantRecords == null || session.ParticipantRecords.Count(x => x.UserId == participant.UserId) == 0)
                throw new InvalidOperationException($"{nameof(participant)} is not found");

            var joined = session.ParticipantRecords.Single(x => x.UserId == participant.UserId);

            joined.Score = participant.Score;
            joined.Duration = participant.Duration;

            var filter = Builders<QuizSession>.Filter.Eq(d => d.Id, id) & Builders<QuizSession>.Filter.ElemMatch(d => d.ParticipantRecords, item => item.UserId == participant.UserId);

            var update = Builders<QuizSession>.Update.Set("ParticipantRecords.$.Score", participant.Score).Set("ParticipantRecords.$.Duration", participant.Duration);

            UpdateResult result = await _quizSessions.UpdateOneAsync(filter, update);

            if (result.MatchedCount == 1)
            {
                if(SessionSubmitted != null)
                {
                    SessionSubmitted(this, new SessionSubmittedEventArgs { Duration = participant.Duration, Score = participant.Score, SessionId = id, UserId = participant.UserId });
                }

                return participant;
            }
            else
            {
                throw new InvalidOperationException($"{nameof(participant)} is not found");
            }
        }
    }
}