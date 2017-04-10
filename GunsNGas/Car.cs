using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using XnafanAPI.Graphics;
using XnafanAPI.ExtensionMethods;
using System.Collections.Generic;

namespace GunsNGas
{
    public class Car : Sprite
    {

        public int MinesLeft { get; set; } = 5;
        public float MachineGunCoolDownInMilliseconds { get; set; } = 200;
        public float MachineGunCoolDownLeftInMilliseconds { get; set; }

        public float MineDispenserCoolDownInMilliseconds { get; set; } = 1000;
        public float MineDispenserCoolDownLeftInMilliseconds { get; internal set; }
        public Vector2 FrontAxleOffset { get; private set; }
        public Vector2 BackAxleOffset { get; private set; }
        public float Acceleration { get; set; }
        public float MaxAcceleration { get; set; }
        public float MinAcceleration { get; set; }
        public float Speed { get; set; }
        public float MaxSpeed { get; set; }
        public float MinSpeed { get; set; }

        public IController Controller { get; set; }

        Vector2 _frontLeftTire = new Vector2(15, -16);
        Vector2 _frontRightTire = new Vector2(15, 16);
        Vector2 _backLeftTire = new Vector2(-16, -17);
        Vector2 _backRightTire = new Vector2(-16, 17);

        private float _frontwheelrotationoffset;

        public Sprite Tire { get; set; }

        public float GetAbsoluteWheelDirection() { return CurrentRotation + _frontwheelrotationoffset; }

        public float FrontWheelRotationOffset
        {
            get { return _frontwheelrotationoffset; }
            set { _frontwheelrotationoffset = (float)MathHelper.Clamp(value, (float)-Math.PI * .08f, (float)Math.PI * .08f); }
        }

        public Car(Texture2D texture, Texture2D tireTexture, Vector2 position, Vector2 frontAxleOffset, Vector2 backAxleOffset, IController controller) : base(texture, position)
        {
            Tire = new Sprite(tireTexture, Vector2.Zero);
            Scale = 1;
            FrontAxleOffset = frontAxleOffset;
            BackAxleOffset = backAxleOffset;
            _frontwheelrotationoffset = .5f;
            MaxSpeed = 8;
            MinSpeed = -4f;
            MaxAcceleration = .5f;
            MinAcceleration = -.3f;
            Controller = controller;
        }
        public void FireMachineGun()
        {
            if(MachineGunCoolDownLeftInMilliseconds <= 0)
            {
                MachineGunCoolDownLeftInMilliseconds = MachineGunCoolDownInMilliseconds;
            }
        }

        public void DispenseMine()
        {
            if (MineDispenserCoolDownLeftInMilliseconds <= 0 && MinesLeft > 0)
            {
                MineDispenserCoolDownLeftInMilliseconds = MineDispenserCoolDownInMilliseconds;
                MinesLeft--;
            }
        }
        public override void Update(GameTime gameTime)
        {
            Acceleration = (float)MathHelper.Clamp(Acceleration, MinAcceleration, MaxAcceleration);
            if (MachineGunCoolDownLeftInMilliseconds > 0){MachineGunCoolDownLeftInMilliseconds -= (float)gameTime.ElapsedGameTime.TotalMilliseconds; }
            if (MineDispenserCoolDownLeftInMilliseconds > 0) { MineDispenserCoolDownLeftInMilliseconds -= (float)gameTime.ElapsedGameTime.TotalMilliseconds; }
            Speed += Acceleration;
            Acceleration *= .95f;
            Speed *= .97f;
           // if (Speed < .2f) { Speed = 0; }
            Speed = (float)MathHelper.Clamp(Speed, MinSpeed, MaxSpeed);
            MoveForward(Speed);
            base.Update(gameTime);
            UpdateCarBasedOnController(gameTime);
        }

