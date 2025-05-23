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
    internal class Disk : Transformable, Drawable
    {
        private Sprite _finalSprite;
        private RenderTexture _renderTexture;
        public uint ID { get; set; } = 0;

        public Disk(Texture texture, string text, float widthscale)
        {
            // create a RenderTexture to draw the disk and text
            texture.Smooth = true; // set texture smoothing
            float width = texture.Size.X * widthscale;
            _renderTexture = new RenderTexture((uint)width, texture.Size.Y);

            // background image with scale
            var baseSprite = new Sprite(texture);
            baseSprite.Scale = new Vector2f(widthscale, 1.0f); //set scale according to Param widthscale
            _renderTexture.Draw(baseSprite);

            // txt, eg. 1,2,3
            var sfText = new Text(text, Config.DEAFULT_FONT)
            {
                //Position = textPosition,
                FillColor = Color.White
            };

            // calculate text center and position
            FloatRect textRect = sfText.GetLocalBounds();
            sfText.Origin = new Vector2f(textRect.Left + textRect.Width / 2.0f, textRect.Top + textRect.Height / 2.0f);
            sfText.Position = new Vector2f(width / 2.0f, texture.Size.Y / 2.0f);

            _renderTexture.Draw(sfText);

            // create final Sprite
            _renderTexture.Display();
            _finalSprite = new Sprite(_renderTexture.Texture);

            //Set the origin to the center bottom of the disk
            this.Origin = new Vector2f(_finalSprite.Texture.Size.X / 2.0f, _finalSprite.Texture.Size.Y); 
        }

        // Drawable
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform; //
            target.Draw(_finalSprite, states);
        }
        public Vector2u Size
        {
            get => _finalSprite.Texture.Size;
            set
            {
                //
            }
        }

        // 释放资源
        ~Disk()
        {
            _renderTexture?.Dispose();
        }

    }
}
