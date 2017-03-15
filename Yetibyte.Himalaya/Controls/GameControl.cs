using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Yetibyte.Himalaya.Controls {

    public class GameControl {

        // Properties

        public Keys Key { get; set; }
        public Buttons Button { get; set; }
        public Keys AlternativeKey { get; set; }
        public Buttons AlternativeButton { get; set; }

        public float HoldTime { get; set; }
        public bool DoRepeat { get; set; }
        public float RepeatInterval { get; set; }
        public bool IsDown { get; set; }
        public bool WasDown { get; set; }
        public bool IsPressed { get; set; }
        public bool IsReleased { get; set; }

        // Constructor

        public GameControl(bool doRepeat = false, float repeatInterval = 2f) {
            
        }

        // Methods

        public void ResetState() {

            IsDown = false;
            WasDown = false;
            IsPressed = false;
            IsReleased = false;
            HoldTime = 0f;
            
        }

    }

}
