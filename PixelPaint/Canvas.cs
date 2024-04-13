using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PixelPaint
{
    internal class Canvas
    {
        // pixel border width
        private int BORDER_WIDTH = 1;

        // pixel color
        private Color[,] pixelColor;

        // pixel size
        private int pixelSize;

        // pixel texture
        private Texture2D pixelTexture;

        // shapes
        private ShapeStack shapeStack;

        // canvas size
        private int canvasSize;

        /// <summary>
        /// The size of the canvas in terms of canvas pixels, (i.e. 32x32)
        /// </summary>
        public int CanvasSize
        {
            get { return canvasSize; }
            set
            {
                canvasSize = value;

                pixelSize = ScreenSize / canvasSize;
                pixelColor = new Color[canvasSize, canvasSize];
            }
        }

        /// <summary>
        /// The size of the screen in terms of display pixels, (i.e. 1920x1080)
        /// </summary>
        public int ScreenSize { get; set; }

        /// <summary>
        /// Constructor of the canvas
        /// </summary>
        /// <param name="canvasSize"> the size of the canvas in terms of canvas pixels, (i.e. 32x32) </param>
        /// <param name="screenSize"> the size of the screen in terms of display pixels, (i.e. 1920x1080) </param>
        /// <param name="pixelTexture"> the pixel texture </param>
        public Canvas(int canvasSize, int screenSize, Texture2D pixelTexture)
        {
            CanvasSize = canvasSize; // in terms of canvas pixels

            ScreenSize = screenSize; // in terms of display pixels

            pixelSize = screenSize / canvasSize; // size of each canvas pixel

            pixelColor = new Color[canvasSize, canvasSize]; // initialize the pixel color 2d array

            this.pixelTexture = pixelTexture; // set the pixel texture

            // set all pixels to white
            ClearPixels();

            // initialize the shape stack
            shapeStack = new ShapeStack();
        }

        /// <summary>
        /// Create a shape
        /// </summary>
        /// <param name="tool"> the tool being used </param>
        /// <param name="mouse"> the current mouse state </param>
        /// <param name="curColor"> the current color selected </param>
        public void Create(Game1.Tool tool, MouseState mouse, Color curColor)
        {
            switch (tool)
            {
                case Game1.Tool.Box:
                    CreateBox(mouse, curColor);
                    break;
                case Game1.Tool.Circle:
                    CreateCircle(mouse, curColor);
                    break;
                case Game1.Tool.Fill:
                    break;
            }
        }

        /// <summary>
        /// Update the canvas
        /// </summary>
        /// <param name="tool"> the tool being used </param>
        /// <param name="mouse"> the current mouse state </param>
        /// <param name="prevMouse"> the previous mouse state </param>
        /// <returns> returns if the canvas still needs to be updated, false if the canvas is finished, true if the canvas needs to be updated </returns>
        public bool Update(Game1.Tool tool, MouseState mouse, MouseState prevMouse)
        {
            // check if the mouse is on the canvas and mouse if clicked
            // if so, a new shape is finalized and well be drawn to the canvas and the update function will return false
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed && mouse.Position.X >= 0 && mouse.Position.Y >= 0 && mouse.Position.X < ScreenSize && mouse.Position.Y < ScreenSize)
            {
                // update the pixel color
                AllShapeToPixels();

                return false; // return false because the canvas in no longer needs to be updated
            }

            // update the drawing tool
            switch (tool)
            {
                case Game1.Tool.Box:
                    // only update the box if the mouse is on the canvas
                    if (mouse.Position.X >= 0 && mouse.Position.Y >= 0 && mouse.Position.X < ScreenSize && mouse.Position.Y < ScreenSize) UpdateBox(mouse, prevMouse);
                    break;
                case Game1.Tool.Circle:
                    // update the circle if the mouse is on window screen
                    UpdateCircle(mouse, prevMouse);
                    break;
                case Game1.Tool.Fill:
                    break;
            }

            return true; // return true because the canvas needs to be updated
        }

        /// <summary>
        /// Update the box position
        /// </summary>
        /// <param name="mouse"></param>
        /// <param name="prevMouse"></param>
        private void UpdateBox(MouseState mouse, MouseState prevMouse)
        {
            // update the location of the mouse to the top shape, in terms of canvas pixels
            shapeStack.Top().Update(MousePosToPixelCoord(mouse));

            // check if the mouse is on the same x or y as the origin, pop shape
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed && (MousePosToPixelCoord(mouse).X == shapeStack.Top().Origin.X || MousePosToPixelCoord(mouse).Y == shapeStack.Top().Origin.Y)) shapeStack.Pop();
        }

        /// <summary>
        /// Update the circle position
        /// </summary>
        /// <param name="mouse"></param>
        /// <param name="prevMouse"></param>
        private void UpdateCircle(MouseState mouse, MouseState prevMouse)
        {
            // update the location of the mouse to the top shape, in terms of canvas pixels
            shapeStack.Top().Update(MousePosToPixelCoord(mouse));

            // check if the mouse position as the origin, pop shape
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed && mouse.Position.ToVector2() == shapeStack.Top().Origin) shapeStack.Pop();
        }

        /// <summary>
        /// Create a box
        /// </summary>
        /// <param name="mouse"> the current mouse state </param>
        /// <param name="color"> the current color selected </param>
        private void CreateBox(MouseState mouse, Color color)
        {
            // add a box to the shape stack
            shapeStack.Add(new Box(MousePosToPixelCoord(mouse), color));
        }

        /// <summary>
        /// Create a circle
        /// </summary>
        /// <param name="mouse"> the current mouse state </param>
        /// <param name="color"> the current color selected </param>
        private void CreateCircle(MouseState mouse, Color color)
        {
            // add a circle to the shape stack
            shapeStack.Add(new Circle(MousePosToPixelCoord(mouse), color));
        }

        /// <summary>
        /// Draw all the shapes to the canvas
        /// </summary>
        private void AllShapeToPixels()
        {
            // check if the stack is empty, if so return (nothing to change)
            if (shapeStack.Count() == 0) return; 

            // only need to draw the top shape, as the other shapes have already been drawn
            foreach (Vector2 pos in shapeStack.Top().Points)
            {
                // check if the pixel is in bounds
                if (pos.X < 0 || pos.X >= pixelColor.GetLength(0) || pos.Y < 0 || pos.Y >= pixelColor.GetLength(1)) continue; // continue if the pixel is out of bounds
                else pixelColor[(int)pos.X, (int)pos.Y] = shapeStack.Top().Color;
            }
        }

        /// <summary>
        /// Clear all the canvas pixels 
        /// </summary>
        private void ClearPixels()
        {
            for (int i = 0; i < pixelColor.GetLength(0); i++)
                for (int j = 0; j < pixelColor.GetLength(1); j++)
                    pixelColor[i, j] = Color.White;
        }

        /// <summary>
        /// Draw the canvas
        /// </summary>
        /// <param name="spriteBatch"> the spritebatch to draw to </param>
        /// <param name="isActive"> check if the canvas is actively being updated or is use (dirty canvas) </param>
        public void Draw(SpriteBatch spriteBatch, bool isActive)
        {
            // draw the pixels
            for (int i = 0; i < pixelColor.GetLength(0); i++)
            {
                for (int j = 0; j < pixelColor.GetLength(1); j++)
                {
                    spriteBatch.Draw(pixelTexture, new Rectangle(i * pixelSize, j * pixelSize, pixelSize, pixelSize), pixelColor[i, j]); // draw each canvas pixel

                }
            }

            // check if currently drawing (dirty canvas)
            if (isActive)
            {
                // draw the current shape
                for (int i = 0; i < shapeStack.Top().Points.Count; i++)
                {
                    // check if the pixel is in bounds
                    if (shapeStack.Top().Points[i].X < 0 || shapeStack.Top().Points[i].X >= pixelColor.GetLength(0) || shapeStack.Top().Points[i].Y < 0 || shapeStack.Top().Points[i].Y >= pixelColor.GetLength(1)) continue; // continue if the pixel is out of bounds

                    // draw the canvas pixel with the shape color
                    spriteBatch.Draw(pixelTexture, new Rectangle((int)shapeStack.Top().Points[i].X * pixelSize, (int)shapeStack.Top().Points[i].Y * pixelSize, pixelSize, pixelSize), shapeStack.Top().Color);
                }


                // DEBUG draw the origin
                if (shapeStack.Count() < 1) return;
                spriteBatch.Draw(pixelTexture, new Rectangle((int)shapeStack.Top().Origin.X * pixelSize, (int)shapeStack.Top().Origin.Y * pixelSize, pixelSize, pixelSize), Color.Red); // draw the origin of the shape
            }

            // draw the grid
            for (int i = 0; i <= pixelColor.GetLength(0); i++)
            {
                // draw horizontal lines
                spriteBatch.Draw(pixelTexture, new Rectangle(0, i * pixelSize, ScreenSize, BORDER_WIDTH), Color.LightGray);

                // draw vertical lines
                spriteBatch.Draw(pixelTexture, new Rectangle(i * pixelSize, 0, BORDER_WIDTH, ScreenSize), Color.LightGray);
            }
        }

        /// <summary>
        /// Converts mouse position to canvas pixel coordinate
        /// </summary>
        /// <param name="mouse"></param>
        /// <returns></returns>
        public Vector2 MousePosToPixelCoord(MouseState mouse)
        {
            return new Vector2(mouse.X / pixelSize, mouse.Y / pixelSize);
        }
    }
}
