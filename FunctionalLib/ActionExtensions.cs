using System;
using Unit = System.ValueTuple;

namespace PendleCodeMonkey.FunctionalLib
{
	using static FnLib;

	/// <summary>
	/// Implementation of extension methods for Action delegates.
	/// </summary>
	public static partial class ActionExtensions
	{
		/// <summary>
		/// Convert the specified Action into a Func that returns a Unit.
		/// </summary>
		/// <param name="action">An Action that takes no parameters.</param>
		/// <returns>A Func that takes no parameters and returns a Unit.</returns>
		public static Func<Unit> ToUnitFunc(this Action action)
			=> () => { action(); return Unit(); };
	}
}
