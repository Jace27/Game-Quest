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
    public partial class Crossword : Form
    {
        public Crossword()
        {
            InitializeComponent();

            dataGridView1.RowCount = 25;
            dataGridView1.ColumnCount = 25;
            for (int i = 0; i < 19; i++)
            {
                dataGridView1.Rows[i + 2 + 1].Cells[2].Style.BackColor = Color.Snow;
                dataGridView1.Rows[i + 1].Cells[9 + 2].Style.BackColor = Color.Snow;
                dataGridView1.Rows[10].Cells[i + 2].Style.BackColor = Color.Snow;
            }
            dataGridView1.Rows[10].Cells[19 + 2].Style.BackColor = Color.Snow;
            dataGridView1.Rows[10].Cells[20 + 2].Style.BackColor = Color.Snow;
            dataGridView1.Rows[16 + 1].Cells[9 + 2].Style.BackColor = Color.Black;
            dataGridView1.Rows[17 + 1].Cells[9 + 2].Style.BackColor = Color.Black;
            dataGridView1.Rows[18 + 1].Cells[9 + 2].Style.BackColor = Color.Black;
            dataGridView1.Rows[19 + 1].Cells[9 + 2].Style.BackColor = Color.Black;
            for (int i = 0; i < 8; i++)
            {
                dataGridView1.Rows[i + 4 + 1].Cells[13 + 2].Style.BackColor = Color.Snow;
                dataGridView1.Rows[i + 8 + 1].Cells[6 + 2].Style.BackColor = Color.Snow;
                dataGridView1.Rows[11 + 1].Cells[i + 11 + 2].Style.BackColor = Color.Snow;
            }
            dataGridView1.Rows[17].Cells[6 + 2].Style.BackColor = Color.Snow;
            dataGridView1.Rows[11 + 1].Cells[19 + 2].Style.BackColor = Color.Snow;
            dataGridView1.Rows[11 + 1].Cells[20 + 2].Style.BackColor = Color.Snow;
            for (int i = 0; i < 12; i++)
            {
                dataGridView1.Rows[i + 11 + 1].Cells[11 + 2].Style.BackColor = Color.Snow;
            }
            for (int i = 0; i < 4; i++)
            {
                dataGridView1.Rows[5 + 1].Cells[i + 12 + 2].Style.BackColor = Color.Snow;
                dataGridView1.Rows[i + 17 + 1].Cells[9 + 1].Style.BackColor = Color.Snow;
                dataGridView1.Rows[19 + 1].Cells[i + 9 + 1].Style.BackColor = Color.Snow;
            }
            dataGridView1.Rows[5 + 1].Cells[16 + 2].Style.BackColor = Color.Snow;
            for (int i = 0; i < 25; i++)
            {
                for (int i1 = 0; i1 < 25; i1++)
                {
                    dataGridView1.Rows[i].Height = 20;  //Высота таблицы
                    dataGridView1.Columns[i1].Width = 20;   //Ширина таблицы
                    if (dataGridView1.Rows[i].Cells[i1].Style.BackColor == Color.Snow)
                    {
                        dataGridView1.Rows[i].Cells[i1].ReadOnly = false;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[i1].Style.BackColor = Color.Black;
                        dataGridView1.Rows[i].Cells[i1].Style.ForeColor = Color.White;
                        dataGridView1.Rows[i].Cells[i1].ReadOnly = true;
                    }
                }
            }
            dataGridView1.Rows[11].Cells[2].Value = " ";
            dataGridView1.Rows[10].Cells[6].Value = " ";
            dataGridView1.Rows[11].Cells[2].Style.BackColor = Color.DimGray;
            dataGridView1.Rows[10].Cells[6].Style.BackColor = Color.DimGray;
            dataGridView1.Rows[0].Cells[11].Value = "1";
            dataGridView1.Rows[20].Cells[9].Value = "2";
            dataGridView1.Rows[17].Cells[10].Value = "3";
            dataGridView1.Rows[6].Cells[13].Value = "4";
            dataGridView1.Rows[10].Cells[1].Value = "5";
            dataGridView1.Rows[2].Cells[2].Value = "6";
            dataGridView1.Rows[11 + 1].Cells[12].Value = "7";
            dataGridView1.Rows[11].Cells[13].Value = "8";
            dataGridView1.Rows[8].Cells[8].Value = "9";
            dataGridView1.Rows[4].Cells[13 + 2].Value = "10";
            dataGridView1.Rows[11].Cells[2].ReadOnly = true;
            dataGridView1.Rows[10].Cells[6].ReadOnly = true;
            dataGridView1.Height = dataGridView1.RowCount * 20 + 3; //Вся высота таблицы
            dataGridView1.Width = dataGridView1.ColumnCount * 20 + 3;   //Вся ширина таблицы
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[1]; //Выбор другой ячейки
            dataGridView1.ClearSelection(); //Снимаем выделение ячейки
            dataGridView1.MultiSelect = false;  //Выделение нескольких ячеек

            CrosswordStart++;
            this.Text = "Кроссворд";

            Size = new Size(12 + dataGridView1.Width + 12 + 400, dataGridView1.Height + 24);

            //Настройка listBox
            label1.Width = Width - (dataGridView1.Width + 24);    //Координаты listBox
            label1.Text = "1) Процесс создания компьютерных программ";
            label1.Text += "\n2) Топология данного типа представляет собой общий кабель, " +
                "к которому подсоединены все рабочие станции. На концах кабеля находятся терминаторы, " +
                "для предотвращения отражения сигнала";
            label1.Text += "\n3) Координатное устройство для управления курсором и отдачи различных команд компьютеру";
            label1.Text += "\n4) Применяют, когда необходимо определить стиль для индивидуального элемента " +
                "веб-страницы или задать разные стили для одного тега";
            label1.Text += "\n5) Формальный язык, предназначенный для записи компьютерных программ";
            label1.Text += "\n6) Важный принцип объектно-ориентированного программирования, " +
                "используемый для уменьшения зацепления в компьютерных программах";
            label1.Text += "\n7) Программа или техническое средство, выполняющее компиляцию программы";
            label1.Text += "\n8) Операция склеивания объектов линейной структуры, обычно строк";
            label1.Text += "\n9) Электронный блок, либо интегральная схема, исполняющая машинные инструкции";
            label1.Text += "\n10) Конечная совокупность точно заданных правил решения произвольного класса " +
                "задач или набор инструкций, описывающих порядок действий исполнителя для решения некоторой задачи";
        }

        public static int CrosswordStart = 0;

        string[] crossword = new string[] { "инверсия управления", "процессор", "программирование", "алгоритм", "конкатенация", "мышь",
                                            "класс", "язык программирования","компилятор", "шина"};
        int prav = 0;

        private void VvodBlock(int number)
        {
            switch (number)
            {
                case 0: //Инверсия
                    {
                        for (int i = 0; i < 19; i++)
                        {
                            dataGridView1.Rows[i + 2 + 1].Cells[2].ReadOnly = true;
                        }
                        break;
                    }
                case 1: //Процессор
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            dataGridView1.Rows[i + 8 + 1].Cells[6 + 2].ReadOnly = true;
                        }
                        break;
                    }
                case 2: //Программирование
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            dataGridView1.Rows[i + 1].Cells[9 + 2].ReadOnly = true;
                        }
                        break;
                    }
                case 3: //Алгоритм
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            dataGridView1.Rows[i + 4 + 1].Cells[13 + 2].ReadOnly = true;
                        }
                        break;
                    }
                case 4: //Конкатенация
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            dataGridView1.Rows[i + 11 + 1].Cells[11 + 2].ReadOnly = true;
                        }
                        break;
                    }
                case 5: //Мышь
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            dataGridView1.Rows[i + 18].Cells[8 + 2].ReadOnly = true;
                        }
                        break;
                    }
                case 6: //Класс
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            dataGridView1.Rows[6].Cells[i + 14].ReadOnly = true;
                        }
                        break;
                    }
                case 7: //Язык
                    {
                        for (int i = 0; i < 21; i++)
                        {
                            dataGridView1.Rows[10].Cells[i + 2].ReadOnly = true;
                        }
                        break;
                    }
                case 8: //Компилятор
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            dataGridView1.Rows[12].Cells[i + 13].ReadOnly = true;
                        }
                        break;
                    }
                case 9: //Шина
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            dataGridView1.Rows[20].Cells[i + 10].ReadOnly = true;
                        }
                        break;
                    }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
                {
                    string a = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    a = a.Substring(0, 1);
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = a;
                }
            }
            catch
            {

            }
            try
            {
                string slovo = "";
                if ((e.RowIndex >= (2 + 1) && e.RowIndex <= (19 + 2)) && e.ColumnIndex == (2)
                    && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly == false)  //Инверсия
                {
                    for (int i = 0; i < 19; i++)
                    {
                        slovo += Convert.ToString(dataGridView1.Rows[i + 2 + 1].Cells[2].Value);
                    }
                    Proverka(slovo);
                }
                if ((e.RowIndex >= (1) && e.RowIndex <= (16)) && e.ColumnIndex == (9 + 2)
                    && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly == false)  //Программирование
                {
                    for (int i = 0; i < 16; i++)
                    {
                        slovo += Convert.ToString(dataGridView1.Rows[i + 1].Cells[9 + 2].Value);
                    }
                    Proverka(slovo);
                }
                if ((e.RowIndex >= (5) && e.RowIndex <= (12)) && e.ColumnIndex == (13 + 2)
                    && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly == false)   //Алгоритм
                {
                    for (int i = 0; i < 8; i++)
                    {
                        slovo += Convert.ToString(dataGridView1.Rows[i + 4 + 1].Cells[13 + 2].Value);
                    }
                    Proverka(slovo);
                }
                if ((e.RowIndex >= (9) && e.RowIndex <= (17)) && e.ColumnIndex == (6 + 2)
                    && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly == false)    //Процессор
                {
                    for (int i = 0; i < 9; i++)
                    {
                        slovo += Convert.ToString(dataGridView1.Rows[i + 8 + 1].Cells[6 + 2].Value);
                    }
                    Proverka(slovo);
                }
                if ((e.RowIndex == (20)) && (e.ColumnIndex >= (8 + 2) && e.ColumnIndex <= (11 + 2))
                    && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly == false)  //Шина
                {
                    for (int i = 0; i < 4; i++)
                    {
                        slovo += Convert.ToString(dataGridView1.Rows[20].Cells[i + 10].Value);
                    }
                    Proverka(slovo);
                }
                if ((e.RowIndex >= (12) && e.RowIndex <= (23)) && e.ColumnIndex == (11 + 2)
                    && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly == false)  //Конкатенация
                {
                    for (int i = 0; i < 12; i++)
                    {
                        slovo += Convert.ToString(dataGridView1.Rows[i + 11 + 1].Cells[11 + 2].Value);
                    }
                    Proverka(slovo);
                }
                if ((e.RowIndex >= (18) && e.RowIndex <= (22)) && e.ColumnIndex == (8 + 2)
                    && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly == false)  //Мышь
                {
                    for (int i = 0; i < 4; i++)
                    {
                        slovo += Convert.ToString(dataGridView1.Rows[i + 18].Cells[8 + 2].Value);
                    }
                    Proverka(slovo);
                }
                if ((e.RowIndex == (10)) && (e.ColumnIndex >= (2) && e.ColumnIndex <= (20 + 2))
                    && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly == false)  //Язык
                {
                    for (int i = 0; i < 21; i++)
                    {
                        slovo += Convert.ToString(dataGridView1.Rows[10].Cells[i + 2].Value);
                    }
                    Proverka(slovo);
                }
                if ((e.RowIndex == (6)) && (e.ColumnIndex >= (14) && e.ColumnIndex <= (19 + 2))
                    && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly == false)  //Класс
                {
                    for (int i = 0; i < 5; i++)
                    {
                        slovo += Convert.ToString(dataGridView1.Rows[6].Cells[i + 14].Value);
                    }
                    Proverka(slovo);
                }
                if ((e.RowIndex == (12)) && (e.ColumnIndex >= (13) && e.ColumnIndex <= (20 + 2))
                    && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly == false)  //Компилятор
                {
                    for (int i = 0; i < 10; i++)
                    {
                        slovo += Convert.ToString(dataGridView1.Rows[12].Cells[i + 13].Value);
                    }
                    Proverka(slovo);
                }
            }
            catch
            {
                MessageBox.Show("Такого слова нет");
            }
        }

        private void Proverka(string otvet)
        {
            for (int i = 0; i < crossword.Count(); i++)
            {
                if (otvet == crossword[i])
                {
                    prav++;
                    MessageBox.Show("Вы отгадали слово");
                    VvodBlock(i);
                    if (prav == crossword.Count())
                    {
                        MessageBox.Show("Все отгадано");
                        Game.NextMap3 = true;
                        this.Dispose(); //Закрытие формы
                    }
                    return;
                }
            }
        }
    }
}
