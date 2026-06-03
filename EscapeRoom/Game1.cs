using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.CompilerServices;

namespace EscapeRoom
{
    enum Screen
    {
        Intro,
        ClassicPuzzles,
        CipherPuzzles,
        FunPuzzles,
        Outro
    }

    public enum CellState
    {
        Empty,
        Filled,
        Crossed
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Rectangle window; // window     

        Screen screen;

        Texture2D rectTexture, xTexture;
        Texture2D phTexture; // placeholder texture, remove after textures are finalized.
        Texture2D lightsPhTexture, nonogramPhTexture, fifteenPhTexture; // placeholder texture
        Rectangle lightsBtn, nonogramBtn, fifteenBtn;

        LightGrid lightGrid;
        CellGrid cellGrid;
        MouseState mouseState, prevMouseState;
        KeyboardState keyboardState, prevKeyboardState;

        int[,] solution1, solution2, solution3;
        int randomSolution;


        Random generator;
        int puzzle; // puzzles

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            window = new Rectangle(0, 0, 800, 500);
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.ApplyChanges();

            screen = Screen.ClassicPuzzles;
            puzzle = 0; // zero puzzle = original window
            generator = new Random();

            lightsBtn = new Rectangle(450, 0, 100, 100);
            nonogramBtn = new Rectangle(25, 215, 100, 100);
            fifteenBtn = new Rectangle(610, 240, 100, 100);

            // TODO: Add your initialization logic here

            base.Initialize();

