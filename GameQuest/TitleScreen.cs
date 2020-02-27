using System;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace GameQuest
{
    public partial class TitleScreen : Form
    {
        public static string IconPath = Environment.CurrentDirectory + "\\Resources\\Sprites\\AppIco.ico";

        public TitleScreen()
        {
            InitializeComponent();

            Icon = new Icon(IconPath);

            try
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
                if (outputDevice == null)
                {
                    outputDevice = new WaveOutEvent();
                }
                if (curAudioFile == null)
                {
                    audioFiles = new string[6];
                    audioFiles[0] = Environment.CurrentDirectory + "\\Resources\\title_theme.mp3";
                    audioFiles[1] = Environment.CurrentDirectory + "\\Resources\\city_theme.mp3";
                    audioFiles[2] = Environment.CurrentDirectory + "\\Resources\\forest_theme.mp3";
                    audioFiles[3] = Environment.CurrentDirectory + "\\Resources\\plains_theme.mp3";
                    audioFiles[4] = Environment.CurrentDirectory + "\\Resources\\mountains_theme.mp3";
                    audioFiles[5] = Environment.CurrentDirectory + "\\Resources\\dungeon_theme.mp3";
                    curAudioFile = new AudioFileReader(audioFiles[0]);
                    outputDevice.Init(curAudioFile);
                }
                outputDevice.Play();
            }
            catch
            {
                audioInitiate = false;
            }

            Game = new Game(this);
            GameField = Game.GameField;

            textBox1.Focus();
        }

        public static WaveOutEvent outputDevice;
        public static string[] audioFiles;
        public static AudioFileReader curAudioFile;
        public bool audioInitiate { get; set; } = true;

        Game Game;
        GameField GameField;
        Settings GameSettings;

        private void StartNewGame_Click(object sender, EventArgs e)
        {
            Game = new Game(this);
            Game.GameField = GameField;
            Game.isStarted = false;
            Game.NextMap2 = false;
            Game.NextMap3 = false;
            Game.NextMap4 = false;
            Game.NextMap5 = false;
            HTMLCode.NumberStart = 0;
            Crossword.CrosswordStart = 0;
            NumberSystemTranslation.NumberTranslation = 0;
            GuessingWords.GuessingWordStart = 0;
            Game.PlayerNick = textBox1.Text;
            Task.WaitAll();
            Intro intro = new Intro(Game);
            intro.ShowDialog();
            intro.Dispose();
            Game.Show();
            this.Hide();
        }

        private void LoadGame_Click(object sender, EventArgs e)
        {
            Game = new Game(this);
            Game.GameField = GameField;
            Game.PlayerNick = textBox1.Text;
            Game.загрузитьИгруToolStripMenuItem.PerformClick();
            if (Game.SuccessfulLoad)
            {
                Game.Show();
                this.Hide();
            }
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            GameSettings = new Settings(curAudioFile);
            GameSettings.ShowDialog();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (audioInitiate)
                    if (curAudioFile.CurrentTime > curAudioFile.TotalTime - new TimeSpan(0, 0, 2))
                        curAudioFile.CurrentTime = new TimeSpan(0, 0, 0);
            }
            catch
            {
                float vol = curAudioFile.Volume;
                outputDevice.Stop();
                FileInfo fi1 = new FileInfo(audioFiles[0]);
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
                curAudioFile = new AudioFileReader(audioFiles[0]);
                outputDevice.Init(curAudioFile);
                outputDevice.Play();
                curAudioFile.Volume = vol;
            }
        }

        //ПЕРЕТАСКИВАНИЕ ФОРМЫ
        private Point mouseOffset;
        private bool isMouseDown = false;
        private void TitleScreen_MouseDown(object sender, MouseEventArgs e)
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

        private void TitleScreen_MouseMove(object sender, MouseEventArgs e)
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

        private void TitleScreen_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) isMouseDown = false;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrWhiteSpace(textBox1.Text))
            {
                StartNewGame.Enabled = false;
                LoadGame.Enabled = false;
            }
            else
            {
                StartNewGame.Enabled = true;
                LoadGame.Enabled = true;
            }
        }

        private void ExitGame_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                e.Handled = true;
                return;
            }
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((char)e.KeyChar == ':')
                e.Handled = true;
        }

        Image btn = new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\btn.png");
        TextFormatFlags Flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;

        private void StartNewGame_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(200, 189, 144));
            e.Graphics.DrawImage(btn, 0, 0, StartNewGame.Width, StartNewGame.Height);
            if (!StartNewGame.Enabled)
                StartNewGame.ForeColor = Color.Gray;
            else
                StartNewGame.ForeColor = Color.Black;
            TextRenderer.DrawText(e.Graphics, StartNewGame.Text, StartNewGame.Font, new Rectangle(0, 0, StartNewGame.Width, StartNewGame.Height), StartNewGame.ForeColor, Flags);
        }

        private void LoadGame_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(200, 189, 144));
            e.Graphics.DrawImage(btn, 0, 0, LoadGame.Width, LoadGame.Height);
            if (!LoadGame.Enabled)
                LoadGame.ForeColor = Color.Gray;
            else
                LoadGame.ForeColor = Color.Black;
            TextRenderer.DrawText(e.Graphics, LoadGame.Text, LoadGame.Font, new Rectangle(0, 0, LoadGame.Width, LoadGame.Height), LoadGame.ForeColor, Flags);
        }

        private void Settings_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(200, 189, 144));
            e.Graphics.DrawImage(btn, 0, 0, Settings.Width, Settings.Height);
            if (!Settings.Enabled)
                Settings.ForeColor = Color.Gray;
            else
                Settings.ForeColor = Color.Black;
            TextRenderer.DrawText(e.Graphics, Settings.Text, Settings.Font, new Rectangle(0, 0, Settings.Width, Settings.Height), Settings.ForeColor, Flags);
        }

        private void ExitGame_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(200, 189, 144));
            e.Graphics.DrawImage(btn, 0, 0, ExitGame.Width, ExitGame.Height);
            if (!ExitGame.Enabled)
                ExitGame.ForeColor = Color.Gray;
            else
                ExitGame.ForeColor = Color.Black;
            TextRenderer.DrawText(e.Graphics, ExitGame.Text, ExitGame.Font, new Rectangle(0, 0, ExitGame.Width, ExitGame.Height), ExitGame.ForeColor, Flags);
        }
    }
}
