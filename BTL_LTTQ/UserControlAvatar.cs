using BTL_LTTQ;
using Client.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class UserControlAvatar : UserControl
    {
        public string FrID;
        public string MyID;
        public string status = "";
        public UserControlAvatar(string myid, string Yourid, string username, string stt, Image avatarImage)
        {
            InitializeComponent();
            lbAvt.Text = username;
            picAvt.Image = MakeCircle.MakeCircularImage(avatarImage, picAvt); 
            FrID = Yourid;
            MyID = myid;
            status = stt;
            if (stt == "1") picIcon.Image = Properties.Resources.IconNewMessage;
        }
        // Thuộc tính công khai để thiết lập tên đăng nhập
        public string Username
        {
            get => lbAvt.Text;
            set => lbAvt.Text = value;
        }

        // Thuộc tính công khai để thiết lập ảnh đại diện

        // Hàm tạo ảnh tròn       

        private void UserControlAvatar_Click(object sender, EventArgs e)
        {
            ClientChat chat = new ClientChat(MyID, FrID, status);
            chat.Show();
        }

        private void picAvt_Click(object sender, EventArgs e)
        {
            ClientChat chat = new ClientChat(MyID, FrID, status);
            chat.Show();           
        }
    }
}
