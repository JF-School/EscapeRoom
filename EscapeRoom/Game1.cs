using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace EscapeRoom
{
    enum Screen
    {
        Intro,
        Tutorial,
        PosterRoom,
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

        // intro screen
        Texture2D bulletinBack, introBack, creditsBack, tutorialBack; // backgrounds
        Texture2D playTexture, creditsTexture;
        Rectangle playBtn, creditsBtn, yesBtn, noBtn;
        int intro; // 0 = main screen; 1 = tutorial?; 2 = credits;
        SoundEffect introMusic, buttonClick;
        SoundEffectInstance introMusicInstance;

        // tutorial screen
        Texture2D gamePosterFront, gamePosterBack;
        Rectangle gameRect;
        Texture2D leftClickTexture, solveTexture, continueTexture;
        Rectangle leftClickRect, solveRect, continueBtn;
        bool backGame;
        bool gameSpecial; // just to fill the method LOL
        bool visitedBack;
        SoundEffect tutorialMusic;
        SoundEffectInstance tutorialMusicInstance;

        // LIGHTS OUT PUZZLE & NONOGRAM
        Texture2D rectTexture, xTexture, backTexture;
        Texture2D solOneTexture, solTwoTexture, solThreeTexture;
        Rectangle backBtn;
        Rectangle lightsBack;

        // DAY POSTERS -- LIGHTS ON
        Texture2D sunPosterFront, todayPosterFront, alertPosterFront, bdayPosterFront; // front textures
        Texture2D sunPosterBack, todayPosterBack, alertPosterBack, bdayPosterBack; // back textures
        Texture2D bulbOnTexture, bulbBrokenTexture;
        Rectangle sunRect, todayRect, alertRect, bdayRect, maxPosterRect;
        Rectangle bulbRect, bulbOnRect;
        bool lights; // true = on, false = off;
        bool backSun, backToday, backAlert, backBday; // false = front, true = back;
        bool lightsToggle; // true = ability to turn lights on/off. false = no ability to do so;
        bool bulbAppear;
        SoundEffect dayMusic, backBtnSound, clickPoster, flipPoster, clickLight;
        SoundEffectInstance dayMusicInstance;

        // NIGHT POSTERS -- LIGHTS OFF
        Texture2D moonPosterFront, yesterdayPosterFront, barcodePosterFront, canadaPosterFront;
        Texture2D moonPosterBack, yesterdayPosterBack, barcodePosterBack, canadaPosterBack;
        Texture2D bulbOffTexture;
        SoundEffect nightMusic;
        SoundEffectInstance nightMusicInstance;

        // SPECIAL POSTERS
        bool sunDisappear, warningClick, tomorrowToggle, buttonClicked, lockPoster;
        Texture2D tomorrowPosterFront, chestPosterFront, scannerPosterFront, lockPosterFront;
        Texture2D tomorrowPosterBack, chestPosterBack, scannerPosterBack, lockPosterBack;
        Texture2D scannerBtnTexture, cursorTexture, screwdriverTexture;
        Rectangle chestToggleRect, warningSignRect, scannerBtn, cursorRect, lockBtn, chestBtn;
        Rectangle scannerBtnSmall, cursorSmallRect, screwdriverRect, tardisRect;
        bool showScrewdriver, screwdriver;
        bool tardisActivated; // true = you can escape! false = you can't escape.
        bool escaped; // for the sound effect
        int clicks;
        SoundEffect specialSound, tardisWhoosh, completeGame, chestError, chestOpen;
        SoundEffectInstance completeGameInstance;

        // SCANNER
        bool scannerEquipped;
        Texture2D scannerTexture;
        Rectangle scannerRect;
        SoundEffect barcodeScan, itemSwitch;

        // BARCODES
        Rectangle gameBarcode; // tutorial
        Rectangle sunBarcode, todayBarcode, alertBarcode, bdayBarcode; // day posters
        Rectangle moonBarcode, yesterdayBarcode, barcodeBarcode, canadaBarcode; // night posters
        Rectangle chestBarcode, tomorrowBarcode, scannerBarcodeFront, scannerBarcodeBack; // special posters
        float barcodeTimer, barcodeStop; // barcodeTimer counts up, barcodeStop stops showing the text after 4s.
        string barcodeText;
        bool showBarcodeText;
        SpriteFont barcodeFont;

        // LOCK SCREEN
        bool code, keyCollected;
        Rectangle plusBtn1, plusBtn2, plusBtn3, plusBtn4;
        Rectangle subBtn1, subBtn2, subBtn3, subBtn4;
        Rectangle counterRect1, counterRect2, counterRect3, counterRect4;
        Texture2D plusTexture, subTexture, counterTexture, keyTexture;
        Rectangle keyRect;
        SpriteFont numFont;
        Vector2 fontLoca1, fontLoca2, fontLoca3, fontLoca4;
        int num1, num2, num3, num4;
        Color textColor;

        // outro screen
        Texture2D outroBack, escapeTexture;
        Rectangle escapeBtn;
        SpriteFont outroFont;
        float posterTime;
        SoundEffect outroMusic;
        SoundEffectInstance outroMusicInstance;

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

            // needed variables to stay at the top
            screen = Screen.Intro;
            puzzle = 0; // zero puzzle = original window
            generator = new Random();
            lightsToggle = false;
            tardisActivated = false;
            escaped = false;

            // intro screen
            playBtn = new Rectangle(152, 291, 138, 138);
            creditsBtn = new Rectangle(510, 291, 138, 138);
            yesBtn = new Rectangle(50, 250, 275, 80);
            noBtn = new Rectangle(475, 250, 275, 80);
            intro = 0;

            // tutorial
            gameRect = new Rectangle(447, 122, 190, 256);
            leftClickRect = new Rectangle(123, 140, 237, 220);
            solveRect = new Rectangle(-11, 140, 230, 220);
            continueBtn = new Rectangle(690, 390, 75, 75);
            backGame = false;
            gameSpecial = false;
            visitedBack = false;

            // LIGHTS OUT
            lightsBack = new Rectangle(225, 70, 300, 300);
            backBtn = new Rectangle(10, 10, 50, 50);

            // Caesar Cipher Puzzles Room
            // NORMAL VALUES
            sunRect = new Rectangle(10, 122, 200, 256);
            todayRect = new Rectangle(212, 122, 192, 256);
            alertRect = new Rectangle(407, 122, 190, 256);
            bdayRect = new Rectangle(599, 122, 190, 256);
            maxPosterRect = new Rectangle(213, 0, 375, 500);
            bulbRect = new Rectangle(716, 20, 64, 99);
            bulbOnRect = new Rectangle(699, 2, 99, 117);

            // other Rectangles
            chestToggleRect = new Rectangle(213, 193, 171, 171);
            warningSignRect = new Rectangle(270, 397, 51, 46);
            lockBtn = new Rectangle(347, 175, 109, 188);
            chestBtn = new Rectangle(227, 317, 178, 152);
            screwdriverRect = new Rectangle(350, 199, 101, 101);
            tardisRect = new Rectangle(321, 214, 160, 251);
            scannerBtn = new Rectangle(244, 44, 300, 300);
            scannerBtnSmall = new Rectangle(430, 148, 146, 146);
            cursorRect = new Rectangle(400, 212, 100, 129);
            cursorSmallRect = new Rectangle(505, 230, 49, 63);

            backSun = false; backToday = false; backAlert = false; backBday = false;
            lights = true; // lightsToggle = false;
            sunDisappear = false; buttonClicked = false; warningClick = false; tomorrowToggle = false;
            lockPoster = false;
            screwdriver = false; showScrewdriver = false;
            clicks = 0;

            scannerEquipped = false;
            scannerRect = new Rectangle(mouseState.X, mouseState.Y, 45, 45);

            // BARCODE
            gameBarcode = new Rectangle(259, 215, 283, 54);
            sunBarcode = new Rectangle(259, 395, 121, 66);
            moonBarcode = new Rectangle(305, 399, 191, 47);
            chestBarcode = new Rectangle(261, 381, 280, 62);
            todayBarcode = new Rectangle(260, 397, 116, 48);
            yesterdayBarcode = new Rectangle(264, 356, 273, 74);
            tomorrowBarcode = new Rectangle(318, 369, 166, 69);
            alertBarcode = new Rectangle(311, 387, 179, 56);
            barcodeBarcode = new Rectangle(277, 229, 247, 70);
            scannerBarcodeFront = new Rectangle(367, 397, 59, 46);
            scannerBarcodeBack = new Rectangle(262, 370, 277, 68);
            bdayBarcode = new Rectangle(285, 382, 231, 59);
            canadaBarcode = new Rectangle(259, 383, 279, 57);
            barcodeTimer = 0f; barcodeStop = 4f;
            //showBarcodeText = false;

            // lock SCREEN
            plusBtn1 = new Rectangle(68, 88, 80, 80);
            plusBtn2 = new Rectangle(263, 88, 80, 80);
            plusBtn3 = new Rectangle(457, 88, 80, 80);
            plusBtn4 = new Rectangle(652, 88, 80, 80);
            subBtn1 = new Rectangle(68, 333, 80, 80);
            subBtn2 = new Rectangle(263, 333, 80, 80);
            subBtn3 = new Rectangle(457, 333, 80, 80);
            subBtn4 = new Rectangle(652, 333, 80, 80);
            counterRect1 = new Rectangle(46, 188, 125, 125);
            counterRect2 = new Rectangle(240, 188, 125, 125);
            counterRect3 = new Rectangle(435, 188, 125, 125);
            counterRect4 = new Rectangle(630, 188, 125, 125);

            fontLoca1 = new Vector2(74, 192);
            fontLoca2 = new Vector2(266, 192);
            fontLoca3 = new Vector2(466, 192);
            fontLoca4 = new Vector2(658, 192);

            num1 = 0; num2 = 0; num3 = 0; num4 = 0;
            textColor = Color.Black;
            code = false; keyCollected = false;

            keyRect = new Rectangle(325, 168, 165, 165);

            // outro screen
            escapeBtn = new Rectangle(600, 405, 166, 59);

            // TODO: Add your initialization logic here

            base.Initialize();

            lightGrid = new LightGrid(rectTexture, new Point(230, 75), Color.Gold, generator.Next(1, 4));
            cellGrid = new CellGrid(rectTexture, xTexture, new Point(230, 75), Color.White);
            solution1 = new int[10, 10];
            solution2 = new int[10, 10];
            solution3 = new int[10, 10];
            randomSolution = generator.Next(1, 4);
            switch (randomSolution)
            {
                case 1:
                    NonogramSolution1();
                    cellGrid.Solution = solution1;
                    break;
                case 2:
                    NonogramSolution2();
                    cellGrid.Solution = solution2;
                    break;
                case 3:
                    NonogramSolution3();
                    cellGrid.Solution = solution3;
                    break;
            }
            //cellGrid.DebugSolution();


        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // important images
            rectTexture = Content.Load<Texture2D>("Images/rectangle");
            xTexture = Content.Load<Texture2D>("Images/redX");
            backTexture = Content.Load<Texture2D>("Images/backbutton");
            bulletinBack = Content.Load<Texture2D>("Images/bulletinboard");
            introBack = Content.Load<Texture2D>("Images/introscreen");
            playTexture = Content.Load<Texture2D>("Images/playbutton");
            creditsTexture = Content.Load<Texture2D>("Images/creditsbutton");
            creditsBack = Content.Load<Texture2D>("Images/credits");
            tutorialBack = Content.Load<Texture2D>("Images/tutorialBack");

            // lightbulbs
            bulbOnTexture = Content.Load<Texture2D>("Images/onbulb");
            bulbOffTexture = Content.Load<Texture2D>("Images/offbulb");
            bulbBrokenTexture = Content.Load<Texture2D>("Images/brokenbulb");

            // tutorial
            gamePosterFront = Content.Load<Texture2D>("Posters/GamenightFront");
            gamePosterBack = Content.Load<Texture2D>("Posters/GamenightBack");
            leftClickTexture = Content.Load<Texture2D>("Images/leftclick");
            solveTexture = Content.Load<Texture2D>("Images/solvecaesar");
            continueTexture = Content.Load<Texture2D>("Images/continue");

            // day posters
            sunPosterFront = Content.Load<Texture2D>("Posters/SunFront");
            sunPosterBack = Content.Load<Texture2D>("Posters/SunBack");
            todayPosterFront = Content.Load<Texture2D>("Posters/TodayFront");
            todayPosterBack = Content.Load<Texture2D>("Posters/TodayBack");
            alertPosterFront = Content.Load<Texture2D>("Posters/AlertFront");
            alertPosterBack = Content.Load<Texture2D>("Posters/AlertBack");
            bdayPosterFront = Content.Load<Texture2D>("Posters/BirthdayFront");
            bdayPosterBack = Content.Load<Texture2D>("Posters/BirthdayBack");

            // night posters
            moonPosterFront = Content.Load<Texture2D>("Posters/MoonFront");
            moonPosterBack = Content.Load<Texture2D>("Posters/MoonBack");
            yesterdayPosterFront = Content.Load<Texture2D>("Posters/YesterdayFront");
            yesterdayPosterBack = Content.Load<Texture2D>("Posters/YesterdayBack");
            barcodePosterFront = Content.Load<Texture2D>("Posters/BarcodeFront");
            barcodePosterBack = Content.Load<Texture2D>("Posters/BarcodeBack");
            canadaPosterFront = Content.Load<Texture2D>("Posters/CanadaFront");
            canadaPosterBack = Content.Load<Texture2D>("Posters/CanadaBack");

            // special posters
            tomorrowPosterFront = Content.Load<Texture2D>("Posters/TomorrowFront");
            tomorrowPosterBack = Content.Load<Texture2D>("Posters/TomorrowBack");
            chestPosterFront = Content.Load<Texture2D>("Posters/ChestFront");
            chestPosterBack = Content.Load<Texture2D>("Posters/ChestBack");
            scannerPosterFront = Content.Load<Texture2D>("Posters/ScannerFront");
            scannerPosterBack = Content.Load<Texture2D>("Posters/ScannerBack");
            lockPosterFront = Content.Load<Texture2D>("Posters/LockFront");
            lockPosterBack = Content.Load<Texture2D>("Posters/LockBack");

            // scanners
            scannerTexture = Content.Load<Texture2D>("Images/ScannerItem");
            keyTexture = Content.Load<Texture2D>("Images/key");
            screwdriverTexture = Content.Load<Texture2D>("Images/screwdriver");

            // random stuff
            scannerBtnTexture = Content.Load<Texture2D>("Images/button");
            cursorTexture = Content.Load<Texture2D>("Images/cursor");
            plusTexture = Content.Load<Texture2D>("Images/plusButton");
            subTexture = Content.Load<Texture2D>("Images/minusButton");
            counterTexture = Content.Load<Texture2D>("Images/blankCounter");

            // outro screen
            outroBack = Content.Load<Texture2D>("Images/OutroScreen");
            escapeTexture = Content.Load<Texture2D>("Images/escapeButton");

            // text
            numFont = Content.Load<SpriteFont>("Fonts/numFont");
            barcodeFont = Content.Load<SpriteFont>("Fonts/barcodeFont");
            outroFont = Content.Load<SpriteFont>("Fonts/outroFont");
            solOneTexture = Content.Load<Texture2D>("Images/solutionOne");
            solTwoTexture = Content.Load<Texture2D>("Images/solutionTwo");
            solThreeTexture = Content.Load<Texture2D>("Images/solutionThree");

            // songs
            introMusic = Content.Load<SoundEffect>("Songs/introMusic");
            introMusicInstance = introMusic.CreateInstance();
            tutorialMusic = Content.Load<SoundEffect>("Songs/tutorialMusic");
            tutorialMusicInstance = tutorialMusic.CreateInstance();
            dayMusic = Content.Load<SoundEffect>("Songs/dayMusic");
            dayMusicInstance = dayMusic.CreateInstance();
            nightMusic = Content.Load<SoundEffect>("Songs/nightMusic");
            nightMusicInstance = nightMusic.CreateInstance();
            outroMusic = Content.Load<SoundEffect>("Songs/outroMusic");
            outroMusicInstance = outroMusic.CreateInstance();

            // sound effects
            backBtnSound = Content.Load<SoundEffect>("Sounds/BackButton");
            barcodeScan = Content.Load<SoundEffect>("Sounds/BarcodeScan");
            buttonClick = Content.Load<SoundEffect>("Sounds/ButtonClick");
            chestError = Content.Load<SoundEffect>("Sounds/ChestError");
            chestOpen = Content.Load<SoundEffect>("Sounds/ChestOpen");
            clickLight = Content.Load<SoundEffect>("Sounds/ClickLight");
            clickPoster = Content.Load<SoundEffect>("Sounds/ClickPoster");
            completeGame = Content.Load<SoundEffect>("Sounds/CompletedGame");
            completeGameInstance = completeGame.CreateInstance();
            itemSwitch = Content.Load<SoundEffect>("Sounds/ItemSwitch");
            flipPoster = Content.Load<SoundEffect>("Sounds/PosterTurn");
            specialSound = Content.Load<SoundEffect>("Sounds/SpecialPoster");
            tardisWhoosh = Content.Load<SoundEffect>("Sounds/TardisWhooshy");
            

        }

        protected override void Update(GameTime gameTime)
        {
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (showBarcodeText)
            {
                barcodeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (barcodeTimer > barcodeStop)
                {
                    barcodeTimer = 0f;
                    showBarcodeText = false;
                }
            }

            switch (screen) 
            {
                case Screen.Intro:
                    this.Window.Title = "Good luck.";
                    introMusicInstance.Play();
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        if (backBtn.Contains(mouseState.Position))
                        {
                            intro = 0;
                            backBtnSound.Play();
                        }
                        switch (intro)
                        {
                            case 0: // main screen
                                if (playBtn.Contains(mouseState.Position))
                                {
                                    intro = 1;
                                    buttonClick.Play();
                                }
                                if (creditsBtn.Contains(mouseState.Position))
                                {
                                    intro = 2;
                                    buttonClick.Play();
                                }
                                break;
                            case 1: // tutorial?
                                if (yesBtn.Contains(mouseState.Position))
                                {
                                    screen = Screen.Tutorial;
                                    buttonClick.Play();
                                }
                                if (noBtn.Contains(mouseState.Position))
                                {
                                    screen = Screen.PosterRoom;
                                    buttonClick.Play();
                                }
                                break;
                            case 2: // credits (no buttons)
                                break;
                        }
                    }
                    break;
                case Screen.Tutorial:
                    introMusicInstance.Pause();
                    tutorialMusicInstance.Resume();
                    tutorialMusicInstance.Volume = 0.5f;
                    this.Window.Title = "Learn how to play!";
                    ScannerItem();
                    switch (puzzle)
                    {
                        case 0: // main screen
                            CursorChange(gameRect);
                            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                            {
                                if (backBtn.Contains(mouseState.Position) && !visitedBack)
                                {
                                    backBtnSound.Play();
                                    screen = Screen.Intro;
                                    intro = 0;
                                    tutorialMusicInstance.Pause();
                                    introMusicInstance.Resume();
                                }
                                if (continueBtn.Contains(mouseState.Position) && visitedBack)
                                {
                                    buttonClick.Play();
                                    screen = Screen.PosterRoom;
                                    puzzle = 0;
                                }
                                ClickPoster(gameRect, 1);
                            }
                            if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
                            {
                                if (gameRect.Contains(mouseState.Position))
                                {
                                    backGame = !backGame;
                                    flipPoster.Play();
                                    visitedBack = true;
                                }
                            }
                            break;
                        case 1: // gamenight poster
                            ResetMouseCursor(gameRect);
                            BackButton();
                            backGame = BackToggle(maxPosterRect, backGame);
                            if (BackToggle(maxPosterRect, backGame) == true)
                                visitedBack = true;
                            BarcodeScanner(gameBarcode, backGame, !gameSpecial);
                            break;
                    }
                    break;
                case Screen.PosterRoom:
                    introMusicInstance.Pause();
                    tutorialMusicInstance.Pause();
                    if (lights)
                    {
                        if (!lightsToggle)
                            this.Window.Title = "Welcome to the poster room.";
                        else if (lightsToggle && puzzle != 1)
                            this.Window.Title = "Where did all the dark go?";
                        nightMusicInstance.Pause();
                        dayMusicInstance.Play();
                        dayMusicInstance.Volume = 0.5f;
                    }
                    else
                    {
                        this.Window.Title = "Where did all the light go?";
                        dayMusicInstance.Pause();
                        nightMusicInstance.Play();
                        nightMusicInstance.Volume = 0.5f;
                    }
                    posterTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    ScannerItem();
                    switch (puzzle)
                    {
                        case 0:
                            CursorChange(sunRect, todayRect, alertRect, bdayRect);
                            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                            {
                                if (bulbRect.Contains(mouseState.Position) && !lightsToggle && bulbAppear)
                                {
                                    puzzle = 1;
                                    clickLight.Play();
                                    Debug.WriteLine("Press ALT + L to instantly solve the puzzle.");
                                }
                                if (bulbRect.Contains(mouseState.Position) && lightsToggle)
                                {
                                    lights = !lights;
                                    clickLight.Play();
                                }
                                ClickPoster(sunRect, 3);
                                ClickPoster(todayRect, 4);
                                ClickPoster(alertRect, 5);
                                ClickPoster(bdayRect, 6);
                            }
                            if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
                            {
                                if (sunRect.Contains(mouseState.Position))
                                {
                                    backSun = !backSun;
                                    flipPoster.Play();
                                }
                                if (todayRect.Contains(mouseState.Position))
                                {
                                    backToday = !backToday;
                                    flipPoster.Play();
                                }
                                if (alertRect.Contains(mouseState.Position))
                                {
                                    backAlert = !backAlert;
                                    flipPoster.Play();
                                }
                                if (bdayRect.Contains(mouseState.Position))
                                {
                                    backBday = !backBday;
                                    flipPoster.Play();
                                }

                            }
                            break;
                        case 1: // lights out
                            if (!lightsToggle)
                            {
                                if (lightGrid.Update(mouseState, prevMouseState) 
                                    || (keyboardState.IsKeyDown(Keys.LeftAlt) && keyboardState.IsKeyDown(Keys.L) && prevKeyboardState.IsKeyUp(Keys.L)))
                                {
                                    lightsToggle = true;
                                    specialSound.Play();
                                }
                            }
                            if (lightsToggle)
                                BackButton();
                            break;
                        case 2: // nonogram
                            if (!tardisActivated)
                                if (cellGrid.Update(mouseState, prevMouseState) 
                                    || (keyboardState.IsKeyDown(Keys.LeftAlt) && keyboardState.IsKeyDown(Keys.N) && prevKeyboardState.IsKeyUp(Keys.N)))
                                    tardisActivated = true;
                            if (tardisActivated)
                            {
                                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                                {
                                    if (escapeBtn.Contains(mouseState.Position))
                                    {
                                        completeGameInstance.Play();
                                        escaped = true;
                                    }
                                }
                            }
                            if (escaped)
                            {
                                if (completeGameInstance.State == SoundState.Stopped)
                                    screen = Screen.Outro;
                            }
                            break;
                        case 3: // sun poster
                            ResetMouseCursor(sunRect);
                            if (!screwdriver)
                                BackButton();
                            else
                                BackButton(4);
                            bulbAppear = true;
                            backSun = BackToggle(maxPosterRect, backSun);
                            if (lights)
                                BarcodeScanner(sunBarcode, backSun, !sunDisappear);
                            else
                                BarcodeScanner(moonBarcode, backSun, !sunDisappear);
                            if (lights || !lights)
                                BarcodeScanner(chestBarcode, backSun, sunDisappear);
                            if (lights && !backSun && !sunDisappear)
                            {
                                if (keyboardState.IsKeyDown(Keys.LeftShift) && keyboardState.IsKeyDown(Keys.LeftControl) 
                                    && (keyboardState.IsKeyDown(Keys.S) && prevKeyboardState.IsKeyUp(Keys.S)))
                                {
                                    if (chestToggleRect.Contains(mouseState.Position))
                                    {
                                        sunDisappear = true;
                                        specialSound.Play();
                                    }
                                }
                            }
                            if (sunDisappear && keyCollected && tomorrowToggle && !backSun && !screwdriver)
                            {
                                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                                {
                                    if (chestBtn.Contains(mouseState.Position))
                                    {
                                        showScrewdriver = true;
                                        chestOpen.Play();
                                    }
                                    if (screwdriverRect.Contains(mouseState.Position) && showScrewdriver)
                                    {
                                        showScrewdriver = false;
                                        screwdriver = true;
                                        specialSound.Play();
                                    }
                                }
                            }
                            if (sunDisappear && !keyCollected || !tomorrowToggle && !backSun && !screwdriver)
                                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                                    if (chestBtn.Contains(mouseState.Position))
                                        chestError.Play();
                            break;
                        case 4: // today poster
                            Mouse.SetCursor(MouseCursor.Arrow);
                            BackButton();
                            backToday = BackToggle(maxPosterRect, backToday);
                            if (lights)
                            {
                                BarcodeScanner(todayBarcode, backToday, !tomorrowToggle);
                                BarcodeScanner(tomorrowBarcode, backToday, tomorrowToggle);
                            }
                            else
                                BarcodeScanner(yesterdayBarcode, backToday, !tomorrowToggle);
                            if (lights && !backToday)
                            {
                                if (keyboardState.IsKeyDown(Keys.LeftControl) && (keyboardState.IsKeyDown(Keys.K) && prevKeyboardState.IsKeyUp(Keys.K)))
                                {
                                    tomorrowToggle = true;
                                    specialSound.Play();
                                }
                            }
                            if (tomorrowToggle && screwdriver)
                            {
                                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                                {
                                    if (tardisRect.Contains(mouseState.Position))
                                    {
                                        puzzle = 2;
                                        tardisWhoosh.Play();
                                        Debug.WriteLine("Press ALT + N to instantly complete the puzzle.");
                                    }
                                }
                            }
                            break;
                        case 5: // alert poster
                            Mouse.SetCursor(MouseCursor.Arrow);
                            BackButton();
                            backAlert = BackToggle(maxPosterRect, backAlert);
                            if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
                            {
                                showBarcodeText = false;
                                barcodeTimer = 0f;
                            }
                            if (lights)
                                BarcodeScanner(alertBarcode, backAlert, !warningClick);
                            else
                            {
                                BarcodeScanner(barcodeBarcode, !backAlert, !warningClick);
                                BarcodeScanner(scannerBarcodeFront, !backAlert, warningClick);
                                BarcodeScanner(scannerBarcodeBack, backAlert, warningClick);
                            }
                            if (!lights && !backAlert && !warningClick)
                            {
                                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                                {
                                    if (warningSignRect.Contains(mouseState.Position))
                                    {
                                        clicks++;
                                        if (clicks == 5)
                                        {
                                            warningClick = true;
                                            specialSound.Play();
                                        }
                                    }
                                    else if (!warningSignRect.Contains(mouseState.Position))
                                        clicks = 0;
                                }
                            }
                            if (backAlert && backBday && warningClick && !lights)
                            {
                                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                                {
                                    if (scannerBtn.Contains(mouseState.Position))
                                    {
                                        buttonClicked = true;
                                        lockPoster = true;
                                        specialSound.Play();
                                    }
                                }
                            }
                            break;
                        case 6: // birthday poster
                            Mouse.SetCursor(MouseCursor.Arrow);
                            BackButton();
                            backBday = BackToggle(maxPosterRect, backBday);
                            if (lights)
                                BarcodeScanner(bdayBarcode, backBday, !buttonClicked);
                            else
                                BarcodeScanner(canadaBarcode, backBday, !buttonClicked);
                            if (buttonClicked && sunDisappear && !backBday && lights)
                            {
                                SetMouseCursor(lockBtn);
                                ResetMouseCursor(lockBtn);
                                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                                {
                                    if (lockBtn.Contains(mouseState.Position))
                                    {
                                        buttonClick.Play();
                                        puzzle = 7;
                                    }
                                }
                            }
                            if (buttonClicked && !sunDisappear && !backBday && lights)
                                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                                    if (lockBtn.Contains(mouseState.Position))
                                        chestError.Play();
                            break;
                        case 7: // lock
                            Mouse.SetCursor(MouseCursor.Arrow);
                            BackButton(6);
                            if (!code)
                            {
                                // subtract buttons (individual methods not working)
                                SubtractButtons();
                                // plus buttons (individual methods not working)
                                PlusButtons();
                            }
                            if (num1 == 2 && num2 == 9 && num3 == 0 && num4 == 1) // code
                            {
                                textColor = Color.Green;
                                code = true;
                            }
                            if (code)
                            {
                                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                                {
                                    if (keyRect.Contains(mouseState.Position))
                                    {
                                        keyCollected = true;
                                        lockPoster = false;
                                        specialSound.Play();
                                    }
                                }
                            }
                            break;
                    }
                    break;
                case Screen.Outro:
                    this.Window.Title = "The future where you escaped!";
                    dayMusicInstance.Pause();
                    nightMusicInstance.Pause();
                    outroMusicInstance.Play();
                    outroMusicInstance.Volume = 0.3f;
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
                    if (intro == 0) // main screen
                    {
                        _spriteBatch.Draw(introBack, window, Color.White);
                        _spriteBatch.Draw(playTexture, playBtn, Color.White);
                        _spriteBatch.Draw(creditsTexture, creditsBtn, Color.White);
                    }
                    else if (intro == 1) // ask about tutorial
                    {
                        _spriteBatch.Draw(rectTexture, window, Color.LightGray);
                        _spriteBatch.Draw(tutorialBack, window, Color.White);
                        _spriteBatch.Draw(backTexture, backBtn, Color.White);
                    }
                    else if (intro == 2) // credits
                    {
                        _spriteBatch.Draw(creditsBack, window, Color.White);
                        _spriteBatch.Draw(backTexture, backBtn, Color.White);
                    }
                    break;
                case Screen.Tutorial:
                    switch (puzzle)
                    {
                        case 0: // main screen
                            _spriteBatch.Draw(rectTexture, window, Color.LightSkyBlue);
                            _spriteBatch.Draw(leftClickTexture, leftClickRect, Color.White);
                            DrawPoster(backGame, gamePosterFront, gamePosterBack, gameRect);
                            if (!visitedBack)
                                _spriteBatch.Draw(backTexture, backBtn, Color.White);
                            else
                                _spriteBatch.Draw(continueTexture, continueBtn, Color.White);
                            break;
                        case 1: // gamenight poster
                            _spriteBatch.Draw(rectTexture, window, Color.LightSkyBlue);
                            DrawPoster(backGame, gamePosterFront, gamePosterBack, maxPosterRect);
                            if (!backGame)
                                _spriteBatch.Draw(solveTexture, solveRect, Color.White);
                            DrawBarcodeText("[HIDDEN MESSAGE REVEALED]", new Vector2(246, 166), backGame);
                            _spriteBatch.Draw(backTexture, backBtn, Color.White);
                            break;
                    }
                    if (scannerEquipped)
                        _spriteBatch.Draw(scannerTexture, scannerRect, Color.White);
                    break;
                case Screen.PosterRoom:
                    switch (puzzle)
                    {
                        case 0: // posters
                            if (lights)
                            {
                                _spriteBatch.Draw(rectTexture, window, Color.LightSkyBlue);
                                if (!lightsToggle && bulbAppear)
                                    _spriteBatch.Draw(bulbBrokenTexture, bulbRect, Color.White);
                                if (lightsToggle)
                                    _spriteBatch.Draw(bulbOnTexture, bulbOnRect, Color.White);
                                if (!sunDisappear)
                                    DrawPoster(backSun, sunPosterFront, sunPosterBack, sunRect);
                                else
                                    DrawPoster(backSun, chestPosterFront, chestPosterBack, sunRect);
                                if (!tomorrowToggle)
                                    DrawPoster(backToday, todayPosterFront, todayPosterBack, todayRect);
                                else
                                    DrawPoster(backToday, tomorrowPosterFront, tomorrowPosterBack, todayRect);
                                DrawPoster(backAlert, alertPosterFront, alertPosterBack, alertRect);
                                if (!lockPoster)
                                    DrawPoster(backBday, bdayPosterFront, bdayPosterBack, bdayRect);
                                else
                                    DrawPoster(backBday, lockPosterFront, lockPosterBack, bdayRect);
                            }
                            else
                            {
                                _spriteBatch.Draw(rectTexture, window, Color.DarkSlateGray);
                                if (lightsToggle)
                                    _spriteBatch.Draw(bulbOffTexture, bulbRect, Color.White); 
                                if (!sunDisappear)
                                    DrawPoster(backSun, moonPosterFront, moonPosterBack, sunRect);
                                else
                                    DrawPoster(backSun, chestPosterFront, chestPosterBack, sunRect);
                                DrawPoster(backToday, yesterdayPosterFront, yesterdayPosterBack, todayRect);
                                if (!warningClick)
                                    DrawPoster(backAlert, barcodePosterFront, barcodePosterBack, alertRect);
                                else
                                {
                                    DrawPoster(backAlert, scannerPosterFront, scannerPosterBack, alertRect);
                                    if (!buttonClicked && backAlert)
                                    {
                                        _spriteBatch.Draw(scannerBtnTexture, scannerBtnSmall, Color.White);
                                        _spriteBatch.Draw(cursorTexture, cursorSmallRect, Color.White);
                                    }
                                }
                                DrawPoster(backBday, canadaPosterFront, canadaPosterBack, bdayRect);
                            }
                            break;
                        case 1: // lights out
                            _spriteBatch.Draw(rectTexture, window, Color.DarkGray);
                            _spriteBatch.Draw(rectTexture, lightsBack, Color.Black);
                            lightGrid.Draw(_spriteBatch);
                            if (lightsToggle)
                                _spriteBatch.Draw(backTexture, backBtn, Color.White);
                            break;
                        case 2: // nonogram
                            _spriteBatch.Draw(rectTexture, window, Color.Gray);
                            switch (randomSolution)
                            {
                                case 1:
                                    _spriteBatch.Draw(solOneTexture, window, Color.White);
                                    break;
                                case 2:
                                    _spriteBatch.Draw(solTwoTexture, window, Color.White);
                                    break;
                                case 3:
                                    _spriteBatch.Draw(solThreeTexture, window, Color.White);
                                    break;
                            }
                            cellGrid.Draw(_spriteBatch);
                            if (tardisActivated)
                                _spriteBatch.Draw(escapeTexture, escapeBtn, Color.White);
                            break;
                        case 3: // sun poster
                            if (!sunDisappear)
                            {
                                LightsToggle(backSun, sunPosterFront, sunPosterBack, moonPosterFront, moonPosterBack, maxPosterRect);
                                if (lights)
                                    DrawBarcodeText("[noburger]", new Vector2(224, 341), backSun); // sun poster
                                else
                                    DrawBarcodeText("[No Sun = No Moon]", new Vector2(224, 341), backSun); // moon poster
                            }
                            else
                            {
                                LightsToggle(backSun, chestPosterFront, chestPosterBack, chestPosterFront, chestPosterBack, maxPosterRect);
                                if (lights || !lights)
                                    DrawBarcodeText("[Shift your keyboard]", new Vector2(257, 341), backSun); // chest poster
                                if (showScrewdriver)
                                    _spriteBatch.Draw(screwdriverTexture, screwdriverRect, Color.White);
                            }
                            _spriteBatch.Draw(backTexture, backBtn, Color.White);
                            break;
                        case 4: // today poster
                            if (!tomorrowToggle)
                            {
                                LightsToggle(backToday, todayPosterFront, todayPosterBack, yesterdayPosterFront, yesterdayPosterBack, maxPosterRect);
                                if (lights)
                                    DrawBarcodeText("[Vigenere]", new Vector2(247, 294), backToday); // today poster
                            }
                            else
                            {
                                LightsToggle(backToday, tomorrowPosterFront, tomorrowPosterBack, yesterdayPosterFront, yesterdayPosterBack, maxPosterRect);
                                if (lights)
                                    DrawBarcodeText("[Dr. Who?]", new Vector2(247, 294), backToday); // tomorrow poster
                            }
                            if (!lights)
                                DrawBarcodeText("[Musical Artist]", new Vector2(247, 294), backToday); // yesterday poster
                            _spriteBatch.Draw(backTexture, backBtn, Color.White);
                            break;
                        case 5: // alert poster
                            if (!warningClick)
                            {
                                LightsToggle(backAlert, alertPosterFront, alertPosterBack, barcodePosterFront, barcodePosterBack, maxPosterRect);
                                if (lights && backAlert)
                                    DrawBarcodeText("[Two Commands]", new Vector2(278, 340), backAlert); // alert poster
                                if (!lights && !backAlert)
                                    DrawBarcodeText("[What did I just say]", new Vector2(277, 184), !backAlert); // barcode poster
                            }
                            else
                            {
                                LightsToggle(backAlert, alertPosterFront, alertPosterBack, scannerPosterFront, scannerPosterBack, maxPosterRect);
                                if (!lights && !backAlert)
                                    DrawBarcodeText("[hi]", new Vector2(437, 407), !backAlert); // scanner poster front
                                if (!lights && backAlert)
                                    DrawBarcodeText("[Click it]", new Vector2(279, 461), backAlert); // scanner poster back
                                if (backAlert && !buttonClicked)
                                {
                                    _spriteBatch.Draw(scannerBtnTexture, scannerBtn, Color.White);
                                    _spriteBatch.Draw(cursorTexture, cursorRect, Color.White);
                                }
                            }
                            _spriteBatch.Draw(backTexture, backBtn, Color.White);
                            break;
                        case 6: // bday poster
                            if (!lockPoster)
                            {
                                LightsToggle(backBday, bdayPosterFront, bdayPosterBack, canadaPosterFront, canadaPosterBack, maxPosterRect);
                                if (lights)
                                    DrawBarcodeText("[Button. Canada.]", new Vector2(247, 294), backBday); // bday poster
                                else
                                    DrawBarcodeText("[Stay on this screen]", new Vector2(247, 294), backBday); // canada poster
                            }
                            else
                                LightsToggle(backBday, lockPosterFront, lockPosterBack, canadaPosterFront, canadaPosterBack, maxPosterRect);
                            _spriteBatch.Draw(backTexture, backBtn, Color.White);
                            break;
                        case 7: // lock
                            _spriteBatch.Draw(rectTexture, window, Color.DimGray);
                            _spriteBatch.Draw(plusTexture, plusBtn1, Color.White);
                            _spriteBatch.Draw(plusTexture, plusBtn2, Color.White);
                            _spriteBatch.Draw(plusTexture, plusBtn3, Color.White);
                            _spriteBatch.Draw(plusTexture, plusBtn4, Color.White);
                            _spriteBatch.Draw(counterTexture, counterRect1, Color.White);
                            _spriteBatch.Draw(counterTexture, counterRect2, Color.White);
                            _spriteBatch.Draw(counterTexture, counterRect3, Color.White);
                            _spriteBatch.Draw(counterTexture, counterRect4, Color.White);
                            _spriteBatch.Draw(subTexture, subBtn1, Color.White);
                            _spriteBatch.Draw(subTexture, subBtn2, Color.White);
                            _spriteBatch.Draw(subTexture, subBtn3, Color.White);
                            _spriteBatch.Draw(subTexture, subBtn4, Color.White);
                            _spriteBatch.DrawString(numFont, $"{num1}", fontLoca1, textColor);
                            _spriteBatch.DrawString(numFont, $"{num2}", fontLoca2, textColor);
                            _spriteBatch.DrawString(numFont, $"{num3}", fontLoca3, textColor);
                            _spriteBatch.DrawString(numFont, $"{num4}", fontLoca4, textColor);
                            if (code && !keyCollected)
                                _spriteBatch.Draw(keyTexture, keyRect, Color.White);
                            _spriteBatch.Draw(backTexture, backBtn, Color.White);
                            break;
                        case 8: // chest
                            break;
                    }
                    if (scannerEquipped)
                        _spriteBatch.Draw(scannerTexture, scannerRect, Color.White);
                    break;
                case Screen.Outro:
                    _spriteBatch.Draw(outroBack, window, Color.White);
                    _spriteBatch.DrawString(outroFont, $"{Math.Round(posterTime, 0)}s", new Vector2(243, 372), Color.White);
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
                {
                    backBtnSound.Play();
                    puzzle = 0;
                    showBarcodeText = false;
                    barcodeTimer = 0f;
                }
            }
        }

        public void BackButton(int backPuzzle)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                if (backBtn.Contains(mouseState.Position))
                {
                    backBtnSound.Play();
                    puzzle = backPuzzle;
                    showBarcodeText = false;
                    barcodeTimer = 0f;
                }
            }
        }

        public void ClickPoster(Rectangle poster, int backPuzzle)
        {
            if (poster.Contains(mouseState.Position))
            {
                puzzle = backPuzzle;
                clickPoster.Play();
            }    
        }

        public void ScannerItem()
        {
            if (keyboardState.IsKeyDown(Keys.B) && prevKeyboardState.IsKeyUp(Keys.B))
            {
                scannerEquipped = !scannerEquipped;
                itemSwitch.Play();
            }
            if (scannerEquipped)
            {
                scannerRect.X = mouseState.X;
                scannerRect.Y = mouseState.Y;
                IsMouseVisible = false;
            }
            else
                IsMouseVisible = true;
        }

        public void SetMouseCursor(Rectangle rect)
        {
            if (rect.Contains(mouseState.Position))
                Mouse.SetCursor(MouseCursor.Hand);
        }

        public void ResetMouseCursor(Rectangle rect)
        {
            if (!rect.Contains(mouseState.Position))
                Mouse.SetCursor(MouseCursor.Arrow);
        }

        public void CursorChange(Rectangle rect)
        {
            SetMouseCursor(rect);
            ResetMouseCursor(rect);
        }

        public void SetMouseCursor(Rectangle rect1, Rectangle rect2, Rectangle rect3, Rectangle rect4)
        {
            if (rect1.Contains(mouseState.Position) || rect2.Contains(mouseState.Position) || rect3.Contains(mouseState.Position) || rect4.Contains(mouseState.Position))
                Mouse.SetCursor(MouseCursor.Hand);
        }

        public void ResetMouseCursor(Rectangle rect1, Rectangle rect2, Rectangle rect3, Rectangle rect4)
        {
            if (!rect1.Contains(mouseState.Position) && !rect2.Contains(mouseState.Position) && !rect3.Contains(mouseState.Position) && !rect4.Contains(mouseState.Position))
                Mouse.SetCursor(MouseCursor.Arrow);
        }

        public void CursorChange(Rectangle rect1, Rectangle rect2, Rectangle rect3, Rectangle rect4)
        {
            SetMouseCursor(rect1, rect2, rect3, rect4);
            ResetMouseCursor(rect1, rect2, rect3, rect4);
        }

        public void PlusButtons()
        {
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                if (plusBtn1.Contains(mouseState.Position))
                {
                    if (num1 == 9)
                        num1 = 0;
                    else
                        num1++;
                }
                if (plusBtn2.Contains(mouseState.Position))
                {
                    if (num2 == 9)
                        num2 = 0;
                    else
                        num2++;
                }
                if (plusBtn3.Contains(mouseState.Position))
                {
                    if (num3 == 9)
                        num3 = 0;
                    else
                        num3++;
                }
                if (plusBtn4.Contains(mouseState.Position))
                {
                    if (num4 == 9)
                        num4 = 0;
                    else
                        num4++;
                }
            }
        }
        public void SubtractButtons()
        {
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                if (subBtn1.Contains(mouseState.Position))
                {
                    if (num1 == 0)
                        num1 = 9;
                    else
                        num1--;
                }
                if (subBtn2.Contains(mouseState.Position))
                {
                    if (num2 == 0)
                        num2 = 9;
                    else
                        num2--;
                }
                if (subBtn3.Contains(mouseState.Position))
                {
                    if (num3 == 0)
                        num3 = 9;
                    else
                        num3--;
                }
                if (subBtn4.Contains(mouseState.Position))
                {
                    if (num4 == 0)
                        num4 = 9;
                    else
                        num4--;
                }

            }
        }

        public void LightsToggle(bool toggle, Texture2D lightsFront, Texture2D lightsBack, Texture2D noLightsFront, Texture2D noLightsBack, Rectangle posterRect)
        {
            if (lights)
            {
                _spriteBatch.Draw(rectTexture, window, Color.LightSkyBlue);
                DrawPoster(toggle, lightsFront, lightsBack, posterRect);
            }
            else
            {
                _spriteBatch.Draw(rectTexture, window, Color.DarkSlateGray);
                DrawPoster(toggle, noLightsFront, noLightsBack, posterRect);
            }
        }

        public bool BackToggle(Rectangle poster, bool toggle)
        {
            if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
            {
                if (poster.Contains(mouseState.Position))
                {
                    toggle = !toggle;
                    flipPoster.Play();
                }
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

        public void BarcodeScanner(Rectangle specialBarcode, bool backToggle, bool specialPoster)
        {
            if (scannerEquipped && backToggle && specialPoster)
                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    if (specialBarcode.Contains(mouseState.Position))
                    {
                        showBarcodeText = true;
                        barcodeScan.Play();
                        if ((barcodeTimer > barcodeStop) || !backToggle)
                        {
                            barcodeTimer = 0f;
                            showBarcodeText = false;
                        }
                    }
        }

        public void DrawBarcodeText(string specialBarcodeOutput, Vector2 textLocation, bool backToggle)
        {
            if (showBarcodeText && backToggle)
            {
                barcodeText = specialBarcodeOutput;
                _spriteBatch.DrawString(barcodeFont, specialBarcodeOutput, textLocation, Color.Red);
            }
        }


        public void NonogramSolution1()
        {
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                {
                    solution1[x, y] = 0;
                }
            // [column, row] (STARTS AT 0)
            solution1[0, 0] = 1; solution1[0, 1] = 1; solution1[0, 2] = 1; solution1[0, 3] = 1;
            solution1[1, 1] = 1; solution1[1, 2] = 1; solution1[1, 3] = 1;
            solution1[2, 2] = 1;
            solution1[3, 0] = 1; solution1[3, 3] = 1; solution1[3, 4] = 1; solution1[3, 5] = 1; solution1[3, 8] = 1; solution1[3, 9] = 1;
            solution1[4, 0] = 1; solution1[4, 1] = 1; solution1[4, 2] = 1; solution1[4, 4] = 1; solution1[4, 5] = 1; solution1[4, 6] = 1; solution1[4, 7] = 1; solution1[4, 8] = 1;
            solution1[5, 0] = 1; solution1[5, 1] = 1; solution1[5, 2] = 1; solution1[5, 4] = 1; solution1[5, 5] = 1; solution1[5, 6] = 1; solution1[5, 7] = 1; solution1[5, 8] = 1; solution1[5, 9] = 1;
            solution1[6, 0] = 1; solution1[6, 1] = 1; solution1[6, 2] = 1; solution1[6, 4] = 1; solution1[6, 6] = 1; solution1[6, 7] = 1; solution1[6, 8] = 1; solution1[6, 9] = 1;
            solution1[7, 0] = 1; solution1[7, 1] = 1; solution1[7, 9] = 1;
            solution1[8, 0] = 1; solution1[8, 1] = 1; solution1[8, 2] = 1; solution1[8, 9] = 1;
            solution1[9, 0] = 1; solution1[9, 1] = 1; solution1[9, 5] = 1; solution1[9, 7] = 1;

            
        }

        public void NonogramSolution2()
        {
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                {
                    solution2[x, y] = 0;
                }
            // [column, row] (STARTS AT 0)
            solution2[0, 3] = 1; solution2[0, 4] = 1; solution2[0, 5] = 1; solution2[0, 6] = 1; solution2[0, 7] = 1; solution2[0, 8] = 1; solution2[0, 9] = 1;
            solution2[1, 3] = 1; solution2[1, 4] = 1; solution2[1, 5] = 1; solution2[1, 6] = 1; solution2[1, 7] = 1; solution2[1, 8] = 1; solution2[1, 9] = 1;
            solution2[2, 2] = 1; solution2[2, 7] = 1;
            solution2[3, 2] = 1;
            solution2[4, 1] = 1; solution2[4, 2] = 1; solution2[4, 7] = 1; solution2[4, 9] = 1;
            solution2[5, 0] = 1; solution2[5, 1] = 1; solution2[5, 2] = 1; solution2[5, 7] = 1; solution2[5, 9] = 1;
            solution2[6, 0] = 1; solution2[6, 1] = 1; solution2[6, 5] = 1; solution2[6, 6] = 1; solution2[6, 7] = 1; solution2[6, 8] = 1; solution2[6, 9] = 1;
            solution2[7, 0] = 1; solution2[7, 8] = 1; solution2[7, 9] = 1;
            solution2[8, 0] = 1; solution2[8, 1] = 1; solution2[8, 4] = 1; solution2[8, 5] = 1; solution2[8, 8] = 1; solution2[8, 9] = 1;
            solution2[9, 0] = 1; solution2[9, 1] = 1; solution2[9, 2] = 1; solution2[9, 3] = 1; solution2[9, 4] = 1; solution2[9, 6] = 1; solution2[9, 8] = 1; solution2[9, 9] = 1;
        }

        public void NonogramSolution3()
        {
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                {
                    solution3[x, y] = 0;
                }
            // [column, row] (STARTS AT 0)
            solution3[0, 0] = 1; solution3[0, 1] = 1; solution3[0, 5] = 1; solution3[0, 9] = 1;
            solution3[1, 0] = 1; solution3[1, 1] = 1; solution3[1, 2] = 1;
            solution3[2, 0] = 1; solution3[2, 1] = 1; solution3[2, 7] = 1; solution3[2, 8] = 1;
            solution3[3, 0] = 1; solution3[3, 1] = 1; solution3[3, 8] = 1; solution3[3, 9] = 1;
            solution3[4, 0] = 1; solution3[4, 7] = 1; solution3[4, 8] = 1; solution3[4, 9] = 1;
            solution3[5, 3] = 1; solution3[5, 4] = 1; solution3[5, 5] = 1; solution3[5, 7] = 1; solution3[5, 8] = 1; solution3[5, 9] = 1;
            solution3[6, 0] = 1; solution3[6, 2] = 1; solution3[6, 3] = 1; solution3[6, 4] = 1; solution3[6, 5] = 1; solution3[6, 6] = 1; solution3[6, 7] = 1; solution3[6, 8] = 1; solution3[6, 9] = 1;
            solution3[7, 0] = 1; solution3[7, 1] = 1; solution3[7, 2] = 1; solution3[7, 7] = 1; solution3[7, 8] = 1; solution3[7, 9] = 1;
            solution3[8, 0] = 1; solution3[8, 1] = 1; solution3[8, 7] = 1; solution3[8, 8] = 1; solution3[8, 9] = 1;
            solution3[9, 0] = 1; solution3[9, 1] = 1; solution3[9, 7] = 1; solution3[9, 8] = 1; solution3[9, 9] = 1;
        }

    }
}
