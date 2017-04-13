using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Himalaya.Collision {

    /// <summary>
    /// Can be used to determine how precisely collision detection should be performed.
    /// </summary>
    public enum CollisionDetectionMethods {

        /// <summary>
        /// Much faster way of collision detection that may, however, cause the "bullet through paper" problem. I. e. an object that is moving very fast may
        /// not detect collision with another object in its path.
        /// </summary>
        Lazy,

        /// <summary>
        /// Makes collision detection much more precise at the cost of performance. You may want to use this for objects that move at high speeds to avoid the "bullet through paper" problem.
        /// </summary>
        Continuous

    }

}
