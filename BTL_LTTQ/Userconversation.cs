using BTL_LTTQ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Userconversation : UserControl
    {
        private UserControlAvatar avatar;
        public Userconversation(UserControlAvatar avt, string message, bool Sender, bool Unseen)
        {
            InitializeComponent();
            this.avatar = avt;
            this.Controls.Add(avt);
            if (Sender)
            {
                this.labelMessage.Text = "Bạn : ";
            }
            this.labelName.Text = avt.Username;
            avt.Username = "";
            this.labelMessage.Text += message;
            if (Unseen)
            {
                labelName.Font = new Font(labelName.Font, FontStyle.Bold);
                labelMessage.Font = new Font(labelMessage.Font, FontStyle.Bold);
            }
        }

        private void Userconversation_Click(object sender, EventArgs e)
        {
            labelName.Font = new Font(labelName.Font, FontStyle.Regular);
            labelMessage.Font = new Font(labelMessage.Font, FontStyle.Regular);
            ClientChat chat = new ClientChat(avatar.MyID, avatar.FrID, avatar.status);
            chat.Show();
        }

        private void Userconversation_Load(object sender, EventArgs e)
        {
            int cornerRadius = 50; // Độ bo tròn góc

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
    }
}
