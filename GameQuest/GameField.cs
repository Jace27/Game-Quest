using System;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Text.Json.Serialization;

namespace GameQuest
{
    public class GameField : IDisposable
    {
        public GameField(string loc, bool isfogged, Graphics graphics)
        {
            Graphics = graphics;
            DrawScreen("Загрузка...", Color.FromArgb(167, 167, 179), Color.AntiqueWhite);

            Location = loc;
            IsFogged = isfogged;

            Initialize();
        }

        private void Initialize()
        {
            //ссылки на файлы данных локации
            FileInfo fi1 = new FileInfo(Environment.CurrentDirectory + "\\Resources\\Locations\\" + Location);              //местность
            FileInfo fi2 = new FileInfo(Environment.CurrentDirectory + "\\Resources\\Locations\\" + Location + "Objects");  //объекты для взаимодействия
            FileInfo fi3 = new FileInfo(Environment.CurrentDirectory + "\\Resources\\Locations\\" + Location + "Passable"); //карта проходимости
            //если файлов нет, кидаем ошибку
            if (!fi1.Exists || !fi2.Exists || !fi3.Exists) throw new Exception("Не найдено данных о локации " + Location);

            //чтение местности из файла
            string[][] map = new string[0][];
            using (StreamReader sr = new StreamReader(fi1.FullName)) //объект, читающий из файла
            {
                //чтение файла построчно
                int i = 0;       //индекс текущей строки
                int prevLen = 0; //длина предыдущей строки
                while (!sr.EndOfStream) //пока не достигли конца файла
                {
                    string temp = sr.ReadLine();    //текущая строка

                    //если прочитанная строка не пуста и не забита пробелами
                    if (!String.IsNullOrEmpty(temp) && !String.IsNullOrWhiteSpace(temp))
                    {
                        //расширяем массив
                        Array.Resize<string[]>(ref map, map.Length + 1);

                        //записываем на новое пустое место в массиве данные из текущей строки
                        //строка делится по символу табуляции на непустые подстроки
                        map[i] = temp.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        //если длина текущей строки отличается от длины предыдущей,
                        //то поле не прямоугольное, чего быть не может, кидаем ошибку
                        if (i > 0)
                            if (map[i].Length != prevLen)
                                throw new Exception("Ошибка в файле данных локации (" + Location + ")");

                        //запоминаем длину строки для следующей итерации
                        prevLen = map[i].Length; 

                        i++;
                    }
                }
                //определяем размеры игрового поля исходя из прочитанных данных
                Size = new Size(map[0].Length, map.Length);
                Field = new Block[Size.Width, Size.Height];
            }

            //чтение карты объектов из файла
            string[] objects = new string[0];
            using (StreamReader sr = new StreamReader(fi2.FullName)) //объект, читающий из файла
            {
                //чтение файла построчно
                int i = 0;  //индекс текущей строки
                while (!sr.EndOfStream) //пока не достигли конца файла
                {
                    string temp = sr.ReadLine(); //текущая строка

                    //если прочитанная строка не пуста и не забита пробелами
                    if (!String.IsNullOrEmpty(temp) && !String.IsNullOrWhiteSpace(temp))
                    {
                        //расширяем массив
                        Array.Resize<string>(ref objects, objects.Length + 1);

                        //заносим объект в массив
                        objects[i] = temp;

                        //отсутствие двоеточия говорит об отсутствии координат объекта
                        //отсутствие пробелов говорит об отсутствии идентификатора объекта
                        //если их нет, то кидаем ошибку
                        if (objects[i].IndexOf(':') == -1 || objects[i].IndexOf(' ') == -1)
                            throw new Exception("Ошибка при чтении данных объектов локации (" + Location + ")");

                        i++;
                    }
                }
            }

            //чтение карты проходимости из файла
            string[] passable = new string[0];
            using (StreamReader sr = new StreamReader(fi3.FullName)) //объект, читающий из файла
            {
                //чтение файла построчно
                int i = 0;  //индекс текущей строки
                while (!sr.EndOfStream) //пока не достигли конца файла
                {
                    string temp = sr.ReadLine(); //текущая строка

                    //если прочитанная строка не пуста
                    //при этом строка может быть забита пробелами, формат файла проходимости это позволяет
                    if (!String.IsNullOrEmpty(temp))
                    {
                        //расширяем массив
                        Array.Resize<string>(ref passable, passable.Length + 1);

                        //заносим объект в массив
                        passable[i] = temp;

                        //если ширина массива проходимости не соответствует ширине поля, значит ошибка
                        if (passable[i].Length != Size.Width)
                            throw new Exception("Ошибка в файле данных проходимости по ширине поля");
                        i++;
                    }
                }

                //если высота массива проходимости не соответствует высоте поля, значит ошибка
                if (passable.Length != Size.Height)
                    throw new Exception("Ошибка в файле данных проходимости по высоте поля");
            }

            //установка тумана войны
            Fog = new bool[Size.Width, Size.Height];
            for (int i = 0; i < Size.Width; i++) //пробегаемся по Х
                for (int l = 0; l < Size.Height; l++) //пробегаемся по Y
                    Fog[i, l] = true; //по умолчанию вся локация закрыта туманом

            //инициализация блоков и занесение в них местности (фоновых спрайтов)
            for (int i = 0; i < Size.Width; i++) //пробегаемся по Х
            {
                for (int l = 0; l < Size.Height; l++) //пробегаемся по Y
                {
                    //информация о файле спрайта
                    FileInfo fi = new FileInfo(Environment.CurrentDirectory + "\\Resources\\Sprites\\" + map[l][i] + ".png");

                    //если файл не найден, значит подставляем нулевой спрайт
                    if (!fi.Exists) fi = new FileInfo(Environment.CurrentDirectory + "\\Resources\\Sprites\\null.png");

                    //заносим в массив игрового поля информацию о блоках
                    Field[i, l] = new Block();
                    Field[i, l].Background = new Bitmap(fi.FullName); //сам спрайт
                    Field[i, l].BackID = fi.Name;                     //адрес к спрайту (для последующего сохранения в файл)
                }
            }

            //занесение в блоки объектов
            Objects = new GameObject[0];
            for (int i = 0; i < objects.Length; i++)
            {
                //читаем из массива данные об объекте
                //поля разделены пробелами
                string[] obj = objects[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                //минимальная длина массива
                //координаты - идентификатор - количество
                int len = 3;
                //если длина меньше минимальной, кидаем ошибку
                if (obj.Length < len)
                    throw new Exception("Ошибка в файле данных объектов ("+Location+") - неполный объект на строке "+(i+1));

                //если длина массива больше, чем минимально нужно, значит все следующее объединяем в одну ячейку
                if (obj.Length > len)
                {
                    for (int l = len + 1; l < obj.Length; l++)
                        obj[len] += " " + obj[l];
                    Array.Resize<string>(ref obj, len + 1);
                }

                //если у объекта указаны дополнительные данные
                if (obj.Length != len)
                {
                    //дополнительные данные разделяются вертикальной чертой, 
                    //потому что в описании объекта может быть много пробелов

                    //вычленяем тип, имя и описание, если есть
                    int sep = obj[len].IndexOf('|');
                    if (sep != -1)
                    {
                        string[] temp = obj[len].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        Array.Resize<string>(ref obj, len + temp.Length);
                        for (int l = 0; l < temp.Length; l++) obj[len + l] = temp[l];
                    }
                }

                //читаем координаты
                string[] coords = obj[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
#pragma warning disable IDE0059 // Ненужное присваивание значения
                int x = 0;
                int y = 0;
#pragma warning restore IDE0059 // Ненужное присваивание значения
                try
                {
                    x = Convert.ToInt32(coords[0]);
                    y = Convert.ToInt32(coords[1]);
                }
                catch
                {
                    throw new Exception("Ошибка в файле данных объектов (" + Location + ") - нечитаемые координаты на строке " + (i + 1));
                }

                if (obj.Length >= len)
                {
                    //инициализируем объект по ID
                    Field[x, y].Object = new GameObject(obj[1]);
                    //положение объекта
                    Field[x, y].Object.Location = new InGameLocation(new Point(x, y), BlockSize);
                }

                //заносим кол-во объектов
                try
                {
                    Field[x, y].Object.Count = Convert.ToInt32(obj[2]);
                    //если указано 0 объектов
                    if (Field[x, y].Object.Count < 1)
                    {
                        //обнуляем объект
                        Field[x, y].Object = null;
                        //переходим к следующему
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
                    if (obj[3] != "item") //если не указан тип объекта item
                    {
                        //запрещаем поднимать объект
                        Field[x, y].Object.PickUpAble = false;
                        //записываем имя объекта
                        Field[x, y].Object.Name = obj[3];
                        //если есть, записываем описание объекта
                        if (obj.Length == 5)
                            Field[x, y].Object.Description = obj[4];
                    }
                    else //если тип объекта item
                    {
                        //разрешаем поднимать объект
                        Field[x, y].Object.PickUpAble = true;
                        //если есть, записываем имя объекта
                        if (obj.Length >= 5)
                            Field[x, y].Object.Name = obj[4];
                        //если есть, записываем описание объекта
                        if (obj.Length == 6)
                            Field[x, y].Object.Description = obj[5];
                    }
                }

                //добавляем в главный массив объектов текущий прочитанный объект
                GameObject[] objtemp = new GameObject[Objects.Length + 1];
                for (int l = 0; l < Objects.Length; l++)
                    objtemp[l] = Objects[l];
                objtemp[objtemp.Length - 1] = Field[x, y].Object;
                Objects = objtemp;
            }

            //занесение в блоки проходимости;
            for (int i = 0; i < passable.Length; i++) //пробегаемся по Х
            {
                for (int l = 0; l < passable[i].Length; l++) //пробегаемся по Y
                {
                    if (passable[i][l] == 'H') //если Н, то блок непроходим
                        Field[l, i].Passable = false;
                    else //если другой символ (обычно это пробел), то проходим
                        Field[l, i].Passable = true;
                }
            }
        }

        public static Graphics Graphics;                  //объект графики
        public static Point Zero = new Point(0, 0);      //позиция верхнего левого угла
        public static Size Size = new Size(30, 20);      //размер локации (в блоках!)
        public static Size BlockSize = new Size(30, 30); //размер блоков (в пикселях)
        public static Block[,] Field;                     //массив блоков, координатная плоскость
        public static string Location;                    //имя рисуемой локации (используется как имя файла локации в Resources/Locations!)
        public bool IsFogged { get; }                           //включен ли на локации туман войны
        public static bool[,] Fog;                        //туман войны; true - открытый блок, false - неоткрытый
        public static Player Player;                      //игрок на данном поле
        private GameObject[] _objects;
        public GameObject[] Objects                             //массив объектов на игровом поле
        { 
            get { return _objects; } 
            set
            {
                //если что-то изменяет массив, то вносим изменения и в объекты в массиве игрового поля
                _objects = value;
                for (int i = 0; i < _objects.Length; i++)
                    Field[_objects[i].Location.BL.X, _objects[i].Location.BL.Y].Object = _objects[i];
            } 
        }
        public static Dialog Dialog;                      //объект диалога персонажей

        private Mutex Mutex = new Mutex();

        public void Draw()
        {
            Mutex.WaitOne();
            lock (new object())
            {
                Image buffer = new Bitmap(Size.Width * BlockSize.Width, Size.Height * BlockSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics gr = Graphics.FromImage(buffer))
                {
                    gr.Clear(Color.White);
                    if (IsFogged)
                    {
                        Point startView = new Point(Player.Location.BL.X - Player.View, Player.Location.BL.Y - Player.View);
                        Point endView = new Point(Player.Location.BL.X + Player.View + 1, Player.Location.BL.Y + Player.View + 1);
                        if (startView.X < 0) startView.X = 0;
                        if (startView.Y < 0) startView.Y = 0;
                        if (endView.X >= Size.Width) endView.X = Size.Width;
                        if (endView.Y >= Size.Height) endView.Y = Size.Height;
                        for (int t = startView.X; t < endView.X; t++)
                            for (int j = startView.Y; j < endView.Y; j++)
                                Fog[t, j] = false;
                    }
                    for (int i = 0; i < Size.Width; i++)
                    {
                        for (int l = 0; l < Size.Height; l++)
                        {
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
                    if (Dialog.Current.Message != null && Dialog.Current.Font != null && Dialog.Current.Image != null && Dialog.BlockStart != null)
                        gr.DrawImage(Dialog.Current.Image, Dialog.BlockStart.PX.X, Dialog.BlockStart.PX.Y - BlockSize.Height, Dialog.Current.Image.Width, Dialog.Current.Image.Height);
                }
                Graphics.DrawImage(buffer, Zero);
                if (Dialog.Current.Message != null && Dialog.Current.Font != null && Dialog.Current.Image != null && Dialog.BlockStart != null)
                    TextRenderer.DrawText(Graphics, Dialog.Current.Message, Dialog.Current.Font, Dialog.Current.Rectangle, Color.Black, Dialog.Flags);
                buffer.Dispose();
            }
            Mutex.ReleaseMutex();
        }

        public void DrawScreen(string ScreenText, Color TextColor, Color BackColor)
        {
            using (Font font1 = new Font("Segoe Script", 24, FontStyle.Bold, GraphicsUnit.Point))
            {
                Rectangle rect1 = new Rectangle(Zero.X, Zero.Y, BlockSize.Width * Size.Width, BlockSize.Height * Size.Height);
                Graphics.Clear(BackColor);
                TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;
                TextRenderer.DrawText(Graphics, ScreenText, font1, rect1, TextColor, flags);
            }
        }

        //рисовка отдельного блока (координаты p в блоках)
        public void DrawBlock(Point p, bool IsDrawPlayer)
        {
            Image buffer = new Bitmap(BlockSize.Width, BlockSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics gr = Graphics.FromImage(buffer))
            {
                gr.DrawImage(Field[p.X, p.Y].Background, 0, 0, BlockSize.Width, BlockSize.Height);
                if (Field[p.X, p.Y].Object != null)
                    gr.DrawImage(Field[p.X, p.Y].Object.Background, 0, 0, BlockSize.Width, BlockSize.Height);
                if (IsDrawPlayer)
                {
                    if (Math.Abs(Player.Location.BL.X - p.X) <= 1 && Math.Abs(Player.Location.BL.Y - p.Y) <= 1)
                    {
                        gr.DrawImage(Player.Sprite, Player.Location.PX.X - p.X * BlockSize.Width, Player.Location.PX.Y - p.Y * BlockSize.Height, BlockSize.Width, BlockSize.Height);
                    }
                }
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
                        DrawBlock(new Point(i, l), false);
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
                        if (!IsFogged || (IsFogged && !Fog[i, l]))
                            Graphics.DrawImage(Field[i, l].Object.Background, Zero.X + i * BlockSize.Width, Zero.Y + l * BlockSize.Height, BlockSize.Width, BlockSize.Height);
                    }
                }
            }
        }

        public void Dispose()
        {
            Graphics.Dispose();
            Mutex.Dispose();
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
            BackPath = Environment.CurrentDirectory + "\\Resources\\Sprites\\" + Id + ".png";
            FileInfo fi = new FileInfo(BackPath);
            if (!fi.Exists) BackPath = Environment.CurrentDirectory + "\\Resources\\Sprites\\null.png";
            Background = new Bitmap(BackPath);
            PickUpAble = false;
        }
        public GameObject()
        {
            if (BackPath != null)
            {
                FileInfo fi = new FileInfo(BackPath);
                if (!fi.Exists) BackPath = Environment.CurrentDirectory + "\\Resources\\Sprites\\null.png";
                Background = new Bitmap(BackPath);
            }
        }

        [JsonIgnore]
        public Image Background { get; set; }   //спрайт объекта
        public string BackPath { get; set; }
        public string Name { get; set; }        //имя объекта
        public string Description { get; set; } //описание объекта
        public string Id { get; set; }          //идентификатор объекта (используется как имя файла спрайта!)
        public bool PickUpAble { get; set; }    //может ли игрок поднять предмет
        public int Count { get; set; } = 1;     //кол-во
        public InGameLocation Location { get; set; }
    }
    public class Player
    {
        public Player()
        {
            Inventory = new Inventory();
            SpritePath = Environment.CurrentDirectory + "\\Resources\\Sprites\\player.png";
            FileInfo fi = new FileInfo(SpritePath);
            if (!fi.Exists)
                SpritePath = Environment.CurrentDirectory + "\\Resources\\Sprites\\null.png";
            Sprite = new Bitmap(SpritePath);

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
        
        public InGameLocation Location { get; set; }        //позиция игрока на поле
        public int Rotation { get; set; }
        [JsonIgnore]
        public Image Sprite { get; set; }                   //спрайт персонажа
        public string SpritePath { get; set; }
        
        private Image[] MoveStages;
        [JsonIgnore]
        public GameField GameField { get; set; }            //экземпляр игрового поля, на котором находится игрок
        public int View { get; set; } = 2;                  //поле зрения
        public Inventory Inventory { get; set; }            //инвентарь
        public int MoveAnimationTime { get; set; } = 150;   //время анимации движения на 1 клетку (в миллисекундах)
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
                    gr.DrawImage(Sprite, Location.PX.X - startView.X * GameField.BlockSize.Width, Location.PX.Y - startView.Y * GameField.BlockSize.Width, GameField.BlockSize.Width, GameField.BlockSize.Height);
                }
                GameField.Graphics.DrawImage(buffer, GameField.Zero.X + startView.X * GameField.BlockSize.Width, GameField.Zero.Y + startView.Y * GameField.BlockSize.Height);
                buffer.Dispose();
            }
            else
            {
                GameField.Graphics.DrawImage(Sprite, GameField.Zero.X + Location.PX.X, GameField.Zero.Y + Location.PX.Y, GameField.BlockSize.Width, GameField.BlockSize.Height);
            }
        }

        //примитивное движение игрока
        public void MovePlayer(string direction)
        {
            Point newP = new Point(0, 0);
            GameField.DrawBlock(new Point(Location.BL.X, Location.BL.Y), false);
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
        InGameLocation newP;
        InGameLocation newPT;
        InGameLocation oldP;
        int frame = 0;
        public void SmoothlyMovePlayer(string direction)
        {
            lock (new object())
            {
                newP = new InGameLocation(new Point(0, 0), GameField.BlockSize);
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
                        double[] coords = new double[] { Location.PX.X, Location.PX.Y };
                        newPT = new InGameLocation(newP.BL, GameField.BlockSize);
                        while (frame < MoveAnimationFrames)
                        {
                            DateTime beginning = DateTime.Now;
                            if (dir == "left")
                            {
                                coords = new double[] { coords[0] - Convert.ToDouble(GameField.BlockSize.Width) / MoveAnimationFrames, coords[1] };
                                newPT.PX = new Point((int)Math.Round(coords[0], 0), (int)Math.Round(coords[1], 0));
                                if (GameField.Field[newPT.PX.X / GameField.BlockSize.Width,
                                                    newPT.PX.Y / GameField.BlockSize.Height].Passable &&
                                    newPT.PX.X > newP.PX.X)
                                {
                                    Location = new InGameLocation() 
                                    { 
                                        BL = newPT.BL, PX = newPT.PX, 
                                        scaleX = GameField.BlockSize.Width, 
                                        scaleY = GameField.BlockSize.Height 
                                    };
                                }
                                if (Location.PX.X < newP.PX.X)
                                {
                                    Location = new InGameLocation()
                                    {
                                        BL = newP.BL,
                                        PX = newP.PX,
                                        scaleX = GameField.BlockSize.Width,
                                        scaleY = GameField.BlockSize.Height
                                    };
                                }

                                if (frame % 10 == 0)
                                    MoveStage--;
                                if (MoveStage == -1) MoveStage = CurrentMoveStages - 1;
                            }
                            if (dir == "up")
                            {
                                coords = new double[] { coords[0], coords[1] - Convert.ToDouble(GameField.BlockSize.Width) / MoveAnimationFrames };
                                newPT.PX = new Point((int)Math.Round(coords[0], 0), (int)Math.Round(coords[1], 0));
                                if (GameField.Field[newPT.PX.X / GameField.BlockSize.Width,
                                                    newPT.PX.Y / GameField.BlockSize.Height].Passable &&
                                    newPT.PX.Y > newP.PX.Y)
                                {
                                    Location = new InGameLocation()
                                    {
                                        BL = newPT.BL,
                                        PX = newPT.PX,
                                        scaleX = GameField.BlockSize.Width,
                                        scaleY = GameField.BlockSize.Height
                                    };
                                }
                                if (Location.PX.Y < newP.PX.Y)
                                {
                                    Location = new InGameLocation()
                                    {
                                        BL = newP.BL,
                                        PX = newP.PX,
                                        scaleX = GameField.BlockSize.Width,
                                        scaleY = GameField.BlockSize.Height
                                    };
                                }

                                if (frame % 10 == 0)
                                    MoveStage++;
                                if (MoveStage == CurrentMoveStages) MoveStage = 0;
                            }
                            if (dir == "right")
                            {
                                coords = new double[] { coords[0] + Convert.ToDouble(GameField.BlockSize.Width) / MoveAnimationFrames, coords[1] };
                                newPT.PX = new Point((int)Math.Round(coords[0], 0), (int)Math.Round(coords[1], 0));
                                if (GameField.Field[newPT.PX.X / GameField.BlockSize.Width,
                                                    newPT.PX.Y / GameField.BlockSize.Height].Passable &&
                                    newPT.PX.X < newP.PX.X)
                                {
                                    Location = new InGameLocation()
                                    {
                                        BL = newPT.BL,
                                        PX = newPT.PX,
                                        scaleX = GameField.BlockSize.Width,
                                        scaleY = GameField.BlockSize.Height
                                    };
                                }
                                if (Location.PX.X > newP.PX.X)
                                {
                                    Location = new InGameLocation()
                                    {
                                        BL = newP.BL,
                                        PX = newP.PX,
                                        scaleX = GameField.BlockSize.Width,
                                        scaleY = GameField.BlockSize.Height
                                    };
                                }

                                if (frame % 10 == 0)
                                    MoveStage++;
                                if (MoveStage == CurrentMoveStages) MoveStage = 0;
                            }
                            if (dir == "down")
                            {
                                coords = new double[] { coords[0], coords[1] + Convert.ToDouble(GameField.BlockSize.Width) / MoveAnimationFrames };
                                newPT.PX = new Point((int)Math.Round(coords[0], 0), (int)Math.Round(coords[1], 0));
                                if (GameField.Field[newPT.PX.X / GameField.BlockSize.Width,
                                                    newPT.PX.Y / GameField.BlockSize.Height].Passable &&
                                    newPT.PX.Y < newP.PX.Y)
                                {
                                    Location = new InGameLocation()
                                    {
                                        BL = newPT.BL,
                                        PX = newPT.PX,
                                        scaleX = GameField.BlockSize.Width,
                                        scaleY = GameField.BlockSize.Height
                                    };
                                }
                                if (Location.PX.Y > newP.PX.Y)
                                {
                                    Location = new InGameLocation()
                                    {
                                        BL = newP.BL,
                                        PX = newP.PX,
                                        scaleX = GameField.BlockSize.Width,
                                        scaleY = GameField.BlockSize.Height
                                    };
                                }

                                if (frame % 10 == 0)
                                    MoveStage--;
                                if (MoveStage == -1) MoveStage = CurrentMoveStages - 1;
                            }
                            if (GameField.Location != "City")
                                Sprite = MoveStages[MoveStage];
                            if (!GameField.IsFogged)
                            {
                                GameField.DrawBlock(oldP.BL, true);
                                GameField.DrawBlock(newP.BL, true);
                            }
                            DrawPlayer();
                            DateTime ending = DateTime.Now;
                            if (MoveAnimationTime / MoveAnimationFrames - (ending - beginning).TotalMilliseconds > 0)
                                Thread.Sleep((int)Math.Round(MoveAnimationTime / MoveAnimationFrames - (ending - beginning).TotalMilliseconds, 0));
                            frame++;
                        }
                        Location = new InGameLocation(newP.BL, GameField.BlockSize);
                        if (!GameField.IsFogged)
                        {
                            GameField.DrawBlock(oldP.BL, true);
                            GameField.DrawBlock(newP.BL, true);
                        }
                        MoveStage = 0;
                        if (GameField.Location != "City")
                            Sprite = MoveStages[MoveStage];
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
                        Inventory.Money += GameField.Field[X, Y].Object.Count;
                    else
                        Inventory.AddItem(GameField.Field[X, Y].Object);
                    try { onPickUpItem(GameField.Field[X, Y].Object); } catch { }
                    GameField.Field[X, Y].Object = null;
                }
            }
        }
    }

