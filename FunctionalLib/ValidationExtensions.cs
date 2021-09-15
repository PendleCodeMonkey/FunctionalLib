using System;
using System.Linq;
using Unit = System.ValueTuple;

namespace PendleCodeMonkey.FunctionalLib
{
	using static FnLib;

	/// <summary>
	/// Implementation of extension methods for the <see cref="Validation{T}"/> struct.
	/// </summary>
	public static partial class ValidationExtensions
	{
		/// <summary>
		/// Gets the value contained in this <see cref="Validation{T}"/> object if it is a <b>Valid</b> type; otherwise
		/// returns the specified default value (when the <see cref="Validation{T}"/> object is an <b>Invalid</b> type).
		/// </summary>
		/// <typeparam name="T">The type of the value in the source <see cref="Validation{T}"/>.</typeparam>
		/// <param name="thisValidation">The source <see cref="Validation{T}"/> object.</param>
		/// <param name="defaultValue">The default value (of type T) to be returned if the <see cref="Validation{T}"/> object
		/// is an <b>Invalid</b> type.</param>
		/// <returns>The value of the <see cref="Validation{T}"/> object (when a <b>Valid</b> type) or the specified default value.</returns>
		public static T GetValueOrElse<T>(this Validation<T> thisValidation, T defaultValue)
			=> thisValidation.Match(
				err => defaultValue,
				t => t);

		/// <summary>
		/// Gets the value contained in this <see cref="Validation{T}"/> object if it is a <b>Valid</b> type; otherwise
		/// returns the value returned by the specified function (when the <see cref="Validation{T}"/> object is an <b>Invalid</b> type).
		/// </summary>
		/// <typeparam name="T">The type of the value in the source <see cref="Validation{T}"/>.</typeparam>
		/// <param name="thisValidation">The source <see cref="Validation{T}"/> object.</param>
		/// <param name="defaultValueFunc">Function that returns the default value if the <see cref="Validation{T}"/> object
		/// is an <b>Invalid</b> type.</param>
		/// <returns>The value of the <see cref="Validation{T}"/> object (when a <b>Valid</b> type) or the default value returned by
		/// the supplied function.</returns>
		public static T GetValueOrElse<T>(this Validation<T> thisValidation, Func<T> defaultValueFunc)
			=> thisValidation.Match(
				err => defaultValueFunc(),
				t => t);

		/// <summary>
		/// Function that applies the value in the supplied <see cref="Validation{T}"/> object to the function wrapped in this
		/// <see cref="Validation{T}"/> object when both Validation objects are in the <b>Valid</b> state; otherwise the validation
		/// error(s) are propogated to the returned <see cref="Validation{R}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the value wrapped by the <see cref="Validation{T}"/> being applied.</typeparam>
		/// <typeparam name="R">The type of the value in the returned <see cref="Validation{R}"/>.</typeparam>
		/// <param name="thisValidation">A <see cref="Validation{T}"/> object wrapping a function that maps a value of type T to
		/// a value of type R.</param>
		/// <param name="val">A <see cref="Validation{T}"/> object wrapping the value being applied.</param>
		/// <returns>A <see cref="Validation{R}"/> object wrapping the result of the Apply function.</returns>
		public static Validation<R> Apply<T, R>(this Validation<Func<T, R>> thisValidation, Validation<T> val)
			=> thisValidation.Match(
				validFn: f => val.Match(
										validFn: t => Valid(f(t)),
										invalidFn: err => Invalid(err)),
				invalidFn: errF => val.Match(
										validFn: _ => Invalid(errF),
										invalidFn: errT => Invalid(errF.Concat(errT))));

		/// <summary>
		/// Map function for <see cref="Validation{T}"/>.
		/// </summary>
		/// <remarks>
		/// Maps a function taking a value of type T and returning a value of type R onto this <see cref="Validation{T}"/> instance.
		/// </remarks>
		/// <typeparam name="T">The type of the value wrapped by this <see cref="Validation{T}"/> instance.</typeparam>
		/// <typeparam name="R">The type of the value wrapped by the returned <see cref="Validation{R}"/>.</typeparam>
		/// <param name="thisValidation">The source <see cref="Validation{T}"/> object.</param>
		/// <param name="f">Function that maps a value of type T to a value of type R.</param>
		/// <returns>A <see cref="Validation{R}"/> object wrapping the result of the Map function.</returns>
		public static Validation<R> Map<T, R>(this Validation<T> thisValidation, Func<T, R> f)
			=> thisValidation.IsValid
				? Valid(f(thisValidation.Value))
				: Invalid(thisValidation.Errors);

		/// <summary>
		/// Bind function for <see cref="Validation{T}"/>.
		/// </summary>
		/// <remarks>
		/// Binds a function taking a value of type T and returning a value of type <see cref="Validation{R}"/> onto
		/// this <see cref="Validation{T}"/> instance.
		/// </remarks>
		/// <typeparam name="T">The type of the value wrapped by this <see cref="Validation{T}"/> instance.</typeparam>
		/// <typeparam name="R">The type of the value wrapped by the returned <see cref="Validation{R}"/>.</typeparam>
		/// <param name="thisValidation">The source <see cref="Validation{T}"/> object.</param>
		/// <param name="f">Function that maps a value of type T to a value of type <see cref="Validation{R}"/>.</param>
		/// <returns>A <see cref="Validation{R}"/> object wrapping the result of the Bind function.</returns>
		public static Validation<R> Bind<T, R>(this Validation<T> thisValidation, Func<T, Validation<R>> f)
			=> thisValidation.Match(
				invalidFn: err => Invalid(err),
				validFn: r => f(r));

		/// <summary>
		/// ForEach function for <see cref="Validation{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the value wrapped by the source <see cref="Validation{T}"/> when in the <b>Valid</b> state.</typeparam>
		/// <param name="thisValidation">The source <see cref="Validation{T}"/> object.</param>
		/// <param name="action">The method to be called on the value wrapped in the <see cref="Validation{T}"/> instance.</param>
		/// <returns>A <see cref="Validation{T}"/> wrapping a Unit value (an Action is being called; therefore, there is no return value).</returns>
		public static Validation<Unit> ForEach<T>(this Validation<T> thisValidation, Action<T> action)
			=> Map(thisValidation, action.ToUnitFunc());

		/// <summary>
		/// Call a specified method using this <see cref="Validation{T}"/> instance, returning the <see cref="Validation{T}"/>
		/// object (so that subsequent function calls can still be chained).
		/// </summary>
		/// <remarks>
		/// Usually, when an Action delegate is invoked (i.e. using the ForEach method) a Unit value is returned, meaning that this
		/// is the end of the call chain (it is not possible to then invoke another function using the value returned by ForEach).
		/// The Do method, however, invokes the specified Action delegate and then returns the source <see cref="Validation{T}"/> instance (rather
		/// than a Unit value) making it possible to chain further function calls after the call to the Do method.
		/// </remarks>
		/// <typeparam name="T">The type of the value wrapped by the source <see cref="Validation{T}"/> when in the <b>Valid</b> state.</typeparam>
		/// <param name="thisValidation">The source <see cref="Validation{T}"/> object.</param>
		/// <param name="action">The method to be called on the value wrapped in the <see cref="Validation{T}"/> instance.</param>
		/// <returns>The source <see cref="Validation{T}"/> object (allowing further function calls to be chained).</returns>
		public static Validation<T> Do<T>(this Validation<T> thisValidation, Action<T> action)
		{
			thisValidation.ForEach(action);
			return thisValidation;
		}
	}
}
