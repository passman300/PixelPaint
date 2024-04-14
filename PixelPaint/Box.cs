using Microsoft.Xna.Framework;
using System;

namespace PixelPaint
{
    public class Box : Shape
    {
        public Box(Vector2 origin,  Color color) : base(origin, color)
        {
            points.Add(origin);
        }

        public override void Update(Vector2 mousePos)
        {
            points.Clear();

            // add the points on the top and bottom
            for (int i = Math.Min((int)Origin.X, (int)mousePos.X); i <= Math.Max((int)Origin.X, (int)mousePos.X); i++)
            {
                points.Add(new Vector2(i, Origin.Y));
                points.Add(new Vector2(i , mousePos.Y));
            }

            // add the points on the left and right
            for (int i = Math.Min((int)Origin.Y, (int)mousePos.Y); i <= Math.Max((int)Origin.Y, (int)mousePos.Y); i++)
            {
                points.Add(new Vector2(Origin.X, i));
                points.Add(new Vector2(mousePos.X, i));
            }
        }
    }
}
