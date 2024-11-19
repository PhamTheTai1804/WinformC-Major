
using Client.Classes;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Client
{
    partial class Profile : MaterialForm
    {
        private PictureBox picAVT;
        private Panel panelInfo;
        private Panel panelFriend;
        MaterialLabel LoginName,UserName,Birth,Location,Hobbys;
        MaterialRaisedButton EditUserName,EditPassword;
        public void InitializeComponent()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue500, Primary.Blue700, Primary.Blue200, Accent.LightBlue200, TextShade.WHITE);
            this.MaximizeBox = false; // Vô hiệu hóa nút phóng to
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new System.Drawing.Size(650, 920);
            this.AutoScroll = true;
            panelInfo = new Panel();
            panelInfo.Size=new Size(650,460);
            panelInfo.Location = new Point(0, 60);
            panelInfo.AutoScroll = true;
            this.Controls.Add(panelInfo);
            panelFriend = new Panel();
            panelFriend.Size = new Size(650, 360);
            panelFriend.Location = new Point(0, 520);
            panelFriend.AutoScroll = true;
            this.Controls.Add(panelFriend);
            var lblHeader = new Label
            {
                Text = "Thông Tin Cá Nhân",
                Font = new Font("Segoe UI", 26, FontStyle.Bold), // Font lớn và đậm
                Location = new System.Drawing.Point(110, 30), // Căn giữa header
                AutoSize = true,
                ForeColor = Color.FromArgb(33, 150, 243) // Màu xanh dịu mắt
            };
            panelInfo.Controls.Add(lblHeader);
            picAVT = new PictureBox();
            picAVT.SizeMode = PictureBoxSizeMode.StretchImage;  
            picAVT.Location = new Point(250, 100);
            picAVT.Size = new Size(120, 120);
            panelInfo.Controls.Add(picAVT);
            var lblLoginName = new MaterialLabel
            {
                Text = "Tên Đăng Nhập : ",
                Location = new System.Drawing.Point(100, 275),
                Font = new System.Drawing.Font("Roboto", 11),
                Width = 150
            };           
            panelInfo.Controls.Add(lblLoginName);
            LoginName = new MaterialLabel
            {
                Text = "",
                Location = new System.Drawing.Point(300, 275),
                Font = new System.Drawing.Font("Roboto", 11),
                Width = 300
            };
            panelInfo.Controls.Add(LoginName);
            var lblUserName = new MaterialLabel
            {
                Text = "Họ Và Tên : ",
                Location = new System.Drawing.Point(100, 325),
                Font = new System.Drawing.Font("Roboto", 11),
                Width = 150
            };
            panelInfo.Controls.Add(lblUserName);
            UserName = new MaterialLabel
            {
                Text = "",
                Location = new System.Drawing.Point(300, 325),
                Font = new System.Drawing.Font("Roboto", 11),
                Width = 300
            };
            panelInfo.Controls.Add(UserName);
            var lblBirth = new MaterialLabel
            {
                Text = "Ngày Sinh : ",
                Location = new System.Drawing.Point(100, 375),
                Font = new System.Drawing.Font("Roboto", 11),
                Width = 150
            };
            panelInfo.Controls.Add(lblBirth);
            Birth = new MaterialLabel
            {
                Text = "",
                Location = new System.Drawing.Point(300, 375),
                Font = new System.Drawing.Font("Roboto", 11),
                Width = 300
            };
            panelInfo.Controls.Add(Birth);
            var lblLocation = new MaterialLabel
            {
                Text = "Địa Chỉ : ",
                Location = new System.Drawing.Point(100, 425),
                Font = new System.Drawing.Font("Roboto", 11),
                Width = 150
            };
            panelInfo.Controls.Add(lblLocation);
            Location = new MaterialLabel
            {
                Text = "",
                Location = new System.Drawing.Point(300, 425),
                Font = new System.Drawing.Font("Roboto", 11),
                Width = 300
            };
            panelInfo.Controls.Add(Location);
            var lblHobbys = new MaterialLabel
            {
                Text = "Sở Thích : ",
                Location = new System.Drawing.Point(100, 475),
                Font = new System.Drawing.Font("Roboto", 11),
                Width = 150
            };
            panelInfo.Controls.Add(lblHobbys);           
        }
    }
}
