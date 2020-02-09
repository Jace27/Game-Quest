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
    public partial class GuessingWords : Form
    {
        public GuessingWords()
        {
            InitializeComponent();

            dataGridView1.RowCount = 10;    //Кол-во строк
            dataGridView1.ColumnCount = 8; //Кол-во столбцов
            dataGridView1.Rows[0].Cells[0].Value = "м";
            dataGridView1.Rows[0].Cells[1].Value = "а";
            dataGridView1.Rows[0].Cells[2].Value = "р";
            dataGridView1.Rows[0].Cells[3].Value = "а";
            dataGridView1.Rows[0].Cells[4].Value = "з";
            dataGridView1.Rows[0].Cells[5].Value = "р";
            dataGridView1.Rows[0].Cells[6].Value = "о";
            dataGridView1.Rows[0].Cells[7].Value = "т";
            dataGridView1.Rows[1].Cells[0].Value = "м";
            dataGridView1.Rows[1].Cells[1].Value = "д";
            dataGridView1.Rows[1].Cells[2].Value = "а";
            dataGridView1.Rows[1].Cells[3].Value = "ы";
            dataGridView1.Rows[1].Cells[4].Value = "е";
            dataGridView1.Rows[1].Cells[5].Value = "а";
            dataGridView1.Rows[1].Cells[6].Value = "б";
            dataGridView1.Rows[1].Cells[7].Value = "к";
            dataGridView1.Rows[2].Cells[0].Value = "а";
            dataGridView1.Rows[2].Cells[1].Value = "р";
            dataGridView1.Rows[2].Cells[2].Value = "н";
            dataGridView1.Rows[2].Cells[3].Value = "н";
            dataGridView1.Rows[2].Cells[4].Value = "ф";
            dataGridView1.Rows[2].Cells[5].Value = "у";
            dataGridView1.Rows[2].Cells[6].Value = "н";
            dataGridView1.Rows[2].Cells[7].Value = "а";
            dataGridView1.Rows[3].Cells[0].Value = "и";
            dataGridView1.Rows[3].Cells[1].Value = "г";
            dataGridView1.Rows[3].Cells[2].Value = "о";
            dataGridView1.Rows[3].Cells[3].Value = "р";
            dataGridView1.Rows[3].Cells[4].Value = "п";
            dataGridView1.Rows[3].Cells[5].Value = "а";
            dataGridView1.Rows[3].Cells[6].Value = "к";
            dataGridView1.Rows[3].Cells[7].Value = "ц";
            dataGridView1.Rows[4].Cells[0].Value = "н";
            dataGridView1.Rows[4].Cells[1].Value = "ф";
            dataGridView1.Rows[4].Cells[2].Value = "с";
            dataGridView1.Rows[4].Cells[3].Value = "т";
            dataGridView1.Rows[4].Cells[4].Value = "е";
            dataGridView1.Rows[4].Cells[5].Value = "м";
            dataGridView1.Rows[4].Cells[6].Value = "л";
            dataGridView1.Rows[4].Cells[7].Value = "и";
            dataGridView1.Rows[5].Cells[0].Value = "а";
            dataGridView1.Rows[5].Cells[1].Value = "о";
            dataGridView1.Rows[5].Cells[2].Value = "и";
            dataGridView1.Rows[5].Cells[3].Value = "с";
            dataGridView1.Rows[5].Cells[4].Value = "в";
            dataGridView1.Rows[5].Cells[5].Value = "м";
            dataGridView1.Rows[5].Cells[6].Value = "а";
            dataGridView1.Rows[5].Cells[7].Value = "о";
            dataGridView1.Rows[6].Cells[0].Value = "л";
            dataGridView1.Rows[6].Cells[1].Value = "р";
            dataGridView1.Rows[6].Cells[2].Value = "м";
            dataGridView1.Rows[6].Cells[3].Value = "а";
            dataGridView1.Rows[6].Cells[4].Value = "и";
            dataGridView1.Rows[6].Cells[5].Value = "а";
            dataGridView1.Rows[6].Cells[6].Value = "р";
            dataGridView1.Rows[6].Cells[7].Value = "н";
            dataGridView1.Rows[7].Cells[0].Value = "г";
            dataGridView1.Rows[7].Cells[1].Value = "о";
            dataGridView1.Rows[7].Cells[2].Value = "т";
            dataGridView1.Rows[7].Cells[3].Value = "т";
            dataGridView1.Rows[7].Cells[4].Value = "с";
            dataGridView1.Rows[7].Cells[5].Value = "с";
            dataGridView1.Rows[7].Cells[6].Value = "е";
            dataGridView1.Rows[7].Cells[7].Value = "а";
            dataGridView1.Rows[8].Cells[0].Value = "и";
            dataGridView1.Rows[8].Cells[1].Value = "р";
            dataGridView1.Rows[8].Cells[2].Value = "к";
            dataGridView1.Rows[8].Cells[3].Value = "и";
            dataGridView1.Rows[8].Cells[4].Value = "к";
            dataGridView1.Rows[8].Cells[5].Value = "а";
            dataGridView1.Rows[8].Cells[6].Value = "т";
            dataGridView1.Rows[8].Cells[7].Value = "л";
            dataGridView1.Rows[9].Cells[0].Value = "т";
            dataGridView1.Rows[9].Cells[1].Value = "м";
            dataGridView1.Rows[9].Cells[2].Value = "е";
            dataGridView1.Rows[9].Cells[3].Value = "ъ";
            dataGridView1.Rows[9].Cells[4].Value = "б";
            dataGridView1.Rows[9].Cells[5].Value = "о";
            dataGridView1.Rows[9].Cells[6].Value = "и";
            dataGridView1.Rows[9].Cells[7].Value = "л";
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int i1 = 0; i1 < dataGridView1.ColumnCount; i1++)
                {
                    dataGridView1.Rows[i].Height = 30;
                    dataGridView1.Columns[i1].Width = 30;
                    dataGridView1.Rows[i].Cells[i1].ReadOnly = true;
                }
            }
            dataGridView1.Height = dataGridView1.RowCount * 30 + 3;
            dataGridView1.Width = dataGridView1.ColumnCount * 30 + 3;
            Width = dataGridView1.Width + 24;
            Height = dataGridView1.Height + 24;
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[1]; //Выбор другой ячейки
            dataGridView1.ClearSelection(); //Снимаем выделение ячейки
            dataGridView1.MultiSelect = true;   //Выделение нескольких ячеек
            GuessingWordStart++;
            this.Text = "Отгадывание слов";
        }

        public static int GuessingWordStart = 0;

        string Slovo = "";  //Проверка
        int[][] Koord = new int[0][];   //Массив массивов с координатами букв
        string[] proverka = new string[] { "разработка", "программа", "данные", "функционал", "информатика", "система",
            "литерал", "массив", "алгоритм", "объект" };    //Все слова
        int Answer = 0; //Количество отгаданных слов

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
            {
                int k = 0;
                for (int i = 0; i < Koord.Count(); i++)
                {
                    if (Koord[i][0] == e.RowIndex && Koord[i][1] == e.ColumnIndex)
                    {
                        k++;
                    }
                }
                if (k == 0) //Если нет повторений
                {
                    Slovo += Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);   //Запись букв в слово
                    Array.Resize(ref Koord, Koord.Count() + 1); //Увеличение массива
                    Koord[Koord.Count() - 1] = new int[] { e.RowIndex, e.ColumnIndex }; //Запись в массив
                    for (int i = 0; i < Koord.Count(); i++)
                    {
                        dataGridView1.Rows[(Koord[i][0])].Cells[(Koord[i][1])].Selected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < Koord.Count(); i++)
                    {
                        dataGridView1.Rows[(Koord[i][0])].Cells[(Koord[i][1])].Selected = true;
                    }
                }
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)    //Если клавиша Enter
            {
                for (int i = 0; i < proverka.Length; i++)   //Проверка в массиве
                {
                    if (Slovo == proverka[i])   //Если слово есть в массиве
                    {
                        Random G = new Random();
                        int red = G.Next(50, 201);
                        int green = G.Next(50, 201);
                        int blue = G.Next(50, 201);
                        for (int i1 = 0; i1 < Koord.Count(); i1++)
                        {
                            dataGridView1.Rows[(Koord[i1][0])].Cells[(Koord[i1][1])].Style.BackColor = Color.FromArgb(red, green, blue);    //Закрашиваем
                        }
                        Answer++;
                        if (Answer == proverka.Length)
                        {
                            MessageBox.Show("Все отгадано");
                            Game.NextMap5 = true;
                            this.Dispose(); //Закрытие формы
                        }
                    }
                }
                Koord = new int[0][];   //Обнуляем
                Slovo = ""; //Обнуляем
            }
            if (e.KeyCode == Keys.R)
            {
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[1]; //Выбор другой ячейки
                dataGridView1.ClearSelection(); //Снимаем выделение ячейки
                Koord = new int[0][];   //Обнуляем
                Slovo = ""; //Обнуляем
            }
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[1]; //Выбор другой ячейки
                dataGridView1.ClearSelection(); //Снимаем выделение ячейки
            }
        }
    }
}
