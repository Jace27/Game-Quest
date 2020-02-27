using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameQuest
{
    public partial class Hint : Form
    {
        public Hint(Crossword cr)
        {
            InitializeComponent();

            Crossword = cr;
        }
        public Hint(NumberSystemTranslation cr)
        {
            InitializeComponent();

            NumberSystemTranslation = cr;
            comboBox1.KeyDown += ComboBox1_KeyDown;
            comboBox1.KeyPress += ComboBox1_KeyPress;
            comboBox1.KeyUp += ComboBox1_KeyUp;
            comboBox1.Text = cr.question;
        }

        private void ComboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void ComboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void ComboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        NumberSystemTranslation NumberSystemTranslation;
        Crossword Crossword;
        Image background = Image.FromFile(Environment.CurrentDirectory + "\\Resources\\Sprites\\smartphone.png");

        public void AddQuestion(string question)
        {
            comboBox1.Items.Add(question);
        }

        private void Hint_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(background, 0, 0, Width, Height);
        }

        private void Hint_MouseClick(object sender, MouseEventArgs e)
        {
            int Money = 0;
            if (Crossword != null) Money = Crossword.Player.Inventory.Money;
            else if (NumberSystemTranslation != null) Money = NumberSystemTranslation.Player.Inventory.Money;
            if (Money < 10)
            {
                MessageBox.Show("Извините, недостаточно средств на счете.", "Сообщение");
                return;
            }
            if (e.X >= 252 && e.X <= 252 + 25 && e.Y >= 237 && e.Y <= 237 + 25)
            {
                if (Crossword != null)
                {
                    Crossword.BuyedWord = Convert.ToInt32(comboBox1.SelectedItem.ToString().Split(new char[] { ')' }, StringSplitOptions.RemoveEmptyEntries)[0]);
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }
                if (NumberSystemTranslation != null)
                {
                    if (comboBox1.Text.IndexOf(" -> " + NumberSystemTranslation.answer) == -1)
                    {
                        comboBox1.Text += " -> " + NumberSystemTranslation.answer;
                        return;
                    }
                }
            }
        }
        
        new Button Close;
        private void Hint_Load(object sender, EventArgs e)
        {
            Close = new Button();
            Close.Name = "Close";
            Close.Text = "X";
            Close.Font = new Font("Arial", 18);
            Close.ForeColor = Color.Black;
            Close.BackColor = Color.FromArgb(255,255,255);
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

        private void Close_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Close.BackColor);
            TextFormatFlags Flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;
            TextRenderer.DrawText(e.Graphics, Close.Text, Close.Font, new Rectangle(0, 0, Close.Width, Close.Height), Close.ForeColor, Flags);
        }

        private void Close_MouseLeave(object sender, EventArgs e)
        {
            Close.BackColor = Color.FromArgb(255, 255, 255);
            Close.Invalidate();
        }

        private void Close_MouseEnter(object sender, EventArgs e)
        {
            Close.BackColor = Color.FromArgb(240, 60, 70);
            Close.Invalidate();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            if (Crossword != null)
            {
                DialogResult = DialogResult.Cancel;
            }
            else if (NumberSystemTranslation != null)
            {
                if (comboBox1.Text.IndexOf(" -> " + NumberSystemTranslation.answer) == -1)
                {
                    DialogResult = DialogResult.Cancel;
                }
                else
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
