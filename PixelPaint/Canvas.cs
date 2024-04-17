using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PixelPaint
{
    internal class Canvas
    {
        // pixel border width
        private int BORDER_WIDTH = 1;

        // pixel color
        private int[,] pixelColor;

        // pixel size
        private int pixelSize;

        // pixel texture
        private Texture2D pixelTexture;

        // shapes
        private ShapeStack undoStack;
        private ShapeStack redoStack;

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
                pixelColor = new int[canvasSize, canvasSize];
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

            pixelColor = new int[canvasSize, canvasSize]; // initialize the pixel color 2d array

            this.pixelTexture = pixelTexture; // set the pixel texture

            // set all pixels to white
            ClearPixels();

            // initialize the shape stack
            undoStack = new ShapeStack();
            redoStack = new ShapeStack();
        }

        public void Undo()
        {
            // remove the last element of the undo stack
            if (undoStack.Count() > 0) redoStack.Add(undoStack.Pop());

            // clear all the pixels
            ClearPixels();

            // re-draw all shapes
            AllShapesToPixels();
        }

        public void Redo()
        {
            if (redoStack.Count() > 0) undoStack.Add(redoStack.Pop());

            ClearPixels();

            AllShapesToPixels();
        }

        /// <summary>
        /// Create a shape
        /// </summary>
        /// <param name="tool"> the tool being used </param>
        /// <param name="mouse"> the current mouse state </param>
        /// <param name="curColor"> the current color selected </param>
        /// <param name="fillColor"> the color of the fill </param>
        public void Create(Game1.Tool tool, MouseState mouse, int curColor)
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
                    CreateFill(mouse, curColor, GetMouseColor(mouse));
                    break;
            }

            Console.WriteLine(undoStack.Count());
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

                    return false;
                    break;
            }

            // check if the mouse is on the canvas and mouse if clicked
            // if so, a new shape is finalized and well be drawn to the canvas and the update function will return false
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed && mouse.Position.X >= 0 && mouse.Position.Y >= 0 && mouse.Position.X < ScreenSize && mouse.Position.Y < ScreenSize)
            {
                // update the pixel color
                TopShapeToPixels();
                
                // clear the redo stack
                redoStack.Clear();

                return false; // return false because the canvas in no longer needs to be updated
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
            undoStack.Top().Update(MousePosToPixelCords(mouse));

            // check if the mouse is on the same x or y as the origin, pop shape
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed && (MousePosToPixelCords(mouse).X == undoStack.Top().Origin.X || MousePosToPixelCords(mouse).Y == undoStack.Top().Origin.Y)) undoStack.Pop();
        }

        /// <summary>
        /// Update the circle position
        /// </summary>
        /// <param name="mouse"></param>
        /// <param name="prevMouse"></param>
        private void UpdateCircle(MouseState mouse, MouseState prevMouse)
        {
            // update the shape based on the location of the mouse
            undoStack.Top().Update(MousePosToPixelCords(mouse));

            // check if the mouse position as the origin, pop shape
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed && mouse.Position.ToVector2() == undoStack.Top().Origin) undoStack.Pop();
        }

        private void UpdateFill()
        {
            // update fill
            undoStack.Top().Update();

            // instantly update the canvas pixels
            TopShapeToPixels();

            // clear the redo stack
            redoStack.Clear();
        }

        /// <summary>
        /// Create a box
        /// </summary>
        /// <param name="mouse"> the current mouse state </param>
        /// <param name="color"> the current color selected </param>
        private void CreateBox(MouseState mouse, int color)
        {
            // add a box to the shape stack
            undoStack.Add(new Box(MousePosToPixelCords(mouse), color));
        }

        /// <summary>
        /// Create a circle
        /// </summary>
        /// <param name="mouse"> the current mouse state </param>
        /// <param name="color"> the current color selected </param>
        private void CreateCircle(MouseState mouse, int color)
        {
            // add a circle to the shape stack
            undoStack.Add(new Circle(MousePosToPixelCords(mouse), color));
        }

        private void CreateFill(MouseState mouse, int fillColor, int clickColor)
        {
            // add a fill to the shape stack
            undoStack.Add(new Fill(MousePosToPixelCords(mouse), fillColor, clickColor, pixelColor));

            // update the fill instantly
            UpdateFill();
        }

        private int GetMouseColor(MouseState mouse)
        {
             return pixelColor[(int)MousePosToPixelCords(mouse).X, (int)MousePosToPixelCords(mouse).Y];
        }

        /// <summary>
        /// Draw the top shapes to the canvas
        /// </summary>
        private void TopShapeToPixels()
        {
            // check if the stack is empty, if so return (nothing to change)
            if (undoStack.Count() == 0) return; 

            // only need to draw the top shape, as the other shapes have already been drawn
            foreach (Vector2 pos in undoStack.Top().GetPoints())
            {
                // check if the pixel is in bounds
                if (pos.X < 0 || pos.X >= pixelColor.GetLength(0) || pos.Y < 0 || pos.Y >= pixelColor.GetLength(1)) continue; // continue if the pixel is out of bounds
                else pixelColor[(int)pos.X, (int)pos.Y] = undoStack.Top().Color;
            }
        }

        /// <summary>
        /// Draw all the shapes to the canvas
        /// </summary>
        private void AllShapesToPixels()
        {
            foreach (Shape shape in undoStack.GetAll())
            {
                foreach (Vector2 pos in shape.GetPoints())
                {
                    // check if the pixel is in bounds
                    if (pos.X < 0 || pos.X >= pixelColor.GetLength(0) || pos.Y < 0 || pos.Y >= pixelColor.GetLength(1)) continue; // continue if the pixel is out of bounds
                    else pixelColor[(int)pos.X, (int)pos.Y] = shape.Color;
                }
            }
        }

        /// <summary>
        /// Clear all the canvas pixels 
        /// </summary>
        private void ClearPixels()
        {
            for (int i = 0; i < pixelColor.GetLength(0); i++)
                for (int j = 0; j < pixelColor.GetLength(1); j++)
                    pixelColor[i, j] = Game1.WHITE;
        }

        /// <summary>
        /// Get the top five shapes of the undo stack
        /// </summary>
        /// <returns></returns>
        public List<string> GetTopFiveUndo() { return undoStack.GetTopFive(); }

        /// <summary>
        /// Get the top five shapes of the redo stack
        /// </summary>
        /// <returns></returns>
        public List<string> GetTopFiveRedo() { return redoStack.GetTopFive(); }

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
                    spriteBatch.Draw(pixelTexture, new Rectangle(i * pixelSize, j * pixelSize, pixelSize, pixelSize), Game1.colors[pixelColor[i, j]]); // draw each canvas pixel
                }
            }

            // check if currently drawing (dirty canvas)
            if (isActive)
            {
                // draw the current shape
                for (int i = 0; i < undoStack.Top().GetPoints().Count; i++)
                {
                    // check if the pixel is in bounds
                    if (undoStack.Top().GetPoint(i).X < 0 || undoStack.Top().GetPoint(i).X >= pixelColor.GetLength(0) || undoStack.Top().GetPoint(i).Y < 0 || undoStack.Top().GetPoint(i).Y >= pixelColor.GetLength(1)) continue; // continue if the pixel is out of bounds

                    // draw the canvas pixel with the shape color
                    spriteBatch.Draw(pixelTexture, new Rectangle((int)undoStack.Top().GetPoint(i).X * pixelSize, (int)undoStack.Top().GetPoint(i).Y * pixelSize, pixelSize, pixelSize), Game1.colors[undoStack.Top().Color]);
                }


                // DEBUG draw the origin
                if (undoStack.Count() < 1) return;
                spriteBatch.Draw(pixelTexture, new Rectangle((int)undoStack.Top().Origin.X * pixelSize, (int)undoStack.Top().Origin.Y * pixelSize, pixelSize, pixelSize), Color.DarkViolet); // draw the origin of the shape
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
        public Vector2 MousePosToPixelCords(MouseState mouse)
        {
            return new Vector2(mouse.X / pixelSize, mouse.Y / pixelSize);
        }
    }
}
