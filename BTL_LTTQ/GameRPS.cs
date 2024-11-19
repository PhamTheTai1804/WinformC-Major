using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace Client
{
    public partial class GameRPS : Form
    {
        private string MyID;
        private string FriendID;
        private Socket socket;
        private Thread messageReceiverThread;
        private volatile bool isRunning = true;
        private string myChoice = "";
        private string opponentChoice = "";
        private bool isMyTurn = true;
        private readonly object _socketLock = new object();

        public GameRPS(Socket serverSocket, string myID, string opponentID)
        {
            InitializeComponent();

            socket = serverSocket;
            MyID = myID;
            FriendID = opponentID;

            messageReceiverThread = new Thread(ReceiveMessages);
            messageReceiverThread.IsBackground = true;
            messageReceiverThread.Start();
        }



        private void UpdateButtonsState(bool enabled)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    picbRock.Enabled = enabled;
                    picbPaper.Enabled = enabled;
                    picbScissors.Enabled = enabled;
                }));
            }
            else
            {
                picbRock.Enabled = enabled;
                picbPaper.Enabled = enabled;
                picbScissors.Enabled = enabled;
            }
        }

        private void picbRock_Click(object sender, EventArgs e)
        {
            MakeChoice("ROCK");
        }

        private void picbPaper_Click(object sender, EventArgs e)
        {
            MakeChoice("PAPER");
        }

        private void picbScissors_Click(object sender, EventArgs e)
        {
            MakeChoice("SCISSORS");
        }

        private void MakeChoice(string choice)
        {


            myChoice = choice;
            myChoiceLabel.Text = $"Your choice: {choice}";
            UpdateButtonsState(false);
            SendChoice(choice);

        }

        private void SendChoice(string choice)
        {
            try
            {
                string move = $"#GAME_RPS_MOVE:{MyID}:{FriendID}:{choice}";
                byte[] data = Encoding.UTF8.GetBytes(move);

                lock (_socketLock)
                {
                    if (socket != null && socket.Connected)
                    {
                        socket.Send(data);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending move: {ex.Message}");
                Close();
            }
        }

        private void ReceiveMessages()
        {
            while (isRunning)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 5000];
                    int bytesRead = socket.Receive(buffer);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (message.StartsWith("#GAME_RPS_MOVE:"))
                    {
                        HandleOpponentMove(message);
                    }
                    else if (message.StartsWith("#GAME_RPS_RESULT:"))
                    {
                        HandleGameResult(message);
                    }
                }
                catch (SocketException)
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show("Connection lost!");
                        Close();
                    }));
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                        Close();
                    }));
                    break;
                }
            }
        }

        private void HandleOpponentMove(string message)
        {
            string[] parts = message.Split(':');
            if (parts.Length >= 4)
            {
                string senderId = parts[1];
                string targetId = parts[2];
                string choice = parts[3];

                if (targetId == MyID)
                {
                    this.Invoke(new Action(() =>
                    {
                        opponentChoice = choice;
                        opponentChoiceLabel.Text = $"Opponent choice: {choice}";

                        if (!string.IsNullOrEmpty(myChoice))
                        {
                            string result = DetermineWinner();
                            DisplayResult(result);
                            SendGameResult(result);
                        }
                    }));
                }
            }
        }

        private void HandleGameResult(string message)
        {
            string[] parts = message.Split(':');
            if (parts.Length >= 4)
            {
                string senderId = parts[1];
                string targetId = parts[2];
                string result = parts[3];

                if (targetId == MyID)
                {
                    this.Invoke(new Action(() =>
                    {
                        DisplayResult(result);
                    }));
                }
            }
        }

        private string DetermineWinner()
        {
            if (string.IsNullOrEmpty(myChoice) || string.IsNullOrEmpty(opponentChoice))
                return "";


            if (myChoice == opponentChoice)
            {
                return "It's a tie!";
            }
            else if ((myChoice == "ROCK" && opponentChoice == "SCISSORS") ||
                     (myChoice == "PAPER" && opponentChoice == "ROCK") ||
                     (myChoice == "SCISSORS" && opponentChoice == "PAPER"))
            {
                return "You win!";
            }
            else
            {
                return "You lose!";
            }

            //resultLabel.Text = result;
            //MessageBox.Show(result);

            //// Reset the game
            //ResetGame();
        }

        private void SendGameResult(string result)
        {
            try
            {
                // Invert the result for the opponent
                string opponentResult = result == "You win!" ? "You lose!" :
                                      result == "You lose!" ? "You win!" :
                                      result;

                string resultMessage = $"#GAME_RPS_RESULT:{MyID}:{FriendID}:{opponentResult}";
                byte[] data = Encoding.UTF8.GetBytes(resultMessage);

                lock (_socketLock)
                {
                    if (socket != null && socket.Connected)
                    {
                        socket.Send(data);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending result: {ex.Message}");
            }
        }

        private void DisplayResult(string result)
        {
            resultLabel.Text = result;
            MessageBox.Show(result);
            // Tạo một Timer để delay việc đóng form
            System.Windows.Forms.Timer closeTimer = new System.Windows.Forms.Timer();
            closeTimer.Interval = 2000; // 2 giây
            closeTimer.Tick += (s, e) =>
            {
                closeTimer.Stop();
                this.Invoke(new Action(() => this.Close()));
            };
            closeTimer.Start();
            //ResetGame();
        }

        private void ResetGame()
        {
            myChoice = "";
            opponentChoice = "";
            myChoiceLabel.Text = "Your choice: ";
            opponentChoiceLabel.Text = "Opponent choice: ";

            UpdateButtonsState(true);

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            isRunning = false;

            try
            {
                string endMessage = $"#GAME_RPS_END:{MyID}:{FriendID}";
                byte[] data = Encoding.UTF8.GetBytes(endMessage);
                socket.Send(data);
            }
            catch { /* Ignore cleanup errors */ }

            if (messageReceiverThread != null && messageReceiverThread.IsAlive)
            {
                messageReceiverThread.Join(1000);
            }
        }
    }
}
