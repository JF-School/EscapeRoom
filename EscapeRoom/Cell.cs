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

        public Cell(Texture2D crossTexture, Texture2D normalTexture, Rectangle rect, Color color)
        {
            _crossTexture = crossTexture;
            _normalTexture = normalTexture;
            _rect = rect;
            _color = color;
            //_leftClicked = false;
            //_rightClicked = false;
            _cellState = CellState.Unchecked;
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public CellState State
        {
            get { return _cellState; }
            set { _cellState = value; }
        }

        public void Update(MouseState mouseState, MouseState prevMouseState)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
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
