using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static TestMiniProj_1.myButton;

namespace TestMiniProj_1
{
    internal class Tower : Transformable, Drawable
    {
        public Stack<Disk> _disks { get; }
        private RectangleShape _transparentRectangle;
        public bool AllowSpaceKeyTrigger { get; set; } = true;
        private bool _wasSpacePressed = false;


        public enum ButtonState
        {
            Normal,    // 默认状态
            Hovered,   // 鼠标悬停
            Pressed    // 鼠标按下
        }
        private ButtonState _currentState = ButtonState.Normal;

        // 事件委托
        public event Action OnClick;
        public event Action OnMouseEnter;
        public event Action OnMouseLeave;

        public Tower(Vector2f pos)
        {
            _disks = new Stack<Disk>();
            this.Position = pos;
            this.Origin = pos;

            _transparentRectangle = new RectangleShape(new Vector2f(240, 250));
            _transparentRectangle.FillColor = new Color(0, 0, 0, 0); // Transparent color
            _transparentRectangle.Origin = new Vector2f(
                _transparentRectangle.GetGlobalBounds().Left + _transparentRectangle.GetGlobalBounds().Width/2,
                _transparentRectangle.GetGlobalBounds().Top + _transparentRectangle.GetGlobalBounds().Height
                );
            _transparentRectangle.Position = pos;

            //Console.WriteLine("Tower Created, Position:" + this.Position.X +" , " + this.Position.Y);
            //Console.WriteLine("Tower Created, Origin:" + this.Origin.X + " , " + this.Origin.Y);
        }



        //Tower need to handle mouse input
        public void HandleMouseInput(RenderWindow window)
        {

            Vector2f mousePos = window.MapPixelToCoords(
                Mouse.GetPosition(window)
            );

            // 
            Vector2f localMousePos = Transform.GetInverse().TransformPoint(mousePos);

            bool contains = _transparentRectangle.GetGlobalBounds().Contains(localMousePos.X, localMousePos.Y);

            ButtonState oldState = _currentState;

            if (contains)
            {
                _currentState = Mouse.IsButtonPressed(Mouse.Button.Left)
                    ? ButtonState.Pressed
                    : ButtonState.Hovered;

                // 触发事件
                if (_currentState == ButtonState.Pressed && oldState != ButtonState.Pressed)
                    OnClick?.Invoke();

                if (oldState != ButtonState.Hovered && _currentState == ButtonState.Hovered)
                    OnMouseEnter?.Invoke();
            }
            else
            {
                _currentState = ButtonState.Normal;
                if (oldState == ButtonState.Hovered)
                    OnMouseLeave?.Invoke();
            }
        }


        // Drawable Intf，To Draw the disk
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;

            target.Draw(_transparentRectangle);

            if(_disks.Count > 0)
            {
                foreach (var disk in _disks)
                {
                    disk.Draw(target, states);
                }
            }
        }

        public bool AddDisk(Disk disk)
        {
            if (disk != null)
            {
                int count = _disks.Count;//disk position is based on the count of disks
                if(count > 0)
                {
                    Disk topDisk = _disks.Peek();
                    if (disk.ID <= topDisk.ID)//Smaller ID means Bigger disk
                    {
                        //Console.WriteLine("Invalid move: Cannot place a larger disk on top of a smaller disk.");
                        return false;
                    }
                }

                //eg. if count=0, this disk is the first one, so its position is 0
                disk.Position = new Vector2f(this.Position.X, this.Position.Y - count * disk.Size.Y); // Adjust the Y position for stacking
                //Console.WriteLine("Tower add disk, Tower Position:" + this.Position.X + " , " + this.Position.Y + " , Origin: " + this.Origin.X + " , " + this.Origin.Y);
                //Console.WriteLine("Tower add disk, disk Position:" + disk.Position.X + " , " + disk.Position.Y + " , Origin: " + disk.Origin.X + " , " + disk.Origin.Y);
                //disk.Position = new Vector2f(145 - texture.Size.X * 0.5f, -i * 50) // Adjust the Y position for stacking
                _disks.Push(disk);
                return true;
            }
            return false;
        }

        public Disk PopDisk()
        {
            if (_disks.Count > 0)
            {
                return _disks.Pop();
            }
            return null;
        }

        public Disk PeekDisk()
        {
            if (_disks.Count > 0)
            {
                return _disks.Peek();
            }
            return null;
        }

        public int Count
        {
            get { return _disks.Count; }
        }



    }
}
