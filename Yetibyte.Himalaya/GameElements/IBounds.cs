using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Himalaya.GameElements {

    /// <summary>
    /// An interface for anything that has an axis-aligned bounding box.
    /// </summary>
    public interface IBounds {

        RectangleF Bounds { get; }

    }
    
}
