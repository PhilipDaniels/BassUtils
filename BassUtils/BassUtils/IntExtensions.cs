using System;
using System.Collections.Generic;
using System.Linq;

namespace BassUtils
{
    /// <summary>
    /// Extensions to the <code>Int32</code> class.
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// Returns a range 0..count - 1.
        /// </summary>
        /// <param name="count">The upper boundary of the range (exlusive).</param>
        /// <returns>0, 1, 2, 3, ... count -1.</returns>
        public static IEnumerable<int> Times(this int count)
        {
            return Enumerable.Range(0, count);
        }

        /// <summary>
        /// Executes an action <paramref name="count"/> times.
        /// </summary>
        /// <param name="count">The number of times to execute the action.</param>
        /// <param name="action">The action to execute.</param>
        public static void Times(this int count, Action action)
        {
            action.ThrowIfNull("action");

            for (int i = 0; i < count; i++)
                action();
        }

        /// <summary>
        /// Executes an action <paramref name="count"/> times, passing the index
        /// into the action each time.
        /// </summary>
        /// <param name="count">The number of times to execute the action.</param>
        /// <param name="action">The action to execute.</param>
        public static void Times(this int count, Action<int> action)
        {
            action.ThrowIfNull("action");

            for (int i = 0; i < count; i++)
                action(i);
        }
    }
}
