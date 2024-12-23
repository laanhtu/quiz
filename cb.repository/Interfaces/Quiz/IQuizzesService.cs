using CP.Api.Models; 
using Elsa.Entities.Quiz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.Repository.Interfaces
{
    public interface IQuizzesService
    {
        Task<Quiz> Add(Quiz quiz);

        Task<List<Quiz>> GetAll();

        /// <summary>
        /// Get next quiz (random) that is never started. (Quiz.IsStarted = false)
        /// </summary>
        /// <returns></returns>
        Task<Quiz> Next();
        Task<Quiz> GetById(string id);
    }
}
