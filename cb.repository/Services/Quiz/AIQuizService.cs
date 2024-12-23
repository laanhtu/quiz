
using Elsa.Repository.Interfaces;
using Elsa.Entities.Quiz;
using System;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Elsa.Repository.Services
{
    public class ResponsedContent
    {
        public string response { get; set; }
    }

    public class AIQuizService : IAIQuizService
    {
        public async Task<Quiz> Generate(string about)
        {
            var requestData = new
            {
                model = "llama3.1",
                prompt = string.Format("Help me to generate a quiz with 5 questions about \"{0}\". A quiz has a name, description, 5 questions. Each question has an answer for it.", about) +
                            "Always answer in the following json format:" +
                            "{" +
                            "\"name\": \"name of quiz\"," +
                            "\"description\": \"description of quiz\"," +
                            "\"questions\": [" +
                            "{" +
                            " \"question\": \"question for audience to think\"," +
                            "\"choices\": [" +
                            "\"a. posible answer for question\"," +
                            "\"b. posible answer for question\", ..." +
                            "]," +
                            "\"correctChoice\": 1 or 2 or 3..." +
                            "}," +
                            "...." +
                            "]" +
                            "}" +
                            "Return ONLY vaid JSON. No explanation or other text is allowed.",

                stream = false
            };

            HttpClient client = new HttpClient { Timeout = TimeSpan.FromMinutes(10) };

            var content = new StringContent(JsonSerializer.Serialize(requestData), System.Text.Encoding.UTF8, "application/json");

            var requestId = DateTime.Now.Ticks.ToString();

            content.Headers.Add("requestId", requestId);

            var response = await client.PostAsync("http://localhost:11434/api/generate", content);

            string responseContent = await response.Content.ReadAsStringAsync();

            var generatedContent = JsonSerializer.Deserialize<ResponsedContent>(responseContent);

            try
            {
                var result = JsonSerializer.Deserialize<Quiz>(generatedContent.response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result;
            }
            catch (Exception ex)
            { 
                return null;
            }
        }

        
    }

 

}
