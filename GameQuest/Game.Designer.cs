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
            this.выйтиНаГлавныйЭкранToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выйтиИзИгрыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.картаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.городToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.лесToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.равнинаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.подножиеГорToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.подземельяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.инвентарьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AnimateDuration = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.менюToolStripMenuItem,
            this.картаToolStripMenuItem,
            this.инвентарьToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(900, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // менюToolStripMenuItem
            // 
            this.менюToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сохранитьИгруToolStripMenuItem,
            this.загрузитьИгруToolStripMenuItem,
            this.выйтиНаГлавныйЭкранToolStripMenuItem,
            this.выйтиИзИгрыToolStripMenuItem});
            this.менюToolStripMenuItem.Name = "менюToolStripMenuItem";
            this.менюToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.менюToolStripMenuItem.Text = "Меню";
            // 
            // сохранитьИгруToolStripMenuItem
            // 
            this.сохранитьИгруToolStripMenuItem.Name = "сохранитьИгруToolStripMenuItem";
            this.сохранитьИгруToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.сохранитьИгруToolStripMenuItem.Text = "Сохранить игру";
            // 
            // загрузитьИгруToolStripMenuItem
            // 
            this.загрузитьИгруToolStripMenuItem.Name = "загрузитьИгруToolStripMenuItem";
            this.загрузитьИгруToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.загрузитьИгруToolStripMenuItem.Text = "Загрузить игру";
            // 
            // выйтиНаГлавныйЭкранToolStripMenuItem
            // 
            this.выйтиНаГлавныйЭкранToolStripMenuItem.Name = "выйтиНаГлавныйЭкранToolStripMenuItem";
            this.выйтиНаГлавныйЭкранToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.выйтиНаГлавныйЭкранToolStripMenuItem.Text = "Выйти на главный экран";
            // 
            // выйтиИзИгрыToolStripMenuItem
            // 
            this.выйтиИзИгрыToolStripMenuItem.Name = "выйтиИзИгрыToolStripMenuItem";
            this.выйтиИзИгрыToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.выйтиИзИгрыToolStripMenuItem.Text = "Выйти из игры";
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
            this.картаToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.картаToolStripMenuItem.Text = "Карта";
            // 
            // городToolStripMenuItem
            // 
            this.городToolStripMenuItem.Name = "городToolStripMenuItem";
            this.городToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.городToolStripMenuItem.Text = "Город";
            this.городToolStripMenuItem.Click += new System.EventHandler(this.городToolStripMenuItem_Click);
            // 
            // лесToolStripMenuItem
            // 
            this.лесToolStripMenuItem.Name = "лесToolStripMenuItem";
            this.лесToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.лесToolStripMenuItem.Text = "Лес";
            this.лесToolStripMenuItem.Click += new System.EventHandler(this.лесToolStripMenuItem_Click);
            // 
            // равнинаToolStripMenuItem
            // 
            this.равнинаToolStripMenuItem.Name = "равнинаToolStripMenuItem";
            this.равнинаToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.равнинаToolStripMenuItem.Text = "Равнина";
            this.равнинаToolStripMenuItem.Click += new System.EventHandler(this.РавнинаToolStripMenuItem_Click);
            // 
            // подножиеГорToolStripMenuItem
            // 
            this.подножиеГорToolStripMenuItem.Name = "подножиеГорToolStripMenuItem";
            this.подножиеГорToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.подножиеГорToolStripMenuItem.Text = "Подножие гор";
            // 
            // подземельяToolStripMenuItem
            // 
            this.подземельяToolStripMenuItem.Name = "подземельяToolStripMenuItem";
            this.подземельяToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.подземельяToolStripMenuItem.Text = "Подземелья";
            // 
            // инвентарьToolStripMenuItem
            // 
            this.инвентарьToolStripMenuItem.Name = "инвентарьToolStripMenuItem";
            this.инвентарьToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.инвентарьToolStripMenuItem.Text = "Инвентарь";
            this.инвентарьToolStripMenuItem.Click += new System.EventHandler(this.ИнвентарьToolStripMenuItem_Click);
            // 
            // AnimateDuration
            // 
            this.AnimateDuration.Interval = 150;
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 624);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Game";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Игра квест";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem менюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьИгруToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьИгруToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выйтиНаГлавныйЭкранToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выйтиИзИгрыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem картаToolStripMenuItem;
        private System.Windows.Forms.Timer AnimateDuration;
        private System.Windows.Forms.ToolStripMenuItem инвентарьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem городToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem лесToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem равнинаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem подножиеГорToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem подземельяToolStripMenuItem;
    }
}

