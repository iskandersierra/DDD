using System;
using System.Collections.Generic;

namespace ModelSoft.Messaging.MessageHandlers
{
    public static class RetryMessagePatterns
    {
        private const double FibConstant = 1.6180339887498948482045868343656;

        /// <summary>
        /// By default retries 5, 8, 13, 21, 34, 55, 89, 144, 233, 377 (ms on each case)
        /// </summary>
        /// <param name="retries"></param>
        /// <param name="startAt"></param>
        /// <returns></returns>
        public static IEnumerable<TimeSpan> GetFibonacciPattern(int retries = 10, int startAt = 5)
        {
            if (retries < 0) throw new ArgumentOutOfRangeException(nameof(retries), "Must be a non negative integer");
            if (startAt < 1) throw new ArgumentOutOfRangeException(nameof(startAt), "Must start from a possitive integer");

            return GetFibonacciPatternInternal(retries, startAt);
        }
        private static IEnumerable<TimeSpan> GetFibonacciPatternInternal(int retries, int startAt)
        {
            var a = (int)Math.Round(startAt / FibConstant, 0);
            var b = startAt;

            for (int i = 0; i < retries; i++)
            {
                yield return TimeSpan.FromMilliseconds(b);

                var next = a + b;
                a = b;
                b = next;
            }
        }

        /// <summary>
        /// Generates a random sequence based on another time pattern
        /// </summary>
        /// <param name="retryPattern"></param>
        /// <param name="fromPreviousPattern"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        public static IEnumerable<TimeSpan> GetRandomPattern(this IEnumerable<TimeSpan> retryPattern, bool fromPreviousPattern = false, Random random = null)
        {
            if (random == null)
                random = new Random(Guid.NewGuid().GetHashCode());

            var previousPattern = TimeSpan.Zero;

            foreach (var timeSpan in retryPattern)
            {
                var min = fromPreviousPattern ? (int)previousPattern.TotalMilliseconds : 0;
                var max = (int)timeSpan.TotalMilliseconds;
                if (min > max)
                {
                    var temp = min;
                    min = max;
                    max = temp;
                }
                var current = random.Next(min, max + 1);
                previousPattern = timeSpan;
                yield return TimeSpan.FromMilliseconds(current);
            }
        }
    }
}