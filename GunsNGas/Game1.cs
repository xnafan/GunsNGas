using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using XnafanAPI;
using XnafanAPI.Graphics;

namespace GunsNGas
{
    public class Game1 : ExtendedGame
    {
        SpriteFont _defaultFont;
        List<Car> _cars;
        Track _track;
        Texture2D _shotTexture, _mineTexture;
        List<Sprite> _shots, _mines;

        void NewGame()
        {

            Texture2D carTexture = Content.Load<Texture2D>("cars");
            Texture2D tireTexture = Content.Load<Texture2D>("tire");
            _shotTexture = Content.Load<Texture2D>("shot");
            _mineTexture = Content.Load<Texture2D>("mine");

            _cars = new List<Car>();
            _shots = new List<Sprite>();
            _mines = new List<Sprite>();

            var controller1 = new KeyboardCarController()
            {
                SpeederKey = Keys.Up,
                BrakesKey = Keys.Down,
                LeftKey = Keys.Left,
                RightKey = Keys.Right,
                FireKey = Keys.Space,
                NitroKey = Keys.RightControl,
                MineKey = Keys.Enter
            };

            var controller2 = new KeyboardCarController()
            {
                SpeederKey = Keys.W,
                BrakesKey = Keys.S,
                LeftKey = Keys.A,
                RightKey = Keys.D,
                FireKey = Keys.LeftControl,
                NitroKey = Keys.LeftShift,
                MineKey = Keys.Tab
            };

            Car car1 = new Car(carTexture, tireTexture, new Vector2(850, 70), new Vector2(16, 0), new Vector2(-16, 0), controller1) { SourceRectangle = new Rectangle(32 * 0, 0, 32, 56) };
            _cars.Add(car1);
            Car car2 = new Car(carTexture, tireTexture, new Vector2(850, 160), new Vector2(16, 0), new Vector2(-16, 0), controller2) { SourceRectangle = new Rectangle(32 * 1, 0, 32, 56) };
            _cars.Add(car2);
            _defaultFont = Content.Load<SpriteFont>("DefaultFont");
            _track = new Track();
        }
        public Game1()
        {
            GraphicsDeviceManager.PreferredBackBufferHeight = 1080;
            GraphicsDeviceManager.PreferredBackBufferWidth = 1920;
            Content.RootDirectory = "Content";
            GraphicsDeviceManager.IsFullScreen = true;
        }


        protected override void LoadContent()
        {
            base.LoadContent();
            NewGame();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _shots.ForEach(shot => shot.Update(gameTime));
            _cars.ForEach(shot => shot.Update(gameTime));
            foreach (var car in _cars)
            {
                
                ManipulateCarBasedOnEnvironment(gameTime, car);

                if (car.Controller.IsFiring)
                {
                    CreateShotForCarIfReady(car);
                }
                if (car.Controller.IsDroppingMine)
                {
                    CreateMineForCarIfReady(car);
                }
            }
            DoShotCollisions();
            RemoveDeadShots();
            DoMineCollisions();

            if (Input.CurrentKeyboardState.IsKeyDown(Keys.Escape)) { Exit(); }
            if (Input.WasJustPressed(Keys.F11)) { GraphicsDeviceManager.ToggleFullScreen(); }
            if (Input.WasJustPressed(Keys.F5)) { NewGame(); }
        }

        private void DoShotCollisions()
        {
            for (int i = _shots.Count - 1; i >= 0; i--)
            {
                foreach (var car in _cars)
                {
                    if (Vector2.DistanceSquared(_shots[i].Position, car.Position) < 256)
                    {
                        _shots.RemoveAt(i);
                        break;
                    }
                }
            }

        }
            private void DoMineCollisions()
            {
                for (int i = _mines.Count - 1; i >= 0; i--)
                {
                    foreach (var car in _cars)
                    {
                        if (Vector2.DistanceSquared(_mines[i].Position, car.Position) < 256)
                        {
                            _mines.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            private void RemoveDeadShots()
        {
            for (int i = _shots.Count - 1; i >= 0; i--)
            {
                if (!ScreenBounds.Contains(_shots[i].Position))
                {
                    _shots.RemoveAt(i);
                }
            }
        }

        private void CreateShotForCarIfReady(Car car)
        {
            if (car.MachineGunCoolDownLeftInMilliseconds <= 0)
            {
                var shotPosition = car.Position + car.DirectionOfHoodOfCar * 30;
                var newShot = new Sprite(_shotTexture, shotPosition) { MovementPerUpdate = car.DirectionOfHoodOfCar * .8f };
                _shots.Add(newShot);
                car.FireMachineGun();
            }
        }

        private void CreateMineForCarIfReady(Car car)
        {
            if (car.MineDispenserCoolDownLeftInMilliseconds<= 0 && car.MinesLeft > 0)
            {
                var minePosition = car.Position - car.DirectionOfHoodOfCar * 30;
                var newMine = new Mine(_mineTexture, minePosition) { CurrentRotation = ExtendedGame.RandomBetween(0,10)};
                _mines.Add(newMine);
                car.DispenseMine();
            }
        }

        private void ManipulateCarBasedOnEnvironment(GameTime gameTime, Car car)
        {
            if ((_track.GetSurfaceAt(car.Position.ToPoint()) == TrackMaskColors.Grass)){car.Speed *= .9f; car.Acceleration *= .9f; }
        }

        
        protected override void Draw(GameTime gameTime)
        {
            Vector2 textPos = new Vector2(25, 0);
            Vector2 lineOffset = Vector2.UnitY * 20;
            GraphicsDevice.Clear(Color.CornflowerBlue);
            SpriteBatch.Begin();
            //SpriteBatch.DrawString(_defaultFont, "Front wheel relative rotation: " + _car.FrontWheelRotationOffset, textPos, Color.White);
            //textPos += lineOffset;
            //SpriteBatch.DrawString(_defaultFont, "Car position: " + _car.Position, textPos, Color.White);
            //textPos += lineOffset;
            //SpriteBatch.DrawString(_defaultFont, "Car rotation: " + _car.CurrentRotation, textPos, Color.White);
            //textPos += lineOffset;
            //SpriteBatch.DrawString(_defaultFont, "Front wheel absolute position: " + _car.GetAbsoluteWheelDirection(), textPos, Color.White);
            _track.Draw();
            _shots.ForEach(shot => shot.Draw(gameTime));
            _mines.ForEach(mine => mine.Draw(gameTime));
            _cars.ForEach(shot => shot.Draw(gameTime));
            DrawableComponents.ForEach(component => component.Draw(gameTime));
            SpriteBatch.End();
        }
    }


    public static class HelperStuff
    {
        public static Point ToPoint(this Vector2 vect) { return new Point((int)vect.X, (int)vect.Y); }
        public static Vector2 ToVector2(this float angleInRadians)
        {
            return new Vector2((float)Math.Cos(angleInRadians), (float)Math.Sin(angleInRadians));
        }
        public static float ToRadians(this Vector2 angleVector) { return (float)Math.Atan2(angleVector.Y, angleVector.X ); }
        public static float ToDrawingRadians(this Vector2 angleVector) {return (float)Math.Atan2(angleVector.X, -angleVector.Y);}
        public static Vector2 GetRotatedCopy(this Vector2 originVector, float rotation) { return Vector2.Transform(originVector, Matrix.CreateRotationZ(rotation)); }
    }
}
