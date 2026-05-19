using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    public class LightGrid
    {
        private Light[,] _lightBoard;
        private Texture2D _texture;
        private Color _onColor;
        private Point _location;
        

        public LightGrid(Texture2D texture, Point location, Color color)
        {
            _location = location;
            _onColor = color;
            _texture = texture;
            _lightBoard = new Light[5, 5];
            for(int y = 0; y < 5; y++)
                for (int x = 0; x < 5; x++)
                {
                    _lightBoard[x, y] = new Light(_texture, new Rectangle(x * 60 + _location.X, y * 60 + _location.Y, 50, 50), _onColor);
                }

        }

        public void Update(MouseState mouseState, MouseState prevMouseState)
        {
            // Turn Light On and Off
            for (int y = 0; y < 5; y++)
                for (int x = 0; x < 5; x++)
                {
                    if (_lightBoard[x, y].Update(mouseState, prevMouseState))
                    {
                        // turn on/off adjacent lights
                        if (x - 1 >= 0)
                            _lightBoard[x - 1, y].Toggle();
                        if (x + 1 < 5)
                            _lightBoard[x + 1, y].Toggle();
                        if (y - 1 >= 0)
                            _lightBoard[x, y - 1].Toggle();
                        if (y + 1 < 5)
                            _lightBoard[x, y + 1].Toggle();
                    }

                }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < 5; y++)
                for (int x = 0; x < 5; x++)
                    _lightBoard[x, y].Draw(spriteBatch);
        }
    }
}
