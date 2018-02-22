using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Utilities.Extensions {

    public static class IEnumerableExtensions {

        /// <summary>
        /// Determines the mode of all the elements in this <see cref="IEnumerable{T}"/>. The mode is the most frequent value in
        /// the collection. 
        /// </summary>
        /// <typeparam name="T">The type of the elements in this collection.</typeparam>
        /// <param name="collection">The collection of elements to determine the mode of.</param>
        /// <returns>The most frequent element in this collection.</returns>
        public static T Mode<T>(this IEnumerable<T> collection) {

            if (collection.Count() <= 0)
                throw new InvalidOperationException("Cannot determine the mode of an empty collection.");

            return collection.GroupBy(x => x).OrderByDescending(g => g.Count()).First().Key;

        }

        /// <summary>
        /// Finds the most frequent elements in this sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="collection">The sequence to check.</param>
        /// <returns>An enumeration of the most frequent elements in this sequence.</returns>
        public static IEnumerable<T> Modes<T>(this IEnumerable<T> collection) {

            if (collection.Count() <= 0)
                throw new InvalidOperationException("Cannot determine the modes of an empty collection.");

            IEnumerable<IGrouping<T, T>> modeMap = collection.GroupBy(x => x);

            int maxFrequency = modeMap.Max(g => g.Count());
            
            return modeMap.Where(g => g.Count() == maxFrequency).Select(g => g.Key);

        }

        /// <summary>
        /// Finds the minimum or maximum element in this sequence by comparing the objects returned by the given selector delegate.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <typeparam name="TResult">The return type of the selector function.</typeparam>
        /// <param name="source">The sequence to find the mininum or maximum element of.</param>
        /// <param name="selector">A function to extract the comparison value for each element. The objects to compare must implement the generic <see cref="IComparable{T}"/> interface.</param>
        /// <param name="doFindMax">Whether to look for the maximum or minimum value. True = maximum; false = minimum. True by default.</param>
        /// <returns>The minimum/maximum element in this sequence.</returns>
        public static T ExtremumBy<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector, bool doFindMax = true) where TResult : IComparable<TResult> {

            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            IEnumerator<T> enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
                throw new InvalidOperationException("The sequence contains no elements.");

            int direction = doFindMax ? 1 : -1;

            T result = enumerator.Current;
            TResult extremeValue = selector(result);

            foreach(T item in source) {
              
                if (selector(item).CompareTo(extremeValue) * direction > 0) {

                    result = item;
                    extremeValue = selector(result);

                }
                
            }

            return result;

        }

        /// <summary>
        /// Finds the maximum of the elements in this sequence by comparing the objects returned by the given transform function.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <typeparam name="TResult">The return type of the selector function.</typeparam>
        /// <param name="source">The sequence to determine the maximum element of.</param>
        /// <param name="selector">A function to extract the comparison value for each element. The objects to compare must implement the generic <see cref="IComparable{T}"/> interface.</param>
        /// <returns>The maximum element in this sequence.</returns>
        public static T MaxBy<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector) where TResult : IComparable<TResult> {

            return ExtremumBy(source, selector, true);

        }

        /// <summary>
        /// Finds the minimum of the elements in this sequence by comparing the objects returned by the given transform function.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <typeparam name="TResult">The return type of the selector function.</typeparam>
        /// <param name="source">The sequence to determine the maximum element of.</param>
        /// <param name="selector">A function to extract the comparison value for each element. The objects to compare must implement the generic <see cref="IComparable{T}"/> interface.</param>
        /// <returns>The minimum element in this sequence.</returns>
        public static T MinBy<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector) where TResult : IComparable<TResult> {

            return ExtremumBy(source, selector, false);

        }

        /// <summary>
        /// Checks whether this <see cref="IEnumerable{T}"/> is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in this collection.</typeparam>
        /// <param name="collection">The collection to check.</param>
        /// <param name="doIgnoreNull">Whether or not elements that are null should be ignored. If true, a collection that
        /// only contains null references is considered empty.</param>
        /// <returns>True if the collection is empty; false otherwise.</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> collection, bool doIgnoreNull = false) => collection.Any(x => x != null || !doIgnoreNull);

        /// <summary>
        /// Creates a new sequence from the current sequence with all null references removed.
        /// </summary>
        /// <typeparam name="T">The type of the elements in this sequence.</typeparam>
        /// <param name="source">The sequence.</param>
        /// <returns>A new sequence of all elements that are not null.</returns>
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> source) => source.Where(x => x != null);

    }

}
