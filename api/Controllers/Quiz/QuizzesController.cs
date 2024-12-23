using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Elsa.Repository.Interfaces;
using Elsa.Entities.Quiz;

namespace Elsa.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizzesController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(QuizzesController));
        private IQuizzesService _quizzesService;

        public QuizzesController(IQuizzesService quizzesService)
        {
            _quizzesService = quizzesService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz>> Get(string id)
        {
            log.Debug("Get Quiz by id: " + id);
            var quiz = await _quizzesService.GetById(id);

            if (quiz == null)
            {
                return NotFound();
            }

            return Ok(quiz);
        }

        [Route("list"), HttpGet()]
        public async Task<ActionResult<Quiz>> List()
        {
            var quizzes = await _quizzesService.GetAll();

            return Ok(quizzes);
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody]Quiz book)
        {
            if (book == null)
            {
                return StatusCode(409);
            }

            var result = await _quizzesService.Add(book);

            return Ok(result);
        } 
    } 
}
