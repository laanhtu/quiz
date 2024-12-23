using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Threading;
using System;
using System.Threading.Tasks;
using Elsa.Repository.Interfaces;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Elsa.Entities;
using Elsa.Entities.Quiz;
using Microsoft.Extensions.Caching.Memory;
using Amazon.Runtime.Internal.Util;
using Elsa.entities.cache;
using CP.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;

namespace Elsa.Api.Controllers
{
    public class WebSocketController : Controller
    {
        private WebSocket socket;
        private IQuizSessionsServiceSingleton _sessionServiceSingleton;
        private IQuizLeadersServiceSingleton _quizLeadersServiceSingleton;
        private MemoryCache _cache;

        public WebSocketController(IQuizSessionsServiceSingleton sessionServiceSingleton, IQuizLeadersServiceSingleton quizLeadersServiceSingleton, MemoryCache cache)
        {
            _sessionServiceSingleton = sessionServiceSingleton;
            _quizLeadersServiceSingleton = quizLeadersServiceSingleton;

            _cache = cache;

            // Register event on session service.
            _sessionServiceSingleton.SessionStarted += _sessionServiceSingleton_SessionStarted;
            _sessionServiceSingleton.UserJoined += _sessionServiceSingleton_UserJoined;
            _sessionServiceSingleton.SessionSubmitted += _sessionServiceSingleton_SessionSubmitted;

            // Register event on leader service
            _quizLeadersServiceSingleton.ScoreRecorded += _quizLeadersServiceSingleton_ScoreRecorded;
        }

        private async void _sessionServiceSingleton_SessionStarted(object sender, SessionStartedEventArgs e)
        {
            var jsonString = JsonSerializer.Serialize(new SocketMessage { Type = SocketMessageType.Quiz_Session_SessionStarted, SessionId = e.SessionId, QuizName = e.QuizName, QuizId = e.QuizId });

            var buffer = Encoding.UTF8.GetBytes(jsonString);

            var arraySegment = new ArraySegment<byte>(buffer);

            await socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async void _sessionServiceSingleton_UserJoined(object sender, UserJoinedEventArgs e)
        {
            var usersDict = _cache.Get(CacheKeys.Users) as Dictionary<string, User>;

            string userFullName = null;
            if (usersDict.ContainsKey(e.UserId))
            {
                userFullName = usersDict[e.UserId].LastName + " " + usersDict[e.UserId].FirstName;
            }
            var jsonString = JsonSerializer.Serialize(new SocketMessage { Type = SocketMessageType.Quiz_Session_UserJoined, UserId = e.UserId, SessionId = e.SessionId, FullName = userFullName });

            var buffer = Encoding.UTF8.GetBytes(jsonString);

            var arraySegment = new ArraySegment<byte>(buffer);

            await socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async void _sessionServiceSingleton_SessionSubmitted(object sender, SessionSubmittedEventArgs e)
        {
            var jsonString = JsonSerializer.Serialize(new SocketMessage { Type = SocketMessageType.Quiz_Session_SessionSubmitted, UserId = e.UserId, SessionId = e.SessionId, Score = e.Score, Duration = e.Duration });

            var buffer = Encoding.UTF8.GetBytes(jsonString);

            var arraySegment = new ArraySegment<byte>(buffer);

            await socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        private async void _quizLeadersServiceSingleton_ScoreRecorded(object sender, ScoreRecoredEventArgs e)
        {
            var jsonString = JsonSerializer.Serialize(new SocketMessage { Type = SocketMessageType.Quiz_Leaders_ScoreRecored, UserId = e.UserId, SessionId = e.SessionId, Score = e.Score, Duration = e.Duration });

            var buffer = Encoding.UTF8.GetBytes(jsonString);

            var arraySegment = new ArraySegment<byte>(buffer);

            await socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        [Route("/ws")]
        public async Task Get()
        {
            //if (HttpContext.WebSockets.IsWebSocketRequest)
            //{
            //    using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            //    await Echo(webSocket);
            //}
            //else
            //{
            //    HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            //}
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var buffer = new byte[1024 * 4];
                var receiveResult = await socket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private static async Task Echo(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(
                    new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                    receiveResult.MessageType,
                    receiveResult.EndOfMessage,
                    CancellationToken.None);

                //receiveResult = await webSocket.ReceiveAsync(
                //    new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }
    }
}
