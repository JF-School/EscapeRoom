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
    public class Cell
    {
        private Texture2D _crossTexture, _normalTexture;
        private Rectangle _rect;
        private Color _color;
        CellState _cellState;

        public Cell(Texture2D normalTexture, Texture2D crossTexture, Rectangle rectangle, Color color)
        {
            _normalTexture = normalTexture;
            _crossTexture = crossTexture;
            _rect = rectangle;
            _color = color;
            _cellState = CellState.Empty;
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
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                if (_rect.Contains(mouseState.Position))
                {
                    FillToggle();
                    return true;
                }
            }
            if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
            {
                if (_rect.Contains(mouseState.Position))
                {
                    CrossToggle();
                    return true;
                }
            }
            return false;
        }
        public void FillToggle()
        {
            if (_cellState == CellState.Empty)
                _cellState = CellState.Filled;
            else if (_cellState == CellState.Filled || _cellState == CellState.Crossed)
                _cellState = CellState.Empty;
        }

        public void CrossToggle()
        {
            if (_cellState == CellState.Empty)
                _cellState = CellState.Crossed;
            else if (_cellState == CellState.Filled || _cellState == CellState.Crossed)
                _cellState = CellState.Empty;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (_cellState)
            {
                case CellState.Empty:
                    spriteBatch.Draw(_normalTexture, _rect, _color);
                    break;
                case CellState.Filled:
                    spriteBatch.Draw(_normalTexture, _rect, Color.Black);
                    break;
                case CellState.Crossed:
                    spriteBatch.Draw(_normalTexture, _rect, _color);
                    spriteBatch.Draw(_crossTexture, _rect, Color.White);
                    break;
            }
        }


    }
}
