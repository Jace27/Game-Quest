using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GameQuest
{
    public partial class HTMLCode : Form
    {
        public HTMLCode()
        {
            InitializeComponent();
            using (StreamReader htmlfile = new StreamReader("HTMLCode.txt", Encoding.UTF8))
            {
                string line;
                while ((line = htmlfile.ReadLine()) != null)
                {
                    listBox1.Items.Add(line);   //Добавление в listBox
                }
            }
            NumberStart++;
        }

        int[] NomeraStrok = new int[] { 5, 10, 19, 35 };    //Правильные ответы
        public static int NumberStart = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)  //Если textBox не пустой
            {
                for (int i = 0; i < NomeraStrok.Count(); i++)
                {
                    try
                    {
                        if (Convert.ToInt32(textBox1.Text.Substring(7)) == NomeraStrok[i])   //Если ответ совпал
                        {
                            switch (NomeraStrok[i])
                            {
                                case 5:
                                    listBox1.Items[4] = "    <title>Ковен \"Три звезды\"</title>";
                                    break;
                                case 10:
                                    listBox1.Items[9] = "      <header class=\"site-header\">";
                                    break;
                                case 19:
                                    listBox1.Items[18] = "      </section>";
                                    break;
                                case 35:
                                    listBox1.Items[34] = "      </footer>";
                                    break;
                            }
                            for (int i1 = i; i1 < NomeraStrok.Count() - 1; i1++)    //Сдвиг влево
                            {
                                NomeraStrok[i1] = NomeraStrok[i1 + 1];  //Смена
                            }
                            Array.Resize(ref NomeraStrok, NomeraStrok.Count() - 1); //Уменьшение массива
                            MessageBox.Show("Да, здесь была ошибка");
                            return;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка в вводе", "textBox1");  //Ошибка
                        textBox1.Focus();
                        return;
                    }
                }
                if (NomeraStrok.Count() == 0)
                {
                    MessageBox.Show("Найдены все ошибки");
                    NomeraStrok = new int[] { 5, 10, 19, 35 };  //Восстановление массива
                    Game.NextMap2 = true;   //Открытие новой локации
                    this.Dispose(); //Удаление формы
                    return;
                }
                MessageBox.Show("Здесь все в порядке");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "Строка " + Convert.ToString(listBox1.SelectedIndex + 1);   //Вывод в textBox
        }
    }
}
