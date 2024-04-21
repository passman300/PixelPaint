using System;
using System.Collections.Generic;
using System.Linq;


namespace PixelPaint
{
    public class ShapeStack
    {

        private List<Shape> shapes;

        public ShapeStack()
        {
            shapes = new List<Shape>();
        }

        public void Add(Shape shape)
        {
            shapes.Add(shape);
        }

        public Shape Pop()
        {
            Shape tempShape = Top();
            shapes.RemoveAt(shapes.Count - 1);

            return tempShape;
        }

        public Shape Top()
        {
            return shapes.Last();
        }

        public int Count()
        {
            return shapes.Count;
        }


        public void Clear()
        {
            shapes.Clear();
        }

        public List<string> GetTopFive()
        {
            List<string> topFive = new List<string>();

            for (int i = Count() - 1; i >= Math.Max(0, Count() - 5); i--)
            {
                topFive.Add(Game1.colorText[shapes[i].Color] + " - " + shapes[i].GetType().Name);
            }

            return topFive;
        }

        public List<Shape> GetAll()
        {
            return shapes;
        }


    }
}
