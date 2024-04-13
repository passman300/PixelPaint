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
    /// This is the main types for your game.
    /// </summary>
    public class Game1 : Game
    {
        public enum Tool
        {
            Box,
            Circle,
            Fill
        }

        //Track the available colors, one will be active at a time
        private const int BLACK = 0;
        private const int WHITE = 1;
        private const int GREY = 2;
        private const int YELLOW = 3;
        private const int RED = 4;
        private const int GREEN = 5;
        private const int BLUE = 6;
        private const int PINK = 7;

        //Track the number of availabe colors
        private const int NUM_COLORS = 8;
        public Tool DrawTool { get; set; }
        private bool drawActive { get; set; }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //Track the image used for every pixel which will be stretched to be one canvas pixel
        private Texture2D pxlImg;

        //Track the fonts used throughout the HUD
        private SpriteFont uiFont;
        private SpriteFont stackFont;

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

        //Track all of the color data; images, HUD rectangles, actual colors, color text, index of active color
        private Texture2D[] colorImgs = new Texture2D[NUM_COLORS];
        private Rectangle[] colorRecs = new Rectangle[NUM_COLORS];
        private Color[] colors = new Color[NUM_COLORS];
        private string[] colorText = new string[NUM_COLORS];
        private int colorIdx = BLACK;

        //Track all of the HUD images (action buttons, undo/redo, Clear Canvas, check boxes)
        private Texture2D squareBtnImg;
        private Texture2D squareBtnActiveImg;
        private Texture2D circleBtnImg;
        private Texture2D circleBtnActiveImg;
        private Texture2D fillBtnImg;
        private Texture2D fillBtnActiveImg;
        private Texture2D undoBtnImg;
        private Texture2D redoBtnImg;
        private Texture2D clearBtnImg;
        private Texture2D checkedBoxImg;
        private Texture2D uncheckedBoxImg;
        private Texture2D toBeCheckedBoxImg;


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

            //Load in the SpriteFonts and pixel image
            uiFont = Content.Load<SpriteFont>("Fonts/UIFont");
            stackFont = Content.Load<SpriteFont>("Fonts/StackFont");
            pxlImg = Content.Load<Texture2D>("Images/Sprites/BlankPixel");

            //Load HUD color box icons
            colorImgs[BLACK] = Content.Load<Texture2D>("Images/Sprites/Black");
            colorImgs[WHITE] = Content.Load<Texture2D>("Images/Sprites/White");
            colorImgs[GREY] = Content.Load<Texture2D>("Images/Sprites/Grey");
            colorImgs[YELLOW] = Content.Load<Texture2D>("Images/Sprites/Yellow");
            colorImgs[RED] = Content.Load<Texture2D>("Images/Sprites/Red");
            colorImgs[GREEN] = Content.Load<Texture2D>("Images/Sprites/Green");
            colorImgs[BLUE] = Content.Load<Texture2D>("Images/Sprites/Blue");
            colorImgs[PINK] = Content.Load<Texture2D>("Images/Sprites/Pink");

            //Load in all HUD buttons
            squareBtnActiveImg = Content.Load<Texture2D>("Images/Sprites/SquareActive");
            squareBtnImg = Content.Load<Texture2D>("Images/Sprites/SquareInactive");
            circleBtnActiveImg = Content.Load<Texture2D>("Images/Sprites/CircleActive");
            circleBtnImg = Content.Load<Texture2D>("Images/Sprites/CircleInactive");
            fillBtnActiveImg = Content.Load<Texture2D>("Images/Sprites/FillActive");
            fillBtnImg = Content.Load<Texture2D>("Images/Sprites/Fill");
            undoBtnImg = Content.Load<Texture2D>("Images/Sprites/Undo");
            redoBtnImg = Content.Load<Texture2D>("Images/Sprites/Redo");
            clearBtnImg = Content.Load<Texture2D>("Images/Sprites/ClearCanvas");
            checkedBoxImg = Content.Load<Texture2D>("Images/Sprites/CheckedBox");
            uncheckedBoxImg = Content.Load<Texture2D>("Images/Sprites/UncheckedBox");
            toBeCheckedBoxImg = Content.Load<Texture2D>("Images/Sprites/ToBeCheckedBox");

            //Setup up the colors and their associated text values
            colors[BLACK] = Color.Black;
            colors[WHITE] = Color.White;
            colors[GREY] = Color.DarkGray;
            colors[YELLOW] = Color.Yellow;
            colors[RED] = Color.Red;
            colors[GREEN] = Color.Green;
            colors[BLUE] = Color.Blue;
            colors[PINK] = Color.Pink;
            colorText[BLACK] = "BLACK";
            colorText[WHITE] = "WHITE";
            colorText[GREY] = "GREY";
            colorText[YELLOW] = "YELLOW";
            colorText[RED] = "RED";
            colorText[GREEN] = "GREEN";
            colorText[BLUE] = "BLUE";
            colorText[PINK] = "PINK";

            //Set the active dimension index to the largest size possible
            dimIdx = dims.Length - 1;

            canvas = new Canvas(dims[dimIdx], screenHeight, pxlImg);

            DrawTool = Tool.Box; // set the default drawing tool
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
            // update current and previous mouse states
            prevMouse = mouse;
            mouse = Mouse.GetState();

            // update the canvas if someone starts drawing
            if (!drawActive && mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
            {
                // create a new shape
                canvas.Create(DrawTool, mouse, colors[colorIdx]);

                // set drawActive to true
                drawActive = true;
            }

            // update the canvas if someone stops drawing
            else if (drawActive)
            {
                // update the canvas, and set drawActive to false if the canvas is no longer being drawn
                if (!canvas.Update(DrawTool, mouse, prevMouse))
                {
                    drawActive = false;
                }
            }
            

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
            canvas.Draw(spriteBatch, drawActive);
            //Draw the HUD
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
