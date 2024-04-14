using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            for (int i = 0; i < Math.Min(Count(), 5); i++)
            {
                topFive.Add(Top().Color.ToString().ToUpper() + " - " + Top().GetType().Name);
            }

            return topFive;
        }

        public List<Shape> GetAll()
        {
            return shapes;
        }


    }
}
