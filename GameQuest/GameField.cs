using System;
using System.Threading;
using System.Drawing;
using System.IO;

namespace GameQuest
{
    public class GameField : IDisposable
    {
        public GameField(string loc, bool isfogged)
        {
            Location = loc;
            IsFogged = isfogged;

            Initialize();
        }

        private void Initialize()
        {
            FileInfo fi1 = new FileInfo(Environment.CurrentDirectory + "\\Resources\\Locations\\" + Location);
            FileInfo fi2 = new FileInfo(Environment.CurrentDirectory + "\\Resources\\Locations\\" + Location + "Objects");
            FileInfo fi3 = new FileInfo(Environment.CurrentDirectory + "\\Resources\\Locations\\" + Location + "Passable");
            if (!fi1.Exists || !fi2.Exists || !fi3.Exists) throw new Exception("Не найдено данных о локации " + Location);

            //чтение местности из файла
            string[][] map = new string[0][];
            using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + "\\Resources\\Locations\\" + Location))
            {
                int i = 0;
                int prevLen = 0;
                while (!sr.EndOfStream)
                {

                    string temp = sr.ReadLine();
                    if (!String.IsNullOrEmpty(temp) && !String.IsNullOrWhiteSpace(temp))
                    {
                        Array.Resize<string[]>(ref map, map.Length + 1);
                        map[i] = temp.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (i > 0)
                        {
                            if (map[i].Length != prevLen)
                                throw new Exception("Ошибка в файле данных локации (" + Location + ")");
                            else
                                prevLen = map[i].Length;
                        }
                        else
                        {
                            prevLen = map[i].Length;
                        }
                        i++;
                    }
                }
                Size = new Size(map[0].Length, map.Length);
                Field = new Block[Size.Width, Size.Height];
            }

