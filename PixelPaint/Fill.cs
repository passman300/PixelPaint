using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PixelPaint
{
    internal class Fill : Shape
    {

        private Vector2[] directions = {new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0)};

        private Color clickColor;

        // private bool[,] isVisited;

        private Color[,] pixelColorCopy; // the pixel color array temporary storage

        private int count = 0;

        public Fill(Vector2 origin, Color fillColor, Color clickColor, Color[,] pixelColor) : base(origin, fillColor)
        {
            this.clickColor = clickColor;

            // fill the isVisited array
            pixelColorCopy = pixelColor;

            // isVisited = new bool[pixelColor.GetLength(0), pixelColor.GetLength(1)];
        }

        public override void Update()
        {
            // only fill if the color is different
            if (clickColor == Color) return;

            FloodFill(Origin);

            Console.WriteLine(count + " - " + points.Count());

        }

        private void FloodFill(Vector2 pos)
        {
            //  if (clickColor == Color) return;

            count++;

            pixelColorCopy[(int)pos.X, (int)pos.Y] = Color;

            points.Add(pos);

            for (int i = 0; i < directions.Length; i++)
            {
                if (pos.X + directions[i].X >= 0 
                    && pos.X + directions[i].X < pixelColorCopy.GetLength(0) 
                    && pos.Y + directions[i].Y >= 0 
                    && pos.Y + directions[i].Y < pixelColorCopy.GetLength(1) 
                    && pixelColorCopy[(int)(pos.X + directions[i].X), (int)(pos.Y + directions[i].Y)] == clickColor)
                {
                    FloodFill(pos + directions[i]);
                }
            }

            


            //if (pixelColor[(int)pos.X, (int)pos.Y] == clickColor)
            //{
            //    pixelColor[(int)pos.X, (int)pos.Y] = Color;

            //    Points.Add(pos);

            //    for (int i = 0; i < directions.Length; i++)
            //    {
            //        if (pos.X + directions[i].X >= 0 && pos.X + directions[i].X < pixelColor.GetLength(0) && pos.Y + directions[i].Y >= 0 && pos.Y + directions[i].Y < pixelColor.GetLength(1) && pixelColor[(int)(pos.X + directions[i].X), (int)(pos.Y + directions[i].Y)] == clickColor)
            //        {
            //            FloodFill(pos + directions[i]);
            //        }
            //    }
            //}
        }
    }
}