    public class InGameLocation
    {
        public InGameLocation(Point location, Size BlockSize)
        {
            BL = location;
            PX = new Point(location.X * BlockSize.Width, location.Y * BlockSize.Height);
            scaleX = BlockSize.Width;
            scaleY = BlockSize.Height;
            onChangeLocation += InGameLocation_onChangeLocation;
        }
        public InGameLocation()
        {
            BL = new Point(0, 0);
            PX = new Point(0, 0);
            scaleX = 30;
            scaleY = 30;
        }

        private bool isHandler = false;

        public int scaleX { get; set; } = 1;
        public int scaleY { get; set; } = 1;
        private void InGameLocation_onChangeLocation(string sender)
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

        //удалить предмет из инвентаря с концами
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
        //удалить только определенное кол-во предметов заданного типа
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

        //проверяет наличие предмета в инвентаре
        public bool FindItemB(string item)
        {
            for (int i = 0; i < Items.Length; i++)
                if (Items[i].Name == item || Items[i].Id == item)
                    return true;
            return false;
        }
        //возвращает кол-во предметов заданного типа в инвентаре
        public int FindItemI(string item)
        {
            for (int i = 0; i < Items.Length; i++)
                if (Items[i].Name == item || Items[i].Id == item)
                    return Items[i].Count;
            return -1;
        }
        //возвращает найденный предмет целиком
        public GameObject FindItemG(string item)
        {
            for (int i = 0; i < Items.Length; i++)
                if (Items[i].Name == item || Items[i].Id == item)
                    return Items[i];
            return null;
        }
    }

