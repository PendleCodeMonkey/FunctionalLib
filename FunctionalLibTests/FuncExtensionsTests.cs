using System;
using Xunit;
using PendleCodeMonkey.FunctionalLib;

namespace FunctionalLibTests
{
	public class FuncExtensionsTests
	{
		private readonly Func<int, int, int> Multiply2Ints = (i1, i2) => i1 * i2;
		private readonly Func<int, int, int, int> Multiply3Ints = (i1, i2, i3) => i1 * i2 * i3;
		private readonly Func<int, int, int, int, int> Multiply4Ints = (i1, i2, i3, i4) => i1 * i2 * i3 * i4;
		private readonly Func<int, int, int, int, int, int> Multiply5Ints = (i1, i2, i3, i4, i5) => i1 * i2 * i3 * i4 * i5;
		private readonly Func<int, int, int, int, int, int, int> Multiply6Ints = (i1, i2, i3, i4, i5, i6) => i1 * i2 * i3 * i4 * i5 * i6;
		private readonly Func<int, int, int, int, int, int, int, int> Multiply7Ints = (i1, i2, i3, i4, i5, i6, i7) => i1 * i2 * i3 * i4 * i5 * i6 * i7;
		private readonly Func<int, int, int, int, int, int, int, int, int> Multiply8Ints = (i1, i2, i3, i4, i5, i6, i7, i8) => i1 * i2 * i3 * i4 * i5 * i6 * i7 * i8;

		// When called as part of the following tests, the above Funcs will calculate the factorial values for 2 (Multiply2Ints) through to 8 (Multiply8Ints).
		// The following _factorials array contains pre-calculated factorial values; this enables us to assert that the functions are calculating the correct
		// values (i.e. that partial application is working as expected).
		private readonly int[] _factorials = new int[] {0, 1, 2, 6, 24, 120, 720, 5040, 40320};

		[Fact]
		public void PartialApplicationToFuncRequiringTwoArgs_YieldsCorrectResult()
		{
			var fn = Multiply2Ints.Apply(1);
			var result = fn(2);

			Assert.Equal(_factorials[2], result);
		}

		[Fact]
		public void PartialApplicationToFuncRequiringThreeArgs_YieldsCorrectResult()
		{
			var fn = Multiply3Ints.Apply(1).Apply(2);
			var result = fn(3);

			Assert.Equal(_factorials[3], result);
		}

		[Fact]
		public void PartialApplicationToFuncRequiringFourArgs_YieldsCorrectResult()
		{
			var fn = Multiply4Ints.Apply(1).Apply(2).Apply(3);
			var result = fn(4);

			Assert.Equal(_factorials[4], result);
		}

		[Fact]
		public void PartialApplicationToFuncRequiringFiveArgs_YieldsCorrectResult()
		{
			var fn = Multiply5Ints.Apply(1).Apply(2).Apply(3).Apply(4);
			var result = fn(5);

			Assert.Equal(_factorials[5], result);
		}

		[Fact]
		public void PartialApplicationToFuncRequiringSixArgs_YieldsCorrectResult()
		{
			var fn = Multiply6Ints.Apply(1).Apply(2).Apply(3).Apply(4).Apply(5);
			var result = fn(6);

			Assert.Equal(_factorials[6], result);
		}

		[Fact]
		public void PartialApplicationToFuncRequiringSevenArgs_YieldsCorrectResult()
		{
			var fn = Multiply7Ints.Apply(1).Apply(2).Apply(3).Apply(4).Apply(5).Apply(6);
			var result = fn(7);

			Assert.Equal(_factorials[7], result);
		}

		[Fact]
		public void PartialApplicationToFuncRequiringEightArgs_YieldsCorrectResult()
		{
			var fn = Multiply8Ints.Apply(1).Apply(2).Apply(3).Apply(4).Apply(5).Apply(6).Apply(7);
			var result = fn(8);

			Assert.Equal(_factorials[8], result);
		}
	}
}