            lightGrid = new LightGrid(rectTexture, new Point(230, 75), Color.Gold, generator.Next(1, 4));
            cellGrid = new CellGrid(rectTexture, xTexture, new Point(230, 75), Color.White);
            solution1 = new int[10, 10];
            solution2 = new int[10, 10];
            solution3 = new int[10, 10];
            NonogramSolution2();
            cellGrid.Solution = solution2;
            cellGrid.DebugSolution();
            //randomSolution = generator.Next(1, 4);
            //switch (randomSolution)
            //{
            //    case 1:
            //        NonogramSolution1();
            //        cellGrid.Solution = solution1;
            //        break;
            //    case 2:
            //        NonogramSolution2();
            //        cellGrid.Solution = solution2;
            //        break;
            //    case 3:
            //        NonogramSolution3();
            //        cellGrid.Solution = solution3;
            //        break;
            //}

        }

        

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            rectTexture = Content.Load<Texture2D>("Images/rectangle");
            xTexture = Content.Load<Texture2D>("Images/Red_X");
            phTexture = Content.Load<Texture2D>("Placeholders/escaperoomplaceholder");
            lightsPhTexture = Content.Load<Texture2D>("Placeholders/lightsoutbutton");
            nonogramPhTexture = Content.Load<Texture2D>("Placeholders/nonogrambutton");
            fifteenPhTexture = Content.Load<Texture2D>("Placeholders/fifteenslidingpuzzle");
        }

        protected override void Update(GameTime gameTime)
        {
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (screen) 
            {
                case Screen.Intro:
                    break;
                case Screen.ClassicPuzzles:
                    switch (puzzle)
                    {
                        case 0: // normal screen
                            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                            {
                                if (lightsBtn.Contains(mouseState.Position))
                                    puzzle = 1;
                                if (nonogramBtn.Contains(mouseState.Position))
                                    puzzle = 2;
                                if (fifteenBtn.Contains(mouseState.Position))
                                    puzzle = 3;
                            }
                            break;
                        case 1: // lights out
                            lightGrid.Update(mouseState, prevMouseState);
                            break;
                        case 2: // nonogram
                            if (keyboardState.IsKeyDown(Keys.LeftAlt) && prevKeyboardState.IsKeyUp(Keys.LeftAlt))
                                cellGrid.DebugState();
                            if (!cellGrid.Update(mouseState, prevMouseState))
                                Exit();
                            
                            break;
                        case 3: // 15 sliding puzzle
                            break;
                    }
                    break;
                case Screen.CipherPuzzles:
                    switch (puzzle)
                    {
                        case 0: // normal screen
                            break;
                        case 1: // caesar cipher
                            break;
                        case 2: // morse code
                            break;
                        case 3: // idk
                            break;
                    }
                    break;
                case Screen.FunPuzzles:
                    switch (puzzle)
                    {
                        case 0: // normal screen
                            break;
                        case 1: // linglox
                            break;
                        case 2: // the square hole
                            break;
                        case 3: // calculator
                            break;
                    }
                    break;
                case Screen.Outro:
                    break;
            }

            // TODO: Add your update logic here

            base.Update(gameTime);            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            switch (screen)
            {
                case Screen.Intro:
                    break;
                case Screen.ClassicPuzzles:
                    switch (puzzle)
                    {
                        case 0: // normal screen
                            _spriteBatch.Draw(phTexture, window, Color.White);
                            _spriteBatch.Draw(lightsPhTexture, lightsBtn, Color.White);
                            _spriteBatch.Draw(nonogramPhTexture, nonogramBtn, Color.White);
                            _spriteBatch.Draw(fifteenPhTexture, fifteenBtn, Color.White);
                            break;
                        case 1: // lights out
                            _spriteBatch.Draw(rectTexture, window, Color.Black);
                            lightGrid.Draw(_spriteBatch);
                            break;
                        case 2: // nonogram
                            _spriteBatch.Draw(rectTexture, window, Color.Gray);
                            cellGrid.Draw(_spriteBatch);
                            break;
                        case 3: // 15 sliding puzzle
                            break;
                    }
                    break;
                case Screen.CipherPuzzles:
                    switch (puzzle)
                    {
                        case 0: // normal screen
                            _spriteBatch.Draw(phTexture, window, Color.White);
                            break;
                        case 1: // caesar cipher
                            break;
                        case 2: // morse code
                            break;
                        case 3: // idk
                            break;
                    }
                    break;
                case Screen.FunPuzzles:
                    switch (puzzle)
                    {
                        case 0: // normal screen
                            _spriteBatch.Draw(phTexture, window, Color.White);
                            break;
                        case 1: // linglox
                            break;
                        case 2: // the square hole
                            break;
                        case 3: // calculator
                            break;
                    }
                    break;
                case Screen.Outro:
                    break;
            }

            _spriteBatch.End();


            base.Draw(gameTime);
        }

        public void NonogramSolution1()
        {
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                {
                    solution1[x, y] = 0;
                }
            // [row, column] (STARTS AT 0)
            //solution1[0, 0] = 1;
            //solution1[0, 1] = 1;
            //solution1[0, 2] = 1;
            //solution1[0, 3] = 1;
            //solution1[0, 5] = 1;
            //solution1[0, 6] = 1;
            //solution1[0, 7] = 1;
            //solution1[0, 9] = 1;
            //solution1[1, 0] = 1;
            //solution1[1, 1] = 1;
            //solution1[1, 2] = 1;
            //solution1[1, 3] = 1;
            //solution1[1, 4] = 1;
            //solution1[1, 5] = 1;
            //solution1[1, 6] = 1;
            //solution1[1, 7] = 1;
            //solution1[1, 9] = 1;
            //solution1[2, 0] = 1;
            //solution1[2, 1] = 1;
            //solution1[2, 2] = 1;
            //solution1[2, 5] = 1;
            //solution1[2, 6] = 1;
            //solution1[2, 7] = 1;
            //solution1[2, 9] = 1;
            //solution1[3, 0] = 1;
            //solution1[3, 1] = 1;
            //solution1[3, 9] = 1;
            //solution1[4, 0] = 1;
            //solution1[4, 9] = 1;
            //solution1[5, 4] = 1;
            //solution1[5, 6] = 1;
            //solution1[5, 8] = 1;
            //solution1[5, 9] = 1;
            //solution1[6, 2] = 1;
            //solution1[6, 4] = 1;
            //solution1[6, 6] = 1;
            //solution1[6, 7] = 1;
            //solution1[6, 8] = 1;
            //solution1[6, 9] = 1;
            //solution1[7, 6] = 1;
            //solution1[7, 7] = 1;
            //solution1[7, 8] = 1;
            //solution1[7, 9] = 1;
            //solution1[8, 6] = 1;
            //solution1[8, 7] = 1;
            //solution1[8, 8] = 1;
            //solution1[8, 9] = 1;
            //solution1[9, 5] = 1;
            //solution1[9, 6] = 1;
            //solution1[9, 7] = 1;
        }

        public void NonogramSolution2()
        {
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                {
                    solution2[x, y] = 0;
                }
            // [row, column] (STARTS AT 0)
            solution2[0, 0] = 1;
            solution2[0, 9] = 1;
            solution2[1, 1] = 1;
            solution2[1, 8] = 1;
            solution2[2, 2] = 1;
            solution2[2, 7] = 1;
            solution2[3, 3] = 1;
            solution2[3, 6] = 1;
            solution2[4, 4] = 1;
            solution2[4, 5] = 1;
            solution2[5, 4] = 1;
            solution2[5, 5] = 1;
            solution2[6, 3] = 1;
            solution2[6, 6] = 1;
            solution2[7, 2] = 1;
            solution2[7, 7] = 1;
            solution2[8, 1] = 1;
            solution2[8, 8] = 1;
            solution2[9, 0] = 1;
            solution2[9, 9] = 1;
        }

        public void NonogramSolution3()
        {

        }

    }
}
