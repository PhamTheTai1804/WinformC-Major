using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Classes
{
    public static class MakeCircle
    {
        public static Image MakeCircularImage(Image img,PictureBox picAVT)
        {
            // Tạo bitmap với kích thước của PictureBox
            Bitmap bmp = new Bitmap(picAVT.Width, picAVT.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Đặt SmoothingMode để làm mịn đường viền
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Tạo đường path hình tròn
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, bmp.Width - 1, bmp.Height - 1);

                    // Tạo hình tròn từ ảnh
                    g.SetClip(path);
                    g.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height));

                    // Tạo viền ngoài (nếu muốn)
                    using (Pen pen = new Pen(Color.White, 4)) // Màu viền là trắng
                    {
                        g.ResetClip(); // Bỏ clip để vẽ viền ngoài
                        g.DrawEllipse(pen, 0, 0, bmp.Width - 1, bmp.Height - 1);
                    }
                }
            }

            return bmp;
        }
    }
}
