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
        private int _puzzle;
        

        public LightGrid(Texture2D texture, Point location, Color color, int puzzle)
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
            _puzzle = puzzle;
            switch (puzzle)
            {
                case 1:
                    SetPuzzle1();
                    break;
                case 2:
                    SetPuzzle2();
                    break;
                case 3:
                    SetPuzzle3();
                    break;
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

        public void SetPuzzle1() // #
        {
            ClearPuzzle();
            _lightBoard[0, 1].Toggle();
            _lightBoard[0, 3].Toggle();
            _lightBoard[1, 0].Toggle();
            _lightBoard[1, 1].Toggle();
            _lightBoard[1, 2].Toggle();
            _lightBoard[1, 3].Toggle();
            _lightBoard[1, 4].Toggle();
            _lightBoard[2, 1].Toggle();
            _lightBoard[2, 3].Toggle();
            _lightBoard[3, 0].Toggle();
            _lightBoard[3, 1].Toggle();
            _lightBoard[3, 2].Toggle();
            _lightBoard[3, 3].Toggle();
            _lightBoard[3, 4].Toggle();
            _lightBoard[4, 1].Toggle();
            _lightBoard[4, 3].Toggle();
        }

        public void SetPuzzle2() // A
        {
            ClearPuzzle();
            _lightBoard[0, 1].Toggle();
            _lightBoard[0, 2].Toggle();
            _lightBoard[0, 3].Toggle();
            _lightBoard[0, 4].Toggle();
            _lightBoard[1, 0].Toggle();
            _lightBoard[1, 1].Toggle();
            _lightBoard[1, 3].Toggle();
            _lightBoard[2, 0].Toggle();
            _lightBoard[2, 3].Toggle();
            _lightBoard[3, 0].Toggle();
            _lightBoard[3, 1].Toggle();
            _lightBoard[3, 3].Toggle();
            _lightBoard[4, 1].Toggle();
            _lightBoard[4, 2].Toggle();
            _lightBoard[4, 3].Toggle();
            _lightBoard[4, 4].Toggle();
        }

        public void SetPuzzle3() // IV
        {
            ClearPuzzle();
            _lightBoard[0, 0].Toggle();
            _lightBoard[0, 1].Toggle();
            _lightBoard[0, 2].Toggle();
            _lightBoard[0, 3].Toggle();
            _lightBoard[0, 4].Toggle();
            _lightBoard[2, 0].Toggle();
            _lightBoard[2, 1].Toggle();
            _lightBoard[2, 2].Toggle();
            _lightBoard[2, 3].Toggle();
            _lightBoard[3, 4].Toggle();
            _lightBoard[4, 0].Toggle();
            _lightBoard[4, 1].Toggle();
            _lightBoard[4, 2].Toggle();
            _lightBoard[4, 3].Toggle();
        }

        public void ClearPuzzle()
        {
            for (int y = 0; y < 5; y++)
                for (int x = 0; x < 5; x++)
                    _lightBoard[x, y].Enabled = false;
                    
        }

        public int Puzzle
        {
            get { return _puzzle; }
            set 
            { 
                _puzzle = value;
                if (_puzzle == 1)
                    SetPuzzle1();
            }
        }

    }
}
