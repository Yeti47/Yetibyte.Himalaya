using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya {

    public static class Vector2Helper {

        /// <summary>
        /// Calculates the angle between the given <see cref="Vector2"/>s in radians.
        /// </summary>
        /// <param name="vectorA">The first vector.</param>
        /// <param name="vectorB">The second vector.</param>
        /// <returns>The angle beween the two vectos in radians.</returns>
        public static float AngleBetween(Vector2 vectorA, Vector2 vectorB) {

            return (float)(Math.Atan2(vectorB.Y, vectorB.X) - Math.Atan2(vectorA.Y, vectorA.X));

        }

        /// <summary>
        /// Calculates the angle between the given <see cref="Vector2"/>s in radians and constrains the result to lie between π and -π.
        /// </summary>
        /// <param name="vectorA">The first vector.</param>
        /// <param name="vectorB">The second vector.</param>
        /// <returns>The angle betwwen the two given vectors in radians clamped between π and -π.</returns>
        public static float AngleBetweenWrapped(Vector2 vectorA, Vector2 vectorB) => MathHelper.WrapAngle(AngleBetween(vectorA, vectorB));

        /// <summary>
        /// Gets the <see cref="Vector2"/> perpendicular to the given <see cref="Vector2"/> in counter-clockwise direction.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns>The <see cref="Vector2"/> perpendicular to this one in counter-clockwise direction.</returns>
        public static Vector2 Perpendicular(Vector2 v) => new Vector2(-v.Y, v.X);

        /// <summary>
        /// Gets the <see cref="Vector2"/> perpendicular to the given <see cref="Vector2"/> in clockwise direction.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns>The <see cref="Vector2"/> perpendicular to this one in clockwise direction.</returns>
        public static Vector2 PerpendicularClockwise(Vector2 v) => new Vector2(v.Y, -v.X);

        /// <summary>
        /// Applies the given rotation in radians to the given <see cref="Vector2"/>.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="radians">The rotation in radians.</param>
        /// <returns>The given Vector2 rotated by the given amount.</returns>
        public static Vector2 Rotate(this Vector2 vector, float radians) {

            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);

            return new Vector2(vector.X * cos - vector.Y * sin, vector.X * sin + vector.Y * cos);

        }

    }

}
