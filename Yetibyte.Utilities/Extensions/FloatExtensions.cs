using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.Utilities;

namespace Yetibyte.Utilities.Extensions {

    public static class FloatExtensions {

        public static bool RoughlyEquals(this float value, float other) {

            return Math.Abs(value - other) < MathUtil.EPSILON;

        }


    }

}
