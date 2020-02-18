namespace GameQuest
{
    partial class Game
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.менюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьИгруToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьИгруToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выйтиНаГлавныйЭкранToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выйтиИзИгрыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.картаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.городToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.лесToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.равнинаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.подножиеГорToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.подземельяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.инвентарьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.журналToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AnimateDuration = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.menuStrip1.Font = new System.Drawing.Font("Segoe Script", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.менюToolStripMenuItem,
            this.картаToolStripMenuItem,
            this.инвентарьToolStripMenuItem,
            this.журналToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(900, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.menuStrip1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MenuStrip1_MouseDown);
            this.menuStrip1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MenuStrip1_MouseMove);
            this.menuStrip1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MenuStrip1_MouseUp);
            // 
            // менюToolStripMenuItem
            // 
            this.менюToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сохранитьИгруToolStripMenuItem,
            this.загрузитьИгруToolStripMenuItem,
            this.настройкиToolStripMenuItem,
            this.выйтиНаГлавныйЭкранToolStripMenuItem,
            this.выйтиИзИгрыToolStripMenuItem});
            this.менюToolStripMenuItem.Name = "менюToolStripMenuItem";
            this.менюToolStripMenuItem.Size = new System.Drawing.Size(58, 24);
            this.менюToolStripMenuItem.Text = "Меню";
            // 
            // сохранитьИгруToolStripMenuItem
            // 
            this.сохранитьИгруToolStripMenuItem.Name = "сохранитьИгруToolStripMenuItem";
            this.сохранитьИгруToolStripMenuItem.Size = new System.Drawing.Size(251, 24);
            this.сохранитьИгруToolStripMenuItem.Text = "Сохранить игру";
            this.сохранитьИгруToolStripMenuItem.Click += new System.EventHandler(this.СохранитьИгруToolStripMenuItem_Click);
            // 
            // загрузитьИгруToolStripMenuItem
            // 
            this.загрузитьИгруToolStripMenuItem.Name = "загрузитьИгруToolStripMenuItem";
            this.загрузитьИгруToolStripMenuItem.Size = new System.Drawing.Size(251, 24);
            this.загрузитьИгруToolStripMenuItem.Text = "Загрузить игру";
            this.загрузитьИгруToolStripMenuItem.Click += new System.EventHandler(this.ЗагрузитьИгруToolStripMenuItem_Click);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(251, 24);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            this.настройкиToolStripMenuItem.Click += new System.EventHandler(this.НастройкиToolStripMenuItem_Click);
            // 
            // выйтиНаГлавныйЭкранToolStripMenuItem
            // 
            this.выйтиНаГлавныйЭкранToolStripMenuItem.Name = "выйтиНаГлавныйЭкранToolStripMenuItem";
            this.выйтиНаГлавныйЭкранToolStripMenuItem.Size = new System.Drawing.Size(251, 24);
            this.выйтиНаГлавныйЭкранToolStripMenuItem.Text = "Выйти на главный экран";
            this.выйтиНаГлавныйЭкранToolStripMenuItem.Click += new System.EventHandler(this.ВыйтиНаГлавныйЭкранToolStripMenuItem_Click);
            // 
            // выйтиИзИгрыToolStripMenuItem
            // 
            this.выйтиИзИгрыToolStripMenuItem.Name = "выйтиИзИгрыToolStripMenuItem";
            this.выйтиИзИгрыToolStripMenuItem.Size = new System.Drawing.Size(251, 24);
            this.выйтиИзИгрыToolStripMenuItem.Text = "Выйти из игры";
            this.выйтиИзИгрыToolStripMenuItem.Click += new System.EventHandler(this.ВыйтиИзИгрыToolStripMenuItem_Click);
            // 
            // картаToolStripMenuItem
            // 
            this.картаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.городToolStripMenuItem,
            this.лесToolStripMenuItem,
            this.равнинаToolStripMenuItem,
            this.подножиеГорToolStripMenuItem,
            this.подземельяToolStripMenuItem});
            this.картаToolStripMenuItem.Name = "картаToolStripMenuItem";
            this.картаToolStripMenuItem.Size = new System.Drawing.Size(68, 24);
            this.картаToolStripMenuItem.Text = "Карта";
            this.картаToolStripMenuItem.Click += new System.EventHandler(this.КартаToolStripMenuItem_Click);
            // 
            // городToolStripMenuItem
            // 
            this.городToolStripMenuItem.Name = "городToolStripMenuItem";
            this.городToolStripMenuItem.Size = new System.Drawing.Size(167, 24);
            this.городToolStripMenuItem.Text = "Город";
            this.городToolStripMenuItem.Visible = false;
            this.городToolStripMenuItem.Click += new System.EventHandler(this.городToolStripMenuItem_Click);
            // 
            // лесToolStripMenuItem
            // 
            this.лесToolStripMenuItem.Name = "лесToolStripMenuItem";
            this.лесToolStripMenuItem.Size = new System.Drawing.Size(167, 24);
            this.лесToolStripMenuItem.Text = "Лес";
            this.лесToolStripMenuItem.Visible = false;
            this.лесToolStripMenuItem.Click += new System.EventHandler(this.лесToolStripMenuItem_Click);
            // 
            // равнинаToolStripMenuItem
            // 
            this.равнинаToolStripMenuItem.Name = "равнинаToolStripMenuItem";
            this.равнинаToolStripMenuItem.Size = new System.Drawing.Size(167, 24);
            this.равнинаToolStripMenuItem.Text = "Равнина";
            this.равнинаToolStripMenuItem.Visible = false;
            this.равнинаToolStripMenuItem.Click += new System.EventHandler(this.равнинаToolStripMenuItem_Click);
            // 
            // подножиеГорToolStripMenuItem
            // 
            this.подножиеГорToolStripMenuItem.Name = "подножиеГорToolStripMenuItem";
            this.подножиеГорToolStripMenuItem.Size = new System.Drawing.Size(167, 24);
            this.подножиеГорToolStripMenuItem.Text = "Подножие гор";
            this.подножиеГорToolStripMenuItem.Visible = false;
            this.подножиеГорToolStripMenuItem.Click += new System.EventHandler(this.подножиеГорToolStripMenuItem_Click);
            // 
            // подземельяToolStripMenuItem
            // 
            this.подземельяToolStripMenuItem.Name = "подземельяToolStripMenuItem";
            this.подземельяToolStripMenuItem.Size = new System.Drawing.Size(167, 24);
            this.подземельяToolStripMenuItem.Text = "Подземелья";
            this.подземельяToolStripMenuItem.Visible = false;
            this.подземельяToolStripMenuItem.Click += new System.EventHandler(this.подземельяToolStripMenuItem_Click);
            // 
            // инвентарьToolStripMenuItem
            // 
            this.инвентарьToolStripMenuItem.Name = "инвентарьToolStripMenuItem";
            this.инвентарьToolStripMenuItem.Size = new System.Drawing.Size(97, 24);
            this.инвентарьToolStripMenuItem.Text = "Инвентарь";
            this.инвентарьToolStripMenuItem.Click += new System.EventHandler(this.ИнвентарьToolStripMenuItem_Click);
            // 
            // журналToolStripMenuItem
            // 
            this.журналToolStripMenuItem.Name = "журналToolStripMenuItem";
            this.журналToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.журналToolStripMenuItem.Text = "Журнал";
            this.журналToolStripMenuItem.Click += new System.EventHandler(this.ЖурналToolStripMenuItem_Click);
            // 
            // AnimateDuration
            // 
            this.AnimateDuration.Interval = 150;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DimGray;
            this.button1.Enabled = false;
            this.button1.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.ForeColor = System.Drawing.Color.Lime;
            this.button1.Location = new System.Drawing.Point(1, 569);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(148, 29);
            this.button1.TabIndex = 1;
            this.button1.Text = "Запуск!";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox1.Enabled = false;
            this.textBox1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Margin = new System.Windows.Forms.Padding(1);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(150, 508);
            this.textBox1.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "json files (*.json)|*.json";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(750, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(150, 600);
            this.panel1.TabIndex = 4;
            this.panel1.Visible = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe Script", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(-3, 510);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 60);
            this.label1.TabIndex = 3;
            this.label1.Text = "Осталось топлива: 100";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.button2.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(751, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(149, 28);
            this.button2.TabIndex = 5;
            this.button2.Text = "Справка";
            this.toolTip1.SetToolTip(this.button2, "Справка по командам автопилота");
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 628);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Game";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IT-Квест";
            this.Activated += new System.EventHandler(this.Game_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Game_FormClosing);
            this.Load += new System.EventHandler(this.Game_Load);
            this.Click += new System.EventHandler(this.Game_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Game_MouseMove);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem менюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьИгруToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выйтиНаГлавныйЭкранToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выйтиИзИгрыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem картаToolStripMenuItem;
        private System.Windows.Forms.Timer AnimateDuration;
        private System.Windows.Forms.ToolStripMenuItem инвентарьToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem городToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem лесToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem равнинаToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem подножиеГорToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem подземельяToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public System.Windows.Forms.ToolStripMenuItem загрузитьИгруToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem журналToolStripMenuItem;
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

