using System;
using System.Collections.Generic;
using Unit = System.ValueTuple;

namespace PendleCodeMonkey.FunctionalLib
{
	using static FnLib;

	/// <summary>
	/// Implementation of extension methods for the <see cref="Option{T}"/> struct.
	/// </summary>
	public static partial class OptionExtensions
	{
		/// <summary>
		/// The Map function for <see cref="Option.None"/>
		/// </summary>
		/// <remarks>
		/// This function just returns None because calling Map for an Option in the <b>None</b> state always yields a None.
		/// </remarks>
		/// <typeparam name="T">The type of the value associated with the source Option object.</typeparam>
		/// <typeparam name="R">The type of the value associated with the target Option object.</typeparam>
		/// <param name="_">The source <see cref="Option.None"/> object [unused]</param>
		/// <param name="f">Function to be called during mapping [unused]</param>
		/// <returns>None.</returns>
		public static Option<R> Map<T, R>(this Option.None _, Func<T, R> f)
			=> None;

		/// <summary>
		/// The Map function for <see cref="Option.Some{T}"/>
		/// </summary>
		/// <typeparam name="T">The type of the value associated with the source Option object.</typeparam>
		/// <typeparam name="R">The type of the value associated with the target Option object.</typeparam>
		/// <param name="optSome">The source <see cref="Option.Some{T}"/> object.</param>
		/// <param name="func">Function that maps the value in the source <see cref="Option.Some{T}"/> to the value in
		/// the returned target <see cref="Option{R}"/>.</param>
		/// <returns>An <see cref="Option{R}"/> in the <b>Some</b> state containing the mapped value.</returns>
		public static Option<R> Map<T, R>(this Option.Some<T> optSome, Func<T, R> func)
			=> Some(func(optSome.Value));

		/// <summary>
		/// Maps a <see cref="Option{T}"/> to <see cref="Option{R}"/> by applying a function to the contained <b>Some</b> value.
		/// </summary>
		/// <typeparam name="T">The type of the <b>Some</b> value in the source <see cref="Option{T}"/>.</typeparam>
		/// <typeparam name="R">The type to map into.</typeparam>
		/// <param name="opt">The source <see cref="Option{T}"/> object.</param>
		/// <param name="func">The mapping function.</param>
		/// <returns>An <see cref="Option{R}"/> containing the result of the mapping.</returns>
		public static Option<R> Map<T, R>(this Option<T> opt, Func<T, R> func)
			=> opt.Match(
			() => None,
			t => Some(func(t)));

		/// <summary>
		/// The Bind function for <see cref="Option{T}"/>
		/// </summary>
		/// <typeparam name="T">The type of the <b>Some</b> value in the source <see cref="Option{T}"/>.</typeparam>
		/// <typeparam name="R">The type of the <b>Some</b> value in the target <see cref="Option{R}"/>.</typeparam>
		/// <param name="opt">The source <see cref="Option{T}"/> object.</param>
		/// <param name="func">Function that binds the source Option's value to an <see cref="Option{R}"/> containing
		/// the result value.</param>
		/// <returns>An <see cref="Option{R}"/> containing the result of the binding.</returns>
		public static Option<R> Bind<T, R>(this Option<T> opt, Func<T, Option<R>> func)
			=> opt.Match(
				() => None,
				t => func(t));

		/// <summary>
		/// Function for binding <see cref="Option{T}"/> to an <see cref="IEnumerable{R}"/>
		/// </summary>
		/// <typeparam name="T">The type of the <b>Some</b> value in the source <see cref="Option{T}"/>.</typeparam>
		/// <typeparam name="R">The type of the target <see cref="IEnumerable{R}"/>.</typeparam>
		/// <param name="opt">The source <see cref="Option{T}"/> object.</param>
		/// <param name="func">Function that binds the source Option's value to an <see cref="IEnumerable{R}"/> containing
		/// the result value.</param>
		/// <returns>An <see cref="IEnumerable{R}"/> containing the result of the binding.</returns>
		public static IEnumerable<R> Bind<T, R>(this Option<T> opt, Func<T, IEnumerable<R>> func)
			=> opt.AsEnumerable().Bind(func);

