using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPaint
{
    public class Shape
    {
        public List<Vector2> Points
        {
            get;
            set;
        }

        public Vector2 Origin
        {
            get;
            set;
        }

        public Color Color
        {
            get;
            set;
        }

        public Shape(Vector2 origin, Color color)
        {
            Origin = origin;

            Color = color;

            Points = new List<Vector2>();
        }

        public virtual void Update(Vector2 mousePos) {}

        public virtual void Update(Color[,] pixelColor) {}

        public virtual void GetPixels()
        {
            
        }
    }
}
