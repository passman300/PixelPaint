using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PixelPaint
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {


        //Track the available colours, one will be active at a time
        private const int BLACK = 0;
        private const int WHITE = 1;
        private const int GREY = 2;
        private const int YELLOW = 3;
        private const int RED = 4;
        private const int GREEN = 5;
        private const int BLUE = 6;
        private const int PINK = 7;

        //Track the number of availabe colours
        private const int NUM_COLOURS = 8;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //Track the image used for every pixel which will be stretched to be one canvas pixel
        //private Texture2D pxlImg;

        //Track the fonts used throughout the HUD
        //private SpriteFont uiFont;
        //private SpriteFont stackFont;

        //Track the full screen dimensions
        private int screenWidth;
        private int screenHeight;

        //Track the width of the hud, which is the screenWidth - screenHeight = 200
        //The canvas is a square of 800x800
        private int hudWidth;

        private MouseState mouse;
        private MouseState prevMouse;

        //Track the canvas object used for all interaction with the Canvas area
        private Canvas canvas;

        //Track the possible NxN grid size dimensions (all even divisors of 800)
        //dimIdx is the index into the array of the active grid size
        private int[] dims = new int[] { 10, 16, 20, 25, 32, 40, 50, 80 };
        private int dimIdx;

        //Track all of the colour data; images, HUD rectangles, actual colours, colour text, index of active colour
        private Texture2D[] colourImgs = new Texture2D[NUM_COLOURS];
        private Rectangle[] colourRecs = new Rectangle[NUM_COLOURS];
        private Color[] colours = new Color[NUM_COLOURS];
        private string[] colourText = new string[NUM_COLOURS];
        private int colourIdx = BLACK;

        //Track all of the HUD images (action buttons, undo/redo, Clear Canvas, check boxes)
        //private Texture2D squareBtnImg;
        //private Texture2D squareBtnActiveImg;
        //private Texture2D circleBtnImg;
        //private Texture2D circleBtnActiveImg;
        //private Texture2D fillBtnImg;
        //private Texture2D fillBtnActiveImg;
        //private Texture2D undoBtnImg;
        //private Texture2D redoBtnImg;
        //private Texture2D clearBtnImg;
        //private Texture2D checkedBoxImg;
        //private Texture2D uncheckedBoxImg;
        //private Texture2D toBeCheckedBoxImg;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Turn on mouse, you may disable this and use a custom pointer
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 800;

            graphics.ApplyChanges();

            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // load all content
            Assets.Content = Content;
            Assets.Initialize();

            ////Load in the SpriteFonts and pixel image
            //uiFont = Content.Load<SpriteFont>("Fonts/UIFont");
            //stackFont = Content.Load<SpriteFont>("Fonts/StackFont");
            //pxlImg = Content.Load<Texture2D>("Images/Sprites/BlankPixel");

            //Load HUD colour box icons
            colourImgs[BLACK] = Assets.blackImg;
            colourImgs[WHITE] = Assets.whiteImg;
            colourImgs[GREY] = Assets.greyImg;
            colourImgs[YELLOW] = Assets.yellowImg;
            colourImgs[RED] = Assets.redImg;
            colourImgs[GREEN] = Assets.greenImg;
            colourImgs[BLUE] = Assets.blueImg;
            colourImgs[PINK] = Assets.pinkImg;
            //colourImgs[BLACK] = Content.Load<Texture2D>("Images/Sprites/Black");
            //colourImgs[WHITE] = Content.Load<Texture2D>("Images/Sprites/White");
            //colourImgs[GREY] = Content.Load<Texture2D>("Images/Sprites/Grey");
            //colourImgs[YELLOW] = Content.Load<Texture2D>("Images/Sprites/Yellow");
            //colourImgs[RED] = Content.Load<Texture2D>("Images/Sprites/Red");
            //colourImgs[GREEN] = Content.Load<Texture2D>("Images/Sprites/Green");
            //colourImgs[BLUE] = Content.Load<Texture2D>("Images/Sprites/Blue");
            //colourImgs[PINK] = Content.Load<Texture2D>("Images/Sprites/Pink");

            //Load in all HUD buttons
            //squareBtnActiveImg = Content.Load<Texture2D>("Images/Sprites/SquareActive");
            //squareBtnImg = Content.Load<Texture2D>("Images/Sprites/SquareInactive");
            //circleBtnActiveImg = Content.Load<Texture2D>("Images/Sprites/CircleActive");
            //circleBtnImg = Content.Load<Texture2D>("Images/Sprites/CircleInactive");
            //fillBtnActiveImg = Content.Load<Texture2D>("Images/Sprites/FillActive");
            //fillBtnImg = Content.Load<Texture2D>("Images/Sprites/Fill");
            //undoBtnImg = Content.Load<Texture2D>("Images/Sprites/Undo");
            //redoBtnImg = Content.Load<Texture2D>("Images/Sprites/Redo");
            //clearBtnImg = Content.Load<Texture2D>("Images/Sprites/ClearCanvas");
            //checkedBoxImg = Content.Load<Texture2D>("Images/Sprites/CheckedBox");
            //uncheckedBoxImg = Content.Load<Texture2D>("Images/Sprites/UncheckedBox");
            //toBeCheckedBoxImg = Content.Load<Texture2D>("Images/Sprites/ToBeCheckedBox");

            //Setup up the colours and their associated text values
            colours[BLACK] = Color.Black;
            colours[WHITE] = Color.White;
            colours[GREY] = Color.DarkGray;
            colours[YELLOW] = Color.Yellow;
            colours[RED] = Color.Red;
            colours[GREEN] = Color.Green;
            colours[BLUE] = Color.Blue;
            colours[PINK] = Color.Pink;
            colourText[BLACK] = "BLACK";
            colourText[WHITE] = "WHITE";
            colourText[GREY] = "GREY";
            colourText[YELLOW] = "YELLOW";
            colourText[RED] = "RED";
            colourText[GREEN] = "GREEN";
            colourText[BLUE] = "BLUE";
            colourText[PINK] = "PINK";

            //Set the active dimension index to the largest size possible
            dimIdx = dims.Length - 1;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            prevMouse = mouse;
            mouse = Mouse.GetState();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            //Draw the Canvas
            //Draw the HUD
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
