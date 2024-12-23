namespace Elsa.Api.Helpers
{
    public interface IDatabaseSettings
    {
        string RoleCollectionName { get; set; }
        string UserCollectionName { get; set; }
        string ElementCollectionName { get; set; }

        string CategoryCollectionName { get; set; }

        string BookCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }

        string QuizSessionsCollectionName { get; set; }
        string QuizzesCollectionName { get; set; }
        string QuizLeadersCollectionName { get; set; }
    }

    public class DatabaseSettings : IDatabaseSettings
    {
        public string BookCollectionName { get; set; }
        public string ElementCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string RoleCollectionName { get; set; }
        public string UserCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string QuizSessionsCollectionName { get ; set ; }
        public string QuizzesCollectionName { get; set; }
        public string QuizLeadersCollectionName { get; set; }
    }
}