using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Elsa.Repository.Interfaces;
using Elsa.Entities.Quiz;
using System;
using Elsa.Repository.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Elsa.Api.Controllers
{
    [ApiController]
    [Route("api/quiz/session")]
    public class QuizSessionController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(QuizSessionController));
        private IQuizSessionsServiceTransient sessionServiceTransient;
        private IQuizSessionsServiceScoped sessionServiceScoped;
        private IQuizSessionsServiceSingleton sessionServiceSingleton;

        private IQuizLeadersServiceSingleton _quizLeadersServiceSingleton;

        public QuizSessionController(IQuizSessionsServiceTransient _sessionServiceTransient, IQuizSessionsServiceScoped _sessionServiceScoped, IQuizSessionsServiceSingleton _sessionServiceSingleton, IQuizLeadersServiceSingleton quizLeadersServiceSingleton)
        {
            sessionServiceTransient = _sessionServiceTransient;
            sessionServiceScoped = _sessionServiceScoped;
            sessionServiceSingleton = _sessionServiceSingleton;

            _quizLeadersServiceSingleton = quizLeadersServiceSingleton;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizSession>> Get(string id)
        {
            log.Debug("Get Quiz Session by id: " + id);
            var session = await sessionServiceScoped.GetSession(id);

            if (session == null)
            {
                return NotFound();
            }

            return Ok(session);
        }

        /// <summary>
        /// Start a new Quiz session
        /// </summary>
        /// <param name="joinSession"></param>
        /// <returns></returns>
        [Route("start"), HttpPost()]
        public async Task<IActionResult> Start(StartSessionRequest startSessionRequest)
        {
            if (startSessionRequest == null)
            {
                return StatusCode(409);
            }

            var newSession = new QuizSession { QuizId = startSessionRequest.QuizId, ParticipantRecords = new System.Collections.Generic.List<ParticipantRecord>() };

            try
            {
                var result = await sessionServiceSingleton.Start(newSession);

                return Ok(result);
            }
            catch (QuizNotFoundException notFoundEx)
            {
                return StatusCode(404, notFoundEx.Message);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(404, ex.Message);
            }
        }


        [Route("join"), HttpPost()]
        public async Task<IActionResult> Join(JoinSessionRequest joinSession)
        {
            if (joinSession == null)
            {
                return StatusCode(409);
            }

            try
            {
                var result = await sessionServiceSingleton.Join(joinSession.SessionId, new ParticipantRecord { UserId = joinSession.UserId });

                return Ok(result);
            }
            catch (SessionNotFoundException notFoundEx)
            {
                return StatusCode(404, notFoundEx.Message);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(404, ex.Message);
            }
        }

        [Route("submit"), HttpPost()]
        public async Task<IActionResult> Submit(SubmitSessionRequest submitRequest)
        {
            if (submitRequest == null)
            {
                return StatusCode(409);
            }

            if (submitRequest.Quiz == null)
                return StatusCode(404, "Quiz with result should be provided.");

            try
            {
                // 1. Calculate score based on submitted result on Quiz object.
                var participantRecord = new ParticipantRecord { UserId = submitRequest.UserId, Duration = submitRequest.Duration };

                int score = 0;

                if (submitRequest.Quiz.Questions != null && submitRequest.Quiz.Questions.Count > 0)
                {
                    foreach (var question in submitRequest.Quiz.Questions)
                    {
                        if (question.CorrectChoice == question.AnswerChoice)
                        {
                            score++;
                        }
                    }
                }

                participantRecord.Score = score;

                // 2. Update record in session collection
                var submitSessionResult = await sessionServiceSingleton.Submit(submitRequest.SessionId, participantRecord);

                // 3. Update record in leader collection
                await _quizLeadersServiceSingleton.Record(participantRecord);

                return Ok(submitSessionResult);
            }
            catch (SessionNotFoundException notFoundEx)
            {
                return StatusCode(404, notFoundEx.Message);
            }
            catch (ArgumentNullException ex)
            {
                log.Error("Submit Quiz result session has error: " + ex.ToString());

                return StatusCode(404, ex.Message);
            }
            catch(Exception ex)
            {
                log.Error("Submit Quiz result session has error: " + ex.ToString());

                return StatusCode(503, "Cannot submit Quiz result");
            }
        }
    }
}
