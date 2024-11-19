
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Client.Classes;
using Client.Properties;
namespace BTL_LTTQ
{
    public partial class ClientChat : Form
    {
        IPEndPoint IP;
        Socket client;
        string FrID;
        string MyID;
        string status;
        private int currentYPosition = 0;
        public static Dictionary<string, Image> ImagesMessage ;
        private List<Image> ImageList;
        public ClientChat(string myid, string uID, string stt)
        {
            MyID = myid;
            FrID = uID;
            status = stt;
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();

            this.Location = new Point(300, 50);

            this.MaximizeBox = false;
            ImageList = new List<Image>();
            ImagesMessage = new Dictionary<string, Image>();
            Connect();
        }

        public void Connect()
        {
            IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9998);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                client.Connect(IP);
                AddSentMessage(client.LocalEndPoint.ToString());
                AddSentMessage(MyID);
                string message1 = "#LdHIM" + MyID + FrID + status;
                byte[] data1 = Encoding.UTF8.GetBytes(message1);
                client.Send(data1);
                ImageList = ReceiveImages(client);
                int i = 0;
                foreach (var image in ImageList)
                {
                    ImagesMessage["IMG"+i] = image;
                    i++;
                }
                string message = "#LoadH" + MyID + FrID + status;
                byte[] data = Encoding.UTF8.GetBytes(message);
                client.Send(data);// send request to server               
                byte[] dataReturn = new byte[1024 * 1000];
                int bytesRead = client.Receive(dataReturn);
                status = "0";
                LoadOldMessage(Encoding.UTF8.GetString(dataReturn, 0, bytesRead));               
            }
            catch
            {
                MessageBox.Show("Can't connect to server !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Thread listenThread = new Thread(Receive);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        public void Disconnect()
        {
            client.Close();
        }

        public void Send()
        {
            if (!string.IsNullOrEmpty(textBoxChat.Text))
            {
                string message = "#Messg" + MyID + FrID + textBoxChat.Text;
                byte[] data = Encoding.UTF8.GetBytes(message);
                client.Send(data);
                AddSentMessage(textBoxChat.Text);
            }
        }
        public void SendImage()
        {
            string header = "#SNDIMAGE" + MyID+FrID;
            // Thêm padding để đủ độ dài cho tên người dùng
            byte[] headerBytes = Encoding.UTF8.GetBytes(header);
            byte[] imageBytes = File.ReadAllBytes(openFileDialogImage.FileName);

            // Tổng chiều dài của dữ liệu cần gửi
            int totalLength = headerBytes.Length + imageBytes.Length;
            byte[] lengthBytes = BitConverter.GetBytes(totalLength);

            // Kết hợp độ dài và dữ liệu để gửi
            byte[] dataToSend = headerBytes.Concat(lengthBytes).Concat(imageBytes).ToArray();

            IPEndPoint IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9998);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                client.Connect(IP);

                // Gửi dữ liệu trong vòng lặp
                int bytesSent = 0;
                while (bytesSent < dataToSend.Length)
                {
                    int sent = client.Send(dataToSend, bytesSent, dataToSend.Length - bytesSent, SocketFlags.None);
                    if (sent == 0)
                    {
                        throw new SocketException(); // Ngắt kết nối hoặc lỗi
                    }
                    bytesSent += sent;
                }
            }
            catch
            {
                MessageBox.Show("Can't connect to server!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                client.Close();
            };
        }
        public void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] bufferPeek = new byte[1024 * 5000];
                    int receivedBytes = client.Receive(bufferPeek, SocketFlags.Peek);
                    string headerPeek = Encoding.UTF8.GetString(bufferPeek.Take(3).ToArray());
                    if (headerPeek == "IMG")
                    {
                        
                        byte[] headerBytes = new byte[3];
                        int receivedHeaderBytes = client.Receive(headerBytes);
                        
                        if (receivedHeaderBytes < 3) continue;

                        byte[] lengthBytes = new byte[4];
                        int receivedLengthBytes = client.Receive(lengthBytes);
                        
                        if (receivedLengthBytes < 4) continue;

                        int totalLength = BitConverter.ToInt32(lengthBytes, 0) - 3;
                        byte[] buffer = new byte[totalLength];
                        int bytesReceived = 0;
                        while (bytesReceived < totalLength)
                        {
                            int received = client.Receive(buffer, bytesReceived, totalLength - bytesReceived, SocketFlags.None);
                            if (received == 0)
                            {
                                throw new SocketException();
                            }
                            bytesReceived += received;
                        }
                        using (MemoryStream ms = new MemoryStream(buffer))
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                AddReceiveImage(Image.FromStream(ms));
                            });
                        }
                    }
                    else
                    {
                        byte[] data = new byte[1024 * 5000];
                        int bytesRead = client.Receive(data);
                        string message = Encoding.UTF8.GetString(data, 0, bytesRead);

                        this.Invoke((MethodInvoker)delegate
                        {
                            AddReceivedMessage(message);
                        });
                    }
                }
            }
            catch
            {
                Disconnect();
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            Send();
            textBoxChat.Clear();
        }

        private void MakeRoundedCorners(Panel panel, int cornerRadius)
        {
            System.Drawing.Drawing2D.GraphicsPath graphicsPath = new System.Drawing.Drawing2D.GraphicsPath();
            graphicsPath.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
            graphicsPath.AddArc(panel.Width - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90);
            graphicsPath.AddArc(panel.Width - cornerRadius, panel.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            graphicsPath.AddArc(0, panel.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            graphicsPath.CloseAllFigures();
            panel.Region = new Region(graphicsPath);
        }

        private void AddSentMessage(string message)
        {
            Panel sentMessagePanel = new Panel();
            Label messageLabel = new Label();

            messageLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            messageLabel.Text = message;
            messageLabel.MaximumSize = new Size(450, 0);
            messageLabel.AutoSize = true;

            sentMessagePanel.Size = new Size(messageLabel.PreferredSize.Width + 20, messageLabel.PreferredSize.Height + 10);
            messageLabel.ForeColor = Color.White;

            sentMessagePanel.BackColor = Color.Blue;
            MakeRoundedCorners(sentMessagePanel, 10);
            sentMessagePanel.Location = new Point(ContainerChat.Width - sentMessagePanel.Width - 22, currentYPosition + ContainerChat.AutoScrollPosition.Y);
            sentMessagePanel.Controls.Add(messageLabel);
            ContainerChat.Controls.Add(sentMessagePanel);
            ContainerChat.VerticalScroll.Value = ContainerChat.VerticalScroll.Maximum;
            ContainerChat.PerformLayout();
            currentYPosition += sentMessagePanel.Height + 10;
        }
        private void AddSentImage(Image img)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = img;
            pictureBox.Size = new Size(500,300);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
 
            pictureBox.Location = new Point(ContainerChat.Width - pictureBox.Width - 22, currentYPosition + ContainerChat.AutoScrollPosition.Y);
            ContainerChat.Controls.Add(pictureBox);
            ContainerChat.VerticalScroll.Value = ContainerChat.VerticalScroll.Maximum;
            ContainerChat.PerformLayout();
            currentYPosition += pictureBox.Height + 10;
        }
        private void AddReceiveImage(Image img)
        {
            PictureBox pictureBox2 = new PictureBox();
            pictureBox2.Image = img;
            pictureBox2.Size = new Size(500, 300);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

            pictureBox2.Location = new Point(10, currentYPosition + ContainerChat.AutoScrollPosition.Y);
            Console.WriteLine("*");
            ContainerChat.Controls.Add(pictureBox2);
            Console.WriteLine("**");
            ContainerChat.VerticalScroll.Value = ContainerChat.VerticalScroll.Maximum;
            ContainerChat.PerformLayout();
            currentYPosition += pictureBox2.Height + 10;
        }
        private void AddReceivedMessage(string message)
        {
            Panel receivedMessagePanel = new Panel();
            Label messageLabel = new Label();

            messageLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            messageLabel.Text = message;
            messageLabel.AutoSize = true;
            messageLabel.MaximumSize = new Size(450, 0);
            receivedMessagePanel.Size = new Size(messageLabel.PreferredSize.Width + 20, messageLabel.PreferredSize.Height + 10);
            messageLabel.ForeColor = Color.Black;

            receivedMessagePanel.BackColor = Color.LightGray;
            MakeRoundedCorners(receivedMessagePanel, 10);
            receivedMessagePanel.Location = new Point(10, currentYPosition + ContainerChat.AutoScrollPosition.Y);
            receivedMessagePanel.Controls.Add(messageLabel);
            ContainerChat.Controls.Add(receivedMessagePanel);
            currentYPosition += receivedMessagePanel.Height + 10;
        }
        public void LoadOldMessage(string content)
        {
            if (content == "&^*") { return; }
            string[] lstMessage = content.Substring(3).Split(';');
            for (int i = 0; i < lstMessage.Length - 1; i++)
            {
                string[] split = lstMessage[i].Split(',');
                if (split[0].StartsWith("IMG"))
                {
                    if (split[1] == MyID) AddSentImage(ImagesMessage[split[0]]);
                    else AddReceiveImage(ImagesMessage[split[0]]);
                }
                else
                {
                    if (split[1] == MyID) AddSentMessage(split[0]);
                    else AddReceivedMessage(split[0]);
                }                
            }
        }

        private void btnSendImage_Click(object sender, EventArgs e)
        {
            if (openFileDialogImage.ShowDialog() == DialogResult.OK)
            {
                // Hiển thị hình ảnh vào PictureBox
                SendImage();
                AddSentImage(Image.FromFile(openFileDialogImage.FileName));
            }
        }
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
        private void GetImageFromServer()
        {
            IPEndPoint IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9998);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                client.Connect(IP);
                ImageList = ReceiveImages(client);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);//MessageBox.Show("Can't connect to server!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                ImagesMessage[id] = image;
                i++;
            }

        }
    }
}
