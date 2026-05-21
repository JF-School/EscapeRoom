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
    public class CellGrid
    {
        private Cell[,] _cellBoard;
        private Texture2D _crossTexture;
        private Texture2D _defaultTexture;
        private Color _color;
        private Point _location;

        public CellGrid(Texture2D crossTexture, Texture2D defaultTexture, Point location, Color color)
        {
            _crossTexture = crossTexture;
            _defaultTexture = defaultTexture;
            _location = location;
            _color = color;
            for (int y = 0; y < 5; y++)
                for (int x = 0; x < 5; x++)
                {
                    _cellBoard[x, y] = new Cell(_crossTexture, _defaultTexture, new Rectangle(x * 60 + _location.X, y * 60 + _location.Y, 50, 50), _color);
                }    
        }



    }
}
