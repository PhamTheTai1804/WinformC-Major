using Client.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using System.Drawing.Drawing2D;

namespace Client
{
    public partial class Profile : MaterialSkin.Controls.MaterialForm
    {
        private string MyID;
        private string ID;
        public Profile(UserProfileCard UPC, bool Readonly)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.LoginName.Text += UPC.Name;
            this.picAVT.Image = MakeCircle.MakeCircularImage(UPC.Avatar, picAVT);
            this.ID = UPC.OwnerCardID;
            this.MyID = UPC.ID;
            FillInfo();
        }
        public void FillInfo()
        {
            string Result = "";
            IPEndPoint IP = new IPEndPoint(IPAddress.Parse("192.168.26.149"), 9998);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                client.Connect(IP);
                string getProfileRequest = "#profl" + ID;
                byte[] data = Encoding.UTF8.GetBytes(getProfileRequest);
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
            string[] splitResult = Result.Split(';');
            string[] BasicInfo = splitResult[0].Split(",");
            this.UserName.Text += BasicInfo[0];
            string[] strs = BasicInfo[1].Split(" ");
            this.Birth.Text += strs[0];
            this.Location.Text += BasicInfo[2];
            if (splitResult[1] != "NoHobby")
            {
                string[] hobbys = splitResult[1].Split(",");
                int lct_Y = 475;
                foreach (string hobby in hobbys)
                {
                    var Hobby = new MaterialLabel
                    {
                        Text = "•   " + hobby,
                        Location = new System.Drawing.Point(300, lct_Y),
                        Font = new System.Drawing.Font("Roboto", 11),
                        Width = 150
                    };
                    panelInfo.Controls.Add(Hobby);
                    lct_Y += 30;
                }
            }
            if (splitResult[2] != "NoFriend")
            {
                string[] info = splitResult[2].Split("$");
                int p_x = 28;
                foreach (string s in info)
                {
                    string[] infoCertain = s.Split(",");
                    Console.WriteLine(infoCertain[0] + " , " + infoCertain[1]);
                    UserProfileCard community = new UserProfileCard(MyID, infoCertain[0], infoCertain[1], ImagesGetFromServer.Images[infoCertain[0]], "3");
                    community.Location = new Point(p_x, 10);
                    p_x += 220;
                    panelFriend.Controls.Add(community);
                }
            }
            else
            {
                var lblNoFriend = new MaterialLabel
                {
                    Text = "Không Có Bạn Bè Nào để Hiển Thị !",
                    Location = new System.Drawing.Point(100, 100),
                    Font = new System.Drawing.Font("Roboto", 14),
                    Width = 450
                };
                panelFriend.Controls.Add(lblNoFriend);
            }
        }
    }
}