        private void UpdateCarBasedOnController(GameTime gameTime)
        {
            float deltaRotation = 0;
            if (Controller.IsTurningLeft) { deltaRotation -= .1f * gameTime.ElapsedGameTime.Milliseconds / 8; }
            if (Controller.IsTurningRight) { deltaRotation += .1f * gameTime.ElapsedGameTime.Milliseconds / 8; }

            FrontWheelRotationOffset = deltaRotation;

            if (Math.Abs(FrontWheelRotationOffset) > .01f) { FrontWheelRotationOffset *= .9f; }
            else { FrontWheelRotationOffset = 0; }

            if (Controller.IsSpeederPressed) { Acceleration += ((float)(gameTime.ElapsedGameTime.TotalMilliseconds / 4)); }
            if (Controller.AreBrakesPressed) { Acceleration += (-(float)(gameTime.ElapsedGameTime.TotalMilliseconds / 5)); }

        }

        private void MoveForward(float amount)
        {
            //amount *= Scale;
            //calculate front axle movement
            Vector2 oldFrontAxleCenter = Position + FrontAxleOffset.GetRotatedCopy(CurrentRotation);
            Vector2 direction = GetAbsoluteWheelDirection().ToVector2();
            direction.Normalize();
            Vector2 newFrontAxleCenter = oldFrontAxleCenter + direction * amount;

            //calculate back axle movement
            Vector2 oldBackAxleCenter = Position + BackAxleOffset.GetRotatedCopy(CurrentRotation);
            direction = CurrentRotation.ToVector2 ();
            direction.Normalize();
            Vector2 newBackAxleCenter = oldBackAxleCenter + direction * amount;
            
            //calculate new centerpoint and rotation
            Vector2 rotation =  newFrontAxleCenter- newBackAxleCenter;
            Vector2 midpoint = (newFrontAxleCenter + newBackAxleCenter)/2;
            rotation.Normalize();
            float newRotation = rotation.ToRadians();
            Position = midpoint;

            CurrentRotation = newRotation;
        }


        public Vector2 DirectionOfHoodOfCar
        {
            get {
                Vector2 frontAxleCenter = Position + FrontAxleOffset.GetRotatedCopy(CurrentRotation);
                Vector2 backAxleCenter = Position + BackAxleOffset.GetRotatedCopy(CurrentRotation);
                var direction = frontAxleCenter - backAxleCenter;
                direction.Normalize();
                return direction;
            }
        }

        

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(Tire.Texture, (_frontLeftTire*Scale).GetRotatedCopy(CurrentRotation) + Position, null, Color.White, GetAbsoluteWheelDirection().ToVector2().ToDrawingRadians(), Tire.HalfSize,Scale, SpriteEffects.None, 0);
            SpriteBatch.Draw(Tire.Texture, (_frontRightTire * Scale).GetRotatedCopy(CurrentRotation) + Position, null, Color.White, GetAbsoluteWheelDirection().ToVector2().ToDrawingRadians(), Tire.HalfSize, Scale, SpriteEffects.None, 0);
            SpriteBatch.Draw(Tire.Texture, (_backLeftTire * Scale).GetRotatedCopy(CurrentRotation) + Position, null, Color.White, CurrentRotation.ToVector2().ToDrawingRadians(), Tire.HalfSize, Scale, SpriteEffects.None, 0);
            SpriteBatch.Draw(Tire.Texture, (_backRightTire * Scale).GetRotatedCopy(CurrentRotation) + Position, null, Color.White, CurrentRotation.ToVector2().ToDrawingRadians(), Tire.HalfSize, Scale, SpriteEffects.None, 0);
            SpriteBatch.Draw(Texture, Position + Vector2.One*5, SourceRectangle, Color.Black * .45f, CurrentRotation.ToVector2().ToDrawingRadians(), TextureSize / 2, Scale, SpriteEffects.None, 0);
            SpriteBatch.Draw(Texture, Position, SourceRectangle, Color * Opacity, CurrentRotation.ToVector2().ToDrawingRadians(), TextureSize / 2, Scale, SpriteEffects.None, 0);
            //SpriteBatch.Draw(Texture, Position, null, Color * Opacity, CurrentRotation., HalfSize, Scale, SpriteEffects.None, 0);
            //base.Draw(gameTime);
            //SpriteBatch.Draw(ExtendedGame.CurrentGame.Content.Load<Texture2D>("greendot"), Position - Vector2.One * 8, Color.White);
        }
    }
}
