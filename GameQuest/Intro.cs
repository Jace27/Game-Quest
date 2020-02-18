using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameQuest
{
    public partial class Intro : Form
    {
        public Intro(Game par)
        {
            InitializeComponent();

            Icon = new Icon(TitleScreen.IconPath);

            Size = par.Size;
            Location = par.Location;
            Comics = new Bitmap(Environment.CurrentDirectory + "\\Resources\\comic.bmp");
        }

        Image Comics;
        Point Zero = new Point(0, 0);
        Rectangle Scope = new Rectangle(0, 10, 450, 300);

        TextFormatFlags Flags { get; set; } = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;
        string CurrentPhrase = "";

        private void Intro_Paint(object sender, PaintEventArgs e)
        {
            Draw();
        }

        private void Draw()
        {
            Bitmap bmp = new Bitmap(Scope.Width, Scope.Height, Comics.PixelFormat);
            bmp.SetResolution(72.009f, 72.009f);
            Graphics gr1, gr2, gr3;
            gr1 = Graphics.FromImage((Image)bmp);
            gr1.DrawImage(Comics, Zero.X, Zero.Y, Scope, GraphicsUnit.Pixel);
            gr1.Dispose();
            Image img = new Bitmap(Width, Height);
            gr2 = Graphics.FromImage(img);
            gr2.DrawImage((Image)bmp, 0, 0, Width, Height);
            if (CurrentPhrase != "")
            {
                gr2.FillRectangle(Brushes.White, new Rectangle(Width - 300, 0, 300, 150));
                gr2.DrawRectangle(Pens.Black, Width - 301, 0, 300, 150);
                gr2.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1);
                gr2.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                TextRenderer.DrawText(gr2, CurrentPhrase, new Font("Segoe Script", 12f), new Rectangle(Width - 300, 0, 300, 150), Color.Black, Flags);
            }
            gr2.Dispose();
            gr3 = CreateGraphics();
            gr3.DrawImage(img, 0, 0, Width, Height);
            gr3.Dispose();
            bmp.Dispose();
            img.Dispose();
        }

        private async void Intro_Load(object sender, EventArgs e)
        {
            await Task.Run(() => { Thread.Sleep(500); });

            int x, y;
            x = 0;
            y = 10;
            int x1, x2, y1, y2;
            CurrentPhrase = "Я - обычный программист из небольшой IT-компании";
            while (x < Comics.Width - 450)
            {
                Scope = new Rectangle(x, y, 450, 300);
                Draw();
                await Task.Run(() => { Thread.Sleep(20); });
                x+=3;
            }

            x1 = 220;
            y1 = 10;
            x2 = 0;
            y2 = 345;
            while (x > 0)
            {
                y = (x - x1) * (y2 - y1) / (x2 - x1) + y1;
                Scope = new Rectangle(x, y, 450, 300);
                Draw();
                await Task.Run(() => { Thread.Sleep(20); });
                x -= 6;
            }

            CurrentPhrase = "Однажды мне понадобилось найти в подсобке несколько вещей";
            while (x < Comics.Width - 450)
            {
                Scope = new Rectangle(x, y, 450, 300);
                Draw();
                await Task.Run(() => { Thread.Sleep(20); });
                x+=3;
            }

            x1 = 220;
            x2 = 0;
            y1 = 338;
            y2 = 633;
            int w1, w2, h1, h2;
            w1 = 450;
            w2 = 670;
            h1 = 300;
            h2 = 447;
            Task task = Task.Run(() =>
            {
                CurrentPhrase = "И среди старых дискет мне попалась одна с надписью \"Секретно\". Я не удержался и взял эту дискету";
                Thread.Sleep(6000);
                CurrentPhrase = "На ней оказалась записана карта сокровищ. Интересно, куда она меня приведет?";
                Draw();
                Thread.Sleep(3000);
            });
            while (x > 0)
            {
                y = (x - x1) * (y2 - y1) / (x2 - x1) + y1;
                int w = (x - x1) * (w2 - w1) / (x2 - x1) + w1;
                int h = (x - x1) * (h2 - h1) / (x2 - x1) + h1;
                Scope = new Rectangle(x, y, w, h);
                Draw();
                await Task.Run(() => { Thread.Sleep(20); });
                x -= 5;
            }

            while(task.Status == TaskStatus.Running)
            {
                await Task.Run(() => { Thread.Sleep(100); });
            }
            Thread.Sleep(2000);

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
