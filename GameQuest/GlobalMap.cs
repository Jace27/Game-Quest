using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace GameQuest
{
    public partial class GlobalMap : Form
    {
        public GlobalMap(Point PlayerPosition)
        {
            InitializeComponent();

            Icon = new Icon(TitleScreen.IconPath);


            Position = PlayerPosition;

            isStarted = false;
            Draw();
        }
        public GlobalMap(Point PlayerPosition, Point MovePosition)
        {
            InitializeComponent();

            Position = PlayerPosition;
            NewPosition = MovePosition;

            isStarted = false;
        }

        private void Cycle()
        {
            while (!isStarted)
            {
                Draw();
                Thread.Sleep(500);
            }
        }

        public static Point[] Locations = new Point[]
        {
            new Point(110, 120),
            new Point(250, 220),
            new Point(150, 400),
            new Point(340, 350),
            new Point(480, 460)
        };
        public int AnimationTime { get; set; } = 3000;
        Point NewPosition;
        Point Position;
        Size PlSize = new Size(60, 60);
        Image Player = new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\playerd.png");
        Image Map = new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\map.png");

        object locker = new object();
        public void Draw()
        {
            lock (locker)
            {
                Image buffer = new Bitmap(Width, Height);
                using (Graphics gr = Graphics.FromImage(buffer))
                {
                    gr.DrawImage(Map, 0, 0, Width, Height);
                    gr.DrawImage(Player, Position.X - PlSize.Width / 2, Position.Y - PlSize.Height / 2, PlSize.Width, PlSize.Height);
                }
                this.CreateGraphics().DrawImage(buffer, 0, 0, Width, Height);
                buffer.Dispose();
            }
        }

        new public void Move(Point NewPosition)
        {
            if (Position.X > NewPosition.X)
            {
                for (int x = Position.X; x > NewPosition.X; x--)
                {
                    if (Position != NewPosition)
                    {
                        int x1 = Position.X;
                        int x2 = NewPosition.X;
                        int y1 = Position.Y;
                        int y2 = NewPosition.Y;
                        int y = (x - x1) * (y2 - y1) / (x2 - x1) + y1;
                        Position = new Point(x, y);
                    }
                    Draw();
                }
            }
            else
            {
                for (int x = Position.X; x < NewPosition.X; x++)
                {
                    if (Position != NewPosition)
                    {
                        int x1 = Position.X;
                        int x2 = NewPosition.X;
                        int y1 = Position.Y;
                        int y2 = NewPosition.Y;
                        int y = (x - x1) * (y2 - y1) / (x2 - x1) + y1;
                        Position = new Point(x, y);
                    }
                    Draw();
                }
            }

            Position = NewPosition;
            Draw();
        }

        private void GlobalMap_Paint(object sender, PaintEventArgs e)
        {
            Draw();
        }

        bool isStarted = false;
        new Button Close;
        private async void GlobalMap_Activated(object sender, EventArgs e)
        {
            Draw();
#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
            Task.Run(() => Cycle());
#pragma warning restore CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
            if (NewPosition != null && NewPosition != new Point(0, 0) && !isStarted)
            {
                await Task.Run(() => { Thread.Sleep(2000); });
                isStarted = true;
                Move(NewPosition);
                await Task.Run(() => { Thread.Sleep(2000); });
                Dispose();
            }
            else
            {
                Close = new Button();
                Close.Name = "Close";
                Close.Text = "🞫";
                Close.Font = new Font("Arial", 18);
                Close.ForeColor = Color.Black;
                Close.BackColor = Color.FromArgb(194, 184, 145);
                Close.Visible = true;
                Close.Enabled = true;
                Close.Width = 37;
                Close.Height = 37;
                Close.Location = new Point(Width - Close.Width, 0);
                Close.Click += Close_Click;
                Close.MouseEnter += Close_MouseEnter;
                Close.MouseLeave += Close_MouseLeave;
                Close.Paint += Close_Paint;
                Controls.Add(Close);
            }
        }

        private void Close_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Close.BackColor);
            TextFormatFlags Flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;
            TextRenderer.DrawText(e.Graphics, Close.Text, Close.Font, new Rectangle(0, 0, Close.Width, Close.Height), Close.ForeColor, Flags);
        }

        private void Close_MouseLeave(object sender, EventArgs e)
        {
            Close.BackColor = Color.FromArgb(194, 184, 145);
            Close.Invalidate();
        }

        private void Close_MouseEnter(object sender, EventArgs e)
        {
            Close.BackColor = Color.FromArgb(240, 60, 70);
            Close.Invalidate();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            isStarted = true;
            Task.WaitAll();
            Close();
        }
    }
}
