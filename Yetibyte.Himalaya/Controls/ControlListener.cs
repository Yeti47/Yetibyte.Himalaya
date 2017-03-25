using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Yetibyte.Himalaya.Controls {

    public class ControlListener {

        // Fields
        
        private KeyboardState _previousKeyboardState;
        private GamePadState _previousGamePadState;

        // Events

        public event EventHandler<GameControlEventArgs> ButtonDown;
        public event EventHandler<GameControlEventArgs> ButtonPressed;
        public event EventHandler<GameControlEventArgs> ButtonReleased;

        // Properties

        public PlayerIndex PlayerIndex { get; private set; }
        public ControlSettings Settings { get; set; }
        public KeyboardState CurrentKeyboardState { get; private set; }
        public GamePadState CurrentGamePadState { get; private set; }

        public KeyboardState PreviousKeyboardState { get { return _previousKeyboardState; } }
        public GamePadState PreviousGamePadState { get { return _previousGamePadState; } }

        // Constructor

        public ControlListener(PlayerIndex playerIndex, ControlSettings settings) {

            this.PlayerIndex = playerIndex;
            this.Settings = settings;

        }

        // Methods

        public void Update(GameTime gameTime) {

            CurrentKeyboardState = Keyboard.GetState();
            CurrentGamePadState = GamePad.GetState(PlayerIndex);

            foreach (KeyValuePair<string, GameControl> keyValuePair in Settings.ControlMap) {

                GameControl control = keyValuePair.Value;
                string controlName = keyValuePair.Key;

                Keys currentKey = control.Key;
                Buttons currentButton = control.Button;
                Keys currentAlternativeKey = control.AlternativeKey;
                Buttons currentAlternativeButton = control.AlternativeButton;
                GameControlAxes currentAxis = control.Axis;

                if(control.IsDown) {

                    control.HoldTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (control.DoRepeat && (control.HoldTime >= control.RepeatInterval))
                        control.ResetState();

                }
                else {

                    control.HoldTime = 0f;

                }

                // Axes

                switch (currentAxis) {

                    case GameControlAxes.None:

                        control.AxisValue = 0f;
                        break;

                    case GameControlAxes.LeftThumbstick:

                        if (control.AxisDirection == ControlAxisDirection.Horizontal)
                            control.AxisValue = CurrentGamePadState.ThumbSticks.Left.X;
                        else
                            control.AxisValue = CurrentGamePadState.ThumbSticks.Left.Y;

                        break;

                    case GameControlAxes.RightThumbstick:

                        if (control.AxisDirection == ControlAxisDirection.Horizontal)
                            control.AxisValue = CurrentGamePadState.ThumbSticks.Right.X;
                        else
                            control.AxisValue = CurrentGamePadState.ThumbSticks.Right.Y;

                        break;

                    case GameControlAxes.LeftTrigger:

                        control.AxisValue = CurrentGamePadState.Triggers.Left;

                        break;

                    case GameControlAxes.RightTrigger:

                        control.AxisValue = CurrentGamePadState.Triggers.Right;

                        break;

                    default:
                        break;

                }

                bool axisRegistered = currentAxis != GameControlAxes.None && Math.Abs(control.AxisValue) >= control.AxisDeadzone;

                // ####

                bool isKeyDown = !Settings.IgnoreKeyboard && ((currentKey != 0 && CurrentKeyboardState.IsKeyDown(currentKey)) || (currentAlternativeKey != 0 && CurrentKeyboardState.IsKeyDown(currentAlternativeKey)));
                bool isButtonDown = !Settings.IgnoreGamePad && (CurrentGamePadState.IsConnected && ((currentButton != 0 && CurrentGamePadState.IsButtonDown(currentButton)) || (currentAlternativeButton != 0 && CurrentGamePadState.IsButtonDown(currentAlternativeButton))));

                control.IsDown = isKeyDown || isButtonDown || axisRegistered;
                control.IsPressed = control.IsDown && !control.WasDown;
                control.IsReleased = !control.IsDown && control.WasDown;
                control.WasDown = control.IsDown;

                RaiseGameControlEvents(controlName, control);
                
            }

            _previousKeyboardState = CurrentKeyboardState;
            _previousGamePadState = CurrentGamePadState;

        }

        public bool GetButtonDown(string controlName) => Settings.ControlMap[controlName].IsDown;

        public bool GetButtonUp(string controlName) => !Settings.ControlMap[controlName].IsDown;

        public bool GetButtonPress(string controlName) => Settings.ControlMap[controlName].IsPressed;

        public bool GetButtonRelease(string controlName) => Settings.ControlMap[controlName].IsReleased;

        public float GetButtonHoldTime(string controlName) => Settings.ControlMap[controlName].HoldTime;

        public float GetAxisValue(string controlName) {

            GameControl gameControl = Settings.ControlMap[controlName];

            return Math.Abs(gameControl.AxisValue) >= gameControl.AxisDeadzone ? gameControl.AxisValue : 0f;

        }

        private void RaiseGameControlEvents(string controlName, GameControl gameControl) {

            GameControlEventArgs args = new GameControlEventArgs(controlName, gameControl);

            if(gameControl.IsDown)
                ButtonDown?.Invoke(this, args);

            if(gameControl.IsPressed)
                ButtonPressed?.Invoke(this, args);

            if (gameControl.IsReleased)
                ButtonReleased?.Invoke(this, args);

        }

    }

}
