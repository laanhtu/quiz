using Elsa.Entities.Quiz;
using Elsa.Repository.Services;
using MongoDB.Bson;

namespace Elsa.Repository.Test
{
    public class QuizSessionsServiceTests : BaseTest
    {
        QuizzesService quizService;
        public QuizSessionsServiceTests()
        {
            // Do "global" initialization here; Called before every test method.
            quizService = new QuizzesService(databaseSetting);
        }

        [Fact]
        public void Add_NewSession_ReturnsAdded()
        {
            // Arrange  
            var quiz = quizService.Add(new Quiz { IsStarted = false, Name = "Quiz Test 1", Description = string.Format("Quiz description for {0}", nameof(QuizSessionsServiceTests.Add_NewSession_ReturnsAdded)) }).GetAwaiter().GetResult();
            var service = new QuizSessionsService(databaseSetting, quizService);
            var create = new QuizSession { QuizId = quiz.Id };
            // Act  
            var result = service.Start(create).GetAwaiter().GetResult();

            // Assert  
            Assert.NotNull(result.Id);
        }

        [Fact]
        public void Get_Session_ReturnsExistingSession()
        {
            // Arrange  
            var quiz = quizService.Add(new Quiz { IsStarted = false, Name = "Quiz Test 1", Description = string.Format("Quiz description for {0}", nameof(QuizSessionsServiceTests.Get_Session_ReturnsExistingSession)) }).GetAwaiter().GetResult();
            var service = new QuizSessionsService(databaseSetting, quizService);
            var create = new QuizSession { QuizId = quiz.Id };
            // Act  
            var result = service.Start(create).GetAwaiter().GetResult();
            var existing = service.GetSession(result.Id).GetAwaiter().GetResult();
            // Assert  
            Assert.NotNull(result.Id);
            Assert.NotNull(existing);
        }

        [Fact]
        public void Join_Session_ReturnsExistingSession()
        {
            // Arrange  
            var quiz = quizService.Add(new Quiz { IsStarted = false, Name = "Quiz Test 1", Description = string.Format("Quiz description for {0}", nameof(QuizSessionsServiceTests.Join_Session_ReturnsExistingSession)) }).GetAwaiter().GetResult();
            var service = new QuizSessionsService(databaseSetting, quizService);
            var create = new QuizSession { QuizId = quiz.Id };
            // Act  
            var result = service.Start(create).GetAwaiter().GetResult();
            var existing = service.GetSession(result.Id).GetAwaiter().GetResult();
            var paticipant = new ParticipantRecord { Duration = new TimeSpan(0, 5, 10), Score = 5, UserId = ObjectId.GenerateNewId().ToString() };
            service.Join(existing.Id, paticipant).GetAwaiter().GetResult();

            var exitingWithPaticipant = service.GetSession(existing.Id).GetAwaiter().GetResult();
            // Assert  

            Assert.NotNull(exitingWithPaticipant.ParticipantRecords);
            Assert.Single(exitingWithPaticipant.ParticipantRecords);
            Assert.Equal(exitingWithPaticipant.ParticipantRecords[0].UserId, paticipant.UserId);
        }

        [Fact]
        public void Submit_Session_ReturnsSubmitted()
        {
            // Arrange  
            var quiz = quizService.Add(new Quiz { IsStarted = false, Name = "Quiz Test 1", Description = string.Format("Quiz description for {0}", nameof(QuizSessionsServiceTests.Submit_Session_ReturnsSubmitted)) }).GetAwaiter().GetResult();
            var service = new QuizSessionsService(databaseSetting, quizService);
            var create = new QuizSession { QuizId = quiz.Id };
            // Act  
            var result = service.Start(create).GetAwaiter().GetResult();
            var existing = service.GetSession(result.Id).GetAwaiter().GetResult();
            var paticipant = new ParticipantRecord { Duration = new TimeSpan(0, 0, 0), Score = 0, UserId = ObjectId.GenerateNewId().ToString() };
            service.Join(existing.Id, paticipant).GetAwaiter().GetResult();

            paticipant.Score = 8;
            paticipant.Duration = new TimeSpan(0, 8, 9);
            var submitted = service.Submit(existing.Id, paticipant).GetAwaiter().GetResult();
            var exitingWithPaticipant = service.GetSession(existing.Id).GetAwaiter().GetResult();
            // Assert  

            Assert.NotNull(exitingWithPaticipant.ParticipantRecords);
            Assert.Single(exitingWithPaticipant.ParticipantRecords);
            Assert.Equal(exitingWithPaticipant.ParticipantRecords[0].UserId, paticipant.UserId);
            Assert.Equal(exitingWithPaticipant.ParticipantRecords[0].Score, paticipant.Score);
            Assert.Equal(paticipant.Score, submitted.Score);
            Assert.Equal(paticipant.Duration, submitted.Duration);
        }
    }
}