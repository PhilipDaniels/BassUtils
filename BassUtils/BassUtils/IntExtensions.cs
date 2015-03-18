using System;
using System.Collections.Generic;
using System.Linq;

namespace BassUtils
{
    public static class IntExtensions
    {
        public static IEnumerable<int> Times(this int count)
        {
            return Enumerable.Range(0, count);
        }

        public static void Times(this int count, Action action)
        {
            action.ThrowIfNull("action");

            for (int i = 0; i < count; i++)
            {
                action();
            }
        }

        public static void Times(this int count, Action<int> action)
        {
            action.ThrowIfNull("action");

            for (int i = 0; i < count; i++)
            {
                action(i);
            }
        }
    }
}
