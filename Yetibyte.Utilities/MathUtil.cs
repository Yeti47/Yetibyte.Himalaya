using System;
using System.Collections;
using System.Collections.Generic;

namespace Yetibyte.Utilities {
	
	public static class MathUtil {

        // Constants

        /// <summary>
        /// A very small floating point number. Can be used to check whether a float or a double value is 0. 
        /// Any number smaller than EPSILON can be assumed to be equal to 0.
        /// </summary>
        public const float EPSILON = 0.000005f;

        /// <summary>
        /// The factor for converting meters to feet.
        /// </summary>
        private const float CONVERSION_METERS_TO_FEET = 3.28084f;

        /// <summary>
        /// The factor for converting feet to meters.
        /// </summary>
        private const float CONVERSION_FEET_TO_METERS = 0.3048f;

        // Fields

        private static readonly Random _random = new Random();

        /// <summary>
        /// Provides a static collection of prime numbers.
        /// </summary>
        private static readonly List<int> _primes = new List<int> {

            3, 7, 11, 17, 23, 29, 37,
            47, 59, 71, 89, 107, 131,
            163, 197, 239, 293, 353,
            431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931,
            2333, 2801, 3371, 4049,
            4861, 5839, 7013, 8419,
            10103, 12143, 14591, 17519,
            21023, 25229, 30293, 36353,
            43627, 52361, 62851, 75431,
            90523, 108631, 130363,
            156437, 187751, 225307,
            270371, 324449, 389357,
            467237, 560689, 672827,
            807403, 968897, 1162687,
            1395263, 1674319, 2009191,
            2411033, 2893249, 3471899,
            4166287, 4999559, 5999471,
            7199369

        };

        // Properties

        public static IReadOnlyList<int> Primes => _primes.AsReadOnly();

        /// <summary>
        /// Provides a static instance of the Random class for quick access to random number generation. The seed for this Random
        /// instance never changes.
        /// </summary>
        public static Random Random => _random;

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
        /// Clamps the given value between the minimun and maximum value. I. e. the returned value will never be smaller than min
        /// or greater than max.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The value clamped between min and max.</returns>
        public static float Clamp (float min, float max, float value) {

            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;

        }

        /// <summary>
        /// Clamps the given value between the minimun and maximum value. I. e. the returned value will never be smaller than min
        /// or greater than max.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The value clamped between min and max.</returns>
        public static double Clamp (double min, double max, double value) {

            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;

        }

        /// <summary>
        /// Clamps the given value between the minimun and maximum value. I. e. the returned value will never be smaller than min
        /// or greater than max.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The value clamped between min and max.</returns>
        public static int Clamp(int min, int max, int value) {

            if (value < min)
                return min;

            if (value > max)
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

        public static int RandomInt(int minimum, int maximum, bool includeMaximum = false) {

            return _random.Next(minimum, maximum + (includeMaximum ? 1 : 0));

        }
		
        /// <summary>
        /// Returns the sign of the given value. It will either be 1 or -1, but never 0.
        /// </summary>
        /// <param name="value">The value of which the sign is to be determined.</param>
        /// <returns>1 for a positive value or -1 for a negative value.</returns>
		public static int Sign1(float value) {

            if (value < 0)
                return -1;

            return 1;
						
		}
		
        /// <summary>
        /// Checks if the given number can be assumed to be equal to 0. 
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the absolute value of the given number is smaller than EPSILON.</returns>
        public static bool IsCloseToZero(double value) => Math.Abs(value) < EPSILON;

        /// <summary>
        /// Checks if the given number can be assumed to be equal to 0. 
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the absolute value of the given number is smaller than <see cref="EPSILON"/>.</returns>
        public static bool IsCloseToZero(float value) => Math.Abs(value) < EPSILON;

        /// <summary>
        /// Checks whether the given values are approximately equal.
        /// </summary>
        /// <returns>True if the difference between the given values is smaller than <see cref="EPSILON"/></returns>
        public static bool RoughlyEquals(double a, double b) => Math.Abs(a - b) < EPSILON;

        /// <summary>
        /// Checks whether the given values are approximately equal.
        /// </summary>
        /// <returns>True if the difference between the given values is smaller than <see cref="EPSILON"/></returns>
        public static bool RoughlyEquals(float a, float b) => Math.Abs(a - b) < EPSILON;

        /// <summary>
        /// Rounds and casts the given value to the nearest integer. Shorthand for <c>(int)System.Math.Round(value)</c>.
        /// </summary>
        /// <param name="value">The value to round and cast</param>
        /// <returns>The integer nearest to <c>value</c></returns>
        public static int RoundToInt(double value) => (int)Math.Round(value);

        /// <summary>
        /// Returns the smallest integer greater than or equal to the given value. Shorthand for <c>(int)System.Math.Ceiling(value)</c>.
        /// </summary>
        /// <param name="value">The value to cast.</param>
        /// <returns>The smallest integer greater than or equal to<c>value</c></returns>
        public static int CeilingToInt(double value) => (int)Math.Ceiling(value);

        /// <summary>
        /// Returns the largest integer that is less than or equal to the given value. Shorthand for <c>(int)System.Math.Floor(value)</c>.
        /// </summary>
        /// <param name="value">The value to cast.</param>
        /// <returns>The smallest integer greater than or equal to<c>value</c></returns>
        public static int FloorToInt(double value) => (int)Math.Floor(value);

        /// <summary>
        /// Swaps the given values.
        /// </summary>
        /// <param name="valueA">The first value.</param>
        /// <param name="valueB">The second value.</param>
        public static void Swap(ref float valueA, ref float valueB) {

            float temp = valueA;
            valueA = valueB;
            valueB = temp;

        }

        /// <summary>
        /// Swaps the given values.
        /// </summary>
        /// <param name="valueA">The first value.</param>
        /// <param name="valueB">The second value.</param>
        public static void Swap(ref int valueA, ref int valueB) {

            int temp = valueA;
            valueA = valueB;
            valueB = temp;

        }

        /// <summary>
        /// Converts meters to feet.
        /// </summary>
        /// <param name="meters">The distance in meters to convert.</param>
        /// <returns>The distance converted to feet.</returns>
        public static float MetersToFeet(float meters) => meters * CONVERSION_METERS_TO_FEET;

        /// <summary>
        /// Converts feet to meters.
        /// </summary>
        /// <param name="feet">The distance in feet to convert.</param>
        /// <returns>The distance converted to meters.</returns>
        public static float FeetToMeters(float feet) => feet * CONVERSION_FEET_TO_METERS;

        public static uint GaussianSum(uint n) => (n * (n + 1)) / 2;

        /// <summary>
        /// Finds the greatest common divisor of the two given numbers.
        /// </summary>
        /// <param name="x">The first number.</param>
        /// <param name="y">The second number.</param>
        /// <returns>The greatest common divisor of the two numbers.</returns>
        public static int Gcd(int x, int y) {

            while(y != 0) {

                int remainder = x % y;
                x = y;
                y = remainder;

            }

            return x;

        }

        /// <summary>
        /// Finds the lowest common multiple of the two given numbers.
        /// </summary>
        /// <param name="x">The first number.</param>
        /// <param name="y">The second number.</param>
        /// <returns>The lowest common multiple of the two numbers.</returns>
        public static int Lcm(int x, int y) => (x * y) / Gcd(x, y);

    }
		
}
