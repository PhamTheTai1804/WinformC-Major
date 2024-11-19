using MaterialSkin;
using MaterialSkin.Controls;
using System.Windows.Forms;
using System.Xml.Linq;
namespace Client;
partial class UserProfileCard
{
    private System.ComponentModel.IContainer components = null;
    private PictureBox avatarPictureBox;
    private Label usernameLabel;
    private Label mutualFriendsLabel;
    private MaterialRaisedButton addFriendButton;
    private MaterialRaisedButton BtnProfile;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        avatarPictureBox = new PictureBox();
        usernameLabel = new Label();
        mutualFriendsLabel = new Label();
        addFriendButton = new MaterialRaisedButton();
        BtnProfile = new MaterialRaisedButton();
        ((System.ComponentModel.ISupportInitialize)avatarPictureBox).BeginInit();
        SuspendLayout();
        // 
        // avatarPictureBox
        // 
        avatarPictureBox.Location = new Point(10, 10);
        avatarPictureBox.Name = "avatarPictureBox";
        avatarPictureBox.Size = new Size(180, 150);
        avatarPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        avatarPictureBox.TabIndex = 0;
        avatarPictureBox.TabStop = false;
        // 
        // usernameLabel
        // 
        usernameLabel.Font = new Font("Arial", 12F, FontStyle.Bold);
        usernameLabel.Location = new Point(0, 170);
        usernameLabel.Name = "usernameLabel";
        usernameLabel.Size = new Size(200, 30);
        usernameLabel.TabIndex = 1;
        usernameLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // mutualFriendsLabel
        // 
        mutualFriendsLabel.Location = new Point(0, 0);
        mutualFriendsLabel.Name = "mutualFriendsLabel";
        mutualFriendsLabel.Size = new Size(100, 23);
        mutualFriendsLabel.TabIndex = 2;
        // 
        // addFriendButton
        // 
        addFriendButton.Depth = 0;
        addFriendButton.Location = new Point(20, 214);
        addFriendButton.MouseState = MouseState.HOVER;
        addFriendButton.Name = "addFriendButton";
        addFriendButton.Primary = true;
        addFriendButton.Size = new Size(160, 36);
        addFriendButton.TabIndex = 3;
        addFriendButton.Text = "Thêm bạn bè";
        addFriendButton.Click += addFriendButton_Click;
        // 
        // BtnProfile
        // 
        BtnProfile.Depth = 0;
        BtnProfile.Location = new Point(20, 256);
        BtnProfile.MouseState = MouseState.HOVER;
        BtnProfile.Name = "BtnProfile";
        BtnProfile.Primary = true;
        BtnProfile.Size = new Size(160, 36);
        BtnProfile.TabIndex = 4;
        BtnProfile.Text = "Xem Hồ Sơ";
        BtnProfile.Click += BtnProfile_Click;
        // 
        // UserProfileCard
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        Controls.Add(avatarPictureBox);
        Controls.Add(usernameLabel);
        Controls.Add(mutualFriendsLabel);
        Controls.Add(addFriendButton);
        Controls.Add(BtnProfile);
        Name = "UserProfileCard";
        Size = new Size(200, 320);
        Paint += UserProfileCard_Paint;
        ((System.ComponentModel.ISupportInitialize)avatarPictureBox).EndInit();
        ResumeLayout(false);
    }
}
