using Elsa.Entities.Quiz;
using Elsa.Repository.Services;

namespace Elsa.Repository.Test
{
    public class QuizzesServiceTests : BaseTest
    {
        [Fact]
        public void Add_NewQuiz_ReturnsAdded()
        {
            // Arrange  
            var service = new QuizzesService(databaseSetting);
            var add = new Quiz { Name = "Quiz name", Description = "Quiz Desc" };
            // Act  
            var added = service.Add(add).GetAwaiter().GetResult();

            // Assert  
            Assert.NotNull(added.Id);
            Assert.Equal(add.Name, added.Name);
            Assert.Equal(add.Description, added.Description);
        }

        [Fact]
        public void Next_Quiz_ReturnsJustAdded()
        {
            // Arrange  
            var service = new QuizzesService(databaseSetting);
            var add = new Quiz { Name = "Quiz name", Description = "Quiz Desc" };
            // Act  
            var added = service.Add(add).GetAwaiter().GetResult();
            var next = service.Next().GetAwaiter().GetResult();


            // Assert  
            Assert.NotNull(added.Id);
            Assert.Equal(add.Name, added.Name);
            Assert.Equal(add.Description, added.Description);

            Assert.NotNull(next);
            Assert.False(next.IsStarted);
        }

        [Fact]
        public void Add_AI_Generated_Quiz_Returns_Added()
        {
            // Arrange  
            var aiQuizService = new AIQuizService();
            var generatedQuiz = aiQuizService.Generate("habitats").GetAwaiter().GetResult();

            if (generatedQuiz != null)
            {
                var service = new QuizzesService(databaseSetting);

                // Act  
                var added = service.Add(generatedQuiz).GetAwaiter().GetResult();

                // Assert  
                Assert.NotNull(added.Id);
                Assert.Equal(generatedQuiz.Name, added.Name);
                Assert.Equal(generatedQuiz.Description, added.Description);
            }
            else
            {
                Assert.Fail("Cannot generate quiz from AI.");
            }
        }
    }
}