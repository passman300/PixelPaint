using Microsoft.Xna.Framework;
using System;

namespace PixelPaint
{
    public class Circle : Shape
    {
        private const float ANGLE_STEP = 0.01f;

        public float Radius { get; set; }

        public Circle(Vector2 origin, Color color) : base(origin, color) { Radius = 0; }

        public override void Update(Vector2 mousePos)
        {
            points.Clear();

            Radius = Vector2.Distance(Origin, mousePos);

            // draw the circle points based on the radius, and the origin position

            for (float i = 0; i < Math.PI * 2; i += ANGLE_STEP)
            {
                points.Add(new Vector2((int)Math.Round((float)Math.Cos(i) * Radius) + Origin.X, (int)Math.Round((float)Math.Sin(i) * Radius) + Origin.Y));
            }
        }
    }
}
