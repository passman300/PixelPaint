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
    public class Box : Shape
    {
        public Box(Vector2 origin,  Color color) : base(origin, color)
        {
            Points.Add(origin);
        }

        public override void Update(Vector2 mousePos)
        {
            Points.Clear();

            // add the points on the top and bottom
            for (int i = Math.Min((int)Origin.X, (int)mousePos.X); i <= Math.Max((int)Origin.X, (int)mousePos.X); i++)
            {
                Points.Add(new Vector2(i, Origin.Y));
                Points.Add(new Vector2(i , mousePos.Y));
            }

            // add the points on the left and right
            for (int i = Math.Min((int)Origin.Y, (int)mousePos.Y); i <= Math.Max((int)Origin.Y, (int)mousePos.Y); i++)
            {
                Points.Add(new Vector2(Origin.X, i));
                Points.Add(new Vector2(mousePos.X, i));
            }
        }
    }
}
