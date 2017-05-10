using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yetibyte.Himalaya.GameElements;

namespace Yetibyte.Himalaya.Controls {

    public class ControlListener : EntityComponent, IUpdate {

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

        public KeyboardState PreviousKeyboardState => _previousKeyboardState;
        public GamePadState PreviousGamePadState => _previousGamePadState;

        public override bool AllowMultiple => true;

        // Constructor

        public ControlListener(PlayerIndex playerIndex, ControlSettings settings) {

            this.PlayerIndex = playerIndex;
            this.Settings = settings;

        }

        // Methods

        public void Update(GameTime gameTime, float globalTimeScale) {

            CurrentKeyboardState = Keyboard.GetState();
            CurrentGamePadState = GamePad.GetState(PlayerIndex);

            foreach (KeyValuePair<string, GameControl> keyValuePair in Settings.ControlMap) {

                GameControl control = keyValuePair.Value;
                string controlName = keyValuePair.Key;

                control.Update(gameTime, CurrentKeyboardState, CurrentGamePadState, Settings.IgnoreKeyboard, Settings.IgnoreGamePad);

                RaiseGameControlEvents(controlName, control);
                
            }

            foreach (GameControlAxis axis in Settings.ControlAxes) {

                axis.Update(CurrentKeyboardState, CurrentGamePadState, Settings.IgnoreKeyboard, Settings.IgnoreGamePad);

            }

            _previousKeyboardState = CurrentKeyboardState;
            _previousGamePadState = CurrentGamePadState;

        }

        public bool GetButtonDown(string controlName) => KnowsControl(controlName) ? Settings.ControlMap[controlName].IsDown : false;
        public bool GetButtonUp(string controlName) => KnowsControl(controlName) ? !Settings.ControlMap[controlName].IsDown : false;
        public bool GetButtonPress(string controlName) => KnowsControl(controlName) ? Settings.ControlMap[controlName].IsPressed : false;
        public bool GetButtonRelease(string controlName) => KnowsControl(controlName) ? Settings.ControlMap[controlName].IsReleased : false;

        public float GetButtonHoldTime(string controlName) => KnowsControl(controlName) ? Settings.ControlMap[controlName].HoldTime : 0f;

        public float GetAxisValue(string controlAxisName) => KnowsControlAxis(controlAxisName) ? Settings.ControlAxesMap[controlAxisName].Value : 0f;

        private void RaiseGameControlEvents(string controlName, GameControl gameControl) {

            GameControlEventArgs args = new GameControlEventArgs(controlName, gameControl);

            if(gameControl.IsDown)
                ButtonDown?.Invoke(this, args);

            if(gameControl.IsPressed)
                ButtonPressed?.Invoke(this, args);

            if (gameControl.IsReleased)
                ButtonReleased?.Invoke(this, args);

        }

        public bool KnowsControl(string controlName) => Settings.ControlMap.ContainsKey(controlName);
        public bool KnowsControlAxis(string controlAxisName) => Settings.ControlAxesMap.ContainsKey(controlAxisName);

    }

}
