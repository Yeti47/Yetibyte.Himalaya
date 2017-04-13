using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.Utilities;

namespace Yetibyte.Utilities.Extensions {

    public static class FloatExtensions {

        /// <summary>
        /// Determines whether this float value is approximately equal to the provided value.
        /// </summary>
        /// <param name="other">The other floating point number to compare this value to.</param>
        /// <returns>True if this floating point number is roughly equal to the given value.</returns>
        public static bool RoughlyEquals(this float value, float other) => MathUtil.RoughlyEquals(value, other);

        /// <summary>
        /// Determines whether this float value is approximately equal to zero.
        /// </summary>
        /// <returns>True if this floating point number is approximately zero.</returns>
        public static bool IsCloseToZero(this float value) => MathUtil.IsCloseToZero(value);


    }

}
