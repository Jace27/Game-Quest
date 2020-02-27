using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using NAudio.Wave;
using System.IO.Compression;

namespace GameQuest
{
    public partial class Game : Form
    {
        public Game(TitleScreen ts)
        {
            //MessageBox.Show("Constructor");
            InitializeComponent();

            Icon = new Icon(TitleScreen.IconPath);

            Control.CheckForIllegalCrossThreadCalls = false;

            Par = ts;

            InitGame("City", false);
            isSaved = false;
            label1.Text = String.Format("Осталось топлива: {0:0}", Fuel);

            //Для города
            panel1.Visible = true;
            button2.Visible = true;

            //Переключение карты
            лесToolStripMenuItem.Visible = false;
            равнинаToolStripMenuItem.Visible = false;
            подножиеГорToolStripMenuItem.Visible = false;
            подземельяToolStripMenuItem.Visible = false;

            Location = new Point((Screen.PrimaryScreen.Bounds.Width - ClientSize.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - ClientSize.Height) / 2);
        }

        TitleScreen Par; //родитель - главный экран. также нужен для проигрывания амбиента

        public GameField GameField;
        public static Player Player;
        public static Point GlobalPlayerPosition = GlobalMap.Locations[0];
        FileInfo fig; //файл спрайта игрока

        public static string PlayerNick = "";

        bool[][,] Fogs = new bool[5][,];

        bool isAnimateMoving = true; //анимированное движение или нет
        public static bool NextMap2 = false; //открыт ли проход на 2 уровень
        public static bool NextMap3 = false; //открыт ли проход на 3 уровень
        public static bool NextMap4 = false; //открыт ли проход на 4 уровень
        public static bool NextMap5 = false; //открыт ли проход на 5 уровень
        public static bool isStarted = false;
        public static bool isInQuest = false; //игрок выполняет квест?
        public static bool isInDialog = false; //игрок находится в диалоге?
        public static bool isInGlobalMap = false;
        HTMLCode htmlc; //квест 2 уровня
        Crossword cr; //квест 3 уровня
        NumberSystemTranslation nst; //квест 4 уровня
        GuessingWords gw; //квест 5 уровня
        GlobalMap gm;
        private bool _issaved = false;
        bool isSaved
        {
            get { return _issaved; }
            set
            {
                _issaved = value;
                сохранитьИгруToolStripMenuItem.Enabled = !value;
            }
        }
        public bool SuccessfulLoad { get; set; } = false; //игра успешно загружена?

