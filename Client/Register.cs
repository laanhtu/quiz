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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Register : Form
    {
        public string Username { get; private set; }
        public Register()
        {
            InitializeComponent();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            var requestData = new
            {
                firstname = txtFirstName.Text,
                lastname = txtLastName.Text,
                username = txtUsername.Text,
                password = txtPassword.Text,
            };

            HttpClient client = new HttpClient { Timeout = TimeSpan.FromMinutes(10) };

            var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");

            var requestId = DateTime.Now.Ticks.ToString(); 

            var response = await client.PostAsync(Config.BaseURL + "/account/register", content);

            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                MessageBox.Show(string.Format("Another user with username {0} is existed in system", txtUsername.Text), "Cannot register user", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
            else if (response.StatusCode == HttpStatusCode.OK)
            {
                DialogResult = DialogResult.OK;
                Username = txtUsername.Text;
                Close();
            }
        }
    }

    public class RegisterResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
