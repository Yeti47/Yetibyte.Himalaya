using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Himalaya.Controls {

    public class ControlSettings {

        // Properties

        public Dictionary<string, GameControl> ControlMap { get; set; } = new Dictionary<string, GameControl>();
        public Dictionary<string, GameControl>.ValueCollection Controls => ControlMap.Values;
        public Dictionary<string, GameControl>.KeyCollection DictKeys => ControlMap.Keys;

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

                IgnoreKeyboard = original.IgnoreKeyboard;
                IgnoreGamePad = original.IgnoreGamePad;

            }
            
        }

    }

}
