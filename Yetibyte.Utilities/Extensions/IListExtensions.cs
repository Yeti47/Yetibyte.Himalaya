using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Utilities.Extensions {

    public static class IListExtensions {

        /// <summary>
        /// Shuffles the elements in this list.
        /// </summary>
        /// <typeparam name="T">The type of the elements in this list.</typeparam>
        /// <param name="list">The list to shuffle.</param>
        /// <param name="random">An optional instance of Random to use for radomizing the order of the elements. If 'null' is passed, a static
        /// instance of Random will be used.</param>
        public static void Shuffle<T>(this IList<T> list, Random random = null) {

            if (random == null)
                random = MathUtil.Random;

            int count = list.Count;

            for (int i = 0; i < count-1; i++) {

                int j = random.Next(i, count);

                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
                
            }

        }

    }
}
