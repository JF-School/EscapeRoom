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
        //private bool _leftClicked, _rightClicked;
        CellState _cellState;
        private bool _filled, _crossed;

        public Cell(Texture2D crossTexture, Texture2D normalTexture, Rectangle rect, Color color)
        {
            _crossTexture = crossTexture;
            _normalTexture = normalTexture;
            _rect = rect;
            _color = color;
            _filled = false;
            _crossed = false;
            _cellState = CellState.Empty;
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Rectangle Rect
        {
            get { return _rect; }
            set { _rect = value; }
        }

        public CellState State
        {
            get { return _cellState; }
            set { _cellState = value; }
        }

        public bool Update(MouseState mouseState, MouseState prevMouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                if (_rect.Contains(mouseState.Position))
                {
                    FillToggle();
                    return true;
                }
            if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
                if (_rect.Contains(mouseState.Position))
                {
                    CrossToggle();
                    return true;
                }
            return false;
        }

        public bool Filled
        {
            get { return _filled; }
            set { _filled = value; }
        }

        public void FillToggle()
        {
            if (_cellState == CellState.Empty)
                _cellState = CellState.Filled;
            else if (_cellState == CellState.Filled)
                _cellState = CellState.Empty;
        }

        public void CrossToggle()
        {
            if (_cellState == CellState.Empty || _cellState == CellState.Filled)
                _cellState = CellState.Crossed;
        }

        public bool Crossed
        {
            get { return _crossed; }
            set { _crossed = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (_cellState)
            {
                case CellState.Empty:
                    spriteBatch.Draw(_normalTexture, _rect, _color);
                    break;
                case CellState.Crossed:
                    spriteBatch.Draw(_normalTexture, _rect, _color);
                    spriteBatch.Draw(_crossTexture, _rect, Color.White);
                    break;
                case CellState.Filled:
                    spriteBatch.Draw(_normalTexture, _rect, Color.Black);
                    break;
            }

            //if (_leftClicked)
            //    spriteBatch.Draw(_normalTexture, _rect, Color.Black);
            //else
            //    spriteBatch.Draw(_normalTexture, _rect, Color.White);

            //if (_rightClicked)
            //    spriteBatch.Draw(_crossTexture, _rect, Color.White);
            //else
            //    spriteBatch.Draw(_normalTexture, _rect, Color.White);
        }
    }
}
