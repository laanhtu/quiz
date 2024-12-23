using Elsa.Entities.Quiz;
using System.Threading.Tasks;


namespace Elsa.Repository.Interfaces
{
    public interface IAIQuizService
    {
        Task<Quiz> Generate(string about);
    }
}