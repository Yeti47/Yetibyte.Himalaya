using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Yetibyte.Himalaya.Controls {

    public class ControlsManager {

        // Fields
        
        private KeyboardState _previousKeyboardState;
        private GamePadState _previousGamePadState;

        // Properties

        public PlayerIndex PlayerIndex { get; private set; }
        public ControlSettings Settings { get; set; }
        public KeyboardState CurrentKeyboardState { get; private set; }
        public GamePadState CurrentGamePadState { get; private set; }

        public KeyboardState PreviousKeyboardState { get { return _previousKeyboardState; } }
        public GamePadState PreviousGamePadState { get { return _previousGamePadState; } }

        // Constructor

        public ControlsManager(PlayerIndex playerIndex, ControlSettings settings) {

            this.PlayerIndex = playerIndex;

        }

        // Methods

        public void Update(GameTime gameTime) {

            CurrentKeyboardState = Keyboard.GetState();
            CurrentGamePadState = GamePad.GetState(PlayerIndex);

            foreach (GameControl control in Settings.Controls) {

                Keys currentKey = control.Key;
                Buttons currentButton = control.Button;
                Keys currentAlternativeKey = control.AlternativeKey;
                Buttons currentAlternativeButton = control.AlternativeButton;

                if(control.IsDown) {

                    control.HoldTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (control.HoldTime >= control.RepeatInterval)
                        control.ResetState();

                }
                else {

                    control.HoldTime = 0f;

                }

                bool isKeyDown = CurrentKeyboardState.IsKeyDown(currentKey) || CurrentKeyboardState.IsKeyDown(currentAlternativeKey);
                bool isButtonDown = CurrentGamePadState.IsConnected && (CurrentGamePadState.IsButtonDown(currentButton) || CurrentGamePadState.IsButtonDown(currentAlternativeButton));

                control.IsDown = isKeyDown || isButtonDown;
                control.IsPressed = control.IsDown && !control.WasDown;
                control.IsReleased = !control.IsDown && control.WasDown;
                control.WasDown = control.IsDown;


            }

            _previousKeyboardState = CurrentKeyboardState;
            _previousGamePadState = CurrentGamePadState;

        }

        public bool GetButtonDown(string controlName) {

            return Settings.ControlMap[controlName].IsDown;

        }

        public bool GetButtonUp(string controlName) {

            return !Settings.ControlMap[controlName].IsDown;

        }

        public bool GetButtonPress(string controlName) {

            return Settings.ControlMap[controlName].IsPressed;

        }

        public bool GetButtonRelease(string controlName) {

            return Settings.ControlMap[controlName].IsReleased;

        }

    }

}
