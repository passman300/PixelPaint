using Microsoft.Xna.Framework;
using System;

namespace PixelPaint
{
    public class Box : Shape
    {
        public Box(Point origin, byte color) : base(origin, color)
        {
            points.Add(origin);
        }

        public override void Update(Point mousePos)
        {
            points.Clear();

            // add the points on the top and bottom
            for (int i = Math.Min((int)Origin.X, (int)mousePos.X); i <= Math.Max((int)Origin.X, (int)mousePos.X); i++)
            {
                points.Add(new Point(i, Origin.Y));
                points.Add(new Point(i , mousePos.Y));
            }

            // add the points on the left and right
            for (int i = Math.Min((int)Origin.Y, (int)mousePos.Y); i <= Math.Max((int)Origin.Y, (int)mousePos.Y); i++)
            {
                points.Add(new Point(Origin.X, i));
                points.Add(new Point(mousePos.X, i));
            }
        }
    }
}
