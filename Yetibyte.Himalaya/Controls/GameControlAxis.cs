using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Yetibyte.Himalaya.Controls {

    public class GameControlAxis : ICloneable {

        // Properties

        public AxisDirection Direction { get; set; } = AxisDirection.Horizontal;

        public Keys PositiveKey { get; set; }
        public Keys NegativeKey { get; set; }
        public Keys AlternativePositiveKey { get; set; }
        public Keys AlternativeNegativeKey { get; set; }

        public Buttons PositiveButton { get; set; }
        public Buttons NegativeButton { get; set; }
        public Buttons AlternativePositiveButton { get; set; }
        public Buttons AlternativeNegativeButton { get; set; }

        public GamePadAxes GamePadAxis { get; set; } = GamePadAxes.None;

        public float Value { get; private set; }
        public float Deadzone { get; set; } = 0.15f;

        // Methods

        public void Update(KeyboardState keyboardState, GamePadState gamePadState, bool ignoreKeyboard, bool ignoreGamePad) {

            bool isPosKeyDown = !ignoreKeyboard && ((PositiveKey != 0 && keyboardState.IsKeyDown(PositiveKey)) || (AlternativePositiveKey != 0 && keyboardState.IsKeyDown(AlternativePositiveKey)));
            bool isNegKeyDown = !ignoreKeyboard && ((NegativeKey != 0 && keyboardState.IsKeyDown(NegativeKey)) || (AlternativeNegativeKey != 0 && keyboardState.IsKeyDown(AlternativeNegativeKey)));

            bool isPosButtonDown = !ignoreGamePad && (gamePadState.IsConnected && ((PositiveButton != 0 && gamePadState.IsButtonDown(PositiveButton)) || (AlternativePositiveButton != 0 && gamePadState.IsButtonDown(AlternativePositiveButton))));
            bool isNegButtonDown = !ignoreGamePad && (gamePadState.IsConnected && ((NegativeButton != 0 && gamePadState.IsButtonDown(NegativeButton)) || (AlternativeNegativeButton != 0 && gamePadState.IsButtonDown(AlternativeNegativeButton))));

            float keyButtonValue = 0f;

            if (isPosKeyDown || isPosButtonDown)
                keyButtonValue += 1f;

            if (isNegKeyDown || isNegButtonDown)
                keyButtonValue -= 1f;

            float gamepadAxisValue = 0f;

            if(!ignoreGamePad) {

                switch (GamePadAxis)
                {
                    case GamePadAxes.None:
                        break;
                    case GamePadAxes.LeftThumbstick:

                        if (Direction == AxisDirection.Horizontal)
                            gamepadAxisValue = gamePadState.ThumbSticks.Left.X;
                        else
                            gamepadAxisValue = gamePadState.ThumbSticks.Left.Y;

                        break;

                    case GamePadAxes.RightThumbstick:

                        if (Direction == AxisDirection.Horizontal)
                            gamepadAxisValue = gamePadState.ThumbSticks.Right.X;
                        else
                            gamepadAxisValue = gamePadState.ThumbSticks.Right.Y;

                        break;

                    case GamePadAxes.LeftTrigger:

                        gamepadAxisValue = gamePadState.Triggers.Left;

                        break;

                    case GamePadAxes.RightTrigger:

                        gamepadAxisValue = gamePadState.Triggers.Right;

                        break;

                    default:
                        break;
                }

            }

            float potentialValue = Math.Abs(keyButtonValue) > Math.Abs(gamepadAxisValue) ? keyButtonValue : gamepadAxisValue;

            Value = Math.Abs(potentialValue) >= Deadzone ? potentialValue : 0f;

        }

        public object Clone()
        {

            GameControlAxis clone = new GameControlAxis();

            clone.PositiveKey = this.PositiveKey;
            clone.PositiveButton = this.PositiveButton;
            clone.NegativeKey = this.NegativeKey;
            clone.NegativeButton = this.NegativeButton;

            clone.AlternativePositiveKey = this.AlternativePositiveKey;
            clone.AlternativeNegativeKey = this.AlternativeNegativeKey;
            clone.AlternativePositiveButton = this.AlternativePositiveButton;
            clone.AlternativeNegativeButton = this.AlternativeNegativeButton;

            return clone;

        }

    }

}
