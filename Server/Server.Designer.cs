namespace Server
{
    partial class Server
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            richTextBoxChat = new RichTextBox();
            textBoxChat = new TextBox();
            buttonSend = new Button();
            SuspendLayout();
            // 
            // richTextBoxChat
            // 
            richTextBoxChat.Location = new Point(27, 22);
            richTextBoxChat.Name = "richTextBoxChat";
            richTextBoxChat.Size = new Size(736, 322);
            richTextBoxChat.TabIndex = 0;
            richTextBoxChat.Text = "";
            // 
            // textBoxChat
            // 
            textBoxChat.Location = new Point(27, 350);
            textBoxChat.Multiline = true;
            textBoxChat.Name = "textBoxChat";
            textBoxChat.Size = new Size(619, 65);
            textBoxChat.TabIndex = 1;
            // 
            // buttonSend
            // 
            buttonSend.Location = new Point(652, 349);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(111, 66);
            buttonSend.TabIndex = 2;
            buttonSend.Text = "Send";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // Server
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonSend);
            Controls.Add(textBoxChat);
            Controls.Add(richTextBoxChat);
            Name = "Server";
            Text = "Server";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox richTextBoxChat;
        private TextBox textBoxChat;
        private Button buttonSend;
    }
}
