using Elsa.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Elsa.Entities.Quiz;
using MongoDB.Bson;

namespace Client
{
    public partial class Main : Form
    {
        private ClientWebSocket webSocket;
        private BindingList<QuizLeader> leaderBindingSource = new BindingList<QuizLeader>();

        private BindingList<SessionParticipant> sessionParticipantsBindingSource = new BindingList<SessionParticipant>();

        public static LoginResponse loginInfo;
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            dataGridViewLeaders.AutoGenerateColumns = false;
            dataGridViewLeaders.DataSource = leaderBindingSource;

            dataGridViewParticipants.AutoGenerateColumns = false;
            dataGridViewParticipants.DataSource = sessionParticipantsBindingSource;

            DialogResult result = DialogResult.None;

            Login login = new Login();

            while (result != DialogResult.OK)
            {
                result = login.ShowDialog();

                if (result == DialogResult.OK)
                {
                    loginInfo = login.LoginResult;

                    Text = string.Format("Elsa Quiz - Welcome {0} {1}", loginInfo.LastName, loginInfo.FirstName);

                    Task.Factory.StartNew(ReceiveMessages);
                    Task.Factory.StartNew(LoadLeaders);
                }
                else if (result == DialogResult.Cancel)
                {
                    Close();

                    return;
                }
            }
        }

        private async void ReceiveMessages()
        {
            webSocket = new ClientWebSocket();
            Uri serverUri = new Uri("wss://localhost:44309/ws");

            await webSocket.ConnectAsync(serverUri, CancellationToken.None);
            var buffer = new byte[1024];

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string strMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    var message = JsonConvert.DeserializeObject<Elsa.Entities.SocketMessage>(strMessage);

                    ProcessMessage(message);
                }
            }
        }

        private async void LoadLeaders()
        {
            HttpClient client = new HttpClient { Timeout = TimeSpan.FromMinutes(3) };

            var response = await client.GetAsync(Config.BaseURL + "/quiz/leader/list");

            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var leaders = JsonConvert.DeserializeObject<List<QuizLeader>>(responseContent);

                Invoke(new MethodInvoker(delegate
                {
                    leaderBindingSource.Clear();

                    foreach (var leader in leaders)
                    {
                        leaderBindingSource.Add(leader);
                    }

                }));
            }
        }

        private async Task ProcessMessage(SocketMessage message)
        {
            if (message != null)
            {
                if (message.Type == SocketMessageType.Quiz_Session_UserJoined)
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        var newUser = new SessionParticipant { FullName = message.FullName, UserId = message.UserId };

                        sessionParticipantsBindingSource.Add(newUser);
                    }));
                }
                else if (message.Type == SocketMessageType.Quiz_Session_SessionStarted)
                {
                    if (MessageBox.Show($"New Quiz \"{message.QuizName}\" is started." + Environment.NewLine + "Do you want to join?", "New quiz session is started", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Invoke(new MethodInvoker(delegate
                        {
                            sessionParticipantsBindingSource.Clear();
                        }));
                        currentSessionId = message.SessionId;
                        // Join quiz session 
                        await JoinQuizSession(message.SessionId);
                        // Load quiz
                        string quizId = message.QuizId;
                        await LoadQuiz(quizId);

                    }
                }
                else if (message.Type == SocketMessageType.Quiz_Session_SessionSubmitted)
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        if (currentSessionId == message.SessionId)
                        {
                            var user = sessionParticipantsBindingSource.FirstOrDefault(x => x.UserId == message.UserId);
                            if (user != null)
                            {
                                user.Score = message.Score;
                                sessionParticipantsBindingSource.ResetBindings();
                                dataGridViewParticipants.Refresh();
                            }
                        }

                    }));
                }
                else if (message.Type == SocketMessageType.Quiz_Leaders_ScoreRecored)
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        var user = leaderBindingSource.FirstOrDefault(x => x.Id == message.UserId);

                        if (user != null)
                        {
                            user.Score = message.Score;
                            user.TotalTime = message.Duration;

                            leaderBindingSource.ResetBindings();

                            dataGridViewLeaders.Refresh();
                        } 
                    }));
                }
            }
        }

        private string currentSessionId;
        private Quiz currentQuiz;

        private async Task LoadQuiz(string quizId)
        {
            HttpClient client = new HttpClient { Timeout = TimeSpan.FromMinutes(3) };

            var response = await client.GetAsync(Config.BaseURL + "/quizzes/" + quizId);

            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var quiz = JsonConvert.DeserializeObject<Quiz>(responseContent);


                Invoke(new MethodInvoker(delegate
                {
                    currentQuiz = quiz;

                    btnSubmit.Enabled = true;

                    lblQuizName.Text = "Quiz: " + quiz.Name;
                    lblQuizDesc.Text = "Description: " + quiz.Description;

                    int bottomPrevious = lblQuizDesc.Bottom + 10;

                    foreach (var question in quiz.Questions)
                    {
                        QuizItemControl quizItemControl = new QuizItemControl(question);
                        quizItemControl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                        quizItemControl.Top = bottomPrevious;
                        quizItemControl.Width = splitContainer1.Panel2.Width - 5;
                        splitContainer1.Panel2.Controls.Add(quizItemControl);
                        bottomPrevious = quizItemControl.Bottom + 5;
                    }
                }));
            }
        }

        private async Task JoinQuizSession(string sessionId)
        {
            var requestData = new
            {
                UserId = loginInfo.UserId,
                SessionId = sessionId,
            };

            HttpClient client = new HttpClient { Timeout = TimeSpan.FromMinutes(10) };

            var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(Config.BaseURL + "/quiz/session/join", content);

            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var badRequestResponse = JsonConvert.DeserializeObject<BadRequestResponse>(responseContent);
            }
            else if (response.StatusCode == HttpStatusCode.OK)
            {
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseContent);
            }
        }
        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (webSocket != null)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);

                webSocket.Dispose();
            }
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            if (currentQuiz != null)
            {
                Invoke(new MethodInvoker(delegate
                {
                    btnSubmit.Enabled = false;
                }));

                await SubmitQuiz();

                Invoke(new MethodInvoker(delegate
                {
                    btnSubmit.Enabled = true;
                }));
            }
        }

        private async Task SubmitQuiz()
        {
            if (currentSessionId != null && currentQuiz != null)
            {
                SubmitSessionRequest data = new SubmitSessionRequest();
                data.SessionId = currentSessionId;
                data.Quiz = currentQuiz;
                data.UserId = loginInfo.UserId;

                HttpClient client = new HttpClient { Timeout = TimeSpan.FromMinutes(10) };

                var content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(Config.BaseURL + "/quiz/session/submit", content);

                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var badRequestResponse = JsonConvert.DeserializeObject<BadRequestResponse>(responseContent);
                }
                else if (response.StatusCode == HttpStatusCode.OK)
                {
                    var submittedRecord = JsonConvert.DeserializeObject<ParticipantRecord>(responseContent);
                    var userSubmitted = sessionParticipantsBindingSource.FirstOrDefault(x => x.UserId == submittedRecord.UserId);
                    if (userSubmitted != null)
                    {
                        userSubmitted.Score = submittedRecord.Score;

                        Invoke(new MethodInvoker(delegate
                        {
                            sessionParticipantsBindingSource.ResetBindings();
                            dataGridViewParticipants.Refresh();
                        }));
                    }
                }
            }
        }
    }

    public class SessionParticipant
    {
        public string UserId { get; set; }
        public string FullName { get; set; }

        public int Score { get; set; }
    }
}
