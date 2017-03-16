using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Himalaya.Controls {

    public class GameControlEventArgs : EventArgs {

        public GameControl GameControl { get; private set; }

        public GameControlEventArgs(GameControl gameControl) {

            this.GameControl = gameControl;

        }

    }

}
