using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Unit = System.ValueTuple;

namespace PendleCodeMonkey.FunctionalLib
{
	/// <summary>
	/// Partial implementation of the FnLib class.
	/// </summary>
	public static partial class FnLib
	{
		/// <summary>
		/// Gets an instance of the <see cref="Unit"/> struct.
		/// </summary>
		/// <remarks>
		/// A convenience method that allows a <see cref="Unit"/> to be obtained by calling the Unit() method.
		/// </remarks>
		/// <returns>A <see cref="Unit"/>.</returns>
		public static Unit Unit() => default;

		/// <summary>
		/// Tap combinator function.
		/// </summary>
		/// <remarks>
		/// The Tap function returns a Func that invokes the supplied Action delegate method passing an
		/// object of type T and then returns that object.
		/// </remarks>
		/// <typeparam name="T">The type of the parameter to be passed to the supplied Action delegate.</typeparam>
		/// <param name="action">The Action delegate method to be invoked.</param>
		/// <returns>A function that invokes the supplied Action delegate method passing an object of type T
		/// and then returns that object.</returns>
		public static Func<T, T> Tap<T>(Action<T> action)
			=> x => { action(x); return x; };

		/// <summary>
		/// Pipes the input value to the specified function.
		/// </summary>
		/// <typeparam name="T">The type of the parameter to be passed to the supplied function.</typeparam>
		/// <typeparam name="R">The type of the value returned by the supplied function.</typeparam>
		/// <param name="input">The input value.</param>
		/// <param name="func">The function to be invoked, passing the input value.</param>
		/// <returns>The value returned by the supplied function.</returns>
		public static R Pipe<T, R>(this T input, Func<T, R> func)
			=> func(input);

		/// <summary>
		/// Pipes the input value to the specified method.
		/// </summary>
		/// <remarks>
		/// Uses the Tap function to convert the supplied Action delegate to a Func (that takes the input value
		/// and returns the same input value) and then invokes that Func, passing the input value.
		/// </remarks>
		/// <typeparam name="T">The type of the parameter to be passed to the supplied function.</typeparam>
		/// <param name="input">The input value.</param>
		/// <param name="action">The Action delegate method to be invoked.</param>
		/// <returns>The input value.</returns>
		public static T Pipe<T>(this T input, Action<T> action)
			=> Tap(action)(input);

		/// <summary>
		/// Creates an immutable list from the supplied array of objects.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the array.</typeparam>
		/// <param name="items">An array of elements of type T.</param>
		/// <returns>A <see cref="IEnumerable{T}"/> containing an immutable list of the supplied objects.</returns>
		public static IEnumerable<T> List<T>(params T[] items)
			=> items.ToImmutableList();

		/// <summary>
		/// Creates an immutable list containing the elements of the supplied collection prepended
		/// by the source object. 
		/// </summary>
		/// <typeparam name="T">The type of the source object.</typeparam>
		/// <param name="source">The source object.</param>
		/// <param name="list">The collection onto which the source object is to be prepended.</param>
		/// <returns>A <see cref="IEnumerable{T}"/> containing an immutable list containing the elements of the
		/// supplied collection prepended by the source object.</returns>
		public static IEnumerable<T> Cons<T>(this T source, IEnumerable<T> list)
			=> List(source).Concat(list);

		/// <summary>
		/// Returns a Cons function (i.e. a Func that creates an immutable list containing the elements of a
		/// supplied collection prepended by the source object).
		/// </summary>
		/// <typeparam name="T">The type of the source object expected by the returned Cons function.</typeparam>
		/// <returns>A Func that can be invoked to create an immutable list containing the elements of a source
		/// collection prepended by a source object.</returns>
		public static Func<T, IEnumerable<T>, IEnumerable<T>> Cons<T>()
			=> (t, ts) => t.Cons(ts);

		/// <summary>
		/// Calls the supplied function passing a disposable resource and then disposes of the resource.
		/// </summary>
		/// <typeparam name="TDisp">Type that implements the <see cref="IDisposable"/> interface.</typeparam>
		/// <typeparam name="R">Type of the value returned by the supplied function.</typeparam>
		/// <param name="disposable">Instance of the type that implements the <see cref="IDisposable"/> interface.</param>
		/// <param name="func">Function that takes an instance of the type implementing <see cref="IDisposable"/> and returns
		/// a value of type R.</param>
		/// <returns>Value returned by the supplied function.</returns>
		public static R Using<TDisp, R>(TDisp disposable, Func<TDisp, R> func) where TDisp : IDisposable
		{
			using (var disp = disposable)
			{
				return func(disp);
			}
		}

		/// <summary>
		/// Calls the supplied method passing a disposable resource and then disposes of the resource.
		/// </summary>
		/// <typeparam name="TDisp">Type that implements the <see cref="IDisposable"/> interface.</typeparam>
		/// <param name="disposable">Instance of the type that implements the <see cref="IDisposable"/> interface.</param>
		/// <param name="action">The method to be called on the the <see cref="IDisposable"/> instance.</param>
		/// <returns>A <see cref="Unit"/> (as an Action has no return value).</returns>
		public static Unit Using<TDisp>(TDisp disposable, Action<TDisp> action) where TDisp : IDisposable
			=> Using(disposable, action.ToUnitFunc());

		/// <summary>
		/// Calls the supplied function passing a disposable resource (that is itself created by a function) and then disposes of the resource.
		/// </summary>
		/// <typeparam name="TDisp">Type that implements the <see cref="IDisposable"/> interface.</typeparam>
		/// <typeparam name="R">Type of the value returned by the supplied function.</typeparam>
		/// <param name="createDispFn">A function that creates an instance of the type that implements the <see cref="IDisposable"/> interface.</param>
		/// <param name="func">Function that takes an instance of the type implementing <see cref="IDisposable"/> and returns
		/// a value of type R.</param>
		/// <returns>Value returned by the supplied function.</returns>
		public static R Using<TDisp, R>(Func<TDisp> createDispFn, Func<TDisp, R> func) where TDisp : IDisposable
		{
			using (var disp = createDispFn())
			{
				return func(disp);
			}
		}

		/// <summary>
		/// Calls the supplied method passing a disposable resource (that is created by a function) and then disposes of the resource.
		/// </summary>
		/// <typeparam name="TDisp">Type that implements the <see cref="IDisposable"/> interface.</typeparam>
		/// <param name="createDispFn">A function that creates an instance of the type that implements the <see cref="IDisposable"/> interface.</param>
		/// <param name="action">The method to be called on the the <see cref="IDisposable"/> instance.</param>
		/// <returns>A <see cref="Unit"/> (as an Action has no return value).</returns>
		public static Unit Using<TDisp>(Func<TDisp> createDispFn, Action<TDisp> action) where TDisp : IDisposable
			=> Using(createDispFn, action.ToUnitFunc());
	}
}


