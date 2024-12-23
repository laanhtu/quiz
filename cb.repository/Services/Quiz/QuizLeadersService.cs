
using CP.Api.Models;
using Elsa.Api.Helpers;
using Elsa.Entities.Quiz;
using Elsa.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.Repository.Services
{
    public class QuizLeadersService : IQuizLeadersServiceTransient, IQuizLeadersServiceScoped, IQuizLeadersServiceSingleton
    {
        private readonly IMongoCollection<QuizLeader> _quizLeaders;

        public QuizLeadersService(IDatabaseSettings dbSettings)
        {
            var client = new MongoClient(dbSettings.ConnectionString);
            var database = client.GetDatabase(dbSettings.DatabaseName);

            _quizLeaders = database.GetCollection<QuizLeader>(dbSettings.QuizLeadersCollectionName);
        }

        public event ScoreRecordedEventHandler ScoreRecorded;

        public async Task<List<QuizLeader>> List()
        {
            var leaders = await _quizLeaders.Find(_ => true).ToListAsync();

            return leaders;
        }

        public async Task<QuizLeader> Record(ParticipantRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            if (record.UserId == null)
                throw new ArgumentNullException($"Leader record should belong to a user");

            var cursor = await _quizLeaders.FindAsync(x => x.Id == record.UserId);

            var leader = await cursor.FirstOrDefaultAsync();

            if (leader == null)
            {
                var newLeaderRecord = new QuizLeader { Id = record.UserId, NumberOfQuiz = 1, Score = record.Score, TotalTime = record.Duration };
                
                await _quizLeaders.InsertOneAsync(newLeaderRecord);
                
                return newLeaderRecord;
            }
            else
            {
                leader.NumberOfQuiz++;
                leader.Score += record.Score;
                leader.TotalTime += record.Duration;

                var result = await _quizLeaders.ReplaceOneAsync(e => e.Id == leader.Id, leader);

                if (result.IsAcknowledged)
                {
                    if(ScoreRecorded != null)
                    {
                        ScoreRecorded(this, new ScoreRecoredEventArgs { Duration = leader.TotalTime, Score = leader.Score, UserId = leader.Id });
                    }
                    return leader;
                }
                else
                {
                    throw new KeyNotFoundException(nameof(record));
                }
            } 
        }
    }
}
