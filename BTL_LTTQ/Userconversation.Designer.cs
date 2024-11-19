namespace Client
{
    partial class Userconversation
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelName = new Label();
            labelMessage = new Label();
            SuspendLayout();
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Font = new Font("Segoe UI", 14F);
            labelName.Location = new Point(164, 11);
            labelName.Name = "labelName";
            labelName.Size = new Size(78, 32);
            labelName.TabIndex = 0;
            labelName.Text = "label1";
            // 
            // labelMessage
            // 
            labelMessage.AutoSize = true;
            labelMessage.Font = new Font("Segoe UI", 12F);
            labelMessage.Location = new Point(164, 62);
            labelMessage.Name = "labelMessage";
            labelMessage.Size = new Size(0, 28);
            labelMessage.TabIndex = 1;
            // 
            // Userconversation
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(labelMessage);
            Controls.Add(labelName);
            Name = "Userconversation";
            Size = new Size(1208, 120);
            Load += Userconversation_Load;
            Click += Userconversation_Click;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelName;
        private Label labelMessage;
    }
}
