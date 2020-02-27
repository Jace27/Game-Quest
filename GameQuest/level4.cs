using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameQuest
{
    public partial class NumberSystemTranslation : Form
    {
        public NumberSystemTranslation(Game par)
        {
            InitializeComponent();
            RandomNumber();
            Parent = par;
            //Width = par.Width;
            //Height = par.Height;
            //Location = par.Location;
            Player = Game.Player;

            /*#region позиционирование текстбокса и лейбла
            double[] labelLocationPercent = new double[] { label1.Location.X / (900.0 / 100.0) - 1, label1.Location.Y / (600.0 / 100.0) - 1 };
            double[] textboxLocationPercent = new double[] { textBox1.Location.X / (900.0 / 100.0) - 1, textBox1.Location.Y / (600.0 / 100.0) - 1 };
            double[] labelSizePercent = new double[] { label1.Size.Width / (900.0 / 100.0), label1.Size.Height / (600.0 / 100.0) };
            double[] textboxSizePercent = new double[] { textBox1.Size.Width / (900.0 / 100.0), textBox1.Size.Height / (600.0 / 100.0) };

            label1.Location = new Point(
                (int)Math.Round(labelLocationPercent[0] * (Width / 100.0), 0),
                (int)Math.Round(labelLocationPercent[1] * (Height / 100.0), 0)
            );
            textBox1.Location = new Point(
                (int)Math.Round(textboxLocationPercent[0] * (Width / 100.0), 0),
                (int)Math.Round(textboxLocationPercent[1] * (Height / 100.0), 0)
            );
            label1.Size = new Size(
                (int)Math.Round(labelSizePercent[0] * (Width / 100.0), 0),
                (int)Math.Round(labelSizePercent[1] * (Height / 100.0), 0)
            );
            textBox1.Size = new Size(
                (int)Math.Round(textboxSizePercent[0] * (Width / 100.0), 0),
                (int)Math.Round(textboxSizePercent[1] * (Height / 100.0), 0)
            );
            #endregion*/
        }

        public static int NumberTranslation = 0;
        string Otvet = "";
        int Prav = 0;
        new Game Parent;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string vvod = Convert.ToString(textBox1.Text);  //Ввод числа
                if (vvod == Otvet)
                {
                    Prav++;

                    string[] door_locks = new string[] { "door12o", "door13o", "door22o", "door23o" };
                    GameObject[] tempobjs = new GameObject[Parent.GameField.Objects.Length + 1];
                    for (int i = 0; i < Parent.GameField.Objects.Length; i++)
                        tempobjs[i] = Parent.GameField.Objects[i];
                    tempobjs[tempobjs.Length - 1] = new GameObject(door_locks[Prav - 1]);
                    Parent.GameField.Draw();

                    label1.Text = "";   //Обнуление
                    textBox1.Text = ""; //Обнуление
                    MessageBox.Show(string.Format("Правильно {0}/{1}", Prav, 4));
                    if (Prav == 4)
                    {
                        Game.NextMap4 = true;   //Проход на новый уровень
                        NumberTranslation++;
                        this.Dispose(); //Закрыть форму
                        return;
                    }
                    RandomNumber();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка ввода");
            }
        }

        private void ProverkaVvoda(string Number, int Upper)
        {
            if (Upper != 2 && Upper != 8 && Upper != 16)
                throw new Exception("Ошибка ввода системы счисления");
            if (Upper == 2)
            {
                for (int i = 0; i < Number.Length; i++)
                {
                    if (Convert.ToInt32(Number.Substring(i, 1)) != 0 && Convert.ToInt32(Number.Substring(i, 1)) != 1)
                        throw new Exception("Неправильный ввод числа в систему счисления");
                }
            }
            if (Upper == 8)
            {
                for (int i = 0; i < Number.Length; i++)
                {
                    if (Convert.ToInt32(Number.Substring(i, 1)) < 0 || Convert.ToInt32(Number.Substring(i, 1)) > 7)
                        throw new Exception("Неправильный ввод числа в систему счисления");
                }
            }
            if (Upper == 16)
            {
                for (int i = 0; i < Number.Length; i++)
                {
                    if (int.TryParse(Number.Substring(i, 1), out int a) == false)
                    {
                        SmenaChisla(Number.Substring(i, 1), out a);
                    }
                }
            }
        }

        private void SmenaChisla(string symbol, out int number)
        {
            number = 0;
            switch (symbol)
            {
                case "A":
                    {
                        number = 10;
                        break;
                    }
                case "B":
                    {
                        number = 11;
                        break;
                    }
                case "C":
                    {
                        number = 12;
                        break;
                    }
                case "D":
                    {
                        number = 13;
                        break;
                    }
                case "E":
                    {
                        number = 14;
                        break;
                    }
                case "F":
                    {
                        number = 15;
                        break;
                    }
                default:
                    {
                        throw new Exception("Ошибка ввода числа");
                    }
            }
        }

        private void RandomNumber()
        {
            Random G = new Random();
            int kolvo = G.Next(0, 3);   //Номер системы счисления 0-(2) 1-(8) 2-(16)
            switch (kolvo)
            {
                case 0:
                    {
                        int dlina = G.Next(4, 12);  //Длина символов
                        for (int i = 0; i < dlina; i++)
                        {
                            label1.Text += G.Next(0, 2);  //Генерация числа 0 или 1
                        }
                        question = label1.Text;
                        label1.Text += "(2)";
                        hint = Environment.CurrentDirectory + "\\Resources\\Число2.png";
                        CreateGraphics().DrawImage(new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\btn.png"), 100, Height - 150, Width - 200, 150);
                        CreateGraphics().DrawImage(new Bitmap(hint), 150, Height - 100, Width - 300, 50);
                        break;
                    }
                case 1:
                    {
                        int dlina = G.Next(2, 5);  //Длина символов
                        for (int i = 0; i < dlina; i++)
                        {
                            label1.Text += G.Next(0, 8);  //Генерация числа от 0 до 8
                        }
                        question = label1.Text;
                        label1.Text += "(8)";
                        hint = Environment.CurrentDirectory + "\\Resources\\Число8.png";
                        CreateGraphics().DrawImage(new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\btn.png"), 100, Height - 150, Width - 200, 150);
                        CreateGraphics().DrawImage(new Bitmap(hint), 150, Height - 100, Width - 300, 50);
                        break;
                    }
                case 2:
                    {
                        int dlina = G.Next(1, 4);  //Длина символов
                        for (int i = 0; i < dlina; i++)
                        {
                            int chislo = G.Next(1, 16);  //Генерация числа 0 или 15
                            switch (chislo)
                            {
                                case 10:
                                    {
                                        label1.Text += "A";
                                        break;
                                    }
                                case 11:
                                    {
                                        label1.Text += "B";
                                        break;
                                    }
                                case 12:
                                    {
                                        label1.Text += "C";
                                        break;
                                    }
                                case 13:
                                    {
                                        label1.Text += "D";
                                        break;
                                    }
                                case 14:
                                    {
                                        label1.Text += "E";
                                        break;
                                    }
                                case 15:
                                    {
                                        label1.Text += "F";
                                        break;
                                    }
                                default:
                                    {
                                        label1.Text += chislo;
                                        break;
                                    }
                            }
                        }
                        question = label1.Text;
                        label1.Text += "(16)";
                        hint = Environment.CurrentDirectory + "\\Resources\\Число16.png";
                        CreateGraphics().DrawImage(new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\btn.png"), 100, Height - 150, Width - 200, 150);
                        CreateGraphics().DrawImage(new Bitmap(hint), 150, Height - 100, Width - 300, 50);
                        break;
                    }
            }
            try
            {
                //Перевод числа в из 2, 8, 16 в 10
                string[] perevod = Convert.ToString(label1.Text).Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                ProverkaVvoda(perevod[0], Convert.ToInt32(perevod[1]));
                string ost = Convert.ToString(perevod[0]);  //Число перевода
                int Pow = Convert.ToInt32(perevod[1]);  //Число степени перевода
                string ZapisSumm = "";
                string subst = "";
                int proizv = 0;
                for (int i = 0; i < ost.Length; i++)
                {
                    subst = ost.Substring(ost.Length - i - 1, 1);
                    if (int.TryParse(subst, out proizv) == false)
                    {
                        SmenaChisla(subst, out proizv);
                    }
                    ZapisSumm += (proizv * Math.Pow(Pow, i)).ToString() + " ";
                }
                string[] Podshet = ZapisSumm.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int Perevod = 0;
                for (int i = 0; i < Podshet.Length; i++)
                {
                    Perevod += Convert.ToInt32(Podshet[i]);
                }
                Otvet = Perevod.ToString();
                answer = Otvet;
            }
            catch
            {
                MessageBox.Show("Ошибка ввода");
            }
        }

        private void NumberSystemTranslation_Deactivate(object sender, EventArgs e)
        {
            Focus();
            Activate();
        }

        string hint;
        private void NumberSystemTranslation_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.DrawImage(new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\level4back.png"), 0, 0, Width, Height);
            e.Graphics.DrawImage(new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\lock.png"), 100, 100, Width - 200, Height - 200);
            e.Graphics.DrawImage(new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\btn.png"), 100, Height - 150, Width - 200, 150);
            if (hint != "")
                e.Graphics.DrawImage(new Bitmap(hint), 150, Height - 100, Width - 300, 50);
        }

        public Player Player { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        private void СмартфонToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hint hint = new Hint(this);
            if (hint.ShowDialog() == DialogResult.OK)
                Player.Inventory.Money -= 10;
        }

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
    }
}
