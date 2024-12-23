using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client;
using Newtonsoft.Json;
using Elsa.Entities.Quiz;

namespace Admin
{
    public partial class Form1 : Form
    {
        private BindingList<Quiz> quizbindingSource = new BindingList<Quiz>();
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnLoadQuizzes_Click(object sender, EventArgs e)
        {
            Invoke(new MethodInvoker(delegate
            {
                btnLoadQuizzes.Enabled = false;
                btnStart.Enabled = false;

            }));
            HttpClient client = new HttpClient { Timeout = TimeSpan.FromMinutes(3) };

            var response = await client.GetAsync(Config.BaseURL + "/quizzes/list");

            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var leaders = JsonConvert.DeserializeObject<List<Quiz>>(responseContent);

                Invoke(new MethodInvoker(delegate
                {
                    quizbindingSource.Clear();

                    foreach (var leader in leaders)
                    {
                        quizbindingSource.Add(leader);
                    }

                }));
            }

            btnLoadQuizzes.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridViewQuizzes.DataSource = quizbindingSource;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private Quiz selectedQuiz;
        private void dataGridViewQuizzes_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                selectedQuiz = quizbindingSource[e.RowIndex];

                btnStart.Enabled = true;
            } 
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            Invoke(new MethodInvoker(delegate
            {
 
                btnStart.Enabled = false;

            }));

            var requestData = new
            {
                QuizId = selectedQuiz.Id
            };

            HttpClient client = new HttpClient { Timeout = TimeSpan.FromMinutes(10) };

            var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(Config.BaseURL + "/quiz/session/start", content);

            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageBox.Show($"Quiz with name {selectedQuiz.Name} is started.", "Start Quiz Session", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Invoke(new MethodInvoker(delegate
            {

                btnStart.Enabled = true;

            }));
        }
    }
}
