using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GameQuest
{
    public partial class Game : Form
    {
        public Game()
        {
            InitializeComponent();
            InitGame("Forest", true);
            Player.View = 10;
        }

        GameField GameField;
        Player Player;

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            GameField.Draw();
        }

        private void InitGame(string location, bool fog)
        {
            //экземпляр игрового поля; в аргументы передаем имя локации (используется как имя файлов данных в Resources/Locations!) и туман войны
            //игровое поле может кидать много исключений! в том числе и кастомные
            GameField = new GameField(location, fog);
            GameField.Zero = new Point(0, 24);          //верхний левый угол игрового поля; 0:25 потому что 24 верхних пикселя занимает menuStrip
            GameField.Graphics = CreateGraphics();      //объект графики создавать только из CreateGraphics()!

            //экземпляр игрока
            Player = new Player();

            //даем игровому полю и игроку ссылки друг на друга, чтобы они могли рисовать друг на друге
            Player.GameField = GameField;
            GameField.Player = Player;

            if (location == "City")
                Player.Location = new PlayerLocation(new Point(4, 9), GameField.BlockSize); //для города
            if (location == "Forest")
                Player.Location = new PlayerLocation(new Point(0, 2), GameField.BlockSize); //для леса
            if (location == "Plains")
                Player.Location = new PlayerLocation(new Point(3, 0), GameField.BlockSize); //для леса

            Player.onStepUpItem += Player_onStepUpItem; //когда игрок наступает на объект вызываем обработчик
        }

        private void Player_onStepUpItem(GameObject item)
        {
            Player.PickUpItem(); //допустим, сразу подняли
        }

        bool isAnimateMoving = true; //анимированное движение или нет
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                if (!isAnimateMoving)
                {
                    Player.MovePlayer("left");
                }
                else
                {
                    Player.SmoothlyMovePlayer("left");
                }
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                if (!isAnimateMoving)
                {
                    Player.MovePlayer("up");
                }
                else
                {
                    Player.SmoothlyMovePlayer("up");
                }
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                if (!isAnimateMoving)
                {
                    Player.MovePlayer("right");
                }
                else
                {
                    Player.SmoothlyMovePlayer("right");
                }
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                if (!isAnimateMoving)
                {
                    Player.MovePlayer("down");
                }
                else
                {
                    Player.SmoothlyMovePlayer("down");
                }
            }
        }

        private void ИнвентарьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InventoryForm invf = new InventoryForm(Player);
            invf.ShowDialog();
        }

        private void городToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateGraphics().Clear(Color.White); //заливаем форму белым
            Inventory temp = Player.Inventory;   //сохраняем инвентарь игрока (объект будет пересоздан, инвентарь потеряется)
            InitGame("City", false);             //переинициализируем игру
            Player.Inventory = temp;             //возвращаем инвентарь
            GameField.Draw();
        }

        private void лесToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateGraphics().Clear(Color.White); //заливаем форму белым
            Inventory temp = Player.Inventory;   //сохраняем инвентарь игрока (объект будет пересоздан, инвентарь потеряется)
            InitGame("Forest", true);            //переинициализируем игру
            Player.Inventory = temp;             //возвращаем инвентарь
            GameField.Draw();
        }

        private void РавнинаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateGraphics().Clear(Color.White); //заливаем форму белым
            Inventory temp = Player.Inventory;   //сохраняем инвентарь игрока (объект будет пересоздан, инвентарь потеряется)
            InitGame("Plains", true);            //переинициализируем игру
            Player.Inventory = temp;             //возвращаем инвентарь
            Player.View = 20;
            GameField.Draw();
        }
    }
}
