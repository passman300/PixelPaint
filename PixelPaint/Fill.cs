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

        private Color[,] pixelColor; // the pixel color array temporary storage

        int count = 0;

        public Fill(Vector2 origin, Color fillColor, Color clickColor) : base(origin, fillColor)
        {
            this.clickColor = clickColor;
        }

        public override void Update(Color[,] pixelColor)
        {
            // only fill if the color is different
            if (clickColor == Color) return;

            this.pixelColor = pixelColor;


            FloodFill(Origin);

        }

        private void FloodFill(Vector2 pos)
        {
            //  if (clickColor == Color) return;

            count++;

            Console.WriteLine(count + " - " + Points.Count());

            pixelColor[(int)pos.X, (int)pos.Y] = Color;

            Points.Add(pos);

            for (int i = 0; i < directions.Length; i++)
            {
                if (pos.X + directions[i].X >= 0 
                    && pos.X + directions[i].X < pixelColor.GetLength(0) 
                    && pos.Y + directions[i].Y >= 0 
                    && pos.Y + directions[i].Y < pixelColor.GetLength(1) 
                    && pixelColor[(int)(pos.X + directions[i].X), (int)(pos.Y + directions[i].Y)] == clickColor)
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
