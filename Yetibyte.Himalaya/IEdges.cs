using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Himalaya {

    /// <summary>
    /// Interface for anything that has a shape defined by edges (rectangle, polygon etc.).
    /// </summary>
    public interface IEdges {

        IEnumerable<LineSegment> GetEdges();

    }
}
