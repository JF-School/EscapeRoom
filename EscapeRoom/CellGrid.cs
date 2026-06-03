using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    public class CellGrid
    {
        private Cell[,] _cellBoard;

        private int[,] _solutionBoard;
        private Texture2D _crossTexture;
        private Texture2D _defaultTexture;
        private Color _color;
        private Point _location;
        private string _hHint;
        private string _vHint;

        //Cell[,] solution, 
        public CellGrid(Texture2D crossTexture, Texture2D defaultTexture, Point location, Color color)
        {
            _crossTexture = crossTexture;
            _defaultTexture = defaultTexture;
            _location = location;
            _color = color;
            _cellBoard = new Cell[10, 10];
            _solutionBoard = new int[10, 10];
        

            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                {
                    _cellBoard[x, y] = new Cell(_crossTexture, _defaultTexture, new Rectangle(x * 30 + _location.X, y * 30 + _location.Y, 25, 25), _color);
                }
        }

        public void DebugSolution()
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Debug.Write(_solutionBoard[x, y]);
                }
                Debug.WriteLine(" ");
            }
        }

        public void DebugState()
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Debug.Write(_cellBoard[x, y].State);
                }
                Debug.WriteLine(" ");
            }
            Debug.WriteLine(" ");
            Debug.Write(CheckWin().ToString());
            Debug.WriteLine(" ");
            Debug.WriteLine(" ");
            DebugSolution();
        }


        public bool Update(MouseState mouseState, MouseState prevMouseState)
        {
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                {
                    _cellBoard[x, y].Update(mouseState, prevMouseState);
                }


            return CheckWin();

                // Check for a win
                
        }

        public int[,] Solution
        {
            get { return _solutionBoard; }
            set { _solutionBoard = value; }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                    _cellBoard[x, y].Draw(spriteBatch);
        }

        public bool CheckWin()
        {
            bool win = true;
            for (int y = 0; y < 10; y++)
            {

            
                for (int x = 0; x < 10; x++)
                {
                    //Debug.Write(_cellBoard[x, y].State + _solutionBoard[x, y]);
                    // _cellBoard[x, y].State == CellState.Filled && _solutionBoard[x, y] == 1

                    if (_cellBoard[x, y].State == CellState.Filled && _solutionBoard[x, y] == 1)
                    {

                        win = false;
                        //Debug.WriteLine($"Loss detected at ({x},{y}). Board =  {_cellBoard[x, y].State}, Solution = {_solutionBoard[x, y]} ");
                    }


                }

            }

            return win;

        }

    }
}
