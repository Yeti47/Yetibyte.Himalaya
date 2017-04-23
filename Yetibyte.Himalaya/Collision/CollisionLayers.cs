using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Himalaya.Collision {

    [Flags]
    public enum CollisionLayers {

        All     = ~0,
        None    = 0x0000,
        Layer1  = 0x0001,
        Layer2  = 0x0002,
        Layer3  = 0x0004,
        Layer4  = 0x0008,
        Layer5  = 0x0010,
        Layer6  = 0x0020,
        Layer7  = 0x0040,
        Layer8  = 0x0080,
        Layer9  = 0x0100,
        Layer10 = 0x0200,
        Layer11 = 0x0400,
        Layer12 = 0x0800,
        Layer13 = 0x1000,
        Layer14 = 0x2000,
        Layer15 = 0x4000,
        Layer16 = 0x8000,

    }

}
