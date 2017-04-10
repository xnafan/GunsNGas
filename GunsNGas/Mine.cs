using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using XnafanAPI.Graphics;
using Microsoft.Xna.Framework.Graphics;
using XnafanAPI;

namespace GunsNGas
{
    public class Mine : Sprite
    {
        public Mine(Texture2D texture, Vector2 position) : base (texture, position){}

        public override void Draw(GameTime gameTime) 
        {

            ExtendedGame.CurrentGame.SpriteBatch.Draw(Texture, Position - HalfSize + Vector2.One *2, Color.Black * .4f);
            base.Draw(gameTime);
        }
    }
}
