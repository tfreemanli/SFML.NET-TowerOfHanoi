using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
//using MiniProj;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TestMiniProj_1
{
    internal class Program
    {
        RenderWindow window;
        List<Drawable> drawables;
        Tower placeholder;
        Pointer pointr;
        Sprite bg;
        Text title;
        Text control;
        Tower Tower1;
        Tower Tower2;
        Tower Tower3;
        Tower[] towers;
        bool isWait;
        public bool isRestart { get; set; } = false;
        static void Main(string[] args)
        {
            Program program;
            do
            {
                program = new Program();
                program.Run();
            } while (program.isRestart);
        }

        public Program()
        {
            // Initialize the window and other components here
            window = new RenderWindow(new VideoMode(800, 600), Config.WIN_TITLE+"  By "+Config.STUDENT_INFO);
            window.Closed += (sender, e) => { window.Close(); };
            drawables = new List<Drawable>();

            bg = new Sprite();
            bg.Texture = new Texture(Config.IMG_PATH + "background.png");
            drawables.Add(bg);

            title = new Text("Tower of Hanoi - I'm Stack", Config.DEAFULT_FONT, 50);
            title.Position = new Vector2f(100, 25);
            title.FillColor = Color.Magenta;
            title.OutlineThickness = 2;
            title.OutlineColor = new Color(0,0,0,64);
            drawables.Add(title);

            control = new Text(
                "To Play, use your mouse, or use ←  → and 'Space' keys.\nBig disks always under small ones. To Win, all disks to the RIGHT.\nYou can change Disks number by editing Config.cs :)", 
                Config.DEAFULT_FONT, 20);
            control.Position = new Vector2f(100, 85);
            control.FillColor = Color.White;
            drawables.Add(control);

            placeholder = new Tower(new Vector2f(400, 200)); // Placeholder for the disk
            drawables.Add(placeholder);

            Tower1 = new Tower(new Vector2f(145, 505));
            for(int i = 1; i <= Config.DISK_NUM; i++)
            {
                //create a disk and add to Tower
                int j = i;
                j = (j - 1) % 3 + 1;
                Texture texture = new Texture(Config.IMG_PATH + "disk_" + j + ".png");
                Disk disk = new Disk(texture, "" + i, 1f - (i - 1f) / 10f) ;
                disk.ID = (uint)i;

                Tower1.AddDisk(disk);
            }
            Tower1.OnClick += () => doClick(Tower1);
            
            Tower2 = new Tower(new Vector2f(400, 505));
            Tower2.OnClick += () => doClick(Tower2);
            Tower3 = new Tower(new Vector2f(655, 505));
            Tower3.OnClick += () => doClick(Tower3);
            towers = new Tower[3];
            towers[0] = Tower1;
            towers[1] = Tower2;
            towers[2] = Tower3;

            drawables.Add(Tower1);
            drawables.Add(Tower2);
            drawables.Add(Tower3);

            // Create the pointer
            pointr = new Pointer(towers, placeholder);
            pointr.Scale = new Vector2f(0.5f, 0.5f);
            drawables.Add(pointr);


            window.KeyPressed += (s, e) => { pointr.HandleKeyInput(e); };

            //myButton btn = new myButton("Click Me") { 
            //    Position = new Vector2f(400,300),
            //    AllowSpaceKeyTrigger = true
            //};
            //btn.OnClick += () => { Console.WriteLine("Button clicked!"); };
            //btn.OnMouseEnter += () => { Console.WriteLine("Mouse entered!"); };
            //btn.OnMouseLeave += () => { Console.WriteLine("Mouse left!"); };
        }

        public void Run()
        {

            while (window.IsOpen)
            {
                window.DispatchEvents();
                Tower1.HandleMouseInput(window);
                Tower2.HandleMouseInput(window);
                Tower3.HandleMouseInput(window);
                window.Clear();
                DrawAll(window, drawables);
                window.Display();
                if (isWin())
                {
                    break;
                }
            }

        }

        public void DrawAll(RenderWindow window, List<Drawable> drawables)
        {
            foreach (var drawable in drawables)
            {
                window.Draw(drawable);
            }
        }

        public void doClick(Tower target)
        {
            if (placeholder != null && placeholder.Count > 0)
            {
                int count = target.Count;
                if (count > 0) //exsisting disk, then check if is bigger than exsisting disk
                {
                    Disk topDisk = target.PeekDisk();
                    Disk tmp = placeholder.PeekDisk();
                    if (tmp.ID > topDisk.ID)//Smaller ID means Bigger disk
                    {
                        //Console.WriteLine("Invalid move: Cannot place a larger disk on top of a smaller disk.");
                        target.AddDisk(placeholder.PopDisk());
                    }
                    //else do not add.
                }
                else
                {
                    target.AddDisk(placeholder.PopDisk());
                }
            }
            else
            {
                placeholder.AddDisk(target.PopDisk()); //pull up to the air
            }
        }

        public bool isWin()
        {
            if (Tower3.Count == Config.DISK_NUM)
            {
                int i = Config.DISK_NUM;
                foreach(var disk in Tower3._disks)
                {
                    if (disk.ID != (uint)i) return false;
                    i--;
                }

                //Horay!! You win!!
                isWait = true;
                //drawables.Clear();
                drawables.Add(new Text("           You Win!!\nESC's easy, ENTER 2b a Hero!", Config.DEAFULT_FONT, 50) { 
                    Position = new Vector2f(70, 220), 
                    FillColor = new Color(137,93,192,220), 
                    Rotation = -10f,
                    OutlineThickness=5,
                    OutlineColor = Color.White
                });
                window.Closed += (s, e) => { window.Close(); };
                window.KeyPressed += (s, e) => { HandleKeyInput(e); };
                window.Clear();
                DrawAll(window, drawables);
                window.Display();

                while (isWait && window.IsOpen)
                {
                    window.DispatchEvents();
                }
                return true;
                //window.Close();
            }
            return false;
        }

        public void HandleKeyInput(KeyEventArgs key)
        {
            switch (key.Code)
            {
                case Keyboard.Key.Escape:

                    Config.DISK_NUM--;
                    isRestart = false;
                    isWait = false;
                    window.Close();
                    break;
                case Keyboard.Key.Enter:
                    Config.DISK_NUM++;
                    isRestart = true ;
                    window.Close();
                    break;
                default:
                    break;
            }
        }
    }
}
