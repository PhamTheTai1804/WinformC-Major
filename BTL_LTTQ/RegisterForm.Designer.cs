
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Client.Classes;
namespace Client
{
    partial class RegisterForm : MaterialForm
    {
        private MaterialRaisedButton btnDangKy;
        MaterialSingleLineTextField txtXacNhanMatKhau;
        MaterialSingleLineTextField txtMatKhau;
        Label lbErrorUserName ;
        Label lbErrorCfPass ;
        MaterialSingleLineTextField txtTenDangNhap;
        MaterialSingleLineTextField txtSoThichKhac;
        CheckedListBox clbSoThich;
        MaterialLabel lblSoThich;
        ComboBox cbDiaChi;
        MaterialLabel lblDiaChi;
        DateTimePicker dtNgaySinh;
        MaterialLabel lblNgaySinh;
        Label lbErrorName;
        MaterialSingleLineTextField txtHoTen;
        Label lbErrorLocation;
        OpenFileDialog ChooseImageDialog;
        PictureBox picAVT;
        Button btnChooseImage;
        public void InitializeComponent()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue500, Primary.Blue700, Primary.Blue200, Accent.LightBlue200, TextShade.WHITE);
            this.MaximizeBox = false; // Vô hiệu hóa nút phóng to
            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = "Đăng ký";
            this.Size = new System.Drawing.Size(650, 960);

            // Tạo các điều khiển và thiết lập các thuộc tính

            // Tiêu đề
            ChooseImageDialog = new OpenFileDialog();
            var lblHeader = new Label
            {
                Text = "Nhập Thông Tin Của Bạn",
                Font = new Font("Segoe UI", 26, FontStyle.Bold), // Font lớn và đậm
                Location = new System.Drawing.Point(65, 64), // Căn giữa header
                AutoSize = true,
                ForeColor = Color.FromArgb(33, 150, 243) // Màu xanh dịu mắt
            };
            this.Controls.Add(lblHeader);
            picAVT = new PictureBox();
            picAVT.SizeMode = PictureBoxSizeMode.StretchImage;
            picAVT.Image = MakeCircle.MakeCircularImage(Properties.Resources.defaultAvatar,picAVT);
            picAVT.Location = new Point(250, 180);
            picAVT.Size = new Size(120, 120); 
            this.Controls.Add(picAVT);
            btnChooseImage = new Button();
            btnChooseImage.Image = Properties.Resources.btnChooseImage;           
            btnChooseImage.Text = "";
            btnChooseImage.Size = new Size(37, 35);
            btnChooseImage.Location = new Point(333, 295);
            btnChooseImage.FlatStyle = FlatStyle.Flat;
            btnChooseImage.FlatAppearance.BorderSize = 0;
            btnChooseImage.Click += new EventHandler(this.btnChooseImage_Click);
            this.Controls.Add(btnChooseImage);
            // Họ và Tên
            txtHoTen = new MaterialSingleLineTextField
            {
                Hint = "Họ và tên",
                Location = new System.Drawing.Point(105, 350),
                Width = 400
            };
            Controls.Add(txtHoTen);
            lbErrorName = new Label();
            lbErrorName.Text = "Họ và tên không hợp lệ , vui lòng nhập lại !";
            lbErrorName.ForeColor = Color.Red;  // Thiết lập màu chữ
            lbErrorName.AutoSize = true;
            lbErrorName.Location = new Point(105, 380); // Đặt vị trí theo mong muốn
            lbErrorName.Visible = false;

            // Thêm vào form hoặc một container như Panel
            this.Controls.Add(lbErrorName);
            // Ngày sinh
            lblNgaySinh = new MaterialLabel
            {
                Text = "Ngày sinh",
                Location = new System.Drawing.Point(100, 415),
                Font = new System.Drawing.Font("Roboto", 11)
            };
            this.Controls.Add(lblNgaySinh);

            dtNgaySinh = new DateTimePicker
            {
                Location = new System.Drawing.Point(200, 420),
                Width = 300
            };
            dtNgaySinh.Format = DateTimePickerFormat.Custom;
            dtNgaySinh.CustomFormat = "dd/MM/yyyy";
            Controls.Add(dtNgaySinh);

            // Địa chỉ
            lblDiaChi = new MaterialLabel
            {
                Text = "Địa chỉ",
                Location = new System.Drawing.Point(100, 470),
                Font = new System.Drawing.Font("Roboto", 11)
            };
            Controls.Add(lblDiaChi);

