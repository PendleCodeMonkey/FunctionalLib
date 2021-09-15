using System;
using System.Collections.Generic;
using System.Linq;
using Unit = System.ValueTuple;

namespace PendleCodeMonkey.FunctionalLib
{
	using System.Collections.Immutable;
	using static FnLib;

	/// <summary>
	/// Implementation of extension methods for IEnumerable.
	/// </summary>
	public static partial class EnumerableExtensions
	{
		/// <summary>
		/// Get the value at the head of the <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the <see cref="IEnumerable{T}"/> instance.</typeparam>
		/// <param name="source">The source <see cref="IEnumerable{T}"/> instance.</param>
		/// <returns>An <see cref="Option{T}"/> containing the value wrapped in a <b>Some</b>; otherwise, a <b>None</b> if
		/// no value is available.</returns>
		public static Option<T> Head<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				return None;
			}
			var enumerator = source.GetEnumerator();
			return enumerator.MoveNext() ? Some(enumerator.Current) : None;
		}

		/// <summary>
		/// Append an array of objects of type T to this <see cref="IEnumerable{T}"/> instance.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the <see cref="IEnumerable{T}"/> instance.</typeparam>
		/// <param name="source">The source <see cref="IEnumerable{T}"/> instance.</param>
		/// <param name="ts">Array of objects of type T to be appended to the <see cref="IEnumerable{T}"/> instance.</param>
		/// <returns>The resulting <see cref="IEnumerable{T}"/> instance.</returns>
		public static IEnumerable<T> Append<T>(this IEnumerable<T> source, params T[] ts)
			=> source.Concat(ts);

		/// <summary>
		/// Prepend the supplied object of type T to the front of this <see cref="IEnumerable{T}"/> instance.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the <see cref="IEnumerable{T}"/> instance.</typeparam>
		/// <param name="source">The source <see cref="IEnumerable{T}"/> instance.</param>
		/// <param name="val">The value (of type T) to be prepended to the <see cref="IEnumerable{T}"/> instance.</param>
		/// <returns>The resulting <see cref="IEnumerable{T}"/> instance.</returns>
		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T val)
		{
			yield return val;
			foreach (T t in source)
			{
				yield return t;
			}
		}

		/// <summary>
		/// Prepend an array of objects of type T to this <see cref="IEnumerable{T}"/> instance.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the <see cref="IEnumerable{T}"/> instance.</typeparam>
		/// <param name="source">The source <see cref="IEnumerable{T}"/> instance.</param>
		/// <param name="ts">Array of objects of type T to be prepended to the <see cref="IEnumerable{T}"/> instance.</param>
		/// <returns>The resulting <see cref="IEnumerable{T}"/> instance.</returns>
		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, params T[] ts)
		{
			foreach (T t in ts)
			{
				yield return t;
			}
			foreach (T t in source)
			{
				yield return t;
			}
		}

		/// <summary>
		/// Gets the first item in the <see cref="IEnumerable{T}"/> instance or a specified default value if the
		/// <see cref="IEnumerable{T}"/> contains no elements.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the <see cref="IEnumerable{T}"/> instance.</typeparam>
		/// <param name="source">The source <see cref="IEnumerable{T}"/> instance.</param>
		/// <param name="defaultValue">The default value to be returned when the <see cref="IEnumerable{T}"/> contains no elements.</param>
		/// <returns>The first item in the <see cref="IEnumerable{T}"/> instance or the specified default value (if the <see cref="IEnumerable{T}"/> contains no elements).</returns>
		public static T FirstOr<T>(this IEnumerable<T> source, T defaultValue)
			=> source.Head().Match(
				() => defaultValue,
				t => t);

		/// <summary>
		/// ForEach function for <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the <see cref="IEnumerable{T}"/> instance.</typeparam>
		/// <param name="source">The source <see cref="IEnumerable{T}"/> instance.</param>
		/// <param name="action">Method to be invoked on each element in the <see cref="IEnumerable{T}"/>.</param>
		/// <returns>An <see cref="IEnumerable{Unit}"/> wrapping Unit values (an Action is being invoked; therefore, there are no return values).</returns>
		public static IEnumerable<Unit> ForEach<T>(this IEnumerable<T> source, Action<T> action)
			=> source.Map(action.ToUnitFunc()).ToImmutableList();

		/// <summary>
		/// Call a specified method on each <see cref="IEnumerable{T}"/> element, returning the source <see cref="IEnumerable{T}"/>
		/// object (so that subsequent function calls can be chained).
		/// </summary>
		/// <typeparam name="T">The type of the elements in the <see cref="IEnumerable{T}"/> instance.</typeparam>
		/// <param name="source">The source <see cref="IEnumerable{T}"/> instance.</param>
		/// <param name="action">Method to be invoked on each element in the <see cref="IEnumerable{T}"/>.</param>
		/// <returns>The source <see cref="IEnumerable{T}"/> object (allowing further function calls to be chained).</returns>
		public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
		{
			source.ForEach(action);
			return source;
		}

		/// <summary>
		/// Match function for <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the <see cref="IEnumerable{T}"/> instance.</typeparam>
		/// <typeparam name="R">The type of the returned value.</typeparam>
		/// <param name="source">The source <see cref="IEnumerable{T}"/> instance.</param>
		/// <param name="emptyFn">Function that is invoked when the <see cref="IEnumerable{T}"/> instance is empty.</param>
		/// <param name="hasValueFn">Function that is invoked when the <see cref="IEnumerable{T}"/> instance is not empty.</param>
		/// <returns>The value returned by the match function that is invoked.</returns>
		public static R Match<T, R>(this IEnumerable<T> source, Func<R> emptyFn, Func<T, IEnumerable<T>, R> hasValueFn)
			=> source.Head().Match(
				NoneFn: emptyFn,
				SomeFn: head => hasValueFn(head, source.Skip(1)));

		/// <summary>
		/// Map function for <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <remarks>
		/// For <see cref="IEnumerable{T}"/>, Map is just an alias for the LINQ Select function (and has only been included
		/// to maintain some naming consistency with other FunctionalLib constructs).
		/// </remarks>
		/// <typeparam name="T">The type of the elements in the source <see cref="IEnumerable{T}"/> instance.</typeparam>
		/// <typeparam name="R">The type of the elements in the returned <see cref="IEnumerable{R}"/>.</typeparam>
		/// <param name="source">The source <see cref="IEnumerable{T}"/> instance.</param>
		/// <param name="func">Function that projects a value of type T into a value of type R.</param>
		/// <returns>An <see cref="IEnumerable{R}"/> whose elements are the result of invoking the supplied function on each element of the source.</returns>
		public static IEnumerable<R> Map<T, R>(this IEnumerable<T> source, Func<T, R> func)
			=> source.Select(func);

		/// <summary>
		/// Bind function for <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <remarks>
		/// For <see cref="IEnumerable{T}"/>, Bind is just an alias for the LINQ SelectMany function (and has only been included
		/// to maintain some naming consistency with other FunctionalLib constructs).
		/// </remarks>
		/// <typeparam name="T">The type of the elements in the source <see cref="IEnumerable{T}"/> instance.</typeparam>
		/// <typeparam name="R">The type of the elements in the returned <see cref="IEnumerable{R}"/>.</typeparam>
		/// <param name="source">The source <see cref="IEnumerable{T}"/> instance.</param>
		/// <param name="func">Function that projects each element of the source <see cref="IEnumerable{T}"/> into an <see cref="IEnumerable{R}"/> (which
		/// ar then flattened into a single sequence).</param>
		/// <returns>An <see cref="IEnumerable{R}"/> whose elements are the result of invoking the supplied function on each element of the source.</returns>
		public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> source, Func<T, IEnumerable<R>> func)
			=> source.SelectMany(func);

		/// <summary>
		/// Flatten a sequence of sequences into a single sequence.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the source IEnumerable{IEnumerable{T}} instance.</typeparam>
		/// <param name="source">The source IEnumerable{IEnumerable{T}} instance (i.e. a sequence of sequences).</param>
		/// <returns>>An <see cref="IEnumerable{T}"/> whose elements are the result of flattening the source IEnumerable{IEnumerable{T}}.</returns>
		public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source)
			=> source.SelectMany(x => x);

		/// <summary>
		/// Bind function for <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the source <see cref="IEnumerable{T}"/> instance.</typeparam>
		/// <typeparam name="R">The type of the elements in the returned <see cref="IEnumerable{R}"/>.</typeparam>
		/// <param name="source">The source <see cref="IEnumerable{T}"/> instance.</param>
		/// <param name="func">Function that projects each element of the source <see cref="IEnumerable{T}"/> into an <see cref="Option{R}"/>.</param>
		/// <returns>An <see cref="IEnumerable{R}"/> whose elements are the result of invoking the supplied function on each element of the source.</returns>
		public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> source, Func<T, Option<R>> func)
			=> source.Bind(t => func(t).AsEnumerable());
	}
}
 