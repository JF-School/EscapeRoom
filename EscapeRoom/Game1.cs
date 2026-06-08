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

        Texture2D rectTexture, xTexture, backTexture, tableTexture;
        Texture2D phTexture; // placeholder texture, remove after textures are finalized.
        Texture2D lightsPhTexture, nonogramPhTexture, fifteenPhTexture; // placeholder texture
        Rectangle lightsBtn, nonogramBtn, fifteenBtn, backBtn;
        Rectangle lightsBack;

        Texture2D sunPosterFront, todayPosterFront, alertPosterFront, bdayPosterFront; // front textures
        Texture2D sunPosterBack, todayPosterBack, alertPosterBack, bdayPosterBack; // back textures
        Texture2D wallpaperTexture; // wallpaper background 
        Rectangle sunRect, todayRect, alertRect, bdayRect, maxPosterRect;
        bool backSun, backToday, backAlert, backBday; // false = front, true = back;
        bool lights; // true = on, false = off;

        LightGrid lightGrid;
        CellGrid cellGrid;
        MouseState mouseState, prevMouseState;
        KeyboardState keyboardState, prevKeyboardState;

        int[,] solution1, solution2, solution3;
        int randomSolution;

        Random generator;
        int puzzle; // puzzles
        bool loDone, noDone;

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

            screen = Screen.CipherPuzzles;
            puzzle = 0; // zero puzzle = original window
            generator = new Random();

            // first screen
            lightsBack = new Rectangle(225, 70, 300, 300);
            backBtn = new Rectangle(10, 10, 50, 50);
            lightsBtn = new Rectangle(450, 0, 100, 100);
            nonogramBtn = new Rectangle(25, 215, 100, 100);
            fifteenBtn = new Rectangle(610, 240, 100, 100);

            // second screen
            // NORMAL VALUES
            sunRect = new Rectangle(10, 122, 200, 256);
            todayRect = new Rectangle(212, 122, 192, 256);
            alertRect = new Rectangle(407, 122, 190, 256);
            bdayRect = new Rectangle(599, 122, 190, 256);

            // STRETCHED OUT VALUES
            //sunRect = new Rectangle(11, 54, 200, 396);
            //todayRect = new Rectangle(213, 54, 192, 396);
            //skillsRect = new Rectangle(408, 54, 190, 396);
            //bdayRect = new Rectangle(599, 54, 190, 396);

            maxPosterRect = new Rectangle(213, 0, 375, 500);
            backSun = false; backToday = false; backAlert = false; backBday = false;
            lights = true;


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

            // boolean variables to decide if something is complete
            loDone = false; // lights out
            noDone = false; // nonogram

        }



        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // images
            rectTexture = Content.Load<Texture2D>("Images/rectangle");
            xTexture = Content.Load<Texture2D>("Images/Red_X");
            backTexture = Content.Load<Texture2D>("Images/backbutton");
            tableTexture = Content.Load<Texture2D>("Images/tableback");

            // placeholder textures
            phTexture = Content.Load<Texture2D>("Placeholders/escaperoomplaceholder");
            lightsPhTexture = Content.Load<Texture2D>("Placeholders/lightsoutbutton");
            nonogramPhTexture = Content.Load<Texture2D>("Placeholders/nonogrambutton");
            fifteenPhTexture = Content.Load<Texture2D>("Placeholders/fifteenslidingpuzzle");

            // day posters
            sunPosterFront = Content.Load<Texture2D>("Posters/SunFront");
            sunPosterBack = Content.Load<Texture2D>("Posters/SunBack");
            todayPosterFront = Content.Load<Texture2D>("Posters/TodayFront");
            todayPosterBack = Content.Load<Texture2D>("Posters/TodayBack");
            alertPosterFront = Content.Load<Texture2D>("Posters/AlertFront");
            alertPosterBack = Content.Load<Texture2D>("Posters/AlertBack");
            bdayPosterFront = Content.Load<Texture2D>("Posters/BirthdayFront");
            bdayPosterBack = Content.Load<Texture2D>("Posters/BirthdayBack");
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
                            if (!loDone)
                                if (lightGrid.Update(mouseState, prevMouseState))
                                    loDone = true;
                            if (loDone)
                                BackButton();
                            break;
                        case 2: // nonogram
                            if (keyboardState.IsKeyDown(Keys.LeftAlt) && prevKeyboardState.IsKeyUp(Keys.LeftAlt))
                                cellGrid.DebugState();
                            if (!noDone)
                                if (cellGrid.Update(mouseState, prevMouseState))
                                    noDone = true;
                            if (noDone)
                                BackButton();
                            
                            break;
                        case 3: // 15 sliding puzzle
                            break;
                    }
                    break;
                case Screen.CipherPuzzles:
                    switch (puzzle)
                    {
                        case 0:
                            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                            {
                                if (sunRect.Contains(mouseState.Position))
                                    puzzle = 1;
                                if (todayRect.Contains(mouseState.Position))
                                    puzzle = 2;
                                if (alertRect.Contains(mouseState.Position))
                                    puzzle = 3;
                                if (bdayRect.Contains(mouseState.Position))
                                    puzzle = 4;
                                if (!sunRect.Contains(mouseState.Position) && !todayRect.Contains(mouseState.Position) && !alertRect.Contains(mouseState.Position) && !bdayRect.Contains(mouseState.Position))
                                    lights = !lights;
                            }
                            if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
                            {
                                if (sunRect.Contains(mouseState.Position))
                                    backSun = !backSun;
                                if (todayRect.Contains(mouseState.Position))
                                    backToday = !backToday;
                                if (alertRect.Contains(mouseState.Position))
                                    backAlert = !backAlert;
                                if (bdayRect.Contains(mouseState.Position))
                                    backBday = !backBday;

                            }
                            break;
                        case 1: // sun poster
                            BackButton();
                            backSun = BackToggle(maxPosterRect, backSun);
                            break;
                        case 2: // today poster
                            BackButton();
                            backToday = BackToggle(maxPosterRect, backToday);
                            break;
                        case 3: // alert poster
                            BackButton();
                            backAlert = BackToggle(maxPosterRect, backAlert);
                            break;
                        case 4: // birthday poster
                            BackButton();
                            backBday = BackToggle(maxPosterRect, backBday);
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
                            if (!loDone)
                                _spriteBatch.Draw(lightsPhTexture, lightsBtn, Color.White);
                            if (!noDone)
                                _spriteBatch.Draw(nonogramPhTexture, nonogramBtn, Color.White);
                            _spriteBatch.Draw(fifteenPhTexture, fifteenBtn, Color.White);
                            break;
                        case 1: // lights out
                            _spriteBatch.Draw(tableTexture, window, Color.White);
                            _spriteBatch.Draw(rectTexture, lightsBack, Color.Black);
                            lightGrid.Draw(_spriteBatch);
                            if (loDone)
                                _spriteBatch.Draw(backTexture, backBtn, Color.White);
                            break;
                        case 2: // nonogram
                            _spriteBatch.Draw(rectTexture, window, Color.Gray);
                            cellGrid.Draw(_spriteBatch);
                            if (noDone)
                                _spriteBatch.Draw(backTexture, backBtn, Color.White);
                            break;
                        case 3: // 15 sliding puzzle
                            break;
                    }
                    break;
                case Screen.CipherPuzzles:
                    switch (puzzle)
                    {
                        case 0: // posters
                            if (lights)
                            {
                                _spriteBatch.Draw(rectTexture, window, Color.LightSkyBlue);
                            }
                            else
                            {
                                _spriteBatch.Draw(rectTexture, window, Color.DarkSlateGray);
                            }
                            DrawPoster(backSun, sunPosterFront, sunPosterBack, sunRect);
                            DrawPoster(backToday, todayPosterFront, todayPosterBack, todayRect);
                            DrawPoster(backAlert, alertPosterFront, alertPosterBack, alertRect);
                            DrawPoster(backBday, bdayPosterFront, bdayPosterBack, bdayRect);
                            break;
                        case 1: // sun poster
                            LightsToggle();
                            _spriteBatch.Draw(backTexture, backBtn, Color.White);
                            DrawPoster(backSun, sunPosterFront, sunPosterBack, maxPosterRect);
                            break;
                        case 2: // today poster
                            LightsToggle();
                            _spriteBatch.Draw(backTexture, backBtn, Color.White);
                            DrawPoster(backToday, todayPosterFront, todayPosterBack, maxPosterRect);
                            break;
                        case 3: // alert poster
                            LightsToggle();
                            _spriteBatch.Draw(backTexture, backBtn, Color.White);
                            DrawPoster(backAlert, alertPosterFront, alertPosterBack, maxPosterRect);
                            break;
                        case 4: // bday poster
                            LightsToggle();
                            _spriteBatch.Draw(backTexture, backBtn, Color.White);
                            DrawPoster(backBday, bdayPosterFront, bdayPosterBack, maxPosterRect);
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

        public void BackButton()
        {
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                if (backBtn.Contains(mouseState.Position))
                    puzzle = 0;
            }
        }

        public void LightsToggle()
        {
            if (lights)
                _spriteBatch.Draw(rectTexture, window, Color.LightSkyBlue);
            else
                _spriteBatch.Draw(rectTexture, window, Color.DarkSlateGray);
        }

        public bool BackToggle(Rectangle poster, bool toggle)
        {
            if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
            {
                if (poster.Contains(mouseState.Position))
                    toggle = !toggle;
            }
            return toggle;
        }

        public void DrawPoster(bool toggle, Texture2D posterFront, Texture2D posterBack, Rectangle posterRect)
        {
            if (toggle)
                _spriteBatch.Draw(posterBack, posterRect, Color.White);
            else
                _spriteBatch.Draw(posterFront, posterRect, Color.White);
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
