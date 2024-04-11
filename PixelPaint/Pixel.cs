using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPaint
{
    internal class Pixel
    {
        private const int BORDER_WIDTH = 2;

        private Color borderColorFill = Color.White; // if pixel is filled (colored)
        private Color borderColorBlank = Color.Black; // if pixel is blank

        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle { get; set; }
        public Color Color { get; set; }

        /// <summary>
        /// Creates a new pixel
        /// </summary>
        /// <param name="width"> width of the pixel </param>
        /// <param name="height"> height of the pixel </param>
        /// <param name="x"> x position of the pixel </param>
        /// <param name="y"> y position of the pixel </param>
        /// <param name="color"> color of the pixel </param>
        public Pixel(int width, int height, int x, int y, Color color)
        {
            // save width and height
            Width = width;
            Height = height;

            // save x and y
            X = x;
            Y = y;

            // save position and rectangle
            Position = new Vector2(x, y);
            Rectangle = new Rectangle(x, y, width, height);

            // save color
            Color = color;
        }

        /// <summary>
        /// Draws the pixel
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw border
            if (Color == Color.White) DrawBorder(spriteBatch, Color.Black);
            else DrawBorder(spriteBatch, Color.White);

            // draw the pixel
            spriteBatch.Draw(Assets.pxlImg, Rectangle, Color.Black);
        }

        private void DrawBorder(SpriteBatch spriteBatch, Color borderColor, int lineWidth = BORDER_WIDTH)
        {
            spriteBatch.Draw(Assets.pxlImg, new Rectangle(Rectangle.X, Rectangle.Y, lineWidth, Rectangle.Height + lineWidth), borderColor);
            spriteBatch.Draw(Assets.pxlImg, new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width + lineWidth, lineWidth), borderColor);
            spriteBatch.Draw(Assets.pxlImg, new Rectangle(Rectangle.X + Rectangle.Width, Rectangle.Y, lineWidth, Rectangle.Height + lineWidth), borderColor);
            spriteBatch.Draw(Assets.pxlImg, new Rectangle(Rectangle.X, Rectangle.Y + Rectangle.Height, Rectangle.Width + lineWidth, lineWidth), borderColor);
        }

        /// <summary>
        /// Checks if the mouse is over the pixel
        /// </summary>
        /// <param name="mousePosition"> the position of the mouse </param>
        /// <returns> true if the mouse is over the pixel, false if not </returns>
        public bool IsMouseOver(Vector2 mousePosition)
        {
            return Rectangle.Contains((int)mousePosition.X, (int)mousePosition.Y);
        }
    }
}