            cbDiaChi = new ComboBox
            {
                Location = new System.Drawing.Point(200, 470),
                Width = 300
            };
            cbDiaChi.Items.AddRange(new string[] {
                "Hà Nội", "TP Hồ Chí Minh", "Đà Nẵng", "Hải Phòng", "Cần Thơ",
                "An Giang", "Bà Rịa - Vũng Tàu", "Bắc Giang", "Bắc Kạn", "Bạc Liêu",
                "Bắc Ninh", "Bến Tre", "Bình Định", "Bình Dương", "Bình Phước",
                "Bình Thuận", "Cà Mau", "Cao Bằng", "Đắk Lắk", "Đắk Nông",
                "Điện Biên", "Đồng Nai", "Đồng Tháp", "Gia Lai", "Hà Giang",
                "Hà Nam", "Hà Tĩnh", "Hải Dương", "Hậu Giang", "Hòa Bình",
                "Hưng Yên", "Khánh Hòa", "Kiên Giang", "Kon Tum", "Lai Châu",
                "Lâm Đồng", "Lạng Sơn", "Lào Cai", "Long An", "Nam Định",
                "Nghệ An", "Ninh Bình", "Ninh Thuận", "Phú Thọ", "Quảng Bình",
                "Quảng Nam", "Quảng Ngãi", "Quảng Ninh", "Quảng Trị", "Sóc Trăng",
                "Sơn La", "Tây Ninh", "Thái Bình", "Thái Nguyên", "Thanh Hóa",
                "Thừa Thiên Huế", "Tiền Giang", "Trà Vinh", "Tuyên Quang",
                "Vĩnh Long", "Vĩnh Phúc", "Yên Bái" });
            Controls.Add(cbDiaChi);
            lbErrorLocation = new Label();
            lbErrorLocation.Text = "Địa chỉ không được để trống !";
            lbErrorLocation.ForeColor = Color.Red;  // Thiết lập màu chữ
            lbErrorLocation.AutoSize = true;
            lbErrorLocation.Location = new Point(200, 505); // Đặt vị trí theo mong muốn
            lbErrorLocation.Visible = false;

            // Thêm vào form hoặc một container như Panel
            this.Controls.Add(lbErrorLocation);

            // Sở thích
            lblSoThich = new MaterialLabel
            {
                Text = "Sở thích",
                Location = new System.Drawing.Point(100, 530),
                Font = new System.Drawing.Font("Roboto", 11)
            };
            Controls.Add(lblSoThich);

            clbSoThich = new CheckedListBox
            {
                Location = new System.Drawing.Point(200, 530),
                Width = 300,
                Height = 100
            };
            clbSoThich.Items.AddRange(new object[] { "Đọc sách", "Xem phim", "Du lịch", "Chơi thể thao", "Nghe nhạc", "Nấu ăn", "Chụp ảnh", "Mua sắm" ,"Hát"});
            Controls.Add(clbSoThich);
            clbSoThich.MouseDown += (sender, e) =>
            {
                int index = clbSoThich.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    // Chuyển đổi trạng thái check của mục
                    clbSoThich.SetItemChecked(index, !clbSoThich.GetItemChecked(index));
                }
            };

            // Ô nhập sở thích khác
            txtSoThichKhac = new MaterialSingleLineTextField
            {
                Hint = "Sở thích khác",
                Location = new System.Drawing.Point(200, 625),
                Width = 300
            };
            Controls.Add(txtSoThichKhac);

            // Tên đăng nhập
            txtTenDangNhap = new MaterialSingleLineTextField
            {
                Hint = "Tên đăng nhập ( Tên đăng nhập chỉ gồm a-zA-Z0-9 )",
                Location = new System.Drawing.Point(100, 670),
                Width = 400
            };
            txtTenDangNhap.KeyPress += new KeyPressEventHandler(this.txtTenDangNhap_KeyPress);
            Controls.Add(txtTenDangNhap);
            lbErrorUserName = new Label();
            lbErrorUserName.Text = "Tên đăng nhập đã tồn tại , vui lòng chọn tên đăng nhập khác !";
            lbErrorUserName.ForeColor = Color.Red;  // Thiết lập màu chữ
            lbErrorUserName.AutoSize = true;
            lbErrorUserName.Location = new Point(100, 700); // Đặt vị trí theo mong muốn
            lbErrorUserName.Visible = false;

            // Thêm vào form hoặc một container như Panel
            this.Controls.Add(lbErrorUserName);
            // Mật khẩu
            txtMatKhau = new MaterialSingleLineTextField
            {
                Hint = "Mật khẩu",
                Location = new System.Drawing.Point(100, 730),
                Width = 400,
                PasswordChar = '*'
            };
            Controls.Add(txtMatKhau);

            // Xác nhận mật khẩu
            txtXacNhanMatKhau = new MaterialSingleLineTextField
            {
                Hint = "Xác nhận mật khẩu",
                Location = new System.Drawing.Point(100, 780),
                Width = 400,
                PasswordChar = '*'
            };
            Controls.Add(txtXacNhanMatKhau);
            lbErrorCfPass = new Label();
            lbErrorCfPass.Text = "Xác nhận mật khẩu không khớp , vui lòng kiểm tra lại !";
            lbErrorCfPass.ForeColor = Color.Red;  // Thiết lập màu chữ
            lbErrorCfPass.AutoSize = true;
            lbErrorCfPass.Location = new Point(100, 810); // Đặt vị trí theo mong muốn
            lbErrorCfPass.Visible = false;

            // Thêm vào form hoặc một container như Panel
            this.Controls.Add(lbErrorCfPass);

            // Nút đăng ký
            btnDangKy = new MaterialRaisedButton
            {
                Text = "Đăng ký",
                Location = new System.Drawing.Point(250, 860),
                Width = 150
            };
            btnDangKy.Click += new EventHandler(this.btnRegister_Click);
            Controls.Add(btnDangKy);

        }
        private void txtTenDangNhap_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra nếu ký tự không nằm trong khoảng a-z, A-Z, 0-9
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9') &&
                !(e.KeyChar >= 'a' && e.KeyChar <= 'z') &&
                !(e.KeyChar >= 'A' && e.KeyChar <= 'Z') &&
                !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Loại bỏ ký tự không hợp lệ
            }
        }

    }
}
