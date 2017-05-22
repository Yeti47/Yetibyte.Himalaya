using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Utilities {

    public static class CollectionUtil {

        /// <summary>
        /// Generates a set of random numbers that lie within the given range without any duplicates.
        /// </summary>
        /// <param name="amount">The amount of random numbers to generate.</param>
        /// <param name="min">The minimum value of the range.</param>
        /// <param name="max">The maximum value of the range (inclusive).</param>
        /// <param name="doSort">Whether or not the resulting set of numbers should be sorted. If set to true, the numbers will be sorted in
        /// ascending order.</param>
        /// <returns>An array of non-duplicate random integers that lie between the given minimum and maximum value.</returns>
        public static int[] GenerateUniqueIntegers(int amount, int min, int max, bool doSort = false) {

            if(max < min)
                throw new ArgumentOutOfRangeException(nameof(max), "'max' cannot be smaller than 'min'.");

            int count = max - min + 1;

            // If we want to generate more numbers than there are available, throw an exception
            if(amount > count)
                throw new Exception(amount + " random numbers should be generated, but the given interval only provides " + count + " numbers.");

            int[] result = new int[amount];

            List<int> candidates = Enumerable.Range(min, count).ToList();

            for (int i = 0; i < amount; i++) {

                int randomNumber = MathUtil.RandomInt(0, candidates.Count);
                result[i] = candidates[randomNumber];
                candidates.RemoveAt(randomNumber);

            }

            return doSort ? result.OrderBy(x => x).ToArray() : result;

        }

        /// <summary>
        /// Generates a set of random numbers that lie withing the given range. Note: The set may contain duplicate numbers. 
        /// To generate a set of unique numbers, use <see cref="GenerateUniqueIntegers(int, int, int, bool)"/>.
        /// </summary>
        /// <param name="amount">The amount of random numbers to generate.</param>
        /// <param name="min">The minimum value of the range.</param>
        /// <param name="max">The maximum value of the range.</param>
        /// <param name="doSort">Whether or not the resulting set of numbers should be sorted. If set to true, the numbers will be sorted in
        /// ascending order.</param>
        /// <returns>An array of random integers that lie within the given minimum and maximum value.</returns>
        public static int[] GenerateIntegers(int amount, int min, int max, bool doSort = false) {

            if (max < min)
                throw new ArgumentOutOfRangeException(nameof(max), "'max' cannot be smaller than 'min'.");

            int[] result = new int[amount];

            for (int i = 0; i < result.Length; i++)
                result[i] = MathUtil.RandomInt(min, max, true);

            return doSort ? result.OrderBy(x => x).ToArray() : result;

        }

    }

}
