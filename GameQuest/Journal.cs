using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace GameQuest
{
    public partial class Journal : Form
    {
        public Journal()
        {
            InitializeComponent();

            Icon = new Icon(TitleScreen.IconPath);

            richTextBox1.Focus();
            for (int i = 0; i < Notes.Length; i++)
            {
                string author = Notes[i][0];
                if (author == "{player}") author = PlayerNick;
                richTextBox1.Text += author;
                richTextBox1.Text += ": " + Notes[i][1];
                richTextBox1.Text += "\n";
            }
            PaintAuthors();
        }

        public static string PlayerNick;
        public static string[][] Notes = new string[0][];
        public static void NoteAdd(string Speaker, string Message)
        {
            Array.Resize<string[]>(ref Notes, Notes.Length + 1);
            Notes[Notes.Length - 1] = new string[2];
            Notes[Notes.Length - 1][0] = Speaker;
            Notes[Notes.Length - 1][1] = Message;
        }

        private void ЗакрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void RichTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void RichTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void RichTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void Journal_Paint(object sender, PaintEventArgs e)
        {
            PaintAuthors();
        }

        private void PaintAuthors()
        {
            string[] AllStrings = richTextBox1.Text.Split(new char[] { '\n' });
            string[] Authors = new string[0];
            Color[] Colors = new Color[0];
            int pos1 = 0;
            for (int i = 0; i < AllStrings.Length; i++)
            {
                try
                {
                    string str = AllStrings[i].Split(new char[] { ':' }, 2)[0];

                    int AuthorPosition = -1;
                    for (int l = 0; l < Authors.Length; l++)
                        if (Authors[l] == str)
                            AuthorPosition = l;
                    if (AuthorPosition == -1)
                    {
                        AuthorPosition = Authors.Length;
                        Array.Resize<string>(ref Authors, Authors.Length + 1);
                        Authors[Authors.Length - 1] = str;
                        Array.Resize<Color>(ref Colors, Colors.Length + 1);
                        if (str == "Фиолетовая Ведьма")
                        {
                            Colors[Colors.Length - 1] = Color.FromArgb(113, 9, 170);
                        }
                        else if (str == "Зеленая Ведьма")
                        {
                            Colors[Colors.Length - 1] = Color.FromArgb(26, 153, 26);
                        }
                        else if (str == "Красная Ведьма")
                        {
                            Colors[Colors.Length - 1] = Color.FromArgb(166, 0, 0);
                        }
                        else
                        {
                            Random Random = new Random();
                            int R = Random.Next(0, 256);
                            Thread.Sleep(10);
                            int G = Random.Next(0, 256);
                            Thread.Sleep(10);
                            int B = Random.Next(0, 256);
                            Colors[Colors.Length - 1] = Color.FromArgb(R, G, B);
                        }
                    }

                    int pos2 = str.Length;
                    richTextBox1.Select(pos1, pos2);
                    richTextBox1.SelectionColor = Colors[AuthorPosition];
                    richTextBox1.SelectionFont = new Font("Segoe Script", 10, FontStyle.Bold);
                }
                catch
                {

                }
                pos1 += AllStrings[i].Length + 1;
            }
        }
    }
}
