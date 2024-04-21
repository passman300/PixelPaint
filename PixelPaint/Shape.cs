using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace PixelPaint
{
    public class Shape
    {
        protected List<Point> points = new List<Point>(); // <Vector>

        public Point Origin { get; set; }

        public byte Color { get; set; }

        public Shape(Point origin, byte color)
        {
            Origin = origin;

            Color = color;
        }

        public virtual void Update(Point mousePos) { }

        public virtual void Update() { }

        public virtual List<Point> GetPoints()
        {
            return points;
        }

        public virtual Point GetPoint(int index)
        {
            return points[index];
        }
    }
}