		/// <summary>
		/// ForEach function for <see cref="Option{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the <b>Some</b> value in the source <see cref="Option{T}"/>.</typeparam>
		/// <param name="opt">The source <see cref="Option{T}"/> object.</param>
		/// <param name="action">The method to be called on the value wrapped in the <see cref="Option{T}"/>.</param>
		/// <returns>An <see cref="Option{T}"/> wrapping a Unit value (as an Action is being called, there is no return value).</returns>
		public static Option<Unit> ForEach<T>(this Option<T> opt, Action<T> action)
			=> Map(opt, action.ToUnitFunc());

		/// <summary>
		/// Traverse function for <see cref="Option{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the <b>Some</b> value in the source <see cref="Option{T}"/>.</typeparam>
		/// <typeparam name="R">The type of the target <see cref="IEnumerable{Option{R}}"/>.</typeparam>
		/// <param name="opt">The source <see cref="Option{T}"/> object.</param>
		/// <param name="func">Function that maps the source Option's value to an IEnumerable containing
		/// the result value wrapped in an <see cref="Option{R}"/>.</param>
		/// <returns>An <see cref="IEnumerable{Option{R}}"/> containing the result of the traverse operation.</returns>
		public static IEnumerable<Option<R>> Traverse<T, R>(this Option<T> opt, Func<T, IEnumerable<R>> func)
			=> opt.Match(
				() => List((Option<R>)None),
				t => func(t).Map(r => Some(r)));

		/// <summary>
		/// Match function that can be called by supplying Action methods instead of Funcs.
		/// </summary>
		/// <typeparam name="T">The type of the <b>Some</b> value in the source <see cref="Option{T}"/>.</typeparam>
		/// <param name="opt">The source <see cref="Option{T}"/> object.</param>
		/// <param name="None">The method to be called when the <see cref="Option{T}"/> is a <b>None</b> type.</param>
		/// <param name="Some">The method to be called when the <see cref="Option{T}"/> is a <b>Some</b> type.</param>
		/// <returns>A Unit (i.e. no return value).</returns>
		public static Unit Match<T>(this Option<T> opt, Action None, Action<T> Some)
			=> opt.Match(None.ToUnitFunc(), Some.ToUnitFunc());

		/// <summary>
		/// Gets a value indicating if this <see cref="Option{T}"/> object is a <b>Some</b> type (i.e has a value)
		/// </summary>
		/// <typeparam name="T">The type of the <b>Some</b> value in the source <see cref="Option{T}"/>.</typeparam>
		/// <param name="opt">The source <see cref="Option{T}"/> object.</param>
		/// <returns><c>true</c> if this <see cref="Option{T}"/> object is a <b>Some</b>; otherwise <c>false</c>.</returns>
		internal static bool IsSome<T>(this Option<T> opt)
			=> opt.Match(
				() => false,
				_ => true);

		/// <summary>
		/// Gets the value contained in this <see cref="Option{T}"/> object if it is a <b>Some</b> type; otherwise
		/// throws an InvalidOperationException (when of a <b>None</b> type).
		/// </summary>
		/// <typeparam name="T">The type of the <b>Some</b> value in the source <see cref="Option{T}"/>.</typeparam>
		/// <param name="opt">The source <see cref="Option{T}"/> object.</param>
		/// <returns></returns>
		internal static T GetValueOrThrow<T>(this Option<T> opt)
			=> opt.Match(
				() => { throw new InvalidOperationException(); },
				t => t);

