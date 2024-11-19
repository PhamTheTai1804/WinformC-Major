using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTL_LTTQ.Classes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace Client
{
    public partial class LoginForm : MaterialSkin.Controls.MaterialForm
    {
        DataBaseProcess db = new DataBaseProcess();
        public LoginForm()
        {
            InitializeComponent();
            if (Properties.Settings.Default.IsRemembered)
            {

                txtUsername.Text = Properties.Settings.Default.UserName;
                txtPassword.Text = Properties.Settings.Default.Password;
                chkRememberMe.Checked = true;
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string Result = "";
            IPEndPoint IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9998);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                client.Connect(IP);
                string RequestLogin = "#Login" + username + "," + password;
                byte[] data = Encoding.UTF8.GetBytes(RequestLogin);
                client.Send(data);
                byte[] dataReturn = new byte[1024 * 5000];
                int bytesRead = client.Receive(dataReturn);
                Result = Encoding.UTF8.GetString(dataReturn, 0, bytesRead);
            }
            catch
            {
                MessageBox.Show("Can't connect to server !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally { client.Close(); };

            if (Result != "NotFound")
            {
                if (chkRememberMe.Checked)
                {
                    Properties.Settings.Default.UserName = username;
                    Properties.Settings.Default.IsRemembered = true;
                    Properties.Settings.Default.Password = password;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.UserName = string.Empty;
                    Properties.Settings.Default.Password = string.Empty;
                    Properties.Settings.Default.IsRemembered = false;
                    Properties.Settings.Default.Save();
                }
                ClientIndex index = new ClientIndex(Result);
                index.Show();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
            }
        }
        private void lblSignUp_Click(object sender, EventArgs e)
        {
            // Mở form đăng ký hoặc xử lý điều hướng
            RegisterForm regis = new RegisterForm();
            regis.Show();
        }
    }
}
