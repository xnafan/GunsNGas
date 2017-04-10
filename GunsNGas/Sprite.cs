//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework;

//namespace DrivingTestMonogameOld
//{
//    public class Sprite : DrawableGameComponent
//    {

//        public SpriteBatch SpriteBatch { get; protected set; }
//        public Color Color { get; set; }
//        private Texture2D _texture;
//        private Vector2 _position;
//        private Rectangle _bounds;
//        public Vector2 Movement { get; set; }
//        public float Opacity { get; set; }
//        private float _scale;
//        public float Rotation { get; set; }
//        public float RotationPerUpdate { get; set; }
//        public Vector2 TextureSize { get; private set; }
//        public Vector2 HalfTextureSize { get; private set; }


//        public Rectangle Bounds
//        {
//            get { return _bounds; }
//            private set { _bounds = value; }
//        }

//        public Vector2 Position
//        {
//            get { return _position; }
//            set { _position = value; Recalculate(); }
//        }

//        public float Scale
//        {
//            get { return _scale; }
//            set
//            {
//                _scale = value;
//                Recalculate();
//            }
//        }

//        public Texture2D Texture
//        {
//            get { return _texture; }
//            set
//            {
//                _texture = value;
//                Recalculate();
//            }
//        }

//        public Sprite(Game game, Texture2D texture, Vector2 position, SpriteBatch batch, float scale = 1, float opacity = 1, float rotation = 0, float rotationPerUpdate = 0)
//            : base(game)
//        {
//            Texture = texture;
//            Position = position;
//            SpriteBatch = batch;
//            Scale = scale;
//            Opacity = opacity;
//            Rotation = rotation;
//            RotationPerUpdate = rotationPerUpdate;
//            Color = Color.White;
//        }

//        public override void Draw(GameTime gameTime)
//        {
//            base.Draw(gameTime);
//            SpriteBatch.Draw(_texture, Position, null, Color * Opacity, Rotation.ToVector2().ToDrawingRadians(), HalfTextureSize, Scale, SpriteEffects.None, 0);
//        }

//        public override void Update(GameTime gameTime)
//        {
//            Position += Movement * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
//            Rotation += RotationPerUpdate * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
//        }

//        protected void Recalculate()
//        {
//            TextureSize = new Vector2(_texture.Width, _texture.Height);
//            HalfTextureSize = TextureSize / 2;
//            _bounds = new Rectangle((int)(Position.X - HalfTextureSize.X * Scale), (int)(Position.Y - HalfTextureSize.Y * Scale), (int)(Texture.Width * Scale), (int)(Texture.Height * Scale));
//        }
//    }

//}
