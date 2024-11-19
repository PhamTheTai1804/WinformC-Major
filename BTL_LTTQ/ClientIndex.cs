using BTL_LTTQ.Classes;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Drawing.Drawing2D;
using MaterialSkin.Controls;
using Client.Classes;


namespace Client
{
    public partial class ClientIndex : Form
    {
        private string MyID;
        private string MyUserName;
        private List<Image> ImageList;
        private bool isTextBoxActive = false;
        public ClientIndex(string id_name)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(300, 50);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            panelFr.Visible = false;
            panelNotifications.Visible = false;
            MyID = id_name.Substring(0,3);
            MyUserName = id_name.Substring(3);
        }

        private void ClientIndex_Load(object sender, EventArgs e)
        {
            GetImageFromServer();
            LoadIndex();

            //LoadCommunityPage();
            //LoadAllUser();
            //ReceiveFriendRequest();
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void GetImageFromServer()
        {
            IPEndPoint IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9998);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                client.Connect(IP);
                string GetImage = "#IMAGEGET";
                byte[] data = Encoding.UTF8.GetBytes(GetImage);
                client.Send(data);
                ImageList = ImagesGetFromServer.ReceiveImages(client);
            }
            catch
            {
                MessageBox.Show("Can't connect to server!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { client.Close(); }
            Console.WriteLine("OK");
            int i = 1;
            foreach (var image in ImageList)
            {
                string id = i.ToString();
                while (id.Length < 3)
                {
                    id = "0" + id;
                }
                ImagesGetFromServer.Images[id] = image;
                i++;
            }

        }      
        public void LoadIndex()
        {
            string key = "#LoadF" + MyID;
            string result = GlobalCache.SharedCache.GetOrAdd(key, () =>
            {
                string serverResponse = "";
                IPEndPoint IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9998);
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

                try
                {
                    client.Connect(IP);
                    string requestLogin = "#LoadF" + MyID;
                    byte[] data = Encoding.UTF8.GetBytes(requestLogin);
                    client.Send(data);
                    byte[] dataReturn = new byte[1024 * 5000];
                    int bytesRead = client.Receive(dataReturn);
                    serverResponse = Encoding.UTF8.GetString(dataReturn, 0, bytesRead);
                }
                catch
                {
                    MessageBox.Show("Can't connect to server!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { client.Close(); }

                return serverResponse;
            });
            if (result != "NoFriend")
            {
                string[] listFr = result.Split('&');
                int lctOnl = 0;
                int lctConv = 0;
                for (int i = 0; i < listFr.Length - 1; i++)
                {
                    string[] InfoCertain = listFr[i].Split('$');
                    //UserControlAvatar receive MyID , Friend's ID , Friend's UserName , Friend's status ,Image
                    UserControlAvatar avt = new UserControlAvatar(MyID, InfoCertain[0], InfoCertain[1], InfoCertain[2], ImagesGetFromServer.Images[InfoCertain[0]]);
                    bool sender = InfoCertain[4] == MyID;
                    bool unseen = InfoCertain[2] == "1";
                    Userconversation conversation = new Userconversation(avt, InfoCertain[3], sender, unseen);
                    conversation.Location = new Point(0, lctConv);
                    lctConv += 125;
                    panelAllFr.Controls.Add(conversation);
                    if (InfoCertain[5] == "1")
                    {
                        UserControlAvatar temp = new UserControlAvatar(MyID, InfoCertain[0], InfoCertain[1], InfoCertain[2], ImagesGetFromServer.Images[InfoCertain[0]]); ;
                        temp.Location = new Point(lctOnl, 0);
                        lctOnl += 130;
                        panelOnl.Controls.Add(temp);
                    }
                }
            }
        }
        public void LoadCommunityPage()
        {
            panelKNN.Controls.Clear();
            string key = "#APIAI" + MyID;
            string Result = GlobalCache.SharedCache.GetOrAdd(key, () =>
            {
                string serverResponce = "";
                IPEndPoint IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9998);
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                try
                {
                    client.Connect(IP);
                    string RequestLogin = "#APIAI" + MyID;
                    byte[] data = Encoding.UTF8.GetBytes(RequestLogin);
                    client.Send(data);
                    byte[] dataReturn = new byte[1024 * 5000];
                    int bytesRead = client.Receive(dataReturn);
                    serverResponce = Encoding.UTF8.GetString(dataReturn, 0, bytesRead);
                }
                catch
                {
                    MessageBox.Show("Can't connect to server !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { client.Close(); };
                return serverResponce;
            });
            string[] RcmFriends = Result.Split(';');
            int lct = 28;
            foreach (string item in RcmFriends)
            {
                string[] InfoCertain = item.Split(',');
                UserProfileCard community = new UserProfileCard(MyID, InfoCertain[0], InfoCertain[1], ImagesGetFromServer.Images[InfoCertain[0]], false);
                community.Location = new Point(lct, 0);
                lct += 220;
                panelKNN.Controls.Add(community);
            }
        }

        private void ReceiveFriendRequest()
        {
            panelRequest.Controls.Clear();

            string key = "#RcvFr" + MyID;
            string Result = GlobalCache.SharedCache.GetOrAdd(key, () =>
            {
                string serverresponce = "";
                IPEndPoint IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9998);
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                try
                {
                    client.Connect(IP);
                    string RequestLogin = "#RcvFr" + MyID;
                    byte[] data = Encoding.UTF8.GetBytes(RequestLogin);
                    client.Send(data);
                    byte[] dataReturn = new byte[1024 * 5000];
                    int bytesRead = client.Receive(dataReturn);
                    serverresponce = Encoding.UTF8.GetString(dataReturn, 0, bytesRead);
                }
                catch
                {
                    MessageBox.Show("Can't connect to server !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { client.Close(); };
                return serverresponce;
            });
            if (Result != "NoFriendRequest")
            {
                string[] FriendRequests = Result.Split(';');
                int p_x = 25, p_y = 0;
                foreach (string FriendRequest in FriendRequests)
                {
                    string[] infoCertain = FriendRequest.Split(',');
                    UserProfileCard community = new UserProfileCard(MyID, infoCertain[0], infoCertain[1], ImagesGetFromServer.Images[infoCertain[0]], true);
                    community.Location = new Point(p_x, p_y);
                    p_x += 220;
                    if (p_x > panelNotifications.Width - 220) { p_x = 25; p_y += 330; }
                    panelRequest.Controls.Add(community);
                }
            }
        }
        private void LoadAllUser()
        {
            panelAddFr.Controls.Clear();
            string key = "#LdAll" + MyID;
            string Result = GlobalCache.SharedCache.GetOrAdd(key, () =>
            {
                string serverResponce = "";
                IPEndPoint IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9998);
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                try
                {

                    client.Connect(IP);
                    string RequestLogin = "#LdAll" + MyID;
                    byte[] data = Encoding.UTF8.GetBytes(RequestLogin);
                    client.Send(data);
                    byte[] dataReturn = new byte[1024 * 5000];
                    int bytesRead = client.Receive(dataReturn);
                    serverResponce = Encoding.UTF8.GetString(dataReturn, 0, bytesRead);
                }
                catch
                {
                    MessageBox.Show("Can't connect to server !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                finally { client.Close(); };
                return serverResponce;
            });
            string[] info = Result.Split(";");
            int p_x = 28, p_y = 10;
            foreach (string s in info)
            {
                string[] infoCertain = s.Split(",");
                UserProfileCard community = new UserProfileCard(MyID, infoCertain[0], infoCertain[1], ImagesGetFromServer.Images[infoCertain[0]], false);
                community.Location = new Point(p_x, p_y);
                p_x += 220;
                if (p_x > panelAddFr.Width - 220) { p_x = 28; p_y += 330; }
                panelAddFr.Controls.Add(community);
            }
        }



        private void ClientIndex_Paint(object sender, PaintEventArgs e)
        {
            Color startColor = Color.LightBlue;
            Color endColor = Color.DeepSkyBlue;

            // Tạo dải màu theo chiều ngang hoặc dọc
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, startColor, endColor, LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void btnIndex_Click(object sender, EventArgs e)
        {
            LoadIndex();
            panelIndex.Visible = true;
            panelFr.Visible = false;
            panelNotifications.Visible = false;
        }

        private void btnCommunity_Click(object sender, EventArgs e)
        {
            LoadAllUser();
            LoadCommunityPage();
            panelNotifications.Visible = false;
            panelIndex.Visible = false;
            panelFr.Visible = true;
        }

        private void btnNotifications_Click(object sender, EventArgs e)
        {
            ReceiveFriendRequest();
            panelNotifications.Visible = true;
            panelFr.Visible = false;
            panelIndex.Visible = false;
        }
        private void btnProfile_Click(object sender, EventArgs e)
        {
            UserProfileCard myprofile = new UserProfileCard(MyID, MyID, MyUserName, ImagesGetFromServer.Images[MyID], false);
            Profile MyProfile=new Profile(myprofile,false);
            MyProfile.Show();
        }
        private void panelFr_Paint(object sender, PaintEventArgs e)
        {
            Color startColor = Color.LightBlue;
            Color endColor = Color.DeepSkyBlue;

            // Tạo dải màu theo chiều ngang hoặc dọc
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, startColor, endColor, LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.White, 2); // Định dạng màu sắc và độ dày của đường thẳng
            g.DrawLine(pen, 449, 96, 1120, 96); // Vẽ đường thẳng từ (0, 50) đến (Width của form, 50)
            g.DrawLine(pen, 142, 493, 1120, 493); // Vẽ đường thẳng từ (0, 50) đến (Width của form, 50)
            pen.Dispose();
        }

        private void panelAllFr_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            int cornerRadius = 20; // Độ bo góc

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90);
                path.AddArc(rect.X + rect.Width - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90);
                path.AddRectangle(new Rectangle(rect.X, rect.Y + cornerRadius / 2, rect.Width, rect.Height - cornerRadius / 2));

                this.Region = new Region(path);
            }
        }

        private void panelOnl_Paint(object sender, PaintEventArgs e)
        {
            int cornerRadius = 30; // Độ bo tròn góc

            // Tạo GraphicsPath với các góc bo tròn
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, cornerRadius * 2, cornerRadius * 2), 180, 90); // Góc trái trên
            path.AddLine(cornerRadius, 0, this.Width - cornerRadius, 0);
            path.AddArc(new Rectangle(this.Width - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2), 270, 90); // Góc phải trên
            path.AddLine(this.Width, cornerRadius, this.Width, this.Height - cornerRadius);
            path.AddArc(new Rectangle(this.Width - cornerRadius * 2, this.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2), 0, 90); // Góc phải dưới
            path.AddLine(this.Width - cornerRadius, this.Height, cornerRadius, this.Height);
            path.AddArc(new Rectangle(0, this.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2), 90, 90); // Góc trái dưới
            path.CloseFigure();

            // Gán path này cho thuộc tính Region của Form để tạo hiệu ứng bo góc
            this.Region = new Region(path);

            // Thiết lập Paint event để làm mịn đường cong
            this.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                pe.Graphics.DrawPath(new Pen(this.BackColor, 2), path); // Đường viền mịn hơn
            };
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (isTextBoxActive && e.KeyChar == (char)Keys.Enter)
            {
                string Result = "";
                IPEndPoint IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9998);
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                try
                {
                    client.Connect(IP);
                    string SearchRequest = "#Serch" + MyID + txtSearch.Text;
                    byte[] data = Encoding.UTF8.GetBytes(SearchRequest);
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
                panelAddFr.Controls.Clear();
                if (Result != "")
                {
                    //lblNotiNoResultSearch.Visible = false;
                    string[] info = Result.Split(";");
                    int p_x = 28, p_y = 10;
                    foreach (string s in info)
                    {
                        string[] infoCertain = s.Split(",");
                        UserProfileCard community = new UserProfileCard(MyID, infoCertain[0], infoCertain[1], ImagesGetFromServer.Images[infoCertain[0]], false);
                        community.Location = new Point(p_x, p_y);
                        p_x += 220;
                        if (p_x > panelAddFr.Width - 220) { p_x = 28; p_y += 330; }
                        panelAddFr.Controls.Add(community);
                    }
                }
                else
                {
                    panelAddFr.Controls.Add(lblNotiNoResultSearch);
                    lblNotiNoResultSearch.Visible = true;
                };
            }
        }

        // Sự kiện TextBox nhận focus
        private void txtSearch_Enter(object sender, EventArgs e)
        {
            isTextBoxActive = true;
        }

        // Sự kiện TextBox mất focus
        private void txtSearch_Leave(object sender, EventArgs e)
        {
            isTextBoxActive = false;
        }
    }
}

