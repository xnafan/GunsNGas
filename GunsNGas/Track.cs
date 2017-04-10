using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnafanAPI;

namespace GunsNGas
{
    public class Track
    {
        public Texture2D  Mask { get; set; }
        public Texture2D Surface { get; set; }
        public Color[,] MaskPixels { get; private set; }

        public Track()
        {
            Mask = ExtendedGame.CurrentGame.Content.Load<Texture2D>("Track_03_mask");
            Surface = ExtendedGame.CurrentGame.Content.Load<Texture2D>("Track_03");
            MaskPixels = TextureTo2DArray(Mask);
        }

        public void Draw()
        {
            //ExtendedGame.CurrentGame.SpriteBatch.Draw(Mask, Vector2.Zero, Color.White);
            ExtendedGame.CurrentGame.SpriteBatch.Draw(Surface, Vector2.Zero, Color.White);
        }

        public Color GetSurfaceAt(Point positionToTest)
        {
            if (positionToTest.X >= 0 && positionToTest.X < MaskPixels.GetUpperBound(0) && positionToTest.Y >= 0 && positionToTest.Y < MaskPixels.GetUpperBound(1))
            {
                return MaskPixels[positionToTest.X, positionToTest.Y];
            }
            else
            {
                return Color.Transparent;
            }
        }

        private Color[,] TextureTo2DArray(Texture2D texture)
        {
            Color[] colorsOne = new Color[texture.Width * texture.Height]; //The hard to read,1D array
            texture.GetData(colorsOne); //Get the colors and add them to the array

            Color[,] colorsTwo = new Color[texture.Width, texture.Height]; //The new, easy to read 2D array
            for (int x = 0; x < texture.Width; x++) //Convert!
                for (int y = 0; y < texture.Height; y++)
                    colorsTwo[x, y] = colorsOne[x + y * texture.Width];

            return colorsTwo; //Done!
        }

    }
}
