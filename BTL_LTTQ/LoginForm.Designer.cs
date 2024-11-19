using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Client;
partial class LoginForm : MaterialForm
{
    private Label lblHeader;
    private MaterialSingleLineTextField txtUsername;
    private MaterialSingleLineTextField txtPassword;
    private MaterialCheckBox chkRememberMe;
    private MaterialRaisedButton btnLogin;
    private Label lblSignUp;
    public void InitializeComponent()
    {
        var materialSkinManager = MaterialSkinManager.Instance;
        materialSkinManager.AddFormToManage(this);
        materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
        materialSkinManager.ColorScheme = new ColorScheme(
            Primary.Blue600, Primary.Blue800,
            Primary.Blue500, Accent.LightBlue200,
            TextShade.WHITE);

        // Cài đặt kích thước form và thuộc tính không thể thay đổi kích thước
        this.ClientSize = new Size(650, 700); // Kích thước 500x700
        this.FormBorderStyle = FormBorderStyle.FixedSingle; // Không cho phép thay đổi kích thước
        this.MaximizeBox = false; // Vô hiệu hóa nút phóng to
        this.FormBorderStyle = FormBorderStyle.None;
        int centerX = this.ClientSize.Width / 2; // Tính vị trí giữa form
        lblHeader = new Label
        {
            Text = "Đăng nhập",
            Font = new Font("Segoe UI", 32, FontStyle.Bold), // Font lớn và đậm
            Location = new Point(centerX - 160, 80), // Căn giữa header
            AutoSize = true,
            ForeColor = Color.FromArgb(33, 150, 243) // Màu xanh dịu mắt
        };
        this.Controls.Add(lblHeader);
        // Tên đăng nhập
        txtUsername = new MaterialSingleLineTextField
        {
            Hint = "Tên đăng nhập",
            Width = 300,
            Location = new Point(centerX - 150, 200) // Căn giữa theo chiều ngang
        };
        this.Controls.Add(txtUsername);

        // Mật khẩu
        txtPassword = new MaterialSingleLineTextField
        {
            Hint = "Mật khẩu",
            Width = 300,
            Location = new Point(centerX - 150, 270), // Căn giữa theo chiều ngang
            PasswordChar = '*'
        };
        this.Controls.Add(txtPassword);

        // Checkbox "Lưu đăng nhập"
        chkRememberMe = new MaterialCheckBox
        {
            Text = "Lưu đăng nhập",
            Location = new Point(centerX - 150, 340),
            Width = 200// Căn giữa checkbox
        };
        this.Controls.Add(chkRememberMe);

        // Nút Đăng nhập
        btnLogin = new MaterialRaisedButton
        {
            Text = "Đăng nhập",
            Width = 120,
            Location = new Point(centerX - 60, 420) // Căn giữa nút
        };
        btnLogin.Click += new EventHandler(this.btnLogin_Click);
        this.Controls.Add(btnLogin);

        // Dòng "Chưa có tài khoản?"
        lblSignUp = new Label
        {
            Text = "Chưa có tài khoản?",
            ForeColor = Color.Blue,
            Location = new Point(centerX - 65, 475), // Căn giữa label
            AutoSize = true,
            Cursor = Cursors.Hand
        };
        lblSignUp.Click += new EventHandler(this.lblSignUp_Click);
        this.Controls.Add(lblSignUp);
    }
}
