using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BTL_LTTQ.Classes;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Eventing.Reader;
using Microsoft.VisualBasic.ApplicationServices;
using System.Reflection.Metadata;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json;
using System.Security.AccessControl;
using System.Security.Cryptography;
namespace Server
{
    public partial class Server : Form
    {
        DataBaseProcess db = new DataBaseProcess();
        IPEndPoint IP;
        Socket server;
        Dictionary<string, Socket> clients;
        Dictionary<string, int> ClInDb;
        public Server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
            ClInDb = new Dictionary<string, int>();
            LoadClientFromDb();
        }

        public void Connect()
        {
            IP = new IPEndPoint(IPAddress.Any, 9998);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            clients = new Dictionary<string, Socket>();

            server.Bind(IP);
            Thread listenThread = new Thread(ListenForClients);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void ListenForClients()
        {
            try
            {
                server.Listen(100);
                while (true)
                {
                    Socket client = server.Accept();
                    NewMessage(client.RemoteEndPoint.ToString());
                    Thread receiveThread = new Thread(Receive);
                    receiveThread.IsBackground = true;
                    receiveThread.Start(client);
                }
            }
            catch
            {
                IP = new IPEndPoint(IPAddress.Any, 9998);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            }
        }

        public void Disconnect()
        {
            server.Close();
        }

        public void Send(Socket client, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                client.Send(data);
            }
        }
        public void SendImage(Socket client, byte[] Image)
        {
                client.Send(Image);           
        }
        public async void Receive(object obj)
        {
            Socket client = obj as Socket;
            try
            {
                while (true)
                {
                    byte[] bufferPeek = new byte[1024 * 5000];
                    int receivedBytes = client.Receive(bufferPeek,SocketFlags.Peek);
                    string headerPeek = Encoding.UTF8.GetString(bufferPeek.Take(9).ToArray());
                    if (headerPeek == "#IMAGEAVT")
                    {
                            byte[] headerBytes = new byte[29];
                            int receivedHeaderBytes = client.Receive(headerBytes);
                            if (receivedHeaderBytes < 29) continue;

                            string header = Encoding.UTF8.GetString(headerBytes.Take(9).ToArray());

                            byte[] lengthBytes = new byte[4];
                            int receivedLengthBytes = client.Receive(lengthBytes);
                            if (receivedLengthBytes < 4) continue;

                            int totalLength = BitConverter.ToInt32(lengthBytes, 0)-29;
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
                            SaveUserAvatar(headerBytes.Skip(9).Concat(buffer).ToArray());                                                    
                    }
                    else if (headerPeek== "#SNDIMAGE")
                    {
                        byte[] headerBytes = new byte[15];
                        int receivedHeaderBytes = client.Receive(headerBytes);
                        if (receivedHeaderBytes < 15) continue;

                        string Sender_Receiver = Encoding.UTF8.GetString(headerBytes.Skip(9).Take(6).ToArray());

                        byte[] lengthBytes = new byte[4];
                        int receivedLengthBytes = client.Receive(lengthBytes);
                        if (receivedLengthBytes < 4) continue;

                        int totalLength = BitConverter.ToInt32(lengthBytes, 0) - 15;
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
                        string IDSend=Sender_Receiver.Substring(0, 3);
                        string IDReceive=Sender_Receiver.Substring(3);
                        SaveImageMessage(IDSend,IDReceive,buffer);
                        if (ClInDb.ContainsKey(IDReceive))
                        {
                            if (clients.ContainsKey(IDReceive))
                            {
                                string header = "IMG";
                                byte[] hderBytes = Encoding.UTF8.GetBytes(header);
                                byte[] lengthBytesSend = BitConverter.GetBytes(buffer.Length+hderBytes.Length);
                                clients[IDReceive].Send(hderBytes.Concat(lengthBytesSend).Concat(buffer).ToArray());
                            }
                            else
                            {
                                UpdateStatus(IDReceive, IDSend, "0");
                            }
                        }
                    }    
                    else if (headerPeek== "#IMAGEGET")
                    {
                        SendImages(GetAllAVTPath(), client);
                    }    
                    else
                    {
                        byte[] data = new byte[1024 * 5000];
                        int bytesRead = client.Receive(data);
                        string message = Encoding.UTF8.GetString(data, 0, bytesRead);
                   
                        //Request Login
                        if (message.Substring(0, 6) == "#Login")
                        {
                            string[] info = message.Substring(6).Split(',');
                            string result = CheckLogin(info[0], info[1]);
                            if (result != "NotFound")
                            {
                                byte[] IDReturn = Encoding.UTF8.GetBytes(result);
                                client.Send(IDReturn);
                            }
                            else
                            {
                                byte[] IDReturn = Encoding.UTF8.GetBytes(result);
                                client.Send(IDReturn);
                            }
                        }
                        //Request Load Friends On Index Page
                        else if (message.Substring(0, 6) == "#LoadF")
                        {
                            string result = LoadIndex(message.Substring(6, 3));
                            byte[] ListFrReturn = Encoding.UTF8.GetBytes(result);
                            NewMessage(result);
                            clients[message.Substring(6, 3)] = client;
                            client.Send(ListFrReturn);
                        }
                        else if (message.Substring(0, 6) == "#LoadH")
                        {
                            List<string> ImagesMessage = new List<string>();
                            string IDSend = message.Substring(6, 3);
                            string result = LoadHist(message.Substring(6, 3), message.Substring(9, 3), message.Substring(12),ImagesMessage);                           
                            byte[] HistReturn = Encoding.UTF8.GetBytes(result);
                            client.Send(HistReturn);
                            clients[IDSend] = client;
                            NewMessage(result);
                        }
                        else if (message.Substring(0, 6) == "#LdHIM")
                        {
                            List<string> ImagesMessage = new List<string>();
                            string IDSend = message.Substring(6, 3);
                            string result = LoadHist(message.Substring(6, 3), message.Substring(9, 3), message.Substring(12), ImagesMessage);  
                            SendImages(ImagesMessage, client);
                        }
                        else if (message.Substring(0, 6) == "#Exist")
                        {
                            string userName = message.Substring(6);
                            NewMessage(userName);
                            string result = CheckExist(userName);
                            byte[] Exist = Encoding.UTF8.GetBytes(result);
                            client.Send(Exist);
                        }
                        else if (message.Substring(0, 6) == "#Regis")
                        {
                            NewMessage(message);
                            string[] info = message.Substring(6).Split(',');
                            Register(info);
                        }
                        else if (message.Substring(0, 6) == "#APIAI")
                        {
                            string IDOfRecommendFriends = "";
                            IDOfRecommendFriends = await APIRecommendUser(message.Substring(6));
                            string result = "";
                            result = GetInfoRcmFriend(IDOfRecommendFriends);
                            byte[] Rcm = Encoding.UTF8.GetBytes(result);
                            client.Send(Rcm);
                            NewMessage(result);
                        }
                        else if (message.Substring(0, 6) == "#FrReq")
                        {
                            AddFriendRequest(message.Substring(6, 3), message.Substring(9));
                        }
                        else if (message.Substring(0, 6) == "#RcvFr")
                        {
                            string result = "";
                            result = LoadFriendRequest(message.Substring(6));
                            byte[] FrRequests = Encoding.UTF8.GetBytes(result);
                            NewMessage(result);
                            client.Send(FrRequests);
                        }
                        else if (message.Substring(0, 6) == "#LdAll")
                        {
                            string result = "";
                            result = LoadAllUser(message.Substring(6));
                            byte[] FrRequests = Encoding.UTF8.GetBytes(result);
                            client.Send(FrRequests);
                        }
                        else if (message.Substring(0, 6) == "#AcpRq")
                        {
                            AcceptFriendRequest(message.Substring(6, 3), message.Substring(9));
                        }
                        else if(message.Substring(0,6)=="#Serch")
                        {
                            string result = "";
                            result = SearchUser(message.Substring(6,3),message.Substring(9));
                            byte[] SearchResult = Encoding.UTF8.GetBytes(result);
                            client.Send(SearchResult);
                        }
                        else if (message.Substring(0, 6) == "#profl")
                        {                            
                            string result = "";
                            result = getProfile(message.Substring(6));
                            byte[] ProfileResult = Encoding.UTF8.GetBytes(result);
                            client.Send(ProfileResult);
                        }
                        else
                        {
                            string IDSend = message.Substring(6, 3);
                            string IDReceive = message.Substring(9, 3);
                            this.Invoke((MethodInvoker)delegate
                            {
                                NewMessage(message);

                            });
                            if (ClInDb.ContainsKey(IDReceive))
                            {
                                SaveMessageToDb(IDSend, IDReceive, message.Substring(12));
                                if (clients.ContainsKey(IDReceive))
                                {
                                    Send(clients[IDReceive], message.Substring(12));
                                }
                                else
                                {
                                    UpdateStatus(IDReceive, IDSend, "0");
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

                client.Close();
            }
        }

        public void NewMessage(string message)
        {
            richTextBoxChat.AppendText(message + Environment.NewLine);
            richTextBoxChat.SelectionStart = richTextBoxChat.Text.Length;
            richTextBoxChat.ScrollToCaret();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            foreach (var item in clients)
            {
                Send(item.Value, textBoxChat.Text);
            }
            NewMessage(textBoxChat.Text);
            textBoxChat.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public string CheckLogin(string username, string password)
        {
            string sqlSelect = "SELECT RIGHT('000'+CAST([MaNguoiDung] AS VARCHAR(3)),3), [TenDangNhap] FROM [dbo].[NguoiDung] WHERE [TenDangNhap] = @username AND [MatKhau] = @password";

            SqlCommand command = new SqlCommand(sqlSelect, db.GetConnection());
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            DataTable dt = new DataTable();
            SqlDataAdapter sqlData = new SqlDataAdapter(command);
            sqlData.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString()+dt.Rows[0][1].ToString();
            }
            else return "NotFound";
        }
        public string LoadIndex(string ID)
        {
            string sqlSelect = "SELECT \r\n    RIGHT('000' + CAST(f.[MaBanBe] AS VARCHAR(3)), 3) ,\r\n    u.[TenDangNhap],\r\n    f.[TrangThai],\r\n    m.NoiDung,\r\n\tRIGHT('000' + CAST(m.MaNguoiGui AS VARCHAR(3)), 3)\r\nFROM \r\n    [dbo].[BanBe] f\r\nINNER JOIN \r\n    [dbo].[NguoiDung] u ON f.MaBanBe = u.MaNguoiDung\r\nOUTER APPLY \r\n    (SELECT TOP 1 NoiDung , MaNguoiGui\r\n     FROM [dbo].[TinNhan] \r\n     WHERE (MaNguoiNhan = @IDUser \r\n       AND MaNguoiGui = f.MaBanBe) OR (MaNguoiNhan = f.MaBanBe \r\n       AND MaNguoiGui = @IDUser)\r\n     ORDER BY ThoiGian DESC) m\r\nWHERE \r\n    f.[MaNguoiDung] = @IDUser;";

            SqlCommand command = new SqlCommand(sqlSelect, db.GetConnection());
            command.Parameters.AddWithValue("@IDUser", ID);
            DataTable dt = new DataTable();
            SqlDataAdapter sqlData = new SqlDataAdapter(command);
            sqlData.Fill(dt);
            string res = "";
            //res = ID+UserName+status(is unseen message ? ) 
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    res += dr[0].ToString() + "$" + dr[1].ToString() + "$" + dr[2].ToString() + "$" + dr[3].ToString() + "$" + dr[4].ToString() + "$";
                    if (clients.ContainsKey(dr[0].ToString())) res += "1&"; //meaning this fr is onl
                    else res += "0&";//meaning this friend is not onl now 
                }
            }
            else res = "NoFriend";
            return res;
        }
        public string LoadHist(string SenderID, string ReceiverID, string stt,List<string> ImagesMessage)
        {           
            string sqlSelect = "SELECT [NoiDung],RIGHT('000' + CAST([MaNguoiGui] AS VARCHAR(3)), 3)\r\nFROM [dbo].[TinNhan]\r\nWHERE ([MaNguoiGui] = @IDSend AND [MaNguoiNhan]=@IDRec) \r\nOR ([MaNguoiNhan] =@IDSend AND [MaNguoiGui]=@IDRec)\r\nORDER BY [ThoiGian]";
            SqlCommand command = new SqlCommand(sqlSelect, db.GetConnection());
            command.Parameters.AddWithValue("@IDSend", SenderID);
            command.Parameters.AddWithValue("@IDRec", ReceiverID);
            DataTable dt = new DataTable();
            SqlDataAdapter sqlData = new SqlDataAdapter(command);
            sqlData.Fill(dt);
            string res = "&^*";
            if (dt.Rows.Count > 0)
            {
                int CountImage = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[0].ToString().StartsWith("#%#"))
                    {
                        res += "IMG" + (CountImage++).ToString()+"," + dr[1].ToString() + ";";
                        ImagesMessage.Add(dr[0].ToString().Substring(3));
                    }
                    else { res += dr[0].ToString() + "," + dr[1].ToString() + ";"; }
                }
            }           
            //if this friend have unseen message , update db
            if (stt == "1")
            {
                UpdateStatus(SenderID, ReceiverID, stt);
            }
            return res;
        }
        public void UpdateStatus(string SenderID, string ReceiverID, string stt)
        {
            string sqlInsert = "UPDATE [dbo].[BanBe]\r\nSET [TrangThai]=@vl\r\nWHERE [MaNguoiDung]=@Sid AND [MaBanBe]=@Rid";
            using (SqlCommand command = new SqlCommand(sqlInsert, db.GetConnection()))
            {
                command.Parameters.AddWithValue("@Sid", SenderID);
                command.Parameters.AddWithValue("@Rid", ReceiverID);
                if (stt == "0") command.Parameters.AddWithValue("@vl", "1");
                else command.Parameters.AddWithValue("@vl", "0");
                db.OpenConnect();
                int rowsAffected = command.ExecuteNonQuery();
                db.CloseConnect();
            }
        }
        public void SaveMessageToDb(string SenderID, string ReceiverID, string content)
        {
            string sqlInsert = "INSERT INTO [dbo].[TinNhan]([MaNguoiGui],[MaNguoiNhan],[NoiDung])\r\nVALUES (@Sid,@Rid,@ctn)";
            using (SqlCommand command = new SqlCommand(sqlInsert, db.GetConnection()))
            {
                command.Parameters.AddWithValue("@Sid", SenderID);
                command.Parameters.AddWithValue("@Rid", ReceiverID);
                command.Parameters.AddWithValue("@ctn", content);

                db.OpenConnect();
                int rowsAffected = command.ExecuteNonQuery();
                db.CloseConnect();
            }
        }
        public void LoadClientFromDb()
        {
            string sqlSelect = "SELECT RIGHT('000'+CAST([MaNguoiDung] AS VARCHAR(3)),3)\r\nFROM [dbo].[NguoiDung]";

            SqlCommand command = new SqlCommand(sqlSelect, db.GetConnection());
            DataTable dt = new DataTable();
            SqlDataAdapter sqlData = new SqlDataAdapter(command);
            sqlData.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ClInDb.Add(dr[0].ToString(), i);
            }
        }
        public string CheckExist(string username)
        {
            string sqlSelect = "SELECT 1\r\nFROM [dbo].[NguoiDung]\r\nWHERE [TenDangNhap] = @USN";

            SqlCommand command = new SqlCommand(sqlSelect, db.GetConnection());
            command.Parameters.AddWithValue("@USN", username);
            DataTable dt = new DataTable();
            SqlDataAdapter sqlData = new SqlDataAdapter(command);
            sqlData.Fill(dt);
            string res = "0";
            if (dt.Rows.Count > 0) res = "1";
            return res;
        }
        public void Register(string[] info)
        {
            string sqlInsert = "  INSERT INTO [dbo].[NguoiDung]([TenDangNhap],[MatKhau],[HoTen],[DiaChi])\r\n  VALUES (@USN,@PW,@N,@LC)";

            using (SqlCommand command = new SqlCommand(sqlInsert, db.GetConnection()))
            {
                command.Parameters.AddWithValue("@USN", info[4]);
                command.Parameters.AddWithValue("@PW", info[5]);
                command.Parameters.AddWithValue("@N", info[0]);
                command.Parameters.AddWithValue("@LC", info[2]);

                db.OpenConnect();
                int rowsAffected = command.ExecuteNonQuery();
                db.CloseConnect();
            }
            string[] hobby = info[3].Split(":");
            string sqlAddHobby = "IF NOT EXISTS (SELECT 1 FROM [dbo].[SoThich] WHERE [TenSoThich] COLLATE SQL_Latin1_General_CP1_CI_AS = @nhb)\r\nBEGIN\r\n    INSERT INTO [dbo].[SoThich] ([TenSoThich]) VALUES (@nhb);\r\nEND";
            using (SqlCommand command = new SqlCommand(sqlAddHobby, db.GetConnection()))
            {
                command.Parameters.AddWithValue("@nhb", hobby[hobby.Length - 1]);
                db.OpenConnect();
                int rowsAffected = command.ExecuteNonQuery();
                db.CloseConnect();
            }
            string sqlUser_Hobby = "INSERT INTO [dbo].[NguoiDung_SoThich]([MaNguoiDung],[MaSoThich])\r\nVALUES((SELECT [MaNguoiDung] FROM [dbo].[NguoiDung] WHERE [TenDangNhap]=@us),(SELECT [MaSoThich] FROM [dbo].[SoThich] WHERE [TenSoThich]=@hb ));";
            foreach (string item in hobby)
            {
                using (SqlCommand command = new SqlCommand(sqlUser_Hobby, db.GetConnection()))
                {
                    command.Parameters.AddWithValue("@us", info[4]);
                    command.Parameters.AddWithValue("@hb", item);
                    db.OpenConnect();
                    int rowsAffected = command.ExecuteNonQuery();
                    db.CloseConnect();
                }
            }
        }
        private string GetInfoRcmFriend(string RcmFrs)
        {
            string result = "";
            string[] listRcm = RcmFrs.Split(",");
            foreach (string item in listRcm)
            {
                string sqlSelect = "  SELECT RIGHT('000'+CAST([MaNguoiDung] AS VARCHAR(3)),3),[TenDangNhap]\r\n  FROM [dbo].[NguoiDung] u\r\n  WHERE [MaNguoiDung] = @ID";

                SqlCommand command = new SqlCommand(sqlSelect, db.GetConnection());
                command.Parameters.AddWithValue("@ID", item);
                DataTable dt = new DataTable();
                SqlDataAdapter sqlData = new SqlDataAdapter(command);
                sqlData.Fill(dt);
                result += dt.Rows[0][0].ToString() + "," + dt.Rows[0][1].ToString() + ";";
            }
            result = result.Substring(0, result.Length - 1);
            return result;
        }
        private static async Task<string> APIRecommendUser(string UserID)
        {

            using (HttpClient client = new HttpClient())
            {
                // URL of API Flask 
                string url = $"http://127.0.0.1:5000/tim_nguoi_dung_tuong_dong?MaNguoiDung={UserID}";
                string result = "";
                try
                {
                    //send request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    // read responce and parse JSON
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<int> similarUsers = JsonConvert.DeserializeObject<List<int>>(responseBody);

                    // return result
                    Console.WriteLine("recommened :");
                    foreach (int userId in similarUsers)
                    {
                        Console.WriteLine($"MaNguoiDung: {userId}");
                        result += userId + ",";
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("fail connect to API:");
                    Console.WriteLine(e.Message);
                }
                result = result.Substring(0, result.Length - 1);
                return result;
            }
        }
        private void AddFriendRequest(string myID, string uID)
        {
            string sqlInsert = "INSERT INTO [dbo].[LoiMoiKetBan]([MaNguoiGui],[MaNguoiNhan])\r\nVALUES(@mid,@uid)";

            using (SqlCommand command = new SqlCommand(sqlInsert, db.GetConnection()))
            {
                command.Parameters.AddWithValue("@mid", myID);
                command.Parameters.AddWithValue("@uid", uID);

                db.OpenConnect();
                int rowsAffected = command.ExecuteNonQuery();
                db.CloseConnect();
            }
        }
        private void AcceptFriendRequest(string myID, string uID)
        {
            string sqlQuery = "  INSERT INTO [dbo].[BanBe]([MaNguoiDung],[MaBanBe],[TrangThai])\r\n  VALUES(@mid,@uid,0)";
            using (SqlCommand command = new SqlCommand(sqlQuery, db.GetConnection()))
            {
                command.Parameters.AddWithValue("@mid", myID);
                command.Parameters.AddWithValue("@uid", uID);
                db.OpenConnect();
                int rowsAffected = command.ExecuteNonQuery();
                db.CloseConnect();
            }
            DeleteFrRequest(myID, uID);
        }
        private void DeleteFrRequest(string myID, string uID)
        {
            string query = "DELETE FROM [dbo].[LoiMoiKetBan]\r\n  WHERE ([MaNguoiGui] = @uid AND [MaNguoiNhan] = @mid) OR ([MaNguoiGui] = @mid AND [MaNguoiNhan]=@uid)";
            using (SqlCommand command = new SqlCommand(query, db.GetConnection()))
            {
                command.Parameters.AddWithValue("@mid", myID);
                command.Parameters.AddWithValue("@uid", uID);
                db.OpenConnect();
                int rowsAffected = command.ExecuteNonQuery();
                db.CloseConnect();
            }
        }
        private string LoadFriendRequest(string myID)
        {
            string result = "";
            string sqlSelect = "SELECT RIGHT('000'+CAST(r.[MaNguoiGui] AS VARCHAR(3)),3) , u.TenDangNhap\r\nFROM [dbo].[LoiMoiKetBan] r\r\nINNER JOIN [dbo].[NguoiDung] U\r\nON r.MaNguoiGui=u.MaNguoiDung\r\nWHERE [MaNguoiNhan]=@id";
            SqlCommand command = new SqlCommand(sqlSelect, db.GetConnection());
            command.Parameters.AddWithValue("@id", myID);
            DataTable dt = new DataTable();
            SqlDataAdapter sqlData = new SqlDataAdapter(command);
            sqlData.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    result += dr[0].ToString() + "," + dr[1].ToString() + ";";
                }
                result = result.Substring(0, result.Length - 1);
            }
            else result = "NoFriendRequest";
            return result;
        }
        private string LoadAllUser(string ID)
        {
            string sqlSelect = "SELECT RIGHT('000'+CAST(u.MaNguoiDung AS VARCHAR(3)),3),u.[TenDangNhap]\r\nfrom [dbo].[NguoiDung] u\r\nWHERE [MaNguoiDung] NOT IN (SELECT [MaBanBe] FROM [dbo].[BanBe] WHERE [MaNguoiDung]=@id) AND [MaNguoiDung] !=@id";

            SqlCommand command = new SqlCommand(sqlSelect, db.GetConnection());
            command.Parameters.AddWithValue("@id", ID);
            DataTable dt = new DataTable();
            SqlDataAdapter sqlData = new SqlDataAdapter(command);
            sqlData.Fill(dt);
            string res = "";
            foreach (DataRow dr in dt.Rows)
            {
                res += dr[0].ToString() + "," + dr[1].ToString() + ";";
            }
            res = res.Substring(0, res.Length - 1);
            return res;
        }
        private void SaveUserAvatar(byte[] bytes)
        {
            string username = Encoding.UTF8.GetString(bytes.Take(20).ToArray());
            while (username[username.Length - 1] == '#')
            {
                username = username.Substring(0, username.Length - 1);
            }
            byte[] imageBytes = bytes.Skip(20).ToArray();
            File.WriteAllBytes("C:\\Users\\theta\\OneDrive\\Hình ảnh\\AvatarUser\\" + username + ".jpg", imageBytes);
            SaveAVTPathToDb(username);
        }
        private void SaveImageMessage(string IDSend,string IDReceive,byte[] bytes)
        {
            string path = "C:\\Users\\theta\\OneDrive\\Hình ảnh\\StoreImagesMessage\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
            File.WriteAllBytes(path, bytes);
            path = "#%#" + path;
            SaveMessageToDb(IDSend, IDReceive,path);
        }
        private void SaveAVTPathToDb(string username)
        {
            string path = @"C:\Users\theta\OneDrive\Hình ảnh\AvatarUser\" + username + ".jpg";
            string sqlQuery = "UPDATE [dbo].[NguoiDung]\r\nSET [AvatarPath]=@path\r\nWHERE [TenDangNhap] = @usname";
            using (SqlCommand command = new SqlCommand(sqlQuery, db.GetConnection()))
            {
                command.Parameters.AddWithValue("@usname", username);
                command.Parameters.AddWithValue("@path", path);
                db.OpenConnect();
                int rowsAffected = command.ExecuteNonQuery();
                db.CloseConnect();
            }            
        }
        private List<string> GetAllAVTPath()
        {
            List<string> list = new List<string>();
            string sqlSelect = "  SELECT [AvatarPath]\r\n  FROM [dbo].[NguoiDung]\r\n  ORDER BY [MaNguoiDung]";
            SqlCommand command = new SqlCommand(sqlSelect, db.GetConnection());
            DataTable dt = new DataTable();
            SqlDataAdapter sqlData = new SqlDataAdapter(command);
            sqlData.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(row[0].ToString());
            }        
            return list;
        }
        public byte[] ImageToByteArray(System.Drawing.Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public void SendImages(List<string> imagePaths, Socket client)
        {          
            // Tạo danh sách lưu trữ dữ liệu byte của từng hình ảnh
            List<byte[]> imageByteArrays = new List<byte[]>();
            if (imagePaths.Count == 0)
            {
                Console.WriteLine("Danh sách đường dẫn ảnh trống.");
            }
            else
            {
                foreach (string path in imagePaths)
                {
                    try
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromFile(path);
                        byte[] imgBytes = ImageToByteArray(img);
                        imageByteArrays.Add(imgBytes);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Cant load image from: {path}. error: {ex.Message}");
                    }
                }
            }
            // Tạo gói dữ liệu
            using (MemoryStream ms = new MemoryStream())
            {
                // Gửi số lượng hình ảnh
                byte[] imageCountBytes = BitConverter.GetBytes(imageByteArrays.Count);
                ms.Write(imageCountBytes, 0, imageCountBytes.Length);

                // Gửi độ dài và dữ liệu từng ảnh
                foreach (var imgBytes in imageByteArrays)
                {
                    byte[] imgLengthBytes = BitConverter.GetBytes(imgBytes.Length);
                    ms.Write(imgLengthBytes, 0, imgLengthBytes.Length);
                    ms.Write(imgBytes, 0, imgBytes.Length);
                }

                // Gửi toàn bộ dữ liệu
                byte[] packet = ms.ToArray();
                client.Send(packet);
            }
        }
        public string SearchUser(string userID,string txtSearch)
        {
            string sqlSelect = "SELECT RIGHT('000'+CAST(u.MaNguoiDung AS VARCHAR(3)),3),u.[TenDangNhap]\r\nfrom [dbo].[NguoiDung] u\r\nWHERE [TenDangNhap] LIKE @rq AND ([MaNguoiDung] NOT IN (SELECT [MaBanBe] FROM [dbo].[BanBe] WHERE [MaNguoiDung]=@id) AND [MaNguoiDung] !=@id)";

            SqlCommand command = new SqlCommand(sqlSelect, db.GetConnection());
            command.Parameters.AddWithValue("@id", userID);
            command.Parameters.AddWithValue("@rq", "%"+txtSearch+"%");
            DataTable dt = new DataTable();
            SqlDataAdapter sqlData = new SqlDataAdapter(command);
            sqlData.Fill(dt);
            string res = "";
            int CountImage = 0;
            foreach (DataRow dr in dt.Rows)
            {
                res += dr[0].ToString() + "," + dr[1].ToString() + ";";
            }
            res = res.Substring(0, res.Length - 1);
            return res;
        }
        public string getProfile(string userID)
        {
            //get UserName , Birth , Location
            string sqlSelect = "SELECT [HoTen],[NgaySinh],[DiaChi]\r\nFROM [dbo].[NguoiDung] \r\nWHERE [MaNguoiDung]=@id";
            SqlCommand command = new SqlCommand(sqlSelect, db.GetConnection());
            command.Parameters.AddWithValue("@id", userID);
            DataTable dt = new DataTable();
            SqlDataAdapter sqlData = new SqlDataAdapter(command);
            sqlData.Fill(dt);
            string res = "";
                res += dt.Rows[0][0].ToString() + "," + dt.Rows[0][1].ToString() + ","+dt.Rows[0][2].ToString()+ ";";  
            //get hobbys            
            string sqlSelectHobbys = "SELECT [TenSoThich]\r\nFROM [dbo].[SoThich]\r\nWHERE [MaSoThich] IN (SELECT [MaSoThich] FROM [dbo].[NguoiDung_SoThich] WHERE [MaNguoiDung]=@id)";
            SqlCommand command2 = new SqlCommand(sqlSelectHobbys, db.GetConnection());
            command2.Parameters.AddWithValue("@id", userID);
            DataTable dtHobby = new DataTable();
            SqlDataAdapter sqlData2 = new SqlDataAdapter(command2);
            sqlData2.Fill(dtHobby);
            if(dtHobby.Rows.Count > 0)
            {
                foreach (DataRow dr in dtHobby.Rows)
                {
                    res += dr[0].ToString() + ",";
                }
                res = res.Substring(0, res.Length - 1);
            }
            else { res += "NoHobby"; }
            res +=";";
            //get friends
            string sqlSelectFriends = "  SELECT RIGHT('000' + CAST([MaNguoiDung] AS VARCHAR(3)), 3) , [TenDangNhap]\r\n  FROM [dbo].[NguoiDung]\r\n  WHERE [MaNguoiDung] IN (SELECT [MaBanBe] FROM [dbo].[BanBe] WHERE [MaNguoiDung]=@id)";
            SqlCommand command3 = new SqlCommand(sqlSelectFriends, db.GetConnection());
            command3.Parameters.AddWithValue("@id", userID);
            DataTable dtFriend = new DataTable();
            SqlDataAdapter sqlData3 = new SqlDataAdapter(command3);
            sqlData3.Fill(dtFriend);
            if( dtFriend.Rows.Count > 0 )
            {
                foreach (DataRow dr in dtFriend.Rows)
                {
                    res += dr[0].ToString() + "," + dr[1].ToString() + "$";
                }
                res = res.Substring(0, res.Length - 1);
            }
            else { res += "NoFriend"; }
            return res;
        }
    }
}