		/// <summary>
		/// Gets the value contained in this <see cref="Option{T}"/> object if it is a <b>Some</b> type; otherwise
		/// returns the specified default value (when the <see cref="Option{T}"/> object is a <b>None</b> type).
		/// </summary>
		/// <typeparam name="T">The type of the <b>Some</b> value in the source <see cref="Option{T}"/>.</typeparam>
		/// <param name="opt">The source <see cref="Option{T}"/> object.</param>
		/// <param name="defaultValue">The default value (of type T) to be returned if the <see cref="Option{T}"/> object
		/// is a <b>None</b> type.</param>
		/// <returns>The <b>Some</b> value of the <see cref="Option{T}"/> object or the specified default value.</returns>
		public static T GetValueOrElse<T>(this Option<T> opt, T defaultValue)
			=> opt.Match(
				() => defaultValue,
				t => t);

		/// <summary>
		/// Gets the value contained in this <see cref="Option{T}"/> object if it is a <b>Some</b> type; otherwise
		/// returns the value returned by the specified function (when the <see cref="Option{T}"/> object is a <b>None</b> type).
		/// </summary>
		/// <typeparam name="T">The type of the <b>Some</b> value in the source <see cref="Option{T}"/>.</typeparam>
		/// <param name="opt">The source <see cref="Option{T}"/> object.</param>
		/// <param name="defaultValueFunc">Function that returns the default value if the <see cref="Option{T}"/> object
		/// is a <b>None</b> type.</param>
		/// <returns>The <b>Some</b> value of the <see cref="Option{T}"/> object or the value returned by the specified function.</returns>
		public static T GetValueOrElse<T>(this Option<T> opt, Func<T> defaultValueFunc)
			=> opt.Match(
				() => defaultValueFunc(),
				t => t);

		/// <summary>
		/// Returns this <see cref="Option{T}"/> object if it is a <b>Some</b> type, otherwise returns the supplied <see cref="Option{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the <b>Some</b> value in the source <see cref="Option{T}"/>.</typeparam>
		/// <param name="thisOpt">The source <see cref="Option{T}"/> object.</param>
		/// <param name="elseOpt">The <see cref="Option{T}"/> object to be returned when thisOpt is a <b>None</b> type.</param>
		/// <returns>The resulting <see cref="Option{T}"/> object (thisOpt when it is a <b>Some</b>, otherwise elseOpt).</returns>
		public static Option<T> OrElse<T>(this Option<T> thisOpt, Option<T> elseOpt)
			=> thisOpt.Match(
				() => elseOpt,
				_ => thisOpt);

		/// <summary>
		/// Returns this <see cref="Option{T}"/> object if it is a <b>Some</b> type, otherwise returns the <see cref="Option{T}"/> returned
		/// by the specified function.
		/// </summary>
		/// <typeparam name="T">The type of the <b>Some</b> value in the source <see cref="Option{T}"/>.</typeparam>
		/// <param name="thisOpt">The source <see cref="Option{T}"/> object.</param>
		/// <param name="elseOptFunc">Function that returns the <see cref="Option{T}"/> to be used when the source is a <b>None</b> type.</param>
		/// <returns>The resulting <see cref="Option{T}"/> object (thisOpt when it is a <b>Some</b>, otherwise the <see cref="Option{T}"/> object
		/// returned by the elseOptFunc function).</returns>
		public static Option<T> OrElse<T>(this Option<T> thisOpt, Func<Option<T>> elseOptFunc)
			=> thisOpt.Match(
				() => elseOptFunc(),
				_ => thisOpt);

		/// <summary>
		/// Convert a <see cref="Option{T}"/> into a <see cref="Validation{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the <b>Some</b> value in the source <see cref="Option{T}"/>.</typeparam>
		/// <param name="opt">The source <see cref="Option{T}"/> object.</param>
		/// <param name="error">Function that returns an <see cref="Error"/> object to be used when the <see cref="Option{T}"/> is
		/// a <b>None</b>.</param>
		/// <returns>The resulting <see cref="Validation{T}"/> object.</returns>
		public static Validation<T> ToValidation<T>(this Option<T> opt, Func<Error> error)
			=> opt.Match(
				() => Invalid(error()),
				t => Valid(t));
	}
}
