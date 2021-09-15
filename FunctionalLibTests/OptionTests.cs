using System;
using System.Collections.Generic;
using System.Text;
using PendleCodeMonkey.FunctionalLib;
using Xunit;

namespace FunctionalLibTests
{
	using static FnLib;

	public class OptionTests
	{
		private readonly Func<int, int, int> Multiply2Ints = (i1, i2) => i1 * i2;
		private readonly Func<int, int, int, int> Multiply3Ints = (i1, i2, i3) => i1 * i2 * i3;
		private readonly Func<int, int, int, int, int> Multiply4Ints = (i1, i2, i3, i4) => i1 * i2 * i3 * i4;
		private readonly Func<int, int, int, int, int, int> Multiply5Ints = (i1, i2, i3, i4, i5) => i1 * i2 * i3 * i4 * i5;
		private readonly Func<int, int, int, int, int, int, int> Multiply6Ints = (i1, i2, i3, i4, i5, i6) => i1 * i2 * i3 * i4 * i5 * i6;
		private readonly Func<int, int, int, int, int, int, int, int> Multiply7Ints = (i1, i2, i3, i4, i5, i6, i7) => i1 * i2 * i3 * i4 * i5 * i6 * i7;
		private readonly Func<int, int, int, int, int, int, int, int, int> Multiply8Ints = (i1, i2, i3, i4, i5, i6, i7, i8) => i1 * i2 * i3 * i4 * i5 * i6 * i7 * i8;

		[Fact]
		public void ApplySomeToFuncRequiringTwoArgs_ReturnsSome()
		{
			var result = Some(Multiply2Ints)
						.Apply(Some(2))
						.Apply(Some(3));

			Assert.Equal(Some(6), result);
		}

		[Fact]
		public void ApplyNoneToFuncRequiringTwoArgs_ReturnsNone()
		{
			var result = Some(Multiply2Ints)
						.Apply(None)
						.Apply(Some(3));

			Assert.Equal(None, result);
		}

		[Fact]
		public void MatchExecutesCorrectFunction()
		{
			static string MultiplyBy5ReturnAsString(Option<int> x) => x.Match(
					() => "NONE",
					n => (n * 5).ToString());

			Assert.Equal("15", MultiplyBy5ReturnAsString(Some(3)));
			Assert.Equal("NONE", MultiplyBy5ReturnAsString(None));
		}

		[Fact]
		public void GivenSome_MapReturnsSome()
		{
			static int DoubleInt(int x) => x * 2;

			Option<int> opt = Some(4);
			var result = opt.Map(DoubleInt);
			Assert.True(result.IsSome);
		}

		[Fact]
		public void GivenNone_MapReturnsNone()
		{
			static int DoubleInt(int x) => x * 2;

			Option<int> opt = None;
			var result = opt.Map(DoubleInt);
			Assert.True(result.IsNone);
		}

		[Fact]
		public void MapAndApplySome_ReturnsSome()
		{
			var result = Some(4)
						.Map(Multiply2Ints)
						.Apply(Some(5));

			Assert.Equal(Some(20), result);
		}

		[Fact]
		public void MapAndApplyNone_ReturnsNone()
		{
			var result = Some(4)
						.Map(Multiply2Ints)
						.Apply(None);

			var result2 = ((Option<int>)None)
						.Map(Multiply2Ints)
						.Apply(Some(5));

			Assert.Equal(None, result);
			Assert.Equal(None, result2);
		}

		[Fact]
		public void ApplyToFuncRequiringTwoArgs()
		{
			var result = Some(Multiply2Ints)
						.Apply(Some(10))
						.Apply(Some(9));

			Assert.Equal(Some(90), result);
		}

		[Fact]
		public void ApplyToFuncRequiringThreeArgs()
		{
			var result = Some(Multiply3Ints)
						.Apply(Some(10))
						.Apply(Some(9))
						.Apply(Some(8));

			Assert.Equal(Some(720), result);
		}

		[Fact]
		public void ApplyToFuncRequiringFourArgs()
		{
			var result = Some(Multiply4Ints)
						.Apply(Some(10))
						.Apply(Some(9))
						.Apply(Some(8))
						.Apply(Some(7));

			Assert.Equal(Some(5040), result);
		}

		[Fact]
		public void ApplyToFuncRequiringFiveArgs()
		{
			var result = Some(Multiply5Ints)
						.Apply(Some(10))
						.Apply(Some(9))
						.Apply(Some(8))
						.Apply(Some(7))
						.Apply(Some(6));

			Assert.Equal(Some(30240), result);
		}

		[Fact]
		public void ApplyToFuncRequiringSixArgs()
		{
			var result = Some(Multiply6Ints)
						.Apply(Some(10))
						.Apply(Some(9))
						.Apply(Some(8))
						.Apply(Some(7))
						.Apply(Some(6))
						.Apply(Some(5));

			Assert.Equal(Some(151200), result);
		}

		[Fact]
		public void ApplyToFuncRequiringSevenArgs()
		{
			var result = Some(Multiply7Ints)
						.Apply(Some(10))
						.Apply(Some(9))
						.Apply(Some(8))
						.Apply(Some(7))
						.Apply(Some(6))
						.Apply(Some(5))
						.Apply(Some(4));

			Assert.Equal(Some(604800), result);
		}

		[Fact]
		public void ApplyToFuncRequiringEightArgs()
		{
			var result = Some(Multiply8Ints)
						.Apply(Some(10))
						.Apply(Some(9))
						.Apply(Some(8))
						.Apply(Some(7))
						.Apply(Some(6))
						.Apply(Some(5))
						.Apply(Some(4))
						.Apply(Some(3));

			Assert.Equal(Some(1814400), result);
		}

	}
}
