using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnafanAPI;

namespace GunsNGas
{
    public class KeyboardCarController : IController
    {
        public Keys SpeederKey { get; set; }
        public Keys BrakesKey { get; set; }
        public Keys LeftKey { get; set; }
        public Keys RightKey { get; set; }
        public Keys FireKey { get; set; }
        public Keys MineKey { get; set; }
        public Keys NitroKey { get; set; }
        public bool IsSpeederPressed => Input.CurrentKeyboardState.IsKeyDown(SpeederKey);
        public bool AreBrakesPressed => Input.CurrentKeyboardState.IsKeyDown(BrakesKey);
        public bool IsTurningRight => Input.CurrentKeyboardState.IsKeyDown(RightKey);
        public bool IsTurningLeft => Input.CurrentKeyboardState.IsKeyDown(LeftKey);
        public bool IsFiring => Input.CurrentKeyboardState.IsKeyDown(FireKey);
        public bool IsDroppingMine => Input.CurrentKeyboardState.IsKeyDown(MineKey);
        public bool IsFiringNitro => Input.CurrentKeyboardState.IsKeyDown(NitroKey);
    }
}
