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

    }

}
