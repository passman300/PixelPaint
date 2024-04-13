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

        public Fill(Vector2 origin, Color fillColor, Color clickColor) : base(origin, fillColor)
        {
            this.clickColor = clickColor;
        }

        public override void Update(Color[,] pixelColor)
        {
            FloodFill(Origin, pixelColor);
        }

        private void FloodFill(Vector2 pos, Color[,] pixelColor)
        {
            if (pos.X < 0 || pos.X >= pixelColor.GetLength(0) || pos.Y < 0 || pos.Y >= pixelColor.GetLength(1)) return;

            if (pixelColor[(int)pos.X, (int)pos.Y] != clickColor) return;

            pixelColor[(int)pos.X, (int)pos.Y] = Color;

            Points.Add(pos);

            foreach (Vector2 dir in directions)
            {
                FloodFill(pos + dir, pixelColor);
            }
        }
    }
}
