using Microsoft.Xna.Framework;

namespace PixelPaint
{
    internal class Fill : Shape
    {
        private int clickColor;

        private int[,] pixelColorCopy; // the pixel color array temporary storage

        public Fill(Vector2 origin, int fillColor, int clickColor, int[,] pixelColor) : base(origin, fillColor)
        {
            this.clickColor = clickColor;

            // fill the isVisited array
            pixelColorCopy = pixelColor;
        }

        public override void Update()
        {
            // only fill if the color is different
            if (clickColor == Color) return; // return if the color is the same since it's already filled

            FloodFill((int)Origin.X, (int)Origin.Y);
        }

        private void FloodFill(int x, int y)
        {
            if (x < 0 || x >= pixelColorCopy.GetLength(0) || y < 0 || y >= pixelColorCopy.GetLength(1)) return; // return if the pixel is out of bounds
            if (pixelColorCopy[x, y] != clickColor) return; // return if the pixel is not the color to fill

            points.Add(new Vector2(x, y));
            pixelColorCopy[x, y] = Color;

            FloodFill(x - 1, y);
            FloodFill(x + 1, y);
            FloodFill(x, y - 1);
            FloodFill(x, y + 1);
        }
    }
}
