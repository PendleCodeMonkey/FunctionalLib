using PendleCodeMonkey.FunctionalLib;
using Xunit;

namespace FunctionalLibTests
{
	public class UnionTests
	{
		[Fact]
		public void AssignValueUsingFromMethods_UnionIsOfCorrectType()
		{
			Union<int, double, string> union;

			union = Union<int, double, string>.FromT1(20);

			Assert.True(union.IsT1);
			Assert.False(union.IsT2);
			Assert.False(union.IsT3);

			union = Union<int, double, string>.FromT2(15.5);

			Assert.False(union.IsT1);
			Assert.True(union.IsT2);
			Assert.False(union.IsT3);

			union = Union<int, double, string>.FromT3("Union string");

			Assert.False(union.IsT1);
			Assert.False(union.IsT2);
			Assert.True(union.IsT3);
		}

		[Fact]
		public void ImplicitlyAssignValues_UnionIsOfCorrectType()
		{
			// Create Union with T1 = int, T2 = double, and T3 = string.
			Union<int, double, string> union;

			// Implicitly assign an int (i.e. a T1 value)
			union = 5;

			// Assert that the Union is a T1 type.
			Assert.True(union.IsT1);
			Assert.False(union.IsT2);
			Assert.False(union.IsT3);

			// Implicitly assign a double (i.e. a T2 value)
			union = 3.14;

			// Assert that the Union is a T2 type.
			Assert.False(union.IsT1);
			Assert.True(union.IsT2);
			Assert.False(union.IsT3);

			// Implicitly assign a string (i.e. a T3 value)
			union = "Hello";

			// Assert that the Union is a T3 type.
			Assert.False(union.IsT1);
			Assert.False(union.IsT2);
			Assert.True(union.IsT3);
		}

		[Fact]
		public void CallMatch_CorrectMatchFuncIsInvoked()
		{
			Union<int, double, string> union;

			int intVal = 42;
			double doubleVal = 3.1415926;
			string strVal = "A test string";

			for (int i = 0; i < 3; i++)
			{
				union = i switch
				{
					0 => intVal,
					1 => doubleVal,
					_ => strVal,
				};
				var result = union.Match(
								onT1: i => i.ToString(),
								onT2: d => d.ToString(),
								onT3: s => s);

				// Assert that the string returned by the Match function is correct for each case.
				switch (i)
				{
					case 0:
						Assert.Equal(intVal.ToString(), result);
						break;
					case 1:
						Assert.Equal(doubleVal.ToString(), result);
						break;
					default:
						Assert.Equal(strVal, result);
						break;
				}
			}

		}

	}
}
