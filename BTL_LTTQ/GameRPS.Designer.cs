namespace Client
{
    partial class GameRPS
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            myChoiceLabel = new Label();
            opponentChoiceLabel = new Label();
            resultLabel = new Label();
            picbRock = new PictureBox();
            picbPaper = new PictureBox();
            picbScissors = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picbRock).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picbPaper).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picbScissors).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(106, 9);
            label1.Name = "label1";
            label1.Size = new Size(454, 74);
            label1.TabIndex = 0;
            label1.Text = "Rock Paper Scissors";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // myChoiceLabel
            // 
            myChoiceLabel.AutoSize = true;
            myChoiceLabel.Location = new Point(38, 136);
            myChoiceLabel.Name = "myChoiceLabel";
            myChoiceLabel.Size = new Size(79, 20);
            myChoiceLabel.TabIndex = 1;
            myChoiceLabel.Text = "My choice:";
            // 
            // opponentChoiceLabel
            // 
            opponentChoiceLabel.AutoSize = true;
            opponentChoiceLabel.Location = new Point(38, 181);
            opponentChoiceLabel.Name = "opponentChoiceLabel";
            opponentChoiceLabel.Size = new Size(126, 20);
            opponentChoiceLabel.TabIndex = 2;
            opponentChoiceLabel.Text = "Opponent choice:";
            // 
            // resultLabel
            // 
            resultLabel.AutoSize = true;
            resultLabel.Location = new Point(303, 83);
            resultLabel.Name = "resultLabel";
            resultLabel.Size = new Size(52, 20);
            resultLabel.TabIndex = 3;
            resultLabel.Text = "Result:";
            // 
            // picbRock
            // 
            picbRock.Image = Properties.Resources.rock;
            picbRock.Location = new Point(38, 292);
            picbRock.Name = "picbRock";
            picbRock.Size = new Size(140, 91);
            picbRock.SizeMode = PictureBoxSizeMode.StretchImage;
            picbRock.TabIndex = 7;
            picbRock.TabStop = false;
            picbRock.Click += picbRock_Click;
            // 
            // picbPaper
            // 
            picbPaper.Image = Properties.Resources.paper;
            picbPaper.Location = new Point(254, 292);
            picbPaper.Name = "picbPaper";
            picbPaper.Size = new Size(147, 91);
            picbPaper.SizeMode = PictureBoxSizeMode.StretchImage;
            picbPaper.TabIndex = 8;
            picbPaper.TabStop = false;
            picbPaper.Click += picbPaper_Click;
            // 
            // picbScissors
            // 
            picbScissors.Image = Properties.Resources.scissors;
            picbScissors.Location = new Point(466, 292);
            picbScissors.Name = "picbScissors";
            picbScissors.Size = new Size(140, 91);
            picbScissors.SizeMode = PictureBoxSizeMode.StretchImage;
            picbScissors.TabIndex = 9;
            picbScissors.TabStop = false;
            picbScissors.Click += picbScissors_Click;
            // 
            // GameRPS
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(673, 450);
            Controls.Add(picbScissors);
            Controls.Add(picbPaper);
            Controls.Add(picbRock);
            Controls.Add(resultLabel);
            Controls.Add(opponentChoiceLabel);
            Controls.Add(myChoiceLabel);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "GameRPS";
            Text = "GameRPS";
            ((System.ComponentModel.ISupportInitialize)picbRock).EndInit();
            ((System.ComponentModel.ISupportInitialize)picbPaper).EndInit();
            ((System.ComponentModel.ISupportInitialize)picbScissors).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label myChoiceLabel;
        private Label opponentChoiceLabel;
        private Label resultLabel;
        private PictureBox picbRock;
        private PictureBox picbPaper;
        private PictureBox picbScissors;
    }
}