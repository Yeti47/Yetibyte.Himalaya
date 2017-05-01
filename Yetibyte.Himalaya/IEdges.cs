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

        /// <summary>
        /// Enumerates the edges of this object.
        /// </summary>
        /// <returns>An enumeration of all edges of this object.</returns>
        IEnumerable<LineSegment> GetEdges();

    }
}
