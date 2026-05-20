using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        Unchecked,
        Empty,
        Filled
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Rectangle window; // window     

        Screen screen;

        Texture2D rectTexture;
        Texture2D phTexture; // placeholder texture, remove after textures are finalized.
        Texture2D lightsPhTexture, nonogramPhTexture, fifteenPhTexture; // placeholder texture
        Rectangle lightsBtn, nonogramBtn, fifteenBtn;

        LightGrid lightGrid;
        MouseState mouseState, prevMouseState;
        KeyboardState keyboardState, prevKeyboardState;



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

            lightsBtn = new Rectangle(450, 0, 100, 100);
            nonogramBtn = new Rectangle(25, 215, 100, 100);
            fifteenBtn = new Rectangle(610, 240, 100, 100);

            // TODO: Add your initialization logic here

            base.Initialize();

            lightGrid = new LightGrid(rectTexture, new Point(230, 75), Color.Gold, 1);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            rectTexture = Content.Load<Texture2D>("Images/rectangle");
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
                            break;
                        case 3: // 15 sliding puzzle
                            break;
                        case 4: // idk
                            break;
                        case 5: // idk
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
                        case 4: // idk
                            break;
                        case 5: // idk
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
                        case 2: // idk
                            break;
                        case 3: // idk
                            break;
                        case 4: // idk
                            break;
                        case 5: // idk
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
                            break;
                        case 3: // 15 sliding puzzle
                            break;
                        case 4: // idk
                            break;
                        case 5: // idk
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
                        case 4: // idk
                            break;
                        case 5: // idk
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
                        case 2: // idk
                            break;
                        case 3: // idk
                            break;
                        case 4: // idk
                            break;
                        case 5: // idk
                            break;
                    }
                    break;
                case Screen.Outro:
                    break;
            }

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