    public class Dialog
    {
        public Dialog(Game game)
        {
            Game = game;
        }

        public static class Current
        {
            public static string Message;
            public static Image Image;
            public static Font Font;
            public static Rectangle Rectangle;
        }
        public TextFormatFlags Flags { get; set; } = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;

        public Size SpeakerSize { get; set; } = new Size(2, 2); //размер говорящего в диалоге в блоках

        public delegate void EventHandler();
        public event EventHandler onPhraseEnd;
        public event EventHandler onPhraseClear;

        public static InGameLocation BlockStart;

        private Game Game;

        public void Draw(string Phrase, string SpeakerName, string SpeakerImg, string Type)
        {
            Image Speaker = new Bitmap(SpeakerImg);
            Current.Message = Phrase;
            Current.Font = new Font("Segoe Script", 8, FontStyle.Bold, GraphicsUnit.Point);
            if (Type == "phrase")
            {
                if (SpeakerSize.Height > 3)
                    Current.Image = new Bitmap(GameField.BlockSize.Width * (8 + SpeakerSize.Width), GameField.BlockSize.Height * SpeakerSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                else
                    Current.Image = new Bitmap(GameField.BlockSize.Width * (8 + SpeakerSize.Width), GameField.BlockSize.Height * (3 + 1), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }
            else
            {
                if (SpeakerSize.Height > 3)
                    Current.Image = new Bitmap(GameField.BlockSize.Width * (7 + SpeakerSize.Width), GameField.BlockSize.Height * SpeakerSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                else
                    Current.Image = new Bitmap(GameField.BlockSize.Width * (7 + SpeakerSize.Width), GameField.BlockSize.Height * (3 + 1), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }
            using (Graphics gr = Graphics.FromImage(Current.Image))
            {
                Current.Rectangle = new Rectangle(0, 0, 0, 0);
                BlockStart = new InGameLocation(new Point(0, 0), GameField.BlockSize);
                BlockStart.PX = new Point(0, (GameField.Size.Height - SpeakerSize.Height + 2 - 3) * GameField.BlockSize.Height);
                if (Type == "phrase")
                {
                    BlockStart.PX = new Point((GameField.Size.Width - SpeakerSize.Width - 8) * GameField.BlockSize.Width, BlockStart.PX.Y);
                    gr.DrawImage(new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\phrase.png"), 0, 0, GameField.BlockSize.Width * 8, GameField.BlockSize.Height * 3);
                    Current.Rectangle = new Rectangle(BlockStart.PX.X, BlockStart.PX.Y, GameField.BlockSize.Width * 8, GameField.BlockSize.Height * 2);
                }
                else if (Type == "thoughts")
                {
                    BlockStart.PX = new Point((GameField.Size.Width - SpeakerSize.Width - 7) * GameField.BlockSize.Width, BlockStart.PX.Y);
                    gr.DrawImage(new Bitmap(Environment.CurrentDirectory + "\\Resources\\Sprites\\thoughts.png"), 0, 0, GameField.BlockSize.Width * 7, GameField.BlockSize.Height * 3);
                    Current.Rectangle = new Rectangle(BlockStart.PX.X, BlockStart.PX.Y, GameField.BlockSize.Width * 7, GameField.BlockSize.Height * 2);
                }
                else
                {
                    return;
                }

                gr.DrawImage(Speaker,
                    (GameField.Size.Width - SpeakerSize.Width - BlockStart.BL.X) * GameField.BlockSize.Width,
                    (GameField.Size.Height - SpeakerSize.Height + 1 - BlockStart.BL.Y) * GameField.BlockSize.Height,
                    GameField.BlockSize.Width * SpeakerSize.Width, GameField.BlockSize.Height * SpeakerSize.Height);
                gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                TextRenderer.DrawText(GameField.Graphics, Phrase, Current.Font, Current.Rectangle, Color.Black, Flags);

                Journal.NoteAdd(SpeakerName, Phrase);

                try { onPhraseEnd(); } catch { }

                Game.Invoke(Game.DrawDelegate);
            }
        }

        public void Clear()
        {
            Current.Font = null;
            Current.Image = null;
            Current.Message = null;
            Game.Invoke(Game.DrawDelegate);
            try { onPhraseClear(); } catch { }
        }
    }
}
