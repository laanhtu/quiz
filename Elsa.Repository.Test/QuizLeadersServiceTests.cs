using Elsa.Entities.Quiz;
using Elsa.Repository.Services;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace Elsa.Repository.Test
{
    public class QuizLeadersServiceTests : BaseTest
    {
        [Fact]
        public void List_Leader_ReturnsAdded()
        {
            // Arrange  
            var service = new QuizLeadersService(databaseSetting);
            var userId = ObjectId.GenerateNewId().ToString();
            var record = new ParticipantRecord { UserId = userId, Score = 0, Duration = new TimeSpan(0, 3, 5) };
            service.Record(record).GetAwaiter().GetResult();

            // Act  
            var leaders = service.List().GetAwaiter().GetResult();

            // Assert  
            Assert.True(leaders.Count > 0);
            Assert.Contains(leaders, x => x.Id == userId);
        }

        [Fact]
        public void Record_LeaderRecords_ReturnsLeaderWithScoreIsUpdated()
        {
            // Arrange  
            var service = new QuizLeadersService(databaseSetting);
            var userId = ObjectId.GenerateNewId().ToString();

            var record = new ParticipantRecord { UserId = userId, Score = 0 };
            var record1 = new ParticipantRecord { UserId = userId, Score = 8, Duration = new TimeSpan(0, 5, 5) };
            var record2 = new ParticipantRecord { UserId = userId, Score = 9, Duration = new TimeSpan(0, 6, 55) };


            // Act  
            service.Record(record).GetAwaiter().GetResult();
            service.Record(record1).GetAwaiter().GetResult();
            var currentLeader = service.Record(record2).GetAwaiter().GetResult();


            // Assert  
            Assert.NotNull(currentLeader);
            Assert.Equal(userId, currentLeader.Id);
            Assert.Equal(record.Score + record1.Score + record2.Score, currentLeader.Score);
            Assert.Equal(record.Duration + record1.Duration + record2.Duration, currentLeader.TotalTime);
            Assert.Equal(3, currentLeader.NumberOfQuiz);
        }
    }
}