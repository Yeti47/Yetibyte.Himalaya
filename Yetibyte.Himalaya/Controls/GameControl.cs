using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Yetibyte.Himalaya.Controls {

    public class GameControl : ICloneable {

        public const float DEFAULT_REPEAT_INTERVAL = 2f;

        // Properties

        public Keys Key { get; set; }
        public Buttons Button { get; set; }
        public Keys AlternativeKey { get; set; }
        public Buttons AlternativeButton { get; set; }
        public bool DoRepeat { get; set; }
        public float RepeatInterval { get; set; }

        public float HoldTime { get; set; }
        public bool IsDown { get; set; }
        public bool WasDown { get; set; }
        public bool IsPressed { get; set; }
        public bool IsReleased { get; set; }

        // Constructor

        public GameControl(bool doRepeat = false, float repeatInterval = DEFAULT_REPEAT_INTERVAL) {

            this.DoRepeat = doRepeat;
            this.RepeatInterval = repeatInterval;

        }

        // Methods

        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, bool ignoreKeyboard, bool ignoreGamePad) {

            if (IsDown)
            {

                HoldTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (DoRepeat && (HoldTime >= RepeatInterval))
                    ResetState();

            }
            else
            {

                HoldTime = 0f;

            }

            bool isKeyDown = !ignoreKeyboard && ((Key != 0 && keyboardState.IsKeyDown(Key)) || (AlternativeKey != 0 && keyboardState.IsKeyDown(AlternativeKey)));
            bool isButtonDown = !ignoreGamePad && (gamePadState.IsConnected && ((Button != 0 && gamePadState.IsButtonDown(Button)) || (AlternativeButton != 0 && gamePadState.IsButtonDown(AlternativeButton))));

            IsDown = isKeyDown || isButtonDown;
            IsPressed = IsDown && !WasDown;
            IsReleased = !IsDown && WasDown;
            WasDown = IsDown;

        }

        public void ResetState() {

            IsDown = false;
            WasDown = false;
            IsPressed = false;
            IsReleased = false;
            HoldTime = 0f;
            
        }

        public object Clone() {

            GameControl clone = new GameControl();

            clone.Key = this.Key;
            clone.Button = this.Button;
            clone.AlternativeKey = this.AlternativeKey;
            clone.AlternativeButton = this.AlternativeButton;
            clone.DoRepeat = this.DoRepeat;
            clone.RepeatInterval = this.RepeatInterval;

            return clone;

        }

    }

}
