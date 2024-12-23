using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Login : Form
    {

        public LoginResponse LoginResult { get; private set; }
        public Login()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            var requestData = new
            {
                username = txtUsername.Text,
                password = txtPassword.Text,
            };

            HttpClient client = new HttpClient { Timeout = TimeSpan.FromMinutes(10) };

            var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");

            var requestId = DateTime.Now.Ticks.ToString();

            content.Headers.Add("requestId", requestId);

            var response = await client.PostAsync(Config.BaseURL + "/account/authenticate", content);

            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var badRequestResponse = JsonConvert.DeserializeObject<BadRequestResponse>(responseContent);
                lblError.Text = badRequestResponse.Message;
                btnRegister.Visible = true;
            }
            else if (response.StatusCode == HttpStatusCode.OK)
            {
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseContent);
                LoginResult = loginResponse;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            Register register = new Register();

            if (register.ShowDialog() == DialogResult.OK)
            {
                txtUsername.Text = register.Username;
                txtPassword.Text = string.Empty;
                btnRegister.Visible = false;
            }
        }
    }

    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class BadRequestResponse
    {
        public string Message { get; set; }
    }
}
