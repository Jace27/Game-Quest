using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace GameQuest
{
    public partial class Game
    {
        //Для 1 задания
        int Fuel = 100; //Топливо
        private bool F1Shop { get; set; } = false;    //Посещение 1 магазина
        private bool F2Shop { get; set; } = false;    //Посещение 2 магазина
        private bool F3Shop { get; set; } = false;    //Посещение 3 магазина
        private bool F4Shop { get; set; } = false;    //Посещение 4 магазина

        private void button1_Click(object sender, EventArgs e)
        {
            Thread level1 = new Thread(new ThreadStart(Level1));
            level1.Start();
        }

        private void Level1()
        {
            if (GameField.Location == "City" && !isInDialog)
            {
                isSaved = false;
                string vvod = textBox1.Text;    //Ввод текста
                //Разбиение на массив
                string[] proverka = vvod.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                int symbol1 = 0;    //Номер ( в строке
                int symbol2 = 0;    //Номер ) в строке
                int Hod = 0;
                for (int i = 0; i < proverka.Length; i++)  //Проход строк
                {
                    while (isInDialog) Thread.Sleep(100);
                    try
                    {
                        if (textBox1.Text.IndexOf('\n') > 0)
                        {
                            textBox1.Text = textBox1.Text.Remove(0, textBox1.Text.IndexOf('\n') + 1);
                            if (textBox1.Text.IndexOf('\n') > 0)
                                while (String.IsNullOrWhiteSpace(textBox1.Text.Substring(0, textBox1.Text.IndexOf('\n') - 1)) ||
                                String.IsNullOrWhiteSpace(textBox1.Text.Substring(0, textBox1.Text.IndexOf('\n') - 1)))
                                    textBox1.Text = textBox1.Text.Remove(0, textBox1.Text.IndexOf('\n') + 1);
                        }
                        symbol1 = proverka[i].IndexOf('('); //Номер ( в строке
                        symbol2 = proverka[i].IndexOf(')'); //Номер ) в строке
                        if (proverka[i].IndexOf("Move") != -1)  //Перемещение
                        {
                            try //В случае ошибки
                            {
                                //Количество ходов
                                Hod = Convert.ToInt32(proverka[i].Substring(symbol1 + 1, symbol2 - symbol1 - 1));
                                for (int i1 = 0; i1 < Hod; i1++)
                                {
                                    while (isInDialog) Thread.Sleep(100);
                                    int row = Player.Location.PX.X;    //Текущая позиция игрока X
                                    int cells = Player.Location.PX.Y;  //Текущая позиция игрока Y
                                    if (!isInDialog)
                                    {
                                        if (Player.Rotation == 0)    //Вверх
                                        {
                                            if (!isAnimateMoving)
                                                lock (new object())
                                                    Player.MovePlayer("up");
                                            else
                                                lock (new object())
                                                    Player.SmoothlyMovePlayer("up");
                                            Fuel--;
                                            label1.Text = String.Format("Осталось топлива: {0:0}", Fuel);
                                        }
                                        else if (Player.Rotation == 1)    //Влево
                                        {
                                            if (!isAnimateMoving)
                                                lock (new object())
                                                    Player.MovePlayer("left");
                                            else
                                                lock (new object())
                                                    Player.SmoothlyMovePlayer("left");
                                            Fuel--;
                                            label1.Text = String.Format("Осталось топлива: {0:0}", Fuel);
                                        }
                                        else if (Player.Rotation == 2)    //Вниз
                                        {
                                            if (!isAnimateMoving)
                                                lock (new object())
                                                    Player.MovePlayer("down");
                                            else
                                                lock (new object())
                                                    Player.SmoothlyMovePlayer("down");
                                            Fuel--;
                                            label1.Text = String.Format("Осталось топлива: {0:0}", Fuel);
                                        }
                                        else if (Player.Rotation == 3)    //Вправо
                                        {
                                            if (!isAnimateMoving)
                                                lock (new object())
                                                    Player.MovePlayer("right");
                                            else
                                                lock (new object())
                                                    Player.SmoothlyMovePlayer("right");
                                            Fuel--;
                                            label1.Text = String.Format("Осталось топлива: {0:0}", Fuel);
                                        }
                                        if (Fuel < 1)   //Если топливо закончилось
                                        {
                                            GameField.DrawScreen("Топливо закончилось, вы проиграли.\nУровень начнется заново...", Color.FromArgb(10, 10, 10), Color.AntiqueWhite);
                                            Player.Location = new InGameLocation(new Point(4, 9));
                                            GameField.Draw();
                                            Fuel = 100;
                                            label1.Text = String.Format("Осталось топлива: {0:0}", Fuel);
                                            return;
                                        }
                                        this.Invoke(PlayerDelegate, new object[] { Player });
                                    }
                                }
                            }
                            catch
                            {
                                Phrase(10);
                                return;
                            }
                        }
                        if (proverka[i].IndexOf("Rotate") != -1)    //Поворот
                        {
                            while (isInDialog) Thread.Sleep(100);
                            try
                            {
                                Player.Rotation = Convert.ToInt32(proverka[i].Substring(symbol1 + 1, symbol2 - symbol1 - 1));
                                Fuel--;
                                if (Player.Rotation == 0)
                                    Player.Sprite = new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\car1.png");
                                if (Player.Rotation == 1)
                                    Player.Sprite = new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\car4.png");
                                if (Player.Rotation == 2)
                                    Player.Sprite = new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\car3.png");
                                if (Player.Rotation == 3)
                                    Player.Sprite = new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\car2.png");
                            }
                            catch
                            {
                                Phrase(10);
                                return;
                            }
                        }
                        if (proverka[i].IndexOf("Parking") != -1)
                        {
                            int row = Player.Location.BL.X * GameField.BlockSize.Width;    //Текущая позиция игрока X
                            int cell = Player.Location.BL.Y * GameField.BlockSize.Height;  //Текущая позиция игрока Y
                            if ((row == 7 * GameField.BlockSize.Width && cell == 9 * GameField.BlockSize.Height) || 
                                (row == 8 * GameField.BlockSize.Width && cell == 8 * GameField.BlockSize.Height))
                            {
                                F1Shop = true;
                                Phrase(12);
                            }
                            else if ((row == 13 * GameField.BlockSize.Width && cell == 12 * GameField.BlockSize.Height) || 
                                     (row == 14 * GameField.BlockSize.Width && cell == 11 * GameField.BlockSize.Height))
                            {
                                F2Shop = true;
                                Phrase(13);
                            }
                            else if (row == 21 * GameField.BlockSize.Width && cell == 4 * GameField.BlockSize.Height)
                            {
                                F3Shop = true;
                                Phrase(14);
                            }
                            else if (row == 8 * GameField.BlockSize.Width && cell == 18 * GameField.BlockSize.Height)
                            {
                                F4Shop = true;
                                Phrase(15);
                            }
                            else
                            {
                                Phrase(11);
                            }
                        }
                    }
                    catch
                    {
                        Phrase(10);
                        return;
                    }
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ReferenceForLevel1 rfl1 = new ReferenceForLevel1();
            rfl1.ShowDialog();
        }
    }
}
