using System;

namespace Yetibyte.Utilities {
	
	public static class MathUtil {
		
        // Constants

        /// <summary>
        /// A very small floating point number. Can be used to check whether a float or a double value is 0. 
        /// Any number smaller than EPSILON can be assumed to be equal to 0.
        /// </summary>
		public const float EPSILON = 0.000005f;
		    
        // Fields
           
		private static Random _random = new Random();

        // Properties

        /// <summary>
        /// Provides a static instance of the Random class for quick access to random number generation. The seed for this Random
        /// instance never changes.
        /// </summary>
        public static Random Random { get { return _random; } }

        // Methods

        /// <summary>
        /// Clamps the given value between the minimum and maximum value. I. e. the returned value will never be smaller than min
        /// or greater than max.
        /// </summary>
        /// <typeparam name="T">The type of the value to clamp. Must implement the IComparable interface.</typeparam>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The minimum value if the value is smaller than min.
        /// The maximum value if the value is greater than max.
        /// Otherwise the value will be returned as is.</returns>
        public static T Clamp<T> (T min, T max, T value) where T : IComparable {
			
			if(value.CompareTo(min) < 0)
				return min;
			
			if(value.CompareTo(max) > 0)
				return max;
			
			return value;
						
		}
		
		/// <summary>
		/// Converts an angle in radians to an angle in degrees.
		/// </summary>
		/// <param name="radians">The angle in radians.</param>
		/// <returns>The given angle converted to degrees.</returns>
		public static float RadiansToDegree(float radians) {
			
			return radians * (180f / (float)Math.PI);
			
		}
		
        /// <summary>
        /// Uses the static instance of the Random class provided in this utility class to generate a random floating point number.
        /// </summary>
        /// <param name="minimum">The minimum value this RNG should return.</param>
        /// <param name="maximum">The maximum value this RNG should return (inclusive).</param>
        /// <returns></returns>
		public static float RandomFloat(float minimum, float maximum) {

    		return (float)_random.NextDouble() * (maximum - minimum) + minimum;

		}
		
        /// <summary>
        /// Returns the sign of the given value. It will either be 1 or -1, but never 0.
        /// </summary>
        /// <param name="value">The value of which the sign is to be determined.</param>
        /// <returns>1 for a positive value or -1 for a negative value.</returns>
		public static float Sign1(float value) {
			
			float sign = (float)Math.Sign(value);
				
			return Math.Abs(sign) < EPSILON ? 1f : sign;
						
		}
		
        /// <summary>
        /// Checks if the given number can be assumed to be equal to 0. 
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the absolute value of the given number is smaller than EPSILON.</returns>
        public static bool IsCloseToZero(double value) {

            return Math.Abs(value) < EPSILON;

        }
		
	}
		
}
