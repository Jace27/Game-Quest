namespace GameQuest
{
    partial class TitleScreen
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TitleScreen));
            this.StartNewGame = new System.Windows.Forms.Button();
            this.LoadGame = new System.Windows.Forms.Button();
            this.Settings = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ExitGame = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StartNewGame
            // 
            this.StartNewGame.Enabled = false;
            this.StartNewGame.Font = new System.Drawing.Font("Segoe Script", 13F);
            this.StartNewGame.Location = new System.Drawing.Point(116, 109);
            this.StartNewGame.Name = "StartNewGame";
            this.StartNewGame.Size = new System.Drawing.Size(185, 36);
            this.StartNewGame.TabIndex = 0;
            this.StartNewGame.Text = "Новая игра";
            this.StartNewGame.UseVisualStyleBackColor = true;
            this.StartNewGame.Click += new System.EventHandler(this.StartNewGame_Click);
            this.StartNewGame.Paint += new System.Windows.Forms.PaintEventHandler(this.StartNewGame_Paint);
            // 
            // LoadGame
            // 
            this.LoadGame.Enabled = false;
            this.LoadGame.Font = new System.Drawing.Font("Segoe Script", 13F);
            this.LoadGame.Location = new System.Drawing.Point(116, 151);
            this.LoadGame.Name = "LoadGame";
            this.LoadGame.Size = new System.Drawing.Size(185, 36);
            this.LoadGame.TabIndex = 1;
            this.LoadGame.Text = "Загрузить игру";
            this.LoadGame.UseVisualStyleBackColor = true;
            this.LoadGame.Click += new System.EventHandler(this.LoadGame_Click);
            this.LoadGame.Paint += new System.Windows.Forms.PaintEventHandler(this.LoadGame_Paint);
            // 
            // Settings
            // 
            this.Settings.Font = new System.Drawing.Font("Segoe Script", 13F);
            this.Settings.Location = new System.Drawing.Point(116, 193);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(185, 36);
            this.Settings.TabIndex = 2;
            this.Settings.Text = "Настройки";
            this.Settings.UseVisualStyleBackColor = true;
            this.Settings.Click += new System.EventHandler(this.Settings_Click);
            this.Settings.Paint += new System.Windows.Forms.PaintEventHandler(this.Settings_Paint);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(189)))), ((int)(((byte)(144)))));
            this.label1.Font = new System.Drawing.Font("Segoe Script", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(116, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 35);
            this.label1.TabIndex = 3;
            this.label1.Text = "Как Вас зовут?";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(189)))), ((int)(((byte)(144)))));
            this.textBox1.Font = new System.Drawing.Font("Segoe Script", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.Location = new System.Drawing.Point(116, 58);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(185, 38);
            this.textBox1.TabIndex = 4;
            this.textBox1.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox1_KeyDown);
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox1_KeyPress);
            // 
            // ExitGame
            // 
            this.ExitGame.Font = new System.Drawing.Font("Segoe Script", 13F);
            this.ExitGame.Location = new System.Drawing.Point(116, 235);
            this.ExitGame.Name = "ExitGame";
            this.ExitGame.Size = new System.Drawing.Size(185, 36);
            this.ExitGame.TabIndex = 5;
            this.ExitGame.Text = "Выйти из игры";
            this.ExitGame.UseVisualStyleBackColor = true;
            this.ExitGame.Click += new System.EventHandler(this.ExitGame_Click);
            this.ExitGame.Paint += new System.Windows.Forms.PaintEventHandler(this.ExitGame_Paint);
            // 
            // TitleScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(414, 414);
            this.Controls.Add(this.ExitGame);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Settings);
            this.Controls.Add(this.LoadGame);
            this.Controls.Add(this.StartNewGame);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "TitleScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IT-Квест";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleScreen_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleScreen_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitleScreen_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartNewGame;
        private System.Windows.Forms.Button LoadGame;
        private System.Windows.Forms.Button Settings;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button ExitGame;
    }
}