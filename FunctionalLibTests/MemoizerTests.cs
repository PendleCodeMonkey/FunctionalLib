using Moq;
using PendleCodeMonkey.FunctionalLib;
using System;
using Xunit;

namespace FunctionalLibTests
{
	public class MemoizerTests
	{
		[Fact]
		public void CallingOnce_InvokesOriginalFunctionOnce()
		{
			// Arrange
			var originalFn = new Mock<Func<int, int>>();
			var memoizedFn = Memoizer.Memoize(originalFn.Object);

			// Act - call the memoized function once.
			memoizedFn(3);

			// Assert - original function should have been called once.
			originalFn.Verify(f => f(3), Times.Once());
		}

		[Fact]
		public void CallingMultipleTimesWithSameParameter_InvokesOriginalFunctionOnlyOnce()
		{
			// Arrange
			var fn = new Mock<Func<int, int>>();
			var memoizedFn = Memoizer.Memoize(fn.Object);

			// Act - call the memoized function 3 times with the same parameter.
			memoizedFn(5);
			memoizedFn(5);
			memoizedFn(5);

			// Assert - original function should have been called only once.
			fn.Verify(f => f(5), Times.Once());
		}

		[Fact]
		public void CallingMultipleTimesWithDifferentParameters_InvokesOriginalFunctionOncePerParameterValue()
		{
			// Arrange
			var fn = new Mock<Func<int, int>>();
			var memoizedFn = Memoizer.Memoize(fn.Object);

			// Act - call the memoized function twice with two different parameter values.
			memoizedFn(5);
			memoizedFn(5);
			memoizedFn(7);
			memoizedFn(7);

			// Assert - original function should have been called once for each parameter value.
			fn.Verify(f => f(5), Times.Once());
			fn.Verify(f => f(7), Times.Once());
		}

	}
}
