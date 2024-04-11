using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace PixelPaint
{
    public class Assets
    {
        public static ContentManager Content { get; set; }

        private static string loadPath; // path to load assets from

        // fonts
        public static SpriteFont uiFont;
        public static SpriteFont stackFont;

        // pixel image
        public static Texture2D pxlImg;

        // colour images (for the palette)
        public static Texture2D blackImg;
        public static Texture2D whiteImg;
        public static Texture2D greyImg;
        public static Texture2D yellowImg;
        public static Texture2D redImg;
        public static Texture2D greenImg;
        public static Texture2D blueImg;
        public static Texture2D pinkImg;

        // hud images
        public static Texture2D squareBtnImg;
        public static Texture2D squareBtnActiveImg;
        public static Texture2D circleBtnImg;
        public static Texture2D circleBtnActiveImg;
        public static Texture2D fillBtnImg;
        public static Texture2D fillBtnActiveImg;
        public static Texture2D undoBtnImg;
        public static Texture2D redoBtnImg;
        public static Texture2D clearBtnImg;
        public static Texture2D checkedBoxImg;
        public static Texture2D uncheckedBoxImg;
        public static Texture2D toBeCheckedBoxImg;



        /// <summary>
        /// method loads all assets to the game
        /// </summary>
        public static void Initialize()
        {
            // load all fonts
            loadPath = "Fonts";

            uiFont = Load<SpriteFont>("UIFont");
            stackFont = Load<SpriteFont>("StackFont");

            // load all images
            loadPath = "Images/Sprites";

            pxlImg = Load<Texture2D>("BlankPixel");

            blackImg = Load<Texture2D>("Black");
            whiteImg = Load<Texture2D>("White");
            greyImg = Load<Texture2D>("Grey");
            yellowImg = Load<Texture2D>("Yellow");
            redImg = Load<Texture2D>("Red");
            greenImg = Load<Texture2D>("Green");
            blueImg = Load<Texture2D>("Blue");
            pinkImg = Load<Texture2D>("Pink");


            squareBtnActiveImg = Load<Texture2D>("SquareActive");
            squareBtnImg = Load<Texture2D>("SquareInactive");
            circleBtnActiveImg = Load<Texture2D>("CircleActive");
            circleBtnImg = Load<Texture2D>("CircleInactive");
            fillBtnActiveImg = Load<Texture2D>("FillActive");
            fillBtnImg = Load<Texture2D>("Fill");
            undoBtnImg = Load<Texture2D>("Undo");
            redoBtnImg = Load<Texture2D>("Redo");
            clearBtnImg = Load<Texture2D>("ClearCanvas");
            checkedBoxImg = Load<Texture2D>("CheckedBox");
            uncheckedBoxImg = Load<Texture2D>("UncheckedBox");
            toBeCheckedBoxImg = Load<Texture2D>("ToBeCheckedBox");
        }

        /// <summary>
        /// method loads an asset
        /// </summary>
        /// <typeparam name="T"></typeparam> the type of the asset
        /// <param name="file"></param> file to load
        /// <returns></returns>
        private static T Load<T>(string file) => Content.Load<T>($"{loadPath}/{file}");
    }
}
