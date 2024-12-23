using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Elsa.Repository.Interfaces;

namespace Elsa.Api.Controllers
{
    [ApiController]
    [Route("api/ai/quiz")]
    public class AIQuizController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AIQuizController));

        private IAIQuizService _aIQuizService;
        private IQuizzesService _quizzesService;

        public AIQuizController(IAIQuizService aIQuizService, IQuizzesService quizzesService)
        {
            _aIQuizService = aIQuizService;
            _quizzesService = quizzesService;
        }

        /// <summary>
        /// Generate a quiz and save it into database
        /// </summary>
        /// <param name="joinSession"></param>
        /// <returns></returns>
        [Route("generate"), HttpPost()]
        public async Task<IActionResult> Generate(GenerateQuizRequest requestData)
        {
            var generatedQuiz = await _aIQuizService.Generate(requestData.QuizAbout);

            if (generatedQuiz != null)
            {
                var added = await _quizzesService.Add(generatedQuiz);

                return Ok(added);
            }
            else
            {
                return StatusCode(500, "AI cannot generate quiz at this time, please try again later");
            }
        }

        public class GenerateQuizRequest
        {
            public string QuizAbout { get; set; }
        }
    }
}