        private Dialog Dialog;

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!isInGlobalMap)
            {
                GameField.Graphics = CreateGraphics(); //почему-то требуется пересоздавать объект графики
                GameField.Draw();
                this.ClientSize = new Size(GameField.BlockSize.Width * 30, GameField.BlockSize.Height * 20 + menuStrip1.Height);
                Location = new Point((Screen.PrimaryScreen.Bounds.Width - ClientSize.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - ClientSize.Height) / 2);
                panel1.Width = GameField.BlockSize.Width * 5;
                label1.Location = new Point((panel1.Width - label1.Width) / 2, label1.Location.Y);
                button1.Location = new Point((panel1.Width - button1.Width) / 2, button1.Location.Y);
                button2.Location = new Point(Width - button2.Width, button2.Location.Y);
            }
        }

        private void InitGame(string location, bool fog)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            isSaved = false;
            //экземпляр игрового поля; в аргументы передаем имя локации (используется как имя файлов данных в Resources/Locations!) и туман войны
            //игровое поле может кидать много исключений! в том числе и кастомные
            GameField = new GameField(location, fog, CreateGraphics());
            GameField.Zero = new Point(0, 28);          //верхний левый угол игрового поля; 0:28 потому что 27 верхних пикселей занимает menuStrip

            //экземпляр игрока
            Player = new Player();

            //даем игровому полю и игроку ссылки друг на друга, чтобы они могли рисовать друг на друге
            Player.GameField = GameField;
            GameField.Player = Player;

            //проверяем доступность спрайта игрока
            fig = new FileInfo(Environment.CurrentDirectory + "\\Resources\\Sprites\\player.png");
            if (!fig.Exists) fig = new FileInfo(Environment.CurrentDirectory + "\\Resources\\Sprites\\null.png");
            Player.Sprite = new Bitmap(fig.FullName);
            if (location == "City")
            {
                Player.Sprite = new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\car2.png");
                Player.Location = new InGameLocation(new Point(4, 9)); //для города
                Player.Rotation = 3;
            }
            if (location == "Forest")
            {
                Player.Location = new InGameLocation(new Point(0, 2));     //для леса
            }
            if (location == "Plains")
            {
                Player.Location = new InGameLocation(new Point(3, 0));     //Для равнины
            }
            if (location == "Mountain")
            {
                Player.Location = new InGameLocation(new Point(0, 0));     //Для подножия гор
            }
            if (location == "Dungeon")
            {
                Player.Location = new InGameLocation(new Point(0, 17));    //Для подземелья
            }

            Dialog = new Dialog(this);
            GameField.Dialog = Dialog;

            Player.onStepUpItem += Player_onStepUpItem; //когда игрок наступает на объект вызываем обработчик
        }

        private void Player_onStepUpItem(GameObject item)
        {
            isSaved = false;
            if (item.Id == "exit")
                ExitPoint();
            Player.PickUpItem(); //допустим, сразу подняли
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            GameField.Graphics = CreateGraphics();
            isSaved = false;
            if (GameField.Location != "City" && !isInDialog)
            {
                if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
                {
                    if (!isAnimateMoving)
                        Player.MovePlayer("left");
                    else
                        Player.SmoothlyMovePlayer("left");
                    ActionsPoint();
                }
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
                {
                    if (!isAnimateMoving)
                        Player.MovePlayer("up");
                    else
                        Player.SmoothlyMovePlayer("up");
                    ActionsPoint();
                }
                if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
                {
                    if (!isAnimateMoving)
                        Player.MovePlayer("right");
                    else
                        Player.SmoothlyMovePlayer("right");
                    ActionsPoint();
                }
                if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
                {
                    if (!isAnimateMoving)
                        Player.MovePlayer("down");
                    else
                        Player.SmoothlyMovePlayer("down");
                    ActionsPoint();
                }
            }
        }

        #region ПЕРЕХОД ПО УРОВНЯМ
        private void городToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isSaved = false;
            GlobalPlayerPosition = GlobalMap.Locations[0];
            CreateGraphics().Clear(SystemColors.Control); //заливаем форму белым
            Inventory temp = Player.Inventory;   //сохраняем инвентарь игрока (объект будет пересоздан, инвентарь потеряется)
            SaveFogs();
            InitGame("City", false);             //переинициализируем игру
            LoadFogs();
            Player.Inventory = temp;             //возвращаем инвентарь
            Player.Sprite = new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\car2.png");
            GameField.Draw();
            label1.Text = String.Format("Осталось топлива: {0:0}", Fuel);
            panel1.Visible = true;
            button2.Visible = true;
            menuStrip1.BackColor = Color.FromArgb(195, 195, 195);
            this.Focus();
            isStarted = false;
            Game_Activated(new object(), new EventArgs());
        }

        private void лесToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isSaved = false;
            GlobalPlayerPosition = GlobalMap.Locations[1];
            CreateGraphics().Clear(SystemColors.Control); //заливаем форму белым
            Inventory temp = Player.Inventory;   //сохраняем инвентарь игрока (объект будет пересоздан, инвентарь потеряется)
            SaveFogs();
            InitGame("Forest", true);            //переинициализируем игру
            LoadFogs();
            Player.Inventory = temp;             //возвращаем инвентарь
            Player.Sprite = new Bitmap(fig.FullName);
            GameField.Graphics = CreateGraphics();
            GameField.Draw();
            panel1.Visible = false;
            button2.Visible = false;
            menuStrip1.BackColor = Color.FromArgb(42, 148, 9);
            this.Focus();
            Phrase(18);
        }

        private void равнинаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isSaved = false;
            GlobalPlayerPosition = GlobalMap.Locations[2];
            CreateGraphics().Clear(SystemColors.Control); //заливаем форму белым
            Inventory temp = Player.Inventory;   //сохраняем инвентарь игрока (объект будет пересоздан, инвентарь потеряется)
            SaveFogs();
            InitGame("Plains", true);            //переинициализируем игру
            LoadFogs();
            Player.Inventory = temp;             //возвращаем инвентарь
            Player.Sprite = new Bitmap(fig.FullName);
            GameField.Draw();
            panel1.Visible = false;
            button2.Visible = false;
            menuStrip1.BackColor = Color.FromArgb(91, 196, 0);
            this.Focus();
        }

        private void подножиеГорToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isSaved = false;
            GlobalPlayerPosition = GlobalMap.Locations[3];
            CreateGraphics().Clear(SystemColors.Control); //заливаем форму белым
            Inventory temp = Player.Inventory;   //сохраняем инвентарь игрока (объект будет пересоздан, инвентарь потеряется)
            SaveFogs();
            InitGame("Mountain", true);            //переинициализируем игру
            LoadFogs();
            Player.Inventory = temp;             //возвращаем инвентарь
            Player.Sprite = new Bitmap(fig.FullName);
            GameField.Draw();
            panel1.Visible = false;
            button2.Visible = false;
            menuStrip1.BackColor = Color.FromArgb(184, 135, 81);
            this.Focus();
        }

        private void подземельяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isSaved = false;
            GlobalPlayerPosition = GlobalMap.Locations[4];
            CreateGraphics().Clear(SystemColors.Control); //заливаем форму белым
            Inventory temp = Player.Inventory;   //сохраняем инвентарь игрока (объект будет пересоздан, инвентарь потеряется)
            SaveFogs();
            InitGame("Dungeon", true);            //переинициализируем игру
            LoadFogs();
            Player.Inventory = temp;             //возвращаем инвентарь
            Player.Sprite = new Bitmap(fig.FullName);
            GameField.Draw();
            panel1.Visible = false;
            button2.Visible = false;
            menuStrip1.BackColor = Color.FromArgb(101, 61, 53);
            this.Focus();
            Phrase(34);
        }
        #endregion

        #region POINTS
        private void ActionsPoint()
        {
            if (!isInDialog)
            {
                if (GameField.Location == "Forest") ForestPoint();
                if (GameField.Location == "Plains") PlainsPoint();
                if (GameField.Location == "Mountain") MountainPoint();
                if (GameField.Location == "Dungeon") DungeonPoint();
            }
        }

        private void ForestPoint()
        {
            if ((Player.Location.BL.X == 10 && Player.Location.BL.Y == 5) ||
                (Player.Location.BL.X == 10 && Player.Location.BL.Y == 4) ||
                (Player.Location.BL.X == 11 && Player.Location.BL.Y == 8) ||
                (Player.Location.BL.X == 12 && Player.Location.BL.Y == 13))
            {
                if (HTMLCode.NumberStart == 0)
                {
                    for (int i = 11; i < 21; i++)
                    {
                        for (int l = 3; l < 13; l++)
                        {
                            GameField.Fog[i, l] = false;
                            GameField.DrawBlock(new Point(i, l), false);
                        }
                    }
                    Phrase(19);
                }
            }
        }

        private async void ForestQuestStart()
        {
            isInQuest = true;
            htmlc = new HTMLCode();
            htmlc.Show();

            await Task.Run(() => { while (!NextMap2) Thread.Sleep(1000); });
            Phrase(24);
        }

        private void PlainsPoint()
        {
            if (Player.Location.BL.X == 13 && Player.Location.BL.Y == 8)
            {
                if (Crossword.CrosswordStart == 0)
                {
                    Phrase(26);
                }
            }
        }

        private async void PlainsQuestStart()
        {
            isInQuest = true;
            cr = new Crossword(Player);
            cr.Show();

            await Task.Run(() => { while (!NextMap3) Thread.Sleep(1000); });
            Phrase(28);
        }

        private void MountainPoint()
        {
            if (Player.Location.BL.X == 28 && Player.Location.BL.Y == 1)
            {
                if (NumberSystemTranslation.NumberTranslation == 0)
                {
                    Phrase(29);
                }
            }
        }

        private async void MountainQuestStart()
        {
            isInQuest = true;
            nst = new NumberSystemTranslation(this);
            nst.Show();

            await Task.Run(() => { while (!NextMap4) Thread.Sleep(1000); });
            Phrase(33);
        }

        private void DungeonPoint()
        {
            if ((Player.Location.BL.X == 23 && Player.Location.BL.Y == 1) ||
                (Player.Location.BL.X == 24 && Player.Location.BL.Y == 1) ||
                (Player.Location.BL.X == 25 && Player.Location.BL.Y == 1))
            {
                if (GuessingWords.GuessingWordStart == 0 && !NextMap5)
                {
                    Phrase(35);
                }
            }
        }

        private async void DungeonQuestStart()
        {
            isInQuest = true;
            gw = new GuessingWords();
            gw.Show();

            await Task.Run(() => { while (!NextMap5) Thread.Sleep(1000); });
            Phrase(39);
        }

        private void ExitPoint()
        {
            isSaved = false;
            if (GameField.Location == "City" && F1Shop && F2Shop && F3Shop && F4Shop)
            {
                Task.WaitAll();
                textBox1.Text = "";
                Thread.Sleep(1000);
                Phrase(17);
            }
            else if (GameField.Location == "City" && !F1Shop || !F2Shop || !F3Shop || !F4Shop)
            {
                Phrase(16);
            }
            if (GameField.Location == "Forest" && NextMap2)
            {
                GameField.DrawScreen("Вы прошли второй уровень!", Color.White, Color.ForestGreen);
                isInGlobalMap = true;
                Thread.Sleep(2000);
                gm = new GlobalMap(GlobalPlayerPosition, GlobalMap.Locations[2]);
                gm.ShowDialog();
                GlobalPlayerPosition = GlobalMap.Locations[2];
                gm.Dispose();
                gm = null;
                Focus();
                isInGlobalMap = false;
                SwitchMusic(3);
                равнинаToolStripMenuItem_Click(new object(), new EventArgs());
                равнинаToolStripMenuItem.Visible = false;
            }
            else if (GameField.Location == "Plains" && NextMap3)
            {
                GameField.DrawScreen("Вы прошли третий уровень!", Color.White, Color.ForestGreen);
                isInGlobalMap = true;
                Thread.Sleep(2000);
                gm = new GlobalMap(GlobalPlayerPosition, GlobalMap.Locations[3]);
                gm.ShowDialog();
                GlobalPlayerPosition = GlobalMap.Locations[3];
                gm.Dispose();
                gm = null;
                Focus();
                isInGlobalMap = false;
                SwitchMusic(4);
                подножиеГорToolStripMenuItem_Click(new object(), new EventArgs());
                подножиеГорToolStripMenuItem.Visible = false;
            }
            else if (GameField.Location == "Dungeon" && NextMap5)
            {
                isInDialog = true;
                GameField.DrawScreen("Спасибо за игру!", Color.White, Color.ForestGreen);
                Thread.Sleep(4000);
                Application.Exit();
            }
            this.Focus();
        }
        #endregion

        #region МЕНЮ
        private void ВыйтиИзИгрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isSaved)
            {
                DialogResult dr = MessageBox.Show("Сохранить игру?", "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes) сохранитьИгруToolStripMenuItem.PerformClick();
                if (dr == DialogResult.Cancel) return;
            }
            Application.Exit();
        }

        private void ВыйтиНаГлавныйЭкранToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isSaved)
            {
                DialogResult dr = MessageBox.Show("Сохранить игру?", "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes) сохранитьИгруToolStripMenuItem.PerformClick();
                if (dr == DialogResult.Cancel) return;
            }
            SwitchMusic(0);
            Par.Show();
            this.Close();
        }

        private void НастройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings GameSettings = new Settings(TitleScreen.curAudioFile);
            GameSettings.ShowDialog();
            ClientSize = new Size(GameField.BlockSize.Width * 30, GameField.BlockSize.Height * 20 + 24);
            Location = new Point((Screen.PrimaryScreen.Bounds.Width - ClientSize.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - ClientSize.Height) / 2);
            panel1.Width = GameField.BlockSize.Width * 5;
            textBox1.Width = GameField.BlockSize.Width * 5 - 2;
            textBox1.Height = GameField.BlockSize.Height * 20 - 92;
            label1.Location = new Point(-3, GameField.BlockSize.Height * 20 - 90);
            label1.Width = GameField.BlockSize.Width * 5 + 3;
            button1.Location = new Point(1, GameField.BlockSize.Height * 20 - 31);
            button1.Width = GameField.BlockSize.Width * 5 - 2;
            CreateGraphics().Clear(SystemColors.Control);
            Player.Location = new InGameLocation(new Point(Player.Location.BL.X, Player.Location.BL.Y));
            GameField.Graphics = CreateGraphics();
            GameField.Draw();
        }

        private void КартаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gm = new GlobalMap(GlobalPlayerPosition);
            gm.ShowDialog();
            gm.Dispose();
            gm = null;
        }

        private void НачатьУровеньЗановоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GameField.Location != "City" || Fuel > 0)
            {
                if (!isSaved)
                {
                    DialogResult dr = MessageBox.Show("Сохранить игру?", "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes) сохранитьИгруToolStripMenuItem.PerformClick();
                    if (dr == DialogResult.Cancel) { return; }
                }

                InitGame("City", false);
                Fuel = 100;
                label1.Text = String.Format("Осталось топлива: {0:0}", Fuel);
                panel1.Visible = true;

                лесToolStripMenuItem.Visible = false;
                равнинаToolStripMenuItem.Visible = false;
                подножиеГорToolStripMenuItem.Visible = false;
                подземельяToolStripMenuItem.Visible = false;
            }
        }

        private void ИнвентарьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InventoryForm invf = new InventoryForm(Player);
            invf.ShowDialog();
        }
        #endregion

        #region СОХРАНЕНИЕ И ЗАГРУЗКА

        private void SaveFogs()
        {
            if (GameField.Location == "City")
                Fogs[0] = GameField.Fog;
            if (GameField.Location == "Forest")
                Fogs[1] = GameField.Fog;
            if (GameField.Location == "Plains")
                Fogs[2] = GameField.Fog;
            if (GameField.Location == "Mountain")
                Fogs[3] = GameField.Fog;
            if (GameField.Location == "Dungeon")
                Fogs[4] = GameField.Fog;
        }
        private void LoadFogs()
        {
            if (GameField.Location == "City")
                if (Fogs[0] != null)
                    GameField.Fog = Fogs[0];
            if (GameField.Location == "Forest")
                if (Fogs[1] != null)
                    GameField.Fog = Fogs[1];
            if (GameField.Location == "Plains")
                if (Fogs[2] != null)
                    GameField.Fog = Fogs[2];
            if (GameField.Location == "Mountain")
                if (Fogs[3] != null)
                    GameField.Fog = Fogs[3];
            if (GameField.Location == "Dungeon")
                if (Fogs[4] != null)
                    GameField.Fog = Fogs[4];
        }
        public class Save
        {
            public Save() { }
            public Player Player { get; set; }
            public bool F1Shop { get; set; }
            public bool F2Shop { get; set; }
            public bool F3Shop { get; set; }
            public bool F4Shop { get; set; }
            public bool NextMap2 { get; set; }
            public bool NextMap3 { get; set; }
            public bool NextMap4 { get; set; }
            public bool NextMap5 { get; set; }
            public string Level { get; set; }
            public int Fuel { get; set; }
            public bool[][][] Fogs { get; set; }
            public string[][] Notes { get; set; }
            public Point GlobalPlayerPosition { get; set; }
        }

        private void СохранитьИгруToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isInDialog)
            {
                try
                {
                    SaveFogs();

                    if (!Directory.Exists(Environment.CurrentDirectory + "\\Saves"))
                        Directory.CreateDirectory(Environment.CurrentDirectory + "\\Saves");

                    Save Save = new Save();
                    Save.Player = Player;
                    Save.Level = GameField.Location;
                    Save.Fuel = Fuel;
                    Save.F1Shop = F1Shop;
                    Save.F2Shop = F2Shop;
                    Save.F3Shop = F3Shop;
                    Save.F4Shop = F4Shop;
                    Save.NextMap2 = NextMap2;
                    Save.NextMap3 = NextMap3;
                    Save.NextMap4 = NextMap4;
                    Save.NextMap5 = NextMap5;
                    Save.Notes = Journal.Notes;
                    Save.GlobalPlayerPosition = GlobalPlayerPosition;

                    bool[][][] fogs = new bool[5][][];
                    for (int i = 0; i < 5; i++)
                    {
                        if (Fogs[i] != null)
                        {
                            fogs[i] = new bool[Fogs[i].GetLength(0)][];
                            for (int l = 0; l < Fogs[i].GetLength(0); l++)
                            {
                                fogs[i][l] = new bool[Fogs[i].GetLength(1)];
                                for (int t = 0; t < Fogs[i].GetLength(1); t++)
                                {
                                    fogs[i][l][t] = Fogs[i][l, t];
                                }
                            }
                        }
                    }
                    Save.Fogs = fogs;

                    DateTime Now = DateTime.Now;
                    using (FileStream fs = new FileStream(Environment.CurrentDirectory + "\\Saves\\" + Now.ToString("dd.MM.yyyy hh.mm.ss") + ".json", FileMode.OpenOrCreate))
                    {
                        string json = JsonSerializer.Serialize<Save>(Save);
                        byte[] bytes = System.Text.Encoding.Default.GetBytes(json);
                        fs.Write(bytes, 0, bytes.Length);
                        isSaved = true;
                    }

                    MessageBox.Show("Игра сохранена", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ЗагрузитьИгруToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                isSaved = true;
                isStarted = true;
                isInDialog = false;
                isInQuest = false;
                Save Save = new Save();
                using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                {
                    Save = JsonSerializer.Deserialize<Save>(sr.ReadToEnd());
                }
                F1Shop = Save.F1Shop;
                F2Shop = Save.F2Shop;
                F3Shop = Save.F3Shop;
                F4Shop = Save.F4Shop;
                NextMap2 = Save.NextMap2;
                NextMap3 = Save.NextMap3;
                NextMap4 = Save.NextMap4;
                NextMap5 = Save.NextMap5;
                Journal.Notes = Save.Notes;
                Fuel = Save.Fuel;
                GlobalPlayerPosition = Save.GlobalPlayerPosition;
                Fogs = new bool[5][,];
                for (int i = 0; i < 5; i++)
                {
                    if (Save.Fogs[i] != null)
                    {
                        Fogs[i] = new bool[Save.Fogs[i].Length, Save.Fogs[i][0].Length];
                        for (int l = 0; l < Save.Fogs[i].Length; l++)
                        {
                            for (int t = 0; t < Save.Fogs[i][l].Length; t++)
                            {
                                Fogs[i][l, t] = Save.Fogs[i][l][t];
                            }
                        }
                    }
                }
                if (Save.Level == "City")
                {
                    InitGame(Save.Level, false);
                    panel1.Visible = true;
                    button2.Visible = true;
                }
                else
                {
                    InitGame(Save.Level, true);
                    panel1.Visible = false;
                    button2.Visible = false;
                }
                switch (Save.Level)
                {
                    case "City": SwitchMusic(1); menuStrip1.BackColor = Color.FromArgb(195, 195, 195); break;
                    case "Forest": SwitchMusic(2); menuStrip1.BackColor = Color.FromArgb(42, 148, 9);  break;
                    case "Plains": SwitchMusic(3); menuStrip1.BackColor = Color.FromArgb(91, 196, 0);  break;
                    case "Mountain": SwitchMusic(4); menuStrip1.BackColor = Color.FromArgb(184, 135, 81);  break;
                    case "Dungeon": SwitchMusic(5); menuStrip1.BackColor = Color.FromArgb(101, 61, 53);  break;
                }
                LoadFogs();
                Player = Save.Player;
                Player.GameField = GameField;
                GameField.Player = Save.Player;
                GameField.Player.GameField = GameField;
                Player.Location = new InGameLocation(Save.Player.Location.BL, "bl");
                Player.onStepUpItem += Player_onStepUpItem;
                GameField.Draw();
                SuccessfulLoad = true;
            }
        }
        #endregion

        #region ПЕРЕТАСКИВАНИЕ ФОРМЫ
        private Point mouseOffset;
        private bool isMouseDown = false;

        private void MenuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            int xOffset;
            int yOffset;
            if (e.Button == MouseButtons.Left)
            {
                xOffset = -e.X - SystemInformation.FrameBorderSize.Width;
                yOffset = -e.Y - SystemInformation.FrameBorderSize.Height;

                mouseOffset = new Point(xOffset, yOffset);
                isMouseDown = true;
            }
            else isMouseDown = false;
        }

        private void MenuStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseOffset.X, mouseOffset.Y);
                Location = mousePos;
                if (Location.X < 0) Location = new Point(1, Location.Y);
                if (Location.Y < 0) Location = new Point(Location.X, 1);
            }
        }

        private void MenuStrip1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) isMouseDown = false;
        }

        private void Game_MouseMove(object sender, MouseEventArgs e)
        {
            //label1.Text = (-e.Y - SystemInformation.FrameBorderSize.Height).ToString();
            if (e.Y + SystemInformation.FrameBorderSize.Height > menuStrip1.Height || e.Y + SystemInformation.FrameBorderSize.Height < 0)
                isMouseDown = false;
        }
        #endregion

        private void Game_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("Load");
        }

        private void Game_Activated(object sender, EventArgs e)
        {
            if (gm != null) gm.Focus();
            if (!isInGlobalMap)
            {
                GameField.Graphics = CreateGraphics();
                GameField.Draw();
            }
            if (isInQuest)
            {
                if (!NextMap2)
                    try { htmlc.Focus(); } catch { }
                else if (!NextMap3)
                    try { cr.Focus(); } catch { }
                else if (!NextMap4)
                    try { nst.Focus(); } catch { }
                else if (!NextMap5)
                    try { gw.Focus(); } catch { }
            }
            if (!isStarted)
                GameStart();
        }

        private void SwitchMusic(int id)
        {
            float vol = TitleScreen.curAudioFile.Volume;
            TitleScreen.outputDevice.Stop();
            FileInfo fi1 = new FileInfo(TitleScreen.audioFiles[id]);
            if (!fi1.Exists)
            {
                string zipPath = Environment.CurrentDirectory + "\\Resources\\Music.zip";
                string extractPath = Environment.CurrentDirectory + "\\Resources";
                FileInfo[] fi;
                DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\Resources");
                fi = di.GetFiles("*.mp3");
                for (int i = 0; i < fi.Length; i++)
                {
                    try { fi[i].Delete(); } catch { }
                }
                try { ZipFile.ExtractToDirectory(zipPath, extractPath); } catch { }
            }
            TitleScreen.curAudioFile = new AudioFileReader(TitleScreen.audioFiles[id]);
            TitleScreen.outputDevice.Init(TitleScreen.curAudioFile);
            TitleScreen.outputDevice.Play();
            TitleScreen.curAudioFile.Volume = vol;
        }

        #region ДИАЛОГИ
        public int CurrentPhrase { get; set; }
        private void GameStart()
        {
            if (!isStarted)
            {
                Delegate += Phrase;
                DrawDelegate += GameField.Draw;
                PlayerDelegate += SetPlayer;
                for (int i = 0; i < Dialogs.Length; i++) if (Dialogs[i][1] == "") Dialogs[i][1] = "{player}";
                isInDialog = true;
                SwitchMusic(1);
                CurrentPhrase = 0;
                Phrase(CurrentPhrase);
                isStarted = true;
            }
        }
        public void NextPhrase()
        {
            if (CurrentPhrase < 9)
            {
                CurrentPhrase++;
                Phrase(CurrentPhrase);
            }
            else if (CurrentPhrase == 9)
            {
                Dialog.Current.Message = null;
                GameField.Draw();
                menuStrip1.Enabled = true;
                isInDialog = false;
                ReleaseGUI();
            }
            else if ((CurrentPhrase >= 9 && CurrentPhrase <= 16) || CurrentPhrase == 18 || CurrentPhrase == 25 || CurrentPhrase == 28 || CurrentPhrase == 34)
            {
                Dialog.Current.Message = null;
                GameField.Draw();
                menuStrip1.Enabled = true;
                isInDialog = false;
            }
            else if (CurrentPhrase == 17)
            {
                GameField.DrawScreen("Вы прошли первый уровень!", Color.White, Color.ForestGreen);
                isInGlobalMap = true;
                Thread.Sleep(2000);
                gm = new GlobalMap(GlobalPlayerPosition, GlobalMap.Locations[1]);
                gm.ShowDialog();
                GlobalPlayerPosition = GlobalMap.Locations[1];
                gm.Dispose();
                gm = null;
                Focus();
                isInGlobalMap = false;
                menuStrip1.Enabled = true;
                isInDialog = false;
                SwitchMusic(2);
                лесToolStripMenuItem_Click(new object(), new EventArgs());
                лесToolStripMenuItem.Visible = false;
            }
            else if (CurrentPhrase < 23)
            {
                CurrentPhrase++;
                Phrase(CurrentPhrase);
            }
            else if (CurrentPhrase == 23)
            {
                menuStrip1.Enabled = true;
                isInDialog = false;
                Dialog.Current.Message = null;
                GameField.Draw();
                if (!NextMap2)
                    ForestQuestStart();
            }
            else if (CurrentPhrase == 24)
            {
                Player.Inventory.AddItem(new GameObject() { Count = 1, Id = "amulet", Description = "Ведьминский амулет на удачу.", Name = "Амулет", });
                Player.Inventory.Money += 50;
                Phrase(25);
            }
            else if (CurrentPhrase == 26)
            {
                Phrase(27);
            }
            else if (CurrentPhrase == 27)
            {
                Phrase(42);
            }
            else if (CurrentPhrase == 42)
            {
                Phrase(43);
            }
            else if (CurrentPhrase == 43)
            {
                menuStrip1.Enabled = true;
                isInDialog = false;
                Dialog.Current.Message = null;
                GameField.Draw();
                if (!NextMap3)
                    PlainsQuestStart();
            }
            else if (CurrentPhrase < 32)
            {
                CurrentPhrase++;
                Phrase(CurrentPhrase);
            }
            else if (CurrentPhrase == 32)
            {
                menuStrip1.Enabled = true;
                isInDialog = false;
                Dialog.Current.Message = null;
                GameField.Draw();
                if (!NextMap4)
                    MountainQuestStart();
            }
            else if (CurrentPhrase == 33)
            {
                GameField.DrawScreen("Вы прошли четвертый уровень!", Color.White, Color.ForestGreen);
                Thread.Sleep(2000);
                gm = new GlobalMap(GlobalPlayerPosition, GlobalMap.Locations[4]);
                gm.ShowDialog();
                GlobalPlayerPosition = GlobalMap.Locations[4];
                gm.Dispose();
                gm = null;
                Focus();
                menuStrip1.Enabled = true;
                isInDialog = false;
                SwitchMusic(5);
                CurrentPhrase++;
                подземельяToolStripMenuItem_Click(new object(), new EventArgs());
                подземельяToolStripMenuItem.Visible = true;
            }
            else if (CurrentPhrase < 38)
            {
                CurrentPhrase++;
                Phrase(CurrentPhrase);
            }
            else if (CurrentPhrase == 38)
            {
                menuStrip1.Enabled = true;
                isInDialog = false;
                Dialog.Current.Message = null;
                GameField.Draw();
                if (!NextMap5)
                    DungeonQuestStart();
            }
            else if (CurrentPhrase < 41)
            {
                CurrentPhrase++;
                Phrase(CurrentPhrase);
            }
            else if (CurrentPhrase == 41)
            {
                Player.Inventory.AddItem(new GameObject() { Count = 12, Id = "gold_cup", Description = "Кубок с инкрустированными драгоценными камнями.", Name = "Золотой кубок", });
                Player.Inventory.AddItem(new GameObject() { Count = 23, Id = "gold_ring", Description = "Кольцо из чистого золота.", Name = "Золотое кольцо", });
                Player.Inventory.AddItem(new GameObject() { Count = 560, Id = "old_coin", Description = "Коллекционеры отдадут за них целое состояние.", Name = "Старинные монеты", });
                Player.Inventory.AddItem(new GameObject() { Count = 7, Id = "gold_chain", Description = "Украшение.", Name = "Золотая цепочка", });
                Player.Inventory.AddItem(new GameObject() { Count = 1, Id = "helmet", Description = "Шлем древнего полководца. Бесценный исторический артефакт.", Name = "Шлем", });
                Player.Inventory.Money += 5000;
                menuStrip1.Enabled = true;
                isInDialog = false;
                Dialog.Current.Message = null;
                GameField.Draw();
            }
            else
            {
                menuStrip1.Enabled = true;
                isInDialog = false;
                Dialog.Current.Message = null;
                GameField.Draw();
            }
        }
        public static string[][] Dialogs = new string[][]
        {
            new string[] { "Итак, у меня начался отпуск, пора бы и развеяться на природе.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //0
            new string[] { "Судя по карте, поход предстоит долгий, так что надо хорошо подготовиться.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //1
            new string[] { "Мне нужно, во-первых, заехать в продуктовый магазин за едой на время похода.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //2
            new string[] { "Во-вторых, купить походное снаряжение.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" },
            new string[] { "В-третьих, выбрать хорошую походную одежду.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //4
            new string[] { "В-четвертых, нужно купить лопату, вдруг клад придется выкапывать.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //5
            new string[] { "Каждый магазин отмечен желтой буквой S. Выезд из города отмечен желтым восклицательным знаком.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //6
            new string[] { "У меня есть машина на автопилоте. В интерфейсе справа его можно запрограммировать.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //7
            new string[] { "Как это сделать - описано в справке.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" },
            new string[] { "Топлива у меня мало, поэтому нужно выбрать оптимальный маршрут.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //9

            new string[] { "Ошибка компиляции. Надо бы свериться со справкой....", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //10
            new string[] { "Зачем я здесь остановился?.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //11
            new string[] { "Продуктовый магазин. Сделано. Едем дальше.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //12
            new string[] { "Туристический магазин. Сделано. Едем дальше.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //13
            new string[] { "Магазин одежды. Сделано. Едем дальше.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" },
            new string[] { "Строительный магазин. Сделано. Едем дальше.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //15
            new string[] { "Я купил еще не все из списка. Нужно вернуться.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //16

            new string[] { "Вещи собраны, настроение хорошее, впереди приключения. Вперед!", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //17

            new string[] { "Люблю лес. Птички поют, солнце не жарит...", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //18

            new string[] { "Здравствуйте, молодой человек. Смотрела я в зелье предсказаний и знаю, что ты нам помочь можешь.", "Фиолетовая Ведьма", Environment.CurrentDirectory + "\\Resources\\Sprites\\purplewitch.png", "phrase" }, //19
            new string[] { "Делаем мы сайт ведьминский, магию в массы продвигаем.", "Зеленая Ведьма", Environment.CurrentDirectory + "\\Resources\\Sprites\\greenwitch.png", "phrase" }, //20
            new string[] { "Токма ошибки в него закрались, самим найти их у нас не получается.", "Красная Ведьма", Environment.CurrentDirectory + "\\Resources\\Sprites\\redwitch.png", "phrase" }, //21
            new string[] { "Золотом тебя одарим мы.", "Фиолетовая Ведьма", Environment.CurrentDirectory + "\\Resources\\Sprites\\purplewitch.png", "phrase" },
            new string[] { "Окей, показывайте.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //23

            new string[] { "Благодарствую тебе, путник. Прими в дар амулет на удачу и немного денег.", "Фиолетовая Ведьма", Environment.CurrentDirectory + "\\Resources\\Sprites\\purplewitch.png", "phrase" }, //24
            new string[] { "Проверьте инвентарь.", "Игра", Environment.CurrentDirectory + "\\Resources\\Sprites\\scroll.png", "phrase" }, //25

            new string[] { "Солнце так печет... Я устал, присяду в тени этих деревьев.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //26
            new string[] { "Как раз решу тот кроссворд, который мне коллега посоветовал.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //27

            new string[] { "Это было несложно. А теперь в путь!", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //28
            
            new string[] { "Запертая дверь... За ней точно что-то есть!", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //29
            new string[] { "Четыре замка. На каждом нацарапаны символы...", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //30
            new string[] { "Да это же системы счисления! Судя по всему...", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //31
            new string[] { "Мне нужно перевести эти числа в десятичную систему счисления. Точно.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //32

            new string[] { "Проход открыт. Внутрь!", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //33

            new string[] { "Сыро, темно и страшно. Клад однозначно здесь.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //34

            new string[] { "Сундук! И опять на кодовом замке. Посмотрим, что тут...", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //35
            new string[] { "Кнопки с буквами. Нужно нажать их в правильном порядке.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //36
            new string[] { "В следующем задании вам необходимо найти слова.", "Игра", Environment.CurrentDirectory + "\\Resources\\Sprites\\scroll.png", "phrase" }, //37
            new string[] { "Выделяйте их от первой буквы до последней и жмите Enter. R для очищения выделения.", "Игра", Environment.CurrentDirectory + "\\Resources\\Sprites\\scroll.png", "phrase" }, //38

            new string[] { "Ура! У меня получилось! Сколько тут золота! Я богат!", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //39
            new string[] { "Основной квест выполнен, вы нашли клад. Можете свободно погулять по локации.", "Игра", Environment.CurrentDirectory + "\\Resources\\Sprites\\scroll.png", "phrase" }, //40
            new string[] { "Встаньте на знак конца уровня, когда захотите закончить игру.", "Игра", Environment.CurrentDirectory + "\\Resources\\Sprites\\scroll.png", "phrase" }, //41

            new string[] { "Если будет слишком сложно, я могу поискать ответ в интернете.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //42
            new string[] { "Но я за городом, здесь интернет дорогой, каждое использование будет стоить 10 монет.", "{player}", Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png", "phrase" }, //43
        };

        public void Phrase(int id)
        {
            menuStrip1.Enabled = false;
            isInDialog = true;
            CurrentPhrase = id;
            DrawDelegate = null;
            DrawDelegate += GameField.Draw;
            Dialog.Draw(Dialogs[id][0], Dialogs[id][1], Dialogs[id][2], Dialogs[id][3]);
        }

        private void Game_Click(object sender, EventArgs e)
        {
            if (isInDialog)
            {
                NextPhrase();
            }
        }

        public delegate void DialogDelegate1(int id);
        public delegate void DialogDelegate2();
        public delegate void DialogDelegate3(Player player);
        public DialogDelegate1 Delegate;
        public static DialogDelegate2 DrawDelegate;
        public static DialogDelegate3 PlayerDelegate;
        public void ReleaseGUI()
        {
            textBox1.Enabled = true;
            button1.Enabled = true;
        }
        public void SetPlayer(Player player)
        {
            GameField.Player = player;
            Player = player;
        }

        private void ЖурналToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Journal.PlayerNick = PlayerNick;
            Journal j = new Journal();
            j.ShowDialog();
        }
        #endregion

        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            TitleScreen.outputDevice.Stop();
            TitleScreen.outputDevice.Dispose();
            TitleScreen.curAudioFile.Dispose();
            FileInfo[] fi;
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\Resources");
            fi = di.GetFiles("*.mp3");
            for (int i = 0; i < fi.Length; i++)
            {
                try { fi[i].Delete(); } catch { }
            }
        }
    }
}
