using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.Extensions {

    public static class RectangleExtensions {

        /// <summary>
        /// Gets whether or not the provided coordinates lie within the bounds of this <see cref="Rectangle"/>. Unlike <see cref="Rectangle.Contains(int, int)"/>, this method
        /// also considers coordinates that lie directly on the right or bottom edge to be within the bounds.
        /// </summary>
        /// <param name="x">The x coordinate of the point to check for containment.</param>
        /// <param name="y">The y coordinate of the point to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="RectangleF"/>; <c>false</c> otherwise.</returns>
        public static bool ContainsInclusive(this Rectangle rectangle, int x, int y) {

            return ((((rectangle.X <= x) && (x <= (rectangle.X + rectangle.Width))) && (rectangle.Y <= y)) && (y <= (rectangle.Y + rectangle.Height)));

        }

        /// <summary>
        /// Gets whether or not the provided coordinates lie within the bounds of this <see cref="Rectangle"/>. Unlike <see cref="Rectangle.Contains(float, float)"/>, this method
        /// also considers coordinates that lie directly on the right or bottom edge to be within the bounds.
        /// </summary>
        /// <param name="x">The x coordinate of the point to check for containment.</param>
        /// <param name="y">The y coordinate of the point to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="RectangleF"/>; <c>false</c> otherwise.</returns>
        public static bool ContainsInclusive(this Rectangle rectangle, float x, float y) {

            return ((((rectangle.X <= x) && (x <= (rectangle.X + rectangle.Width))) && (rectangle.Y <= y)) && (y <= (rectangle.Y + rectangle.Height)));

        }

        /// <summary>
        /// Gets whether or not the provided coordinates lie within the bounds of this <see cref="Rectangle"/>. Unlike <see cref="Rectangle.Contains(Point)"/>, this method
        /// also considers coordinates that lie directly on the right or bottom edge to be within the bounds.
        /// </summary>
        /// <param name="value">The point to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="RectangleF"/>; <c>false</c> otherwise.</returns>
        public static bool ContainsInclusive(this Rectangle rectangle, Point value) => ContainsInclusive(rectangle, value.X, value.Y);

        /// <summary>
        /// Gets whether or not the provided coordinates lie within the bounds of this <see cref="Rectangle"/>. Unlike <see cref="Rectangle.Contains(Vector2)"/>, this method
        /// also considers coordinates that lie directly on the right or bottom edge to be within the bounds.
        /// </summary>
        /// <param name="value">The coordinates to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="RectangleF"/>; <c>false</c> otherwise.</returns>
        public static bool ContainsInclusive(this Rectangle rectangle, Vector2 value) => ContainsInclusive(rectangle, value.X, value.Y);

    }
}
