using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TestMiniProj_1
{
    internal class Pointer : Sprite, Drawable
    {
        public int currentIndex { get; set; } = 0;
        public Tower[] Towers;
        public Tower placeholder;
        public Pointer(Texture texture, Tower[] towers, Tower placeholder) : base(texture)
        {
            texture.Smooth = true; // set texture smoothing
            Towers = towers;
            this.placeholder = placeholder;
            this.Origin = new Vector2f(texture.Size.X / 2.0f, texture.Size.Y / 2.0f);
            Position = new Vector2f(Towers[currentIndex].Position.X, 550);
        }
        public Pointer(Tower[] towers, Tower placeholder) : this(new Texture(Config.IMG_PATH + "pointer.png"), towers, placeholder)
        { }

        public void HandleKeyInput(KeyEventArgs key)
        {
            switch (key.Code)
            {
                case Keyboard.Key.Left:
                    if (currentIndex > 0)
                    {
                        currentIndex--;
                    }
                    this.Position = new Vector2f(Towers[currentIndex].Position.X, 550);
                    break;
                case Keyboard.Key.Right:
                    if (currentIndex < Towers.Length-1)
                    {
                        currentIndex++;
                    }
                    this.Position = new Vector2f(Towers[currentIndex].Position.X, 550);
                    break;
                case Keyboard.Key.Space:
                    // Trigger click event
                    if (placeholder != null && placeholder.Count > 0)
                    {
                        Disk tmp = placeholder.PeekDisk();
                        if(Towers[currentIndex].AddDisk(tmp)) placeholder.PopDisk();
                    }
                    else
                    {
                        placeholder.AddDisk(Towers[currentIndex].PopDisk());
                    }
                    break;
                default:
                    break;
            }
        }

        //public void Draw(RenderTarget target, RenderStates states)
        //{
        //    states.Transform *= Transform; 
        //    target.Draw(this, states);
        //}
    }
}
