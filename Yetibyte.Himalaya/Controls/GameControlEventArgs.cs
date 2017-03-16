using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Himalaya.Controls {

    public class GameControlEventArgs : EventArgs {

        public string ControlName { get; private set; }
        public GameControl GameControl { get; private set; }

        public GameControlEventArgs(string controlName, GameControl gameControl) {

            this.ControlName = controlName;
            this.GameControl = gameControl;

        }

    }

}
