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
using System;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;
using Client.Classes;

namespace Client
{
    public partial class RegisterForm : MaterialSkin.Controls.MaterialForm
    {
        public RegisterForm()
        {
            InitializeComponent();

        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            bool isValid = true;
            if (txtHoTen.Text == string.Empty)
            {
                lbErrorName.Visible = true;
                isValid = false;
            }
            else
            {
                lbErrorName.Visible = false;
            }
            if (cbDiaChi.Text == string.Empty)
            {
                lbErrorLocation.Visible = true;
                isValid = false;
            }
            else
            {
                lbErrorLocation.Visible = false;
            }

            
            if (CheckUserNameExist(txtTenDangNhap.Text))
            {
                lbErrorUserName.Visible = true;
                isValid = false;
            }
            else
            {
                lbErrorUserName.Visible = false;
            }

            // Kiểm tra khớp mật khẩu
            if (txtMatKhau.Text != txtXacNhanMatKhau.Text)
            {
                lbErrorCfPass.Visible = true;
                isValid = false;
            }
            else
            {
                lbErrorCfPass.Visible = false;
            }

            // Nếu tất cả đều hợp lệ, hiển thị thông báo đăng ký thành công
            if (isValid)
            {
                SendRegisterToServer();
                SendAvatarImageToServer();
            }
        }       
        private bool CheckUserNameExist(string username)
        {
            string Result = "";
            IPEndPoint IP = new IPEndPoint(IPAddress.Parse("192.168.26.149"), 9998);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                client.Connect(IP);
                string RequestLogin = "#Exist" + username;
                byte[] data = Encoding.UTF8.GetBytes(RequestLogin);
                client.Send(data);
                byte[] dataReturn = new byte[1024 * 5000];
                int bytesRead = client.Receive(dataReturn);
                Result = Encoding.UTF8.GetString(dataReturn, 0, bytesRead);
            }
            catch
            {
                MessageBox.Show("Can't connect to server !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally { client.Close(); };
            if (Result == "1") return true;
            return false;
        }
        private void SendRegisterToServer()
        {
            IPEndPoint IP = new IPEndPoint(IPAddress.Parse("192.168.26.149"), 9998);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                string hobby = "";
                foreach (var item in clbSoThich.CheckedItems)
                {
                    hobby += item.ToString() + ":";
                }
                if (txtSoThichKhac.Text != string.Empty) hobby += txtSoThichKhac.Text;
                else hobby = hobby.Substring(0, hobby.Length - 1);
                client.Connect(IP);
                string RequestLogin = "#Regis" + txtHoTen.Text + "," + dtNgaySinh.Value.ToString("dd/MM/yyyy") + "," + cbDiaChi.SelectedItem.ToString() + "," + hobby + "," + txtTenDangNhap.Text + "," + txtMatKhau.Text;
                byte[] data = Encoding.UTF8.GetBytes(RequestLogin);
                client.Send(data);
                DialogResult result = MessageBox.Show("Bạn đã đăng ký thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (result == DialogResult.OK)
                {
                    LoginForm loginForm = new LoginForm();
                    loginForm.Show();
                }
            }
            catch
            {
                MessageBox.Show("Can't connect to server !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally { client.Close(); };
        }
        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            if (ChooseImageDialog.ShowDialog() == DialogResult.OK)
            {
                // Hiển thị hình ảnh vào PictureBox
                picAVT.Image = MakeCircle.MakeCircularImage(Image.FromFile(ChooseImageDialog.FileName), picAVT); 
            }
        }
        private void SendAvatarImageToServer()
        {
            string header = "#IMAGEAVT" + standardization(txtTenDangNhap.Text).PadRight(20, '#');
            // Thêm padding để đủ độ dài cho tên người dùng
            byte[] headerBytes = Encoding.UTF8.GetBytes(header);
            byte[] imageBytes = File.ReadAllBytes(ChooseImageDialog.FileName);

            // Tổng chiều dài của dữ liệu cần gửi
            int totalLength = headerBytes.Length + imageBytes.Length;
            byte[] lengthBytes = BitConverter.GetBytes(totalLength);

            // Kết hợp độ dài và dữ liệu để gửi
            byte[] dataToSend = headerBytes.Concat(lengthBytes).Concat(imageBytes).ToArray();

            IPEndPoint IP = new IPEndPoint(IPAddress.Parse("192.168.26.149"), 9998);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                client.Connect(IP);

                // Gửi dữ liệu trong vòng lặp
                int bytesSent = 0;
                while (bytesSent < dataToSend.Length)
                {
                    int sent = client.Send(dataToSend, bytesSent, dataToSend.Length - bytesSent, SocketFlags.None);
                    if (sent == 0)
                    {
                        throw new SocketException(); // Ngắt kết nối hoặc lỗi
                    }
                    bytesSent += sent;
                }
            }
            catch
            {
                MessageBox.Show("Can't connect to server!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                client.Close();
            };
        }

        private string standardization(string username)
        {
            while(username.Length<20)
            {
                username += "#";
            }    
            return username;
        }
    }
}
/*
 *   
 */
