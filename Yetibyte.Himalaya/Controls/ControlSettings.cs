using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Yetibyte.Himalaya.Controls {

    public class ControlSettings {

        // Properties

        public Dictionary<string, GameControl> ControlMap { get; private set; } = new Dictionary<string, GameControl>();
        public Dictionary<string, GameControl>.ValueCollection Controls => ControlMap.Values;
        public Dictionary<string, GameControl>.KeyCollection ControlNames => ControlMap.Keys;

        public Dictionary<string, GameControlAxis> ControlAxesMap { get; private set; } = new Dictionary<string, GameControlAxis>();
        public Dictionary<string, GameControlAxis>.ValueCollection ControlAxes => ControlAxesMap.Values;
        public Dictionary<string, GameControlAxis>.KeyCollection ControlAxesNames => ControlAxesMap.Keys;

        public bool IgnoreKeyboard { get; set; } = false;
        public bool IgnoreGamePad { get; set; } = false;

        // Constructor

        public ControlSettings(ControlSettings copy) {

            Copy(copy);

        }

        public ControlSettings(bool ignoreKeyboard = false, bool ignoreGamePad = false) {

            this.IgnoreKeyboard = ignoreKeyboard;
            this.IgnoreGamePad = ignoreGamePad;

        }

        // Methods

        public void Copy(ControlSettings original) {

            ControlMap.Clear();

            if (original != null) {

                foreach (KeyValuePair<string, GameControl> controlKeyValuePair in original.ControlMap) {

                    ControlMap.Add(controlKeyValuePair.Key, (GameControl)controlKeyValuePair.Value.Clone());

                }

                foreach (KeyValuePair<string, GameControlAxis> controlAxisKeyValuePair in original.ControlAxesMap) {

                    ControlAxesMap.Add(controlAxisKeyValuePair.Key, (GameControlAxis)controlAxisKeyValuePair.Value.Clone());

                }

                IgnoreKeyboard = original.IgnoreKeyboard;
                IgnoreGamePad = original.IgnoreGamePad;

            }
            
        }

        /// <summary>
        /// Adds a new <see cref="GameControl"/> with the given name that responds to the provided set of keys and buttons.
        /// </summary>
        /// <param name="name">The name of the new GameControl. Used for identifying and referencing this GameControl later.</param>
        /// <param name="key">The key on the keyboard this GameControl responds to. Pass zero for no key at all.</param>
        /// <param name="button">The gamepad button this GameControl responds to. Pass zero for no button at all..</param>
        /// <param name="alternativeKey">An alternative key on the keyboard this GameControl responds to. Pass zero for no key at all.</param>
        /// <param name="alternativeButton">An alternative gamepad button this GameControl responds to. Pass zero for no key at all.</param>
        /// <param name="doRepeat">Should the control be triggered repeatedly in a certain interval if the key/button is held down?</param>
        /// <param name="repeatInterval">The time in seconds after which this control should be retriggered if 'doRepeat' is set to true.</param>
        /// <returns>The newly created <see cref="GameControl"/>.</returns>
        public GameControl RegisterControl(string name, Keys key, Buttons button, Keys alternativeKey , Buttons alternativeButton, bool doRepeat = false, float repeatInterval = GameControl.DEFAULT_REPEAT_INTERVAL) {

            if (String.IsNullOrEmpty(name))
                throw new Exception("A control's name cannot be blank. Please pass a valid name.");

            if(ControlMap.ContainsKey(name))
                throw new Exception("There is already a control named '" + name + "' registered in this instance of ControlSettings.");

            if(key == 0 && button == 0 && alternativeKey == 0 && alternativeButton == 0)
                throw new Exception("Please assign at least one key or button to the new control.");

            GameControl gameControl = new GameControl(doRepeat, repeatInterval) { Key = key, Button = button, AlternativeKey = alternativeKey, AlternativeButton = alternativeButton };
            ControlMap.Add(name, gameControl);

            return gameControl;
            
        }

        /// <summary>
        /// Adds a new <see cref="GameControl"/> with the given name that responds to the provided set of keys and buttons.
        /// </summary>
        /// <param name="name">The name of the new GameControl. Used for identifying and referencing this GameControl later.</param>
        /// <param name="key">The key on the keyboard this GameControl responds to. Pass zero for no key at all.</param>
        /// <param name="alternativeKey">An alternative key on the keyboard this GameControl responds to. Pass zero for no key at all.</param>
        /// <param name="doRepeat">Should the control be triggered repeatedly in a certain interval if the key/button is held down?</param>
        /// <param name="repeatInterval">The time in seconds after which this control should be retriggered if 'doRepeat' is set to true.</param>
        /// <returns>The newly created <see cref="GameControl"/>.</returns>
        public GameControl RegisterControl(string name, Keys key, Keys alternativeKey = 0, bool doRepeat = false, float repeatInterval = GameControl.DEFAULT_REPEAT_INTERVAL) {

            return RegisterControl(name, key, 0, alternativeKey, 0, doRepeat, repeatInterval);
            
        }

        /// <summary>
        /// Adds a new <see cref="GameControl"/> with the given name that responds to the provided set of keys and buttons.
        /// </summary>
        /// <param name="name">The name of the new GameControl. Used for identifying and referencing this GameControl later.</param>
        /// <param name="button">The gamepad button this GameControl responds to. Pass zero for no button at all..</param>
        /// <param name="alternativeButton">An alternative gamepad button this GameControl responds to. Pass zero for no key at all.</param>
        /// <param name="doRepeat">Should the control be triggered repeatedly in a certain interval if the key/button is held down?</param>
        /// <param name="repeatInterval">The time in seconds after which this control should be retriggered if 'doRepeat' is set to true.</param>
        /// <returns>The newly created <see cref="GameControl"/>.</returns>
        public GameControl RegisterControl(string name, Buttons button, Buttons alternativeButton = 0, bool doRepeat = false, float repeatInterval = GameControl.DEFAULT_REPEAT_INTERVAL) {

            return RegisterControl(name, 0, button, 0, alternativeButton, doRepeat, repeatInterval);

        }

        /// <summary>
        /// Adds a new <see cref="GameControl"/> with the given name that responds to the provided set of keys and buttons.
        /// </summary>
        /// <param name="name">The name of the new GameControl. Used for identifying and referencing this GameControl later.</param>
        /// <param name="key">The key on the keyboard this GameControl responds to. Pass zero for no key at all.</param>
        /// <param name="button">The gamepad button this GameControl responds to. Pass zero for no button at all..</param>
        /// <param name="doRepeat">Should the control be triggered repeatedly in a certain interval if the key/button is held down?</param>
        /// <param name="repeatInterval">The time in seconds after which this control should be retriggered if 'doRepeat' is set to true.</param>
        /// <returns>The newly created <see cref="GameControl"/>.</returns>
        public GameControl RegisterControl(string name, Keys key, Buttons button, bool doRepeat = false, float repeatInterval = GameControl.DEFAULT_REPEAT_INTERVAL) {

            return RegisterControl(name, key, button, 0, 0, doRepeat, repeatInterval);

        }

        /// <summary>
        /// Adds a new <see cref="GameControl"/> with the given name that responds to the provided set of keys and buttons.
        /// </summary>
        /// <param name="name">The name of the new GameControl. Used for identifying and referencing this GameControl later.</param>
        /// <param name="key">The key on the keyboard this GameControl responds to. Pass zero for no key at all.</param>
        /// <param name="button">The gamepad button this GameControl responds to. Pass zero for no button at all..</param>
        /// <param name="alternativeKey">An alternative key on the keyboard this GameControl responds to. Pass zero for no key at all.</param>
        /// <param name="doRepeat">Should the control be triggered repeatedly in a certain interval if the key/button is held down?</param>
        /// <param name="repeatInterval">The time in seconds after which this control should be retriggered if 'doRepeat' is set to true.</param>
        /// <returns>The newly created <see cref="GameControl"/>.</returns>
        public GameControl RegisterControl(string name, Keys key, Buttons button, Keys alternativeKey, bool doRepeat = false, float repeatInterval = GameControl.DEFAULT_REPEAT_INTERVAL) {

            return RegisterControl(name, key, button, alternativeKey, 0, doRepeat, repeatInterval);

        }

        /// <summary>
        /// Adds a new <see cref="GameControl"/> with the given name that responds to the provided set of keys and buttons.
        /// </summary>
        /// <param name="name">The name of the new GameControl. Used for identifying and referencing this GameControl later.</param>
        /// <param name="key">The key on the keyboard this GameControl responds to. Pass zero for no key at all.</param>
        /// <param name="button">The gamepad button this GameControl responds to. Pass zero for no button at all..</param>
        /// <param name="alternativeButton">An alternative gamepad button this GameControl responds to. Pass zero for no key at all.</param>
        /// <param name="doRepeat">Should the control be triggered repeatedly in a certain interval if the key/button is held down?</param>
        /// <param name="repeatInterval">The time in seconds after which this control should be retriggered if 'doRepeat' is set to true.</param>
        /// <returns>The newly created <see cref="GameControl"/>.</returns>
        public GameControl RegisterControl(string name, Keys key, Buttons button, Buttons alternativeButton, bool doRepeat = false, float repeatInterval = GameControl.DEFAULT_REPEAT_INTERVAL) {

            return RegisterControl(name, key, button, 0, alternativeButton, doRepeat, repeatInterval);

        }

        /// <summary>
        /// Adds a new <see cref="GameControlAxis"/> with the given name that responds to the provided set of keys and buttons or a gamepad axis (e.g. a thumbstick).
        /// </summary>
        /// <param name="name">The name of the new GameControlAxis. Used for identifying and referencing this GameControlAxis later.</param>
        /// <param name="direction">The direction (horizontal or vertical) of the axis.</param>
        /// <param name="positiveKey">The key on the keyboard to press for a positive value. Pass zero for no key at all.</param>
        /// <param name="negativeKey">The key on the keyboard to press for a negative value. Pass zero for no key at all.</param>
        /// <param name="positiveButton">The gamepad button to press for a positive value. Pass zero for no button at all.</param>
        /// <param name="negativeButton">The gamepad button to press for a negative value. Pass zero for no button at all.</param>
        /// <param name="alternativePositiveKey">An alternative key on the keyboard to press for a positive value. Pass zero for no key at all.</param>
        /// <param name="alternativeNegativeKey">An alternative key on the keyboard to press for a negative value. Pass zero for no key at all.</param>
        /// <param name="alternativePositiveButton">An alternative gamepad button to press for a positive value. Pass zero for no button at all.</param>
        /// <param name="alternativeNegativeButton">An alternative gamepad button to press for a negative value. Pass zero for no button at all.</param>
        /// <param name="gamePadAxis">The gamepad axis this GameControlAxis should be mapped to.</param>
        /// <param name="deadzone">The threshold that has to be exceeded in order for a value to be picked up.</param>
        /// <returns>The newly created <see cref="GameControlAxis"/>.</returns>
        public GameControlAxis RegisterControlAxis(string name, AxisDirection direction, Keys positiveKey, Keys negativeKey, Buttons positiveButton, Buttons negativeButton, 
            Keys alternativePositiveKey, Keys alternativeNegativeKey, Buttons alternativePositiveButton, Buttons alternativeNegativeButton, GamePadAxes gamePadAxis, float deadzone = GameControlAxis.DEFAULT_DEADZONE) {

            if (String.IsNullOrEmpty(name))
                throw new Exception("A control's name cannot be blank. Please pass a valid name.");

            if (ControlAxesMap.ContainsKey(name))
                throw new Exception("There is already a control axis named '" + name + "' registered in this instance of ControlSettings.");

            if(positiveKey == 0 && negativeKey == 0 && positiveButton == 0 && negativeButton == 0 && alternativePositiveKey == 0 && alternativeNegativeKey == 0
                && alternativePositiveButton == 0 && alternativeNegativeButton == 0 && gamePadAxis == GamePadAxes.None)
                throw new Exception("Please assign at least one key or button or a gamepad axis to the new control axis.");

            GameControlAxis gameControlAxis = new GameControlAxis {
                Direction = direction,
                PositiveKey = positiveKey,
                NegativeKey = negativeKey,
                PositiveButton = positiveButton,
                NegativeButton = negativeButton,
                AlternativePositiveKey = alternativePositiveKey,
                AlternativeNegativeKey = alternativeNegativeKey,
                AlternativePositiveButton = alternativePositiveButton,
                AlternativeNegativeButton = alternativeNegativeButton,
                GamePadAxis = gamePadAxis,
                Deadzone = deadzone
            };

            ControlAxesMap.Add(name, gameControlAxis);

            return gameControlAxis;

        }

        /// <summary>
        /// Adds a new <see cref="GameControlAxis"/> with the given name that responds to the provided set of keys and buttons or a gamepad axis (e.g. a thumbstick).
        /// </summary>
        /// <param name="name">The name of the new GameControlAxis. Used for identifying and referencing this GameControlAxis later.</param>
        /// <param name="direction">The direction (horizontal or vertical) of the axis.</param>
        /// <param name="gamePadAxis">The gamepad axis this GameControlAxis should be mapped to.</param>
        /// <param name="deadzone">The threshold that has to be exceeded in order for a value to be picked up.</param>
        /// <returns>The newly created <see cref="GameControlAxis"/>.</returns>
        public GameControlAxis RegisterControlAxis(string name, AxisDirection direction, GamePadAxes gamePadAxis, float deadzone = GameControlAxis.DEFAULT_DEADZONE) {

            return RegisterControlAxis(name, direction, 0, 0, 0, 0, 0, 0, 0, 0, gamePadAxis, deadzone);

        }

        /// <summary>
        /// Adds a new <see cref="GameControlAxis"/> with the given name that responds to the provided set of keys and buttons or a gamepad axis (e.g. a thumbstick).
        /// </summary>
        /// <param name="name">The name of the new GameControlAxis. Used for identifying and referencing this GameControlAxis later.</param>
        /// <param name="direction">The direction (horizontal or vertical) of the axis.</param>
        /// <param name="positiveKey">The key on the keyboard to press for a positive value. Pass zero for no key at all.</param>
        /// <param name="negativeKey">The key on the keyboard to press for a negative value. Pass zero for no key at all.</param>
        /// <param name="alternativePositiveKey">An alternative key on the keyboard to press for a positive value. Pass zero for no key at all.</param>
        /// <param name="alternativeNegativeKey">An alternative key on the keyboard to press for a negative value. Pass zero for no key at all.</param>
        /// <param name="deadzone">The threshold that has to be exceeded in order for a value to be picked up.</param>
        /// <returns>The newly created <see cref="GameControlAxis"/>.</returns>
        public GameControlAxis RegisterControlAxis(string name, AxisDirection direction, Keys positiveKey, Keys negativeKey, 
            Keys alternativePositiveKey = 0, Keys alternativeNegativeKey = 0, float deadzone = GameControlAxis.DEFAULT_DEADZONE) {

            return RegisterControlAxis(name, direction, positiveKey, negativeKey, 0, 0, alternativePositiveKey, alternativeNegativeKey, 0, 0, GamePadAxes.None, deadzone);

        }

        /// <summary>
        /// Adds a new <see cref="GameControlAxis"/> with the given name that responds to the provided set of keys and buttons or a gamepad axis (e.g. a thumbstick).
        /// </summary>
        /// <param name="name">The name of the new GameControlAxis. Used for identifying and referencing this GameControlAxis later.</param>
        /// <param name="direction">The direction (horizontal or vertical) of the axis.</param>
        /// <param name="positiveButton">The gamepad button to press for a positive value. Pass zero for no button at all.</param>
        /// <param name="negativeButton">The gamepad button to press for a negative value. Pass zero for no button at all.</param>
        /// <param name="alternativePositiveButton">An alternative gamepad button to press for a positive value. Pass zero for no button at all.</param>
        /// <param name="alternativeNegativeButton">An alternative gamepad button to press for a negative value. Pass zero for no button at all.</param>
        /// <param name="deadzone">The threshold that has to be exceeded in order for a value to be picked up.</param>
        /// <returns>The newly created <see cref="GameControlAxis"/>.</returns>
        public GameControlAxis RegisterControlAxis(string name, AxisDirection direction, Buttons positiveButton, Buttons negativeButton, 
            Buttons alternativePositiveButton = 0, Buttons alternativeNegativeButton = 0, float deadzone = GameControlAxis.DEFAULT_DEADZONE) {

            return RegisterControlAxis(name, direction, 0, 0, positiveButton, negativeButton, 0, 0, alternativePositiveButton, alternativeNegativeButton, GamePadAxes.None, deadzone);

        }

        /// <summary>
        /// Removes the <see cref="GameControl"/> with the given name from the map of game controls and returns it.
        /// </summary>
        /// <param name="name">The name of the GameControl to remove.</param>
        /// <returns>The <see cref="GameControl"/> that was removed from the collection of controls. Null if no GameControl with the given name was found.</returns>
        public GameControl UnregisterControl(string name) {

            if (!ControlMap.ContainsKey(name))
                return null;

            GameControl gameControl = ControlMap[name];
            ControlMap.Remove(name);

            return gameControl;

        }

        /// <summary>
        /// Removes the <see cref="GameControlAxis"/> with the given name from the map of game control axes and returns it.
        /// </summary>
        /// <param name="name">The name of the GameControlAxis to remove.</param>
        /// <returns>The <see cref="GameControlAxis"/> that was removed from the collection of controls. Null if no GameControlAxis with the given name was found.</returns>
        public GameControlAxis UnregisterControlAxis(string name) {

            if (!ControlAxesMap.ContainsKey(name))
                return null;

            GameControlAxis gameControlAxis = ControlAxesMap[name];
            ControlAxesMap.Remove(name);

            return gameControlAxis;

        }

        /// <summary>
        /// Removes all <see cref="GameControl"/>s and every <see cref="GameControlAxis"/> registered.
        /// </summary>
        public void ClearGameControls() {

            ControlMap.Clear();
            ControlAxesMap.Clear();

        }

    }

}
