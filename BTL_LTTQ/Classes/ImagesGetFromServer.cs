using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Classes
{
    public static class ImagesGetFromServer
    {
        public static Dictionary<string, Image> Images = new Dictionary<string, Image>();
        public static List<Image> ReceiveImages(Socket client)
        {
            List<Image> receivedImages = new List<Image>();

            // Nhận số lượng hình ảnh
            byte[] imageCountBytes = new byte[4];
            client.Receive(imageCountBytes);
            int imageCount = BitConverter.ToInt32(imageCountBytes, 0);

            for (int i = 0; i < imageCount; i++)
            {
                // Nhận độ dài của từng ảnh
                byte[] imgLengthBytes = new byte[4];
                client.Receive(imgLengthBytes);
                int imgLength = BitConverter.ToInt32(imgLengthBytes, 0);

                // Nhận dữ liệu ảnh
                byte[] imgBytes = new byte[imgLength];
                int totalBytesRead = 0;
                while (totalBytesRead < imgLength)
                {
                    int bytesRead = client.Receive(imgBytes, totalBytesRead, imgLength - totalBytesRead, SocketFlags.None);
                    if (bytesRead == 0) break;
                    totalBytesRead += bytesRead;
                }

                // Chuyển đổi byte array thành Image và thêm vào danh sách
                using (MemoryStream ms = new MemoryStream(imgBytes))
                {
                    Image img = Image.FromStream(ms);
                    receivedImages.Add(img);
                }
            }
            return receivedImages;
        }
    }
}
