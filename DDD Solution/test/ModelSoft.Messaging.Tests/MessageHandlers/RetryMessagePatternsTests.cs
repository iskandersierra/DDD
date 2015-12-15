using System;
using System.Linq;
using Xunit;

namespace ModelSoft.Messaging.MessageHandlers
{
    public class RetryMessagePatternsTests
    {
        [Fact]
        public void NegativeRetriesFibonacciPatternsThrowsException()
        {
            // Given Fibonacci patterns with -4 retries
            // Then a ArgumentOutOfRangeException is thrown
            Assert.ThrowsAny<ArgumentOutOfRangeException>(() => RetryMessagePatterns.GetFibonacciPattern(-4));
        }

        [Fact]
        public void NegativeStartAtFibonacciPatternsThrowsException()
        {
            // Given Fibonacci patterns with 0 startAt
            // Then a ArgumentOutOfRangeException is thrown
            Assert.ThrowsAny<ArgumentOutOfRangeException>(() => RetryMessagePatterns.GetFibonacciPattern(10, 0));
        }

        [Fact]
        public void EmptyFibonacciPatternsIsEmpty()
        {
            // Given Fibonacci patterns with 0 retries
            var patternSource = RetryMessagePatterns.GetFibonacciPattern(0, 5);

            // Then there is no pattern available
            Assert.Empty(patternSource);
        }

        [Fact]
        public void FibonacciPatternsIsOk()
        {
            // Given Fibonacci patterns with 10 retries and starting from 5
            var patternSource = RetryMessagePatterns.GetFibonacciPattern(10, 5);

            // 5, 8, 13, 21, 34, 55, 89, 144, 233, 377
            var expectedValues = new int[] { 5, 8, 13, 21, 34, 55, 89, 144, 233, 377 };
            var expected = expectedValues.Select(t => TimeSpan.FromMilliseconds(t));

            // Then the pattern contains the times 5ms, 8ms, ..., 377ms
            Assert.Equal(expected, patternSource);
        }

        [Fact]
        public void RandomPatternsFromZeroIsOk()
        {
            // Given some time pattern
            var somePattern = new [] { 1, 3, 18, 6, 29, 1000, 45 }.Select(v => TimeSpan.FromMilliseconds(v)).ToArray();
            // Given a random pattern based on the given pattern from zero to each value
            var patternSource = somePattern.GetRandomPattern(false).ToArray();

            var expected = somePattern.ToArray();

            // Then the pattern contains the same ammount of values than the source pattern
            Assert.Equal(7, patternSource.Count());

            // Then the pattern contains random times
            MyAssert.EachPair(expected, patternSource, (max, v) => Assert.True(max >= v, $"Value {v} should be less than {max}"));
        }

        [Fact]
        public void RandomPatternsFromPreviousIsOk()
        {
            // Given some time pattern
            var somePattern = new[] { 1, 3, 18, 6, 29, 1000, 45 }.Select(v => TimeSpan.FromMilliseconds(v)).ToArray();
            // Given a random pattern based on the given pattern from zero to each value
            var patternSource = somePattern.GetRandomPattern(true).ToArray();

            var expectedMinimums = new[] { 0, 1, 3, 6, 6, 29, 45 }.Select(v => TimeSpan.FromMilliseconds(v)).ToArray();
            var expectedMaximums = new[] { 1, 3, 18, 18, 29, 1000, 1000 }.Select(v => TimeSpan.FromMilliseconds(v)).ToArray();

            // Then the pattern contains the same ammount of values than the source pattern
            Assert.Equal(7, patternSource.Count());

            // Then the pattern contains random times 
            MyAssert.EachPair(expectedMinimums, patternSource, (min, v) => Assert.True(min <= v, $"Value {v} should be greater than {min}"));
            MyAssert.EachPair(expectedMaximums, patternSource, (max, v) => Assert.True(max >= v, $"Value {v} should be less than {max}"));
        }

    }
}
