using Elsa.Api.Helpers;

namespace Elsa.Repository.Test
{
    public class BaseTest
    {
        protected DatabaseSettings databaseSetting = new DatabaseSettings
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "Elsa_Test",
            QuizzesCollectionName = "Quizzes",
            QuizSessionsCollectionName = "QuizSessions",
            QuizLeadersCollectionName = "QuizLeaders"
        };
    }
}