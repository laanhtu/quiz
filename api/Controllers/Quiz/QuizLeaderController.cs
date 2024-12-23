using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Elsa.Repository.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Elsa.entities.cache;
using CP.Api.Models;
using System.Collections.Generic;
using Elsa.Repository.Services;

namespace Elsa.Api.Controllers
{
    [ApiController]
    [Route("api/quiz/leader")]
    public class QuizLeaderController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(QuizSessionController));

        private IQuizLeadersServiceTransient sessionServiceTransient;
        private IQuizLeadersServiceScoped sessionServiceScoped;
        private IQuizLeadersServiceSingleton sessionServiceSingleton;
        private MemoryCache _cache;

        public QuizLeaderController(IQuizLeadersServiceTransient _leaderServiceTransient, IQuizLeadersServiceScoped _leaderServiceScoped, IQuizLeadersServiceSingleton _leaderServiceSingleton, MemoryCache cache)
        {
            sessionServiceTransient = _leaderServiceTransient;
            sessionServiceScoped = _leaderServiceScoped;
            sessionServiceSingleton = _leaderServiceSingleton;
            _cache = cache;
        }

        /// <summary>
        /// List all leader of quiz
        /// </summary>
        /// <param name="joinSession"></param>
        /// <returns></returns>
        [Route("list"), HttpGet()]
        public async Task<IActionResult> List()
        {
            var result = await sessionServiceSingleton.List();
            var usersDict = _cache.Get(CacheKeys.Users) as Dictionary<string, User>;
            for (int i = 0; i < result.Count; i++)
            {
                if (usersDict.ContainsKey(result[i].Id))
                {
                    var user = usersDict[result[i].Id];
                    result[i].FullName = user.LastName + " " + user.FirstName;
                }
            }

            return Ok(result);
        }
    }
}