            //чтение карты объектов из файла
            string[] objects = new string[0];
            using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + "\\Resources\\Locations\\" + Location + "Objects"))
            {
                int i = 0;
                while (!sr.EndOfStream)
                {
                    string temp = sr.ReadLine();
                    if (!String.IsNullOrEmpty(temp) && !String.IsNullOrWhiteSpace(temp))
                    {
                        Array.Resize<string>(ref objects, objects.Length + 1);
                        objects[i] = temp;
                        if (objects[i].IndexOf(':') == -1 || objects[i].IndexOf(' ') == -1)
                            throw new Exception("Ошибка при чтении данных объектов локации (" + Location + ")");
                        i++;
                    }
                }
            }

            //чтение карты проходимости из файла
            string[] passable = new string[0];
            using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + "\\Resources\\Locations\\" + Location + "Passable"))
            {
                int i = 0;
                while (!sr.EndOfStream)
                {
                    string temp = sr.ReadLine();
                    if (!String.IsNullOrEmpty(temp))
                    {
                        Array.Resize<string>(ref passable, passable.Length + 1);
                        passable[i] = temp;
                        if (passable[i].Length != Size.Width)
                            throw new Exception("Ошибка в файле данных проходимости по ширине поля");
                        i++;
                    }
                }
                if (passable.Length != Size.Height)
                    throw new Exception("Ошибка в файле данных проходимости по высоте поля");
            }

            //установка тумана войны
            Fog = new bool[Size.Width, Size.Height];
            for (int i = 0; i < Size.Width; i++)
            {
                for (int l = 0; l < Size.Height; l++)
                {
                    Fog[i, l] = true;
                }
            }

            //инициализация блоков и занесение в них местности (фоновых спрайтов)
            for (int i = 0; i < Size.Width; i++)
            {
                for (int l = 0; l < Size.Height; l++)
                {
                    FileInfo fi = new FileInfo(Environment.CurrentDirectory + "\\Resources\\Sprites\\" + map[l][i] + ".png");
                    if (!fi.Exists) fi = new FileInfo(Environment.CurrentDirectory + "\\Resources\\Sprites\\null.png");
                    Field[i, l] = new Block();
                    Field[i, l].Background = new Bitmap(fi.FullName);
                    Field[i, l].BackID = fi.Name;
                }
            }

            //занесение в блоки объектов
            for (int i = 0; i < objects.Length; i++)
            {
                string[] obj = objects[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                //минимальная длина массива
                int len = 3;
                if (obj.Length < len)
                    throw new Exception("Ошибка в файле данных объектов ("+Location+") - неполный объект на строке "+(i+1));

                //если длина массива больше, чем минимально нужно, значит все следующее объединяем в одну ячейку
                if (obj.Length > len)
                {
                    for (int l = len + 1; l < obj.Length; l++)
                        obj[len] += " " + obj[l];
                    Array.Resize<string>(ref obj, len + 1);
                }

                //вычленяем тип, имя и описание, если есть
                int sep = obj[len].IndexOf('|');
                if (sep != -1)
                {
                    string[] temp = obj[len].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    Array.Resize<string>(ref obj, len + temp.Length);
                    for (int l = 0; l < temp.Length; l++) obj[len + l] = temp[l];
                }

                //читаем координаты
                string[] coords = obj[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                int x = 0;
                int y = 0;
                try
                {
                    x = Convert.ToInt32(coords[0]);
                    y = Convert.ToInt32(coords[1]);
                }
                catch
                {
                    throw new Exception("Ошибка в файле данных объектов (" + Location + ") - нечитаемые координаты на строке " + (i + 1));
                }

                //инициализируем объект по ID
                if (obj.Length >= len)
                    Field[x, y].Object = new GameObject(obj[1]);

                //заносим кол-во объектов
                try
                {
                    Field[x, y].Object.Count = Convert.ToInt32(obj[2]);
                    if (Field[x, y].Object.Count < 1)
                    {
                        Field[x, y].Object = null;
                        continue;
                    }
                }
                catch
                {
                    throw new Exception("Ошибка в файле данных объектов (" + Location + ") - нечитаемое кол-во объектов на строке " + (i + 1));
                }

                //заносим поднимаемость, имя и описание объекта
                if (obj.Length > 3)
                {
                    if (obj[3] != "item")
                    {
                        Field[x, y].Object.PickUpAble = false;
                        Field[x, y].Object.Name = obj[3];
                        if (obj.Length == 5)
                            Field[x, y].Object.Description = obj[4];
                    }
                    else
                    {
                        Field[x, y].Object.PickUpAble = true;
                        if (obj.Length >= 5)
                            Field[x, y].Object.Name = obj[4];
                        if (obj.Length == 6)
                            Field[x, y].Object.Description = obj[5];
                    }
                }
            }

            //занесение в блоки проходимости;
            for (int i = 0; i < passable.Length; i++)
            {
                for (int l = 0; l < passable[i].Length; l++)
                {
                    if (passable[i][l] == 'H')
                        Field[l, i].Passable = false;
                    else
                        Field[l, i].Passable = true;
                }
            }
        }

        public Graphics Graphics { get; set; }                  //объект графики
        public Point Zero { get; set; } = new Point(0, 0);      //позиция верхнего левого угла
        public Size Size { get; set; } = new Size(30, 20);      //размер локации (в блоках!)
        public Size BlockSize { get; set; } = new Size(30, 30); //размер блоков (в пикселях)
        public Block[,] Field { get; set; }                     //массив блоков, координатная плоскость
        public string Location { get; set; }                    //имя рисуемой локации (используется как имя файла локации в Resources/Locations!)
        public bool IsFogged { get; }                           //включен ли на локации туман войны
        public bool[,] Fog { get; set; }                        //туман войны; true - открытый блок, false - неоткрытый
        public Player Player { get; set; }                      //игрок на данном поле

        public void Draw()
        {
            Image buffer = new Bitmap(Size.Width * BlockSize.Width, Size.Height * BlockSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics gr = Graphics.FromImage(buffer))
            {
                gr.Clear(Color.White);
                for (int i = 0; i < Size.Width; i++)
                {
                    for (int l = 0; l < Size.Height; l++)
                    {
                        if (IsFogged)
                        {
                            Point startView = new Point(Player.Location.BL.X - Player.View, Player.Location.BL.Y - Player.View);
                            Point endView = new Point(Player.Location.BL.X + Player.View + 1, Player.Location.BL.Y + Player.View + 1);
                            if (startView.X < 0) startView.X = 0;
                            if (startView.Y < 0) startView.Y = 0;
                            if (endView.X >= Size.Width) endView.X = Size.Width;
                            if (endView.Y >= Size.Height) endView.Y = Size.Height;
                            for (int t = startView.X; t < endView.X; t++)
                            {
                                for (int j = startView.Y; j < endView.Y; j++)
                                {
                                    Fog[t, j] = false;
                                }
                            }
                        }
                        if (!Fog[i, l] || !IsFogged)
                        {
                            gr.DrawImage(Field[i, l].Background, i * BlockSize.Width, l * BlockSize.Height, BlockSize.Width, BlockSize.Height);
                            if (Field[i, l].Object != null)
                                gr.DrawImage(Field[i, l].Object.Background, i * BlockSize.Width, l * BlockSize.Height, BlockSize.Width, BlockSize.Height);
                        }
                        else
                        {
                            gr.DrawImage(new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\fog.png"), i * BlockSize.Width, l * BlockSize.Height, BlockSize.Width, BlockSize.Height);
                        }
                    }
                }
                gr.DrawImage(Player.Sprite, Player.Location.PX.X, Player.Location.PX.Y, BlockSize.Width, BlockSize.Height);
            }
            Graphics.DrawImage(buffer, Zero);
            buffer.Dispose();
        }

        //рисовка отдельного блока (координаты p в блоках)
        public void DrawBlock(Point p)
        {
            Image buffer = new Bitmap(BlockSize.Width, BlockSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics gr = Graphics.FromImage(buffer))
            {
                gr.DrawImage(Field[p.X, p.Y].Background, 0, 0, BlockSize.Width, BlockSize.Height);
                if (Field[p.X, p.Y].Object != null)
                    gr.DrawImage(Field[p.X, p.Y].Object.Background, 0, 0, BlockSize.Width, BlockSize.Height);
            }
            Graphics.DrawImage(buffer, Zero.X + p.X * BlockSize.Width, Zero.Y + p.Y * BlockSize.Height, BlockSize.Width, BlockSize.Height);
            buffer.Dispose();
        }

        //вывод фоновых спрайтов (местности)
        public void DrawField()
        {
            for (int i = 0; i < Size.Width; i++)
            {
                for (int l = 0; l < Size.Height; l++)
                {
                    if (!Fog[i, l])
                    {
                        DrawBlock(new Point(i, l));
                    }
                }
            }
        }

        //вывод тумана
        public void DrawFog()
        {
            for (int i = 0; i < Size.Width; i++)
            {
                for (int l = 0; l < Size.Height; l++)
                {
                    if (IsFogged && Fog[i, l])
                    {
                        Graphics.DrawImage(new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\fog.png"), Zero.X + i * BlockSize.Width, Zero.Y + l * BlockSize.Height, BlockSize.Width, BlockSize.Height);
                    }
                }
            }
        }

        //вывод объектов
        public void DrawObjects()
        {
            for (int i = 0; i < Size.Width; i++)
            {
                for (int l = 0; l < Size.Height; l++)
                {
                    if (Field[i, l].Object != null)
                    {
                        Graphics.DrawImage(Field[i, l].Object.Background, Zero.X + i * BlockSize.Width, Zero.Y + l * BlockSize.Height, BlockSize.Width, BlockSize.Height);
                    }
                }
            }
        }

        public void Dispose()
        {
            Graphics.Dispose();
        }
    }
    public class Block
    {
        public Block()
        {
            Passable = false;
        }

        public Image Background { get; set; }   //фоновая картинка (спрайт местности)
        public string BackID { get; set; }
        public GameObject Object { get; set; }  //объект на данном блоке
        public bool Passable { get; set; }      //проходимость для игрока
    }
    public class GameObject
    {
        public GameObject(string id)
        {
            Id = id;
            FileInfo fi = new FileInfo(Environment.CurrentDirectory + "\\Resources\\Sprites\\" + Id + ".png");
            if (!fi.Exists) fi = new FileInfo(Environment.CurrentDirectory + "\\Resources\\Sprites\\null.png");
            Background = new Bitmap(fi.FullName);
            PickUpAble = false;
        }

        public Image Background { get; set; }   //спрайт объекта
        public string Name { get; set; }        //имя объекта
        public string Description { get; set; } //описание объекта
        public string Id { get; set; }          //идентификатор объекта (используется как имя файла спрайта!)
        public bool PickUpAble { get; set; }    //может ли игрок поднять предмет
        public int Count { get; set; } = 1;     //кол-во
    }
    public class Player
    {
        public Player()
        {
            Inventory = new Inventory();
            FileInfo fi = new FileInfo(Environment.CurrentDirectory + "\\Resources\\Sprites\\player.png");
            if (fi.Exists)
                Sprite = new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\player.png");
            else
                Sprite = new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\null.png");

            MoveStages = new Image[CurrentMoveStages];
            for (int i = 0; i < CurrentMoveStages; i++)
            {
                fi = new FileInfo(Environment.CurrentDirectory + "\\Resources\\Sprites\\player"+i+".png");
                if (!fi.Exists)
                    MoveStages[i] = Sprite;
                else
                    MoveStages[i] = new Bitmap(fi.FullName);
            }
        }

        public delegate void EventHandlerItem(GameObject item);
        public event EventHandlerItem onStepUpItem;
        public event EventHandlerItem onPickUpItem;
        
        public PlayerLocation Location { get; set; }        //позиция игрока на поле
        public Image Sprite { get; set; }                   //спрайт персонажа
        private Image[] MoveStages;
        public GameField GameField { get; set; }            //экземпляр игрового поля, на котором находится игрок
        public int View { get; set; } = 2;                  //поле зрения
        public Inventory Inventory { get; set; }            //инвентарь
        public int MoveAnimationTime { get; set; } = 150;  //время анимации движения на 1 клетку (в миллисекундах)
        public int MoveAnimationFrames { get; set; } = 20;  //кол-во кадров анимации движения
        private int MoveStage = 0;
        private int CurrentMoveStages = 3;

        //вывод игрока
        public void DrawPlayer()
        {
            Image buffer;
            if (GameField.IsFogged)
            {
                Point startView = new Point(Location.BL.X - View, Location.BL.Y - View);
                Point endView = new Point(Location.BL.X + View + 1, Location.BL.Y + View + 1);
                if (startView.X < 0) startView.X = 0;
                if (startView.Y < 0) startView.Y = 0;
                if (endView.X >= GameField.Size.Width) endView.X = GameField.Size.Width;
                if (endView.Y >= GameField.Size.Height) endView.Y = GameField.Size.Height;
                buffer = new Bitmap((endView.X - startView.X + 1) * GameField.BlockSize.Width, (endView.Y - startView.Y + 1) * GameField.BlockSize.Height);
                using (Graphics gr = Graphics.FromImage(buffer))
                { 
                    for (int i = startView.X; i < endView.X; i++)
                    {
                        for (int l = startView.Y; l < endView.Y; l++)
                        {
                            GameField.Fog[i, l] = false;
                            gr.DrawImage(GameField.Field[i, l].Background, (i - startView.X) * GameField.BlockSize.Width, (l - startView.Y) * GameField.BlockSize.Height, GameField.BlockSize.Width, GameField.BlockSize.Width);
                            if (GameField.Field[i, l].Object != null)
                                gr.DrawImage(GameField.Field[i, l].Object.Background, (i - startView.X) * GameField.BlockSize.Width, (l - startView.Y) * GameField.BlockSize.Height, GameField.BlockSize.Width, GameField.BlockSize.Width);
                        }
                    }
                    gr.DrawImage(MoveStages[MoveStage], Location.PX.X - startView.X * GameField.BlockSize.Width, Location.PX.Y - startView.Y * GameField.BlockSize.Width, GameField.BlockSize.Width, GameField.BlockSize.Height);
                }
                GameField.Graphics.DrawImage(buffer, GameField.Zero.X + startView.X * GameField.BlockSize.Width, GameField.Zero.Y + startView.Y * GameField.BlockSize.Height);
                buffer.Dispose();
            }
            else
            {
                GameField.Graphics.DrawImage(MoveStages[MoveStage], GameField.Zero.X + Location.PX.X, GameField.Zero.Y + Location.PX.Y, GameField.BlockSize.Width, GameField.BlockSize.Height);
            }
        }

        //примитивное движение игрока
        public void MovePlayer(string direction)
        {
            Point newP = new Point(0, 0);
            GameField.DrawBlock(new Point(Location.BL.X, Location.BL.Y));
            if (direction == "left")
            {
                newP.X = Location.BL.X - 1;
                newP.Y = Location.BL.Y;
                if (newP.X >= 0)
                {
                    if (GameField.Field[newP.X, newP.Y].Passable)
                    {
                        Location.BL = new Point(newP.X, newP.Y);
                    }
                }
            }
            if (direction == "up")
            {
                newP.X = Location.BL.X;
                newP.Y = Location.BL.Y - 1;
                if (newP.Y >= 0)
                {
                    if (GameField.Field[newP.X, newP.Y].Passable)
                    {
                        Location.BL = new Point(newP.X, newP.Y);
                    }
                }
            }
            if (direction == "right")
            {
                newP.X = Location.BL.X + 1;
                newP.Y = Location.BL.Y;
                if (newP.X < GameField.Size.Width)
                {
                    if (GameField.Field[newP.X, newP.Y].Passable)
                    {
                        Location.BL = new Point(newP.X, newP.Y);
                    }
                }
            }
            if (direction == "down")
            {
                newP.X = Location.BL.X;
                newP.Y = Location.BL.Y + 1;
                if (newP.Y < GameField.Size.Height)
                {
                    if (GameField.Field[newP.X, newP.Y].Passable)
                    {
                        Location.BL = new Point(newP.X, newP.Y);
                    }
                }
            }
            DrawPlayer();

            if (GameField.Field[Location.BL.X, Location.BL.Y].Object != null)
            {
                try { onStepUpItem(GameField.Field[Location.BL.X, Location.BL.Y].Object); } catch { }
            }

            Thread.Sleep(MoveAnimationTime);
        }

        //анимированное движение игрока
        string dir;
        PlayerLocation newP;
        PlayerLocation newPT;
        PlayerLocation oldP;
        int frame = 0;
        public void SmoothlyMovePlayer(string direction)
        {
            newP = new PlayerLocation(new Point(0, 0), GameField.BlockSize);
            oldP = Location;
            dir = direction;
            frame = 0;
            if (dir == "left") newP.BL = new Point(oldP.BL.X - 1, oldP.BL.Y);
            if (dir == "up") newP.BL = new Point(oldP.BL.X, oldP.BL.Y - 1);
            if (dir == "right") newP.BL = new Point(oldP.BL.X + 1, oldP.BL.Y);
            if (dir == "down") newP.BL = new Point(oldP.BL.X, oldP.BL.Y + 1);
            if (newP.BL.X >= 0 && newP.BL.Y >= 0 && newP.BL.X < GameField.Size.Width && newP.BL.Y < GameField.Size.Height)
            {
                if (GameField.Field[newP.BL.X, newP.BL.Y].Passable)
                {
                    newPT = new PlayerLocation(newP.BL, GameField.BlockSize);
                    while (frame < MoveAnimationFrames)
                    {
                        if (dir == "left")
                        {
                            newPT.PX = new Point(Location.PX.X - (int)Math.Round(Convert.ToDouble(GameField.BlockSize.Width) / MoveAnimationFrames, 0), Location.PX.Y);
                            if (GameField.Field[newPT.PX.X / GameField.BlockSize.Width,
                                                newPT.PX.Y / GameField.BlockSize.Height].Passable &&
                                newPT.PX.X > newP.PX.X) Location = newPT;
                            if (Location.PX.X < newP.PX.X) Location = newP;

                            if (frame % 10 == 0)
                                MoveStage--;
                            if (MoveStage == -1) MoveStage = CurrentMoveStages - 1;
                        }
                        if (dir == "up")
                        {
                            newPT.PX = new Point(Location.PX.X, Location.PX.Y - (int)Math.Round(Convert.ToDouble(GameField.BlockSize.Width) / MoveAnimationFrames, 0));
                            if (GameField.Field[newPT.PX.X / GameField.BlockSize.Width,
                                                newPT.PX.Y / GameField.BlockSize.Height].Passable &&
                                newPT.PX.Y > newP.PX.Y) Location = newPT;
                            if (Location.PX.Y < newP.PX.Y) Location = newP;

                            if (frame % 10 == 0)
                                MoveStage++;
                            if (MoveStage == CurrentMoveStages) MoveStage = 0;
                        }
                        if (dir == "right")
                        {
                            newPT.PX = new Point(Location.PX.X + (int)Math.Round(Convert.ToDouble(GameField.BlockSize.Width) / MoveAnimationFrames, 0), Location.PX.Y);
                            if (GameField.Field[newPT.PX.X / GameField.BlockSize.Width,
                                                newPT.PX.Y / GameField.BlockSize.Height].Passable &&
                                newPT.PX.X < newP.PX.X) Location = newPT;
                            if (Location.PX.X > newP.PX.X) Location = newP;

                            if (frame % 10 == 0)
                                MoveStage++;
                            if (MoveStage == CurrentMoveStages) MoveStage = 0;
                        }
                        if (dir == "down")
                        {
                            newPT.PX = new Point(Location.PX.X, Location.PX.Y + (int)Math.Round(Convert.ToDouble(GameField.BlockSize.Width) / MoveAnimationFrames, 0));
                            if (GameField.Field[newPT.PX.X / GameField.BlockSize.Width,
                                                newPT.PX.Y / GameField.BlockSize.Height].Passable &&
                                newPT.PX.Y < newP.PX.Y) Location = newPT;
                            if (Location.PX.Y > newP.PX.Y) Location = newP;

                            if (frame % 10 == 0)
                                MoveStage--;
                            if (MoveStage == -1) MoveStage = CurrentMoveStages - 1;
                        }
                        if (!GameField.IsFogged)
                        {
                            GameField.DrawBlock(oldP.BL);
                            GameField.DrawBlock(newP.BL);
                        }
                        DrawPlayer();
                        Thread.Sleep(MoveAnimationTime / MoveAnimationFrames);
                        frame++;
                    }
                    Location.BL = newP.BL;
                    if (!GameField.IsFogged)
                        GameField.DrawBlock(newP.BL);
                    MoveStage = 0;
                    DrawPlayer();
                }
            }

            if (Location.BL.X >= 0 && Location.BL.X < GameField.Size.Width &&
                Location.BL.Y >= 0 && Location.BL.Y < GameField.Size.Height)
            {
                if (GameField.Field[Location.BL.X, Location.BL.Y].Object != null)
                {
                    try { onStepUpItem(GameField.Field[Location.BL.X, Location.BL.Y].Object); } catch { }
                }
            }
        }

        //поднять предмет, на котором стоишь
        public void PickUpItem()
        {
            int X = Location.BL.X;
            int Y = Location.BL.Y;
            if (GameField.Field[X, Y].Object != null)
            {
                if (GameField.Field[X, Y].Object.PickUpAble)
                {
                    if (GameField.Field[X, Y].Object.Id == "coins")
                    {
                        Inventory.Money += GameField.Field[X, Y].Object.Count;
                    }
                    else
                    {
                        Inventory.AddItem(GameField.Field[X, Y].Object);
                    }
                    try { onPickUpItem(GameField.Field[X, Y].Object); } catch { }
                    GameField.Field[X, Y].Object = null;
                }
            }
        }
    }

    public class PlayerLocation
    {
        public PlayerLocation(Point location, Size BlockSize)
        {
            BL = new Point(0, 0);
            PX = new Point(0, 0);
            BL = location;
            PX = new Point(location.X * BlockSize.Width, location.Y * BlockSize.Height);
            scaleX = BlockSize.Width;
            scaleY = BlockSize.Height;
            onChangeLocation += PlayerLocation_onChangeLocation;
        }

        private bool isHandler = false;

        private int scaleX = 1;
        private int scaleY = 1;
        private void PlayerLocation_onChangeLocation(string sender)
        {
            if (PX.X % scaleX == 0 && PX.Y % scaleY == 0)
            {
                isHandler = true;
                if (sender == "px")
                    BL = new Point(PX.X / scaleX, PX.Y / scaleY);
                if (sender == "bl")
                    PX = new Point(BL.X * scaleX, BL.Y * scaleY);
                isHandler = false;
            }
        }

        private delegate void EventHandler(string sender);
        private event EventHandler onChangeLocation;

        private Point _bl;
        public Point BL
        {
            get { return _bl; }
            set
            {
                _bl = value;
                if (!isHandler)
                    try { onChangeLocation("bl"); } catch { }
            }
        }

        private Point _px;
        public Point PX
        {
            get { return _px; }
            set
            {
                _px = value;
                if (!isHandler)
                    try { onChangeLocation("px"); } catch { }
            }
        }
    }

    public class Inventory
    {
        public Inventory()
        {
            Items = new GameObject[0];
        }

        public int Money { get; set; } = 0;
        public GameObject[] Items { get; set; }

        public void AddItem(GameObject item)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].Id == item.Id)
                {
                    Items[i].Count += item.Count;
                    return;
                }
            }
            GameObject[] itemstemp = Items;
            Items = new GameObject[itemstemp.Length + 1];
            for (int i = 0; i < itemstemp.Length; i++)
                Items[i] = itemstemp[i];
            Items[itemstemp.Length] = item;
        }

        public void DeleteItem(string item)
        {
            int id = -1;
            for (int i = 0; i < Items.Length; i++)
                if (Items[i].Name == item || Items[i].Id == item) id = i;
            if (id >= 0)
            {
                GameObject[] itemstemp = new GameObject[Items.Length - 1];
                int i = 0;
                int l = 0;
                while (i < Items.Length)
                {
                    if (i != id)
                    {
                        itemstemp[l] = Items[i];
                        l++;
                    }
                    i++;
                }
                Items = itemstemp;
            }
        }
        public void DeleteItem(string item, int count)
        {
            int id = -1;
            for (int i = 0; i < Items.Length; i++)
                if (Items[i].Name == item || Items[i].Id == item) id = i;
            if (Items[id].Count <= count)
            {
                DeleteItem(item);
                return;
            }
            Items[id].Count -= count;
        }

        public bool FindItemB(string item)
        {
            for (int i = 0; i < Items.Length; i++)
                if (Items[i].Name == item || Items[i].Id == item)
                    return true;
            return false;
        }
        public int FindItemI(string item)
        {
            for (int i = 0; i < Items.Length; i++)
                if (Items[i].Name == item || Items[i].Id == item)
                    return Items[i].Count;
            return -1;
        }
        public GameObject FindItemG(string item)
        {
            for (int i = 0; i < Items.Length; i++)
                if (Items[i].Name == item || Items[i].Id == item)
                    return Items[i];
            return null;
        }
    }
}
