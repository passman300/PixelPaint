using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;

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

        //Tack number of tool action buttons
        private const int NUM_TOOL_BUTTONS = 3;
        private const int NUM_UNDO_REDO_BUTTONS = 2;
        private const int NUM_CANVAS_SIZE_BUTTONS = 8;
        private const int NUM_COLOR_BUTTONS = 8;

        //Track which index button is
        private const int BOX_BUTTON = 0;
        private const int CIRCLE_BUTTON = 1;
        private const int FILL_BUTTON = 2;
        private const int UNDO_BUTTON = 0;
        private const int REDO_BUTTON = 1;
        private const int SMALL_CANVAS_BUTTON = 0;
        private const int MEDIUM_CANVAS_BUTTON = 1;
        private const int LARGE_CANVAS_BUTTON = 2;
        private const int BLACK_BUTTON = 0;
        private const int WHITE_BUTTON = 1;
        private const int GREY_BUTTON = 2;
        private const int YELLOW_BUTTON = 3;
        private const int RED_BUTTON = 4;
        private const int GREEN_BUTTON = 5;
        private const int BLUE_BUTTON = 6;
        private const int PINK_BUTTON = 7;

        //Track the states of the canvas
        private const byte UNCHECKED = 0;
        private const byte TO_BE_CHECKED = 1;
        private const byte CHECKED = 2;

        //Track Y positions of title tool title 
        private const int TITLE_Y = 820;
        private const int TOOL_TITLE_Y = 820;
        private const int DRAW_ACTION_TITLE_Y = 70;
        private const int ACTION_TITLE_Y = 160;
        private const int UNDO_TITLE_Y = 280;
        private const int CANVAS_SIZE_TITLE_Y = 430;
        private const int COLOR_TITLE_Y = 590;
        private const int COLOR_PICKER_TITLE_Y = 700;

        // title button Y offset
        private const int TITLE_BTN_Y_OFFSET = 10;

        // Track the X position of the undo redo buttons
        private const int UNDO_REDO_BUTTON_X = 930;

        // Track the space between two canvas size buttons
        private const int CANVAS_SIZE_BUTTON_SPACING_X = 7;

        //Track HUD button widths and heights
        private const int LARGE_BUTTON_WIDTH = 90;
        private const int LARGE_BUTTON_HEIGHT = 90;
        private const int MEDIUM_BUTTON_WIDTH = 55;
        private const int MEDIUM_BUTTON_HEIGHT = 55;
        private const int SMALL_BUTTON_WIDTH = 40;
        private const int SMALL_BUTTON_HEIGHT = 40;
        private const int EXTRA_SMALL_BUTTON_WIDTH = 15;
        private const int EXTRA_SMALL_BUTTON_HEIGHT = 15;

        //Track the available colors, one will be active at a time
        public const int BLACK = 0;
        public const int WHITE = 1;
        public const int GREY = 2;
        public const int YELLOW = 3;
        public const int RED = 4;
        public const int GREEN = 5;
        public const int BLUE = 6;
        public const int PINK = 7;

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
        private int hudWidth = 200;

        private MouseState mouse;
        private MouseState prevMouse;

        //Track the canvas object used for all interaction with the Canvas area
        private Canvas canvas;

        //Track the possible NxN grid size dimensions (all even divisors of 800)
        //curDimIdx is the index into the array of the active grid size
        private int[] dims = new int[] { 10, 16, 20, 25, 32, 40, 50, 80};
        private int curDimIdx;
        private int nextcurDimIdx;

        //Track all HUD tool buttons rectangles
        private Rectangle[] toolBtnRecs = new Rectangle[NUM_TOOL_BUTTONS];

        //Track all the undo and redo rectangles
        private Rectangle[] undoRedoBtnRecs = new Rectangle[NUM_UNDO_REDO_BUTTONS];
        private Texture2D[] undoRedoBtnImgs = new Texture2D[NUM_UNDO_REDO_BUTTONS];

        //Track all the canvas size rectangles and if they are selected
        private Rectangle[] canvasSizeBtnRecs = new Rectangle[NUM_CANVAS_SIZE_BUTTONS];
        private byte[] canvasSizeSelected = new byte[NUM_CANVAS_SIZE_BUTTONS];

        private Rectangle clearCanvasBtnRec;

        //Track all of the color data; images, HUD rectangles, actual colors, color text, index of active color
        private Texture2D[] colorImgs = new Texture2D[NUM_COLORS];
        private Rectangle[] colorRecs = new Rectangle[NUM_COLORS];
        private Rectangle colorSelectRec;
        public static readonly Color[] colors = new Color[NUM_COLORS];
        public static readonly string[] colorText = new string[NUM_COLORS];
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

            //Load all undo and redo icons
            undoRedoBtnImgs[UNDO_BUTTON] = Content.Load<Texture2D>("Images/Sprites/Undo");
            undoRedoBtnImgs[REDO_BUTTON] = Content.Load<Texture2D>("Images/Sprites/Redo");

            //Load in all HUD buttons
            squareBtnActiveImg = Content.Load<Texture2D>("Images/Sprites/SquareActive");
            squareBtnImg = Content.Load<Texture2D>("Images/Sprites/SquareInactive");
            circleBtnActiveImg = Content.Load<Texture2D>("Images/Sprites/CircleActive");
            circleBtnImg = Content.Load<Texture2D>("Images/Sprites/CircleInactive");
            fillBtnActiveImg = Content.Load<Texture2D>("Images/Sprites/FillActive");
            fillBtnImg = Content.Load<Texture2D>("Images/Sprites/Fill");
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
            curDimIdx = dims.Length - 1;

            // Set the active canvas size
            canvasSizeSelected[curDimIdx] = CHECKED;

            // Create the canvas
            canvas = new Canvas(dims[curDimIdx], screenHeight, pxlImg);

            DrawTool = Tool.Box; // set the default drawing tool
            
            // Setup the tool buttons recs
            toolBtnRecs = new Rectangle[NUM_TOOL_BUTTONS];
            toolBtnRecs = CenterHudRectangles(toolBtnRecs, DRAW_ACTION_TITLE_Y + TITLE_BTN_Y_OFFSET, MEDIUM_BUTTON_WIDTH, MEDIUM_BUTTON_HEIGHT, 0.5f);

            // Setup the undo and redo buttons
            undoRedoBtnRecs[UNDO_BUTTON] = new Rectangle(UNDO_REDO_BUTTON_X, ACTION_TITLE_Y + TITLE_BTN_Y_OFFSET, MEDIUM_BUTTON_WIDTH, MEDIUM_BUTTON_HEIGHT);
            undoRedoBtnRecs[REDO_BUTTON] = new Rectangle(UNDO_REDO_BUTTON_X, UNDO_TITLE_Y + TITLE_BTN_Y_OFFSET, MEDIUM_BUTTON_WIDTH, MEDIUM_BUTTON_HEIGHT);

            // Set up canvas size buttons
            canvasSizeBtnRecs = CenterHudRectangles(canvasSizeBtnRecs, CANVAS_SIZE_TITLE_Y + TITLE_BTN_Y_OFFSET, EXTRA_SMALL_BUTTON_WIDTH, EXTRA_SMALL_BUTTON_HEIGHT, 1, CANVAS_SIZE_BUTTON_SPACING_X);

            // Set up the clear canvas button
            clearCanvasBtnRec = CenterHudRectangle(clearCanvasBtnRec, canvasSizeBtnRecs[0].Bottom + TITLE_BTN_Y_OFFSET, LARGE_BUTTON_WIDTH, LARGE_BUTTON_HEIGHT);

            // Setup the color buttons rectangle
            colorRecs = CenterHudRectangles(colorRecs, COLOR_TITLE_Y + TITLE_BTN_Y_OFFSET, SMALL_BUTTON_WIDTH, SMALL_BUTTON_HEIGHT, 2, 0.5f);
            colorSelectRec = CenterHudRectangle(colorSelectRec, COLOR_PICKER_TITLE_Y, LARGE_BUTTON_WIDTH, LARGE_BUTTON_HEIGHT);
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

            // check if not drawing
            if (!drawActive)
            {
                // check if the mouse is clicks
                if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                {
                    // check if the mouse is on the canvas
                    if (mouse.X >= 0 && mouse.Y >= 0 && mouse.X < canvas.ScreenSize && mouse.Y < canvas.ScreenSize)
                    {
                        // create a new shape
                        canvas.Create(DrawTool, mouse, colorIdx);

                        // set drawActive to true
                        drawActive = true;
                    }
                    else
                    {
                        UpdateToolButtons();

                        UpdateUndoRedoButtons();

                        UpdateCanvasSizeButtons();

                        UpdateColorPalette();
                    }

                }
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


        private void UpdateToolButtons()
        {
            for (int i = 0; i < NUM_TOOL_BUTTONS; i++)
            {
                if (toolBtnRecs[i].Contains(mouse.X, mouse.Y)) DrawTool = (Tool)i;
            }
        }

        private void UpdateUndoRedoButtons()
        {
            if (undoRedoBtnRecs[UNDO_BUTTON].Contains(mouse.X, mouse.Y)) canvas.Undo();
            else if (undoRedoBtnRecs[REDO_BUTTON].Contains(mouse.X, mouse.Y)) canvas.Redo();
        }

        private void UpdateCanvasSizeButtons()
        {
            for (int i = 0; i < NUM_CANVAS_SIZE_BUTTONS; i++)
            {
                if (canvasSizeBtnRecs[i].Contains(mouse.X, mouse.Y))
                {
                    if (i != curDimIdx)
                    {
                        if (nextcurDimIdx != i)
                        {
                            canvasSizeSelected[nextcurDimIdx] = (curDimIdx == nextcurDimIdx) ? CHECKED : UNCHECKED;

                            nextcurDimIdx = i;

                            canvasSizeSelected[i] = TO_BE_CHECKED;
                        }
                        else
                        {
                            nextcurDimIdx = curDimIdx;

                            canvasSizeSelected[i] = UNCHECKED;
                        }
                    }
                    else
                    {
                        canvasSizeSelected[nextcurDimIdx] = (curDimIdx == nextcurDimIdx) ? CHECKED : UNCHECKED;

                        nextcurDimIdx = curDimIdx;
                    }
                }
            }
        }

        private void UpdateClearCanvasButton()
        {
            if (clearCanvasBtnRec.Contains(mouse.X, mouse.Y)) 
            {
                canvas = new Canvas(dims[curDimIdx], screenHeight, pxlImg);
            }

        }

        private void UpdateColorPalette()
        {
            for (int i = 0; i < NUM_COLOR_BUTTONS; i++)
            {
                if (colorRecs[i].Contains(mouse.X, mouse.Y)) colorIdx = i;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            // Draw the Canvas
            canvas.Draw(spriteBatch, drawActive);

            //Draw the HUD
            DrawHud();

            // draw mouse position DEBUG
            DrawMousePos();
            spriteBatch.End();

            base.Draw(gameTime);
        }


        private void DrawHud()
        {
            DrawTools();

            DrawUndoRedo();

            DrawCanvasSizeSelector();

            DrawClear();

            DrawColorPalette();
        }

        private void DrawTools()
        {
            // Draw the color tool
            spriteBatch.Draw((DrawTool == Tool.Box) ? squareBtnActiveImg : squareBtnImg, toolBtnRecs[BOX_BUTTON], Color.White);
            spriteBatch.Draw((DrawTool == Tool.Circle) ? circleBtnActiveImg : circleBtnImg, toolBtnRecs[CIRCLE_BUTTON], Color.White);
            spriteBatch.Draw((DrawTool == Tool.Fill) ? fillBtnActiveImg : fillBtnImg, toolBtnRecs[FILL_BUTTON], Color.White);
        }

        private void DrawUndoRedo()
        {
            // Draw the undo and redo buttons
            spriteBatch.Draw(undoRedoBtnImgs[UNDO_BUTTON], undoRedoBtnRecs[UNDO_BUTTON], Color.White);
            spriteBatch.Draw(undoRedoBtnImgs[REDO_BUTTON], undoRedoBtnRecs[REDO_BUTTON], Color.White);

            // Draw the top five undo actions
            for (int i = 0; i < canvas.GetTopFiveUndo().Count; i++)
            {
                spriteBatch.DrawString(stackFont, canvas.GetTopFiveUndo()[i], new Vector2(TOOL_TITLE_Y, ACTION_TITLE_Y + TITLE_BTN_Y_OFFSET + (i * stackFont.MeasureString(canvas.GetTopFiveUndo()[i]).Y)), Color.White);
            }

            // Draw the top five redo actions
            for (int i = 0; i < canvas.GetTopFiveRedo().Count; i++)
            {
                spriteBatch.DrawString(stackFont, canvas.GetTopFiveRedo()[i], new Vector2(TOOL_TITLE_Y, UNDO_TITLE_Y + TITLE_BTN_Y_OFFSET + (i * stackFont.MeasureString(canvas.GetTopFiveRedo()[i]).Y)), Color.White);
            }
        }

        private void DrawCanvasSizeSelector()
        {
            // draw the canvas size buttons
            for (int i = 0; i < NUM_CANVAS_SIZE_BUTTONS; i++)
            {
                switch (canvasSizeSelected[i])
                {
                    case UNCHECKED:
                        spriteBatch.Draw(uncheckedBoxImg, canvasSizeBtnRecs[i], Color.White);
                        break;
                    case TO_BE_CHECKED:
                        spriteBatch.Draw(toBeCheckedBoxImg, canvasSizeBtnRecs[i], Color.White);
                        break;
                    case CHECKED:
                        spriteBatch.Draw(checkedBoxImg, canvasSizeBtnRecs[i], Color.White);
                        break;
                }
            }
        }

        private void DrawClear()
        {
            // Draw the clear button
            spriteBatch.Draw(clearBtnImg, clearCanvasBtnRec, Color.White);
        }

        private void DrawColorPalette()
        {
            // Draw the color buttons
            for (int i = 0; i < colorRecs.Length; i++)
            {
                spriteBatch.Draw(colorImgs[i], colorRecs[i], Color.White);
            }

            // Draw the color picker
            spriteBatch.Draw(colorImgs[colorIdx], colorSelectRec, Color.White);
        }

        /// <summary>
        /// Center a set of rectangles horizontally, given the number of rectangles
        /// </summary>
        /// <param name="recs"></param>
        /// <param name="posY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private Rectangle[] CenterHudRectangles(Rectangle[] recs, int posY, int width, int height, float offset = 0.5f)
        {
            // start from the left
            Vector2 startPos = new Vector2(screenWidth - (hudWidth * offset) - ((width * recs.Length) * offset), posY);

            for (int i = 0; i < recs.Length; i++)
            {
                recs[i] = new Rectangle((int)startPos.X + i * width, (int)startPos.Y, width, height);
            }

            return recs;
        }

        /// <summary>
        /// Center a set of rectangles horizontally, given the number of rectangles, and how many rows
        /// </summary>
        /// <param name="recs"></param>
        /// <param name="posY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="rows"> number of rows to be centered </param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private Rectangle[] CenterHudRectangles(Rectangle[] recs, int posY, int width, int height, int rows = 1, float offset = 0.5f)
        {
            // start from the left
            Vector2 startPos = new Vector2(screenWidth - (hudWidth * offset) - ((width * recs.Length / rows) * offset), posY);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < recs.Length / rows; j++)
                {
                    recs[i * recs.Length / rows + j] = new Rectangle((int)startPos.X + j * width, (int)startPos.Y + i * height, width, height);
                }
            }

            return recs;
        }

        private Rectangle[] CenterHudRectangles(Rectangle[] recs, int posY, int width, int height, int rows, int buttonSpacing, float offset = 0.5f)
        {
            Vector2 startPos = new Vector2(screenWidth - (hudWidth * offset) - (width * recs.Length / rows) * offset - ((buttonSpacing * (recs.Length / rows - 1)) / rows) * offset, posY);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < recs.Length / rows; j++)
                {
                    recs[i * recs.Length / rows + j] = new Rectangle((int)startPos.X + j * width + buttonSpacing * j, (int)startPos.Y + i * height, width, height);
                }
            }

            return recs;
        }


        private Rectangle CenterHudRectangle(Rectangle rec, int posY, int width, int height, float offset = 0.5f)
        {
            return new Rectangle((int)(screenWidth - (hudWidth * offset) - (width * offset)), posY, width, height);
        }

        private void DrawMousePos()
        {
            // Draw the mouse position
            spriteBatch.DrawString(stackFont, $"Mouse Position: {mouse.X}, {mouse.Y}", new Vector2(10, 10), Color.Black);
        }
    }
}
