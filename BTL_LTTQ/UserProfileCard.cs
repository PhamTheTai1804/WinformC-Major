using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows.Forms;
using MaterialSkin.Controls;
using Client.Classes;
namespace Client;
public partial class UserProfileCard : UserControl
{
    private string MyID;
    private string OwnerCardId;
    public UserProfileCard(string myid, string uid, string username, Image avt, bool IsReceive)
    {
        InitializeComponent();
        this.Avatar = avt;
        this.Name = username;
        usernameLabel.Text = username;
        this.Size = new Size(200, 320);

        // Cấu hình các thành phần
        usernameLabel.TextAlign = ContentAlignment.MiddleCenter;

        if (!IsReceive) addFriendButton.Text = "Thêm bạn bè";
        else
        {
            addFriendButton.Text = "Chấp nhận";
        }
        MyID = myid;
        OwnerCardId = uid;
        // Gọi phương thức để bo góc ảnh
        avatarPictureBox.Paint += AvatarPictureBox_Paint;
    }

    private void AvatarPictureBox_Paint(object sender, PaintEventArgs e)
    {
        Rectangle rect = new Rectangle(0, 0, avatarPictureBox.Width, avatarPictureBox.Height);
        int cornerRadius = 20; // Độ bo góc

        using (GraphicsPath path = new GraphicsPath())
        {
            path.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90);
            path.AddArc(rect.X + rect.Width - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90);
            path.AddRectangle(new Rectangle(rect.X, rect.Y + cornerRadius / 2, rect.Width, rect.Height - cornerRadius / 2));

            avatarPictureBox.Region = new Region(path);
        }
    }

    private void UserProfileCard_Paint(object sender, PaintEventArgs e)
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

    private void addFriendButton_Click(object sender, EventArgs e)
    {
        if (addFriendButton.Text == "Chấp nhận")
        {
            string Result = "";
            IPEndPoint IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9998);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                client.Connect(IP);
                string RequestLogin = "#AcpRq" + MyID + OwnerCardId;
                byte[] data = Encoding.UTF8.GetBytes(RequestLogin);
                client.Send(data);
            }
            catch
            {
                MessageBox.Show("Can't connect to server !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally { client.Close(); };
            addFriendButton.Text = "Đã chấp nhận";
            addFriendButton.Enabled = false;
            GlobalCache.SharedCache.Invalidate("#LoadF" + MyID);
            GlobalCache.SharedCache.Invalidate("#APIAI" + MyID);
            GlobalCache.SharedCache.Invalidate("#LdAll" + MyID);
            GlobalCache.SharedCache.Invalidate("#RcvFr" + MyID);
        }
        else
        {
            IPEndPoint IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9998);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                client.Connect(IP);
                string RequestLogin = "#FrReq" + MyID + OwnerCardId;
                byte[] data = Encoding.UTF8.GetBytes(RequestLogin);
                client.Send(data);
            }
            catch
            {
                MessageBox.Show("Can't connect to server !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally { client.Close(); };
            addFriendButton.Text = "Đã gửi lời mời";
            addFriendButton.Enabled = false;
        }

    }

    private void BtnProfile_Click(object sender, EventArgs e)
    {
        Profile profile = new Profile(this,true);
        profile.Show();
    }

    // Thuộc tính công khai để thiết lập tên người dùng
    public string Username
    {
        get => usernameLabel.Text;
        set => usernameLabel.Text = value;
    }
    public string OwnerCardID
    {
        get => this.OwnerCardId;
    }
    public string ID
    {
        get => this.MyID;
    }
    // Thuộc tính công khai để thiết lập thông tin phụ (số bạn chung)

    // Thuộc tính công khai để thiết lập ảnh đại diện
    public Image Avatar
    {
        get => avatarPictureBox.Image;
        set => avatarPictureBox.Image = value;
    }
}
