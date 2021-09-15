using System;
using Unit = System.ValueTuple;

namespace PendleCodeMonkey.FunctionalLib
{
	using static FnLib;

	/// <summary>
	/// Implementation of extension methods for Func delegates.
	/// </summary>
	public static partial class FuncExtensions
	{

		/// <summary>
		/// Converts a Func that takes a Unit and returns a value of type T into a nullary Func (i.e. one that
		/// takes no parameters and returns a value of type T)
		/// </summary>
		/// <typeparam name="T">The type of the value returned by the function.</typeparam>
		/// <param name="f">Func that takes a Unit and returns a value of type T.</param>
		/// <returns>A nullary function that is equivalent to the Func that takes a Unit parameter.</returns>
		public static Func<T> ToNullary<T>(this Func<Unit, T> f)
			=> () => f(Unit());

		/// <summary>
		/// Return a Func that is composed of this function and another supplied function.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter.</typeparam>
		/// <typeparam name="T2">The type of the second parameter.</typeparam>
		/// <typeparam name="R">The type of the result.</typeparam>
		/// <param name="g">This function (takes a value of type T2 and returns a value of type R)</param>
		/// <param name="f">A function that takes a value of type T1 and returns a value of type T2 (which is then passed into the g function)</param>
		/// <returns>A Func that is composed of this function and another specified function.</returns>
		public static Func<T1, R> Compose<T1, T2, R>(this Func<T2, R> g, Func<T1, T2> f)
		   => x => g(f(x));

		/// <summary>
		/// Returns a Func that is the negated version of the predicate function.
		/// </summary>
		/// <typeparam name="T">The type of the predicate function's input parameter.</typeparam>
		/// <param name="predicate">The predicate function.</param>
		/// <returns>A Func that is the negated version of the predicate function.</returns>
		public static Func<T, bool> Negate<T>(this Func<T, bool> predicate) => t => !predicate(t);
	}
}
