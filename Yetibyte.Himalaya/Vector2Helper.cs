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
                
    }

}
