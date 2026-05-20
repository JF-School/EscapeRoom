using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    public class Light
    {
        private Texture2D _texture;
        private Rectangle _rect;
        private Color _color;
        private bool _enabled;

        public Light(Texture2D texture, Rectangle rectangle, Color color)
        {
            _texture = texture;
            _rect = rectangle;
            _color = color;
            _enabled = false;
        }

        public Rectangle Rect
        {
            get { return _rect; }
            set { _rect = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public bool Update(MouseState mouseState, MouseState prevMouseState)
        {
            if(mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                if (_rect.Contains(mouseState.Position))
                {
                    Toggle();
                    return true;
                }
            }
            return false;
        }
        public void Toggle()
        {
            _enabled = !_enabled;
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_enabled)
                spriteBatch.Draw(_texture, _rect, _color);
            else
                spriteBatch.Draw(_texture, _rect, Color.LightGray);
        }
    }
}
