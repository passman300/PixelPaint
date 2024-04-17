using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace PixelPaint
{
    public class Shape
    {
        
        protected List<Vector2> points = new List<Vector2>(); // <Vector>

        public Vector2 Origin
        {
            get;
            set;
        }

        public int Color
        {
            get;
            set;
        }

        public Shape(Vector2 origin, int color)
        {
            Origin = origin;

            Color = color;
        }

        public virtual void Update(Vector2 mousePos) {}

        public virtual void Update() {}

        public virtual List<Vector2> GetPoints()
        {
            return points;
        }

        public virtual Vector2 GetPoint(int index)
        {
            return points[index];
        }
    }
}
