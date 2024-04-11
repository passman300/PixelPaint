using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PixelPaint
{
    internal class Canvas
    {
        //Track the three types of draw actions, one will be active at a time
        public enum DrawType
        {
            Sqaure = 0,
            Circle = 1,
            Fill = 2
        }

        public enum DrawState
        {
            Inactive = 0,
            Active = 1
        }

        // Canvas pixels
        Pixel[,] pixels;

        public DrawType DrawAction
        {
            get; set;
        }

        public DrawState Curr
        {
            get; set;
        }

        public int CanvasSize 
        {
            get { return CanvasSize; }
            set { pixels = new Pixel[CanvasSize, CanvasSize]; }
        }


        public Canvas(int canvasSize)
        {
            CanvasSize = canvasSize;

            pixels = new Pixel[canvasSize, canvasSize];
        }

        public void Update(MouseState mouse, MouseState prevMouse)
        {
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                switch (DrawAction)
                {
                    case DrawType.Sqaure:
                        break;
                    case DrawType.Circle:
                        break;
                    case DrawType.Fill:
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Pixel pixel in pixels)
            {
                pixel.Draw(spriteBatch);
            }
        }
    }
}
