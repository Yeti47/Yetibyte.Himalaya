using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Himalaya.Collision {

    [Flags]
    public enum TriggerReceivers {

        None = 0,
        ControllerEntity = 1 << 0,
        AttachedEntity = 1 << 1,
        Both = ControllerEntity | AttachedEntity

    }

}
