
using Elsa.Repository.Interfaces;
using Elsa.Entities.Quiz;
using System.Threading.Tasks;
using MongoDB.Driver;
using Elsa.Api.Helpers;
using System.Collections.Generic;

namespace Elsa.Repository.Services
{
    public class QuizzesService : IQuizzesService
    {
        private readonly IMongoCollection<Quiz> _quizzes;

        public QuizzesService(IDatabaseSettings dbSettings)
        {
            var client = new MongoClient(dbSettings.ConnectionString);
            var database = client.GetDatabase(dbSettings.DatabaseName);

            _quizzes = database.GetCollection<Quiz>(dbSettings.QuizzesCollectionName);
        }
        public async Task<Quiz> Add(Quiz quiz)
        {
            await _quizzes.InsertOneAsync(quiz);
            return quiz;
        }

        public async Task<List<Quiz>> GetAll()
        {
            var leaders = await _quizzes.Find(_ => true).ToListAsync();

            return leaders;
        }

        public async Task<Quiz> GetById(string id)
        {
            var cursor = await _quizzes.FindAsync(x => x.Id == id);

            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<Quiz> Next()
        {
            var cursor = await _quizzes.FindAsync(x => !x.IsStarted);

            return await cursor.FirstOrDefaultAsync();
        } 
    }
}
