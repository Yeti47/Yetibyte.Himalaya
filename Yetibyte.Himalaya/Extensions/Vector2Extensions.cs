using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.Extensions {

    public static class Vector2Extensions {

        /// <summary>
        /// Gets the <see cref="Vector2"/> perpendicular to this one in counter-clockwise direction.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns>The <see cref="Vector2"/> perpendicular to this one in counter-clockwise direction.</returns>
        public static Vector2 Perpendicular(this Vector2 v) => Vector2Helper.Perpendicular(v);

        /// <summary>
        /// Gets the <see cref="Vector2"/> perpendicular to this one in clockwise direction.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns>The <see cref="Vector2"/> perpendicular to this one in clockwise direction.</returns>
        public static Vector2 PerpendicularClockwise(this Vector2 v) => Vector2Helper.PerpendicularClockwise(v);

    }

}
