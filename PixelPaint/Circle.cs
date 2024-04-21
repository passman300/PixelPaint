using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace PixelPaint
{
    public class Circle : Shape
    {
        private const float ANGLE_STEP = 0.01f;

        private HashSet<Point> pointsSet = new HashSet<Point>();

        public float Radius { get; set; }

        public Circle(Point origin, byte color) : base(origin, color) { Radius = 0; }

        public override void Update(Point mousePos)
        {
            points.Clear();

            pointsSet.Clear();

            Radius = (float)Math.Sqrt(Math.Pow(mousePos.X - Origin.X, 2) + Math.Pow(mousePos.Y - Origin.Y, 2));

            for (float i = 0; i < Math.PI * 2; i += ANGLE_STEP)
            {
                pointsSet.Add(new Point((int)Math.Round((float)Math.Cos(i) * Radius) + Origin.X, (int)Math.Round((float)Math.Sin(i) * Radius) + Origin.Y));
            } 

            points = new List<Point>(pointsSet);
        }
    }
}
