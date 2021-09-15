using System;
using Unit = System.ValueTuple;

namespace PendleCodeMonkey.FunctionalLib
{
	/// <summary>
	/// Partial implementation of the FnLib class.
	/// </summary>
	public static partial class FnLib
	{
		public static ValueOrException<T> ValueOrException<T>(T value) => new ValueOrException<T>(value);
	}

	/// <summary>
	/// Implementation of the <see cref="ValueOrException{T}"/> struct.
	/// </summary>
	public struct ValueOrException<T>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueOrException{T}"/> struct in the Exception state.
		/// </summary>
		/// <param name="ex">The <see cref="System.Exception"/> wrapped by this instance.</param>
		internal ValueOrException(Exception ex)
		{
			Ex = ex ?? throw new ArgumentNullException(nameof(ex));
			Value = default;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueOrException{T}"/> struct in the Success state.
		/// </summary>
		/// <param name="value">The value wrapped by this instance.</param>
		internal ValueOrException(T value)
		{
			Value = value;
			Ex = null;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The value wrapped by this <see cref="ValueOrException{T}"/> instance when in the Success state.
		/// </summary>
		internal T Value { get; }

		/// <summary>
		/// The <see cref="System.Exception"/> wrapped by this <see cref="ValueOrException{T}"/> instance when in the Exception state.
		/// </summary>
		internal Exception Ex { get; }

		/// <summary>
		/// Boolean value indicating if this <see cref="ValueOrException{T}"/> is in the Success state.
		/// </summary>
		public bool Success => Ex == null;

		/// <summary>
		/// Boolean value indicating if this <see cref="ValueOrException{T}"/> is in the Exception state.
		/// </summary>
		public bool Exception => Ex != null;

		#endregion

		#region Methods

		/// <summary>
		/// Implicitly convert a <see cref="System.Exception"/> to a <see cref="ValueOrException{T}"/> in the Exception state.
		/// </summary>
		/// <param name="ex">The <see cref="System.Exception"/> being wrapped in the <see cref="ValueOrException{T}"/>.</param>
		public static implicit operator ValueOrException<T>(Exception ex) => new ValueOrException<T>(ex);

		/// <summary>
		/// Implicitly convert a value of type T to a <see cref="ValueOrException{T}"/> in the Success state.
		/// </summary>
		/// <param name="value">The value being wrapped in the <see cref="ValueOrException{T}"/>.</param>
		public static implicit operator ValueOrException<T>(T value) => new ValueOrException<T>(value);

		/// <summary>
		/// Calls a specified function depending on the state (i.e. Success or Exception) of the ValueOrException object. 
		/// </summary>
		/// <typeparam name="TResult">The type of the return value of the functions that are called as a result of matching.</typeparam>
		/// <param name="exceptionFn">The function to be called when the ValueOrException object is in the Exception state.</param>
		/// <param name="successFn">The function to be called when the ValueOrException object is in the Success state.</param>
		/// <returns>The value returned by the function that is called as a result of matching.</returns>
		public TResult Match<TResult>(Func<Exception, TResult> exceptionFn, Func<T, TResult> successFn)
		   => Exception ? exceptionFn(Ex) : successFn(Value);

		/// <summary>
		/// Calls a specified Action method depending on the state (i.e. Success or Exception) of the ValueOrException object. 
		/// </summary>
		/// <remarks>
		/// Effectively the same as the Match method that works with Func delegates (see above) but allows Action delegates to
		/// be specified instead of Funcs.
		/// </remarks>
		/// <param name="exceptionAct">The method to be called when the ValueOrException object is in the Exception state.</param>
		/// <param name="successAct">The method to be called when the ValueOrException object is in the Success state.</param>
		/// <returns>A <see cref="Unit"/> (as Action delegate methods have no return value).</returns>
		public Unit Match(Action<Exception> exceptionAct, Action<T> successAct)
		   => Match(exceptionAct.ToUnitFunc(), successAct.ToUnitFunc());

		/// <summary>
		/// Returns a string representation of the <see cref="ValueOrException{T}"/> object's state.
		/// </summary>
		/// <returns>A string representation of the <see cref="ValueOrException{T}"/> object's state.</returns>
		public override string ToString()
			=> Match(
				ex => $"Exception({ex.Message})",
				t => $"Success({t})");

		#endregion
	}

	/// <summary>
	/// Implementation of the static <see cref="ValueOrException"/> class.
	/// </summary>
	public static class ValueOrException
	{
		/// <summary>
		/// Returns a Func that creates an instance of the <see cref="ValueOrException{T}"/> class.
		/// </summary>
		/// <typeparam name="T">The type of the value to be wrapped in the <see cref="ValueOrException{T}"/>.</typeparam>
		/// <returns>A Func delegate that takes a value of type T and returns a <see cref="ValueOrException{T}"/>.</returns>
		public static Func<T, ValueOrException<T>> Return<T>()
			=> t => t;

		/// <summary>
		/// Returns an instance of the <see cref="ValueOrException{T}"/> class in the Exception state, wrapping the specified <see cref="Exception"/>.
		/// </summary>
		/// <typeparam name="T">The type of the value wrapped by the <see cref="ValueOrException{T}"/> when in the Success state.</typeparam>
		/// <param name="ex">The <see cref="Exception"/> to be wrapped by the returned <see cref="ValueOrException{T}"/> object.</param>
		/// <returns>An instance of the <see cref="ValueOrException{T}"/> class in the Exception state, wrapping the specified <see cref="Exception"/></returns>
		public static ValueOrException<T> Of<T>(Exception ex)
			=> new ValueOrException<T>(ex);

		/// <summary>
		/// Returns an instance of the <see cref="ValueOrException{T}"/> class in the Success state, wrapping the specified value.
		/// </summary>
		/// <typeparam name="T">The type of the value wrapped by the <see cref="ValueOrException{T}"/>.</typeparam>
		/// <param name="value">The value to be wrapped.</param>
		/// <returns>An instance of the <see cref="ValueOrException{T}"/> class in the Success state, wrapping the specified value.</returns>
		public static ValueOrException<T> Of<T>(T value)
			=> new ValueOrException<T>(value);

		/// <summary>
		/// Maps the specified function onto a <see cref="ValueOrException{T}"/> instance.
		/// </summary>
		/// <remarks>
		/// If the <see cref="ValueOrException{T}"/> object is in the Success state then the function being mapped is called,
		/// passing the object's Value as the parameter.
		/// If the <see cref="ValueOrException{T}"/> object is in the Exception state then the object's <see cref="Exception"/>
		/// is returned as-is (in a <see cref="ValueOrException{T}"/>) and the Func is not called.
		/// </remarks>
		/// <typeparam name="T">The type of the value wrapped by the <see cref="ValueOrException{T}"/>.</typeparam>
		/// <typeparam name="R">The type of the value returned by the function being mapped.</typeparam>
		/// <param name="valOrExcept">The <see cref="ValueOrException{T}"/> object on which Map is being called.</param>
		/// <param name="func">The function mapping a value of type T to a value of type R.</param>
		/// <returns>A <see cref="ValueOrException{T}"/> object that is the result of the Map operation.</returns>
		public static ValueOrException<R> Map<T, R>(this ValueOrException<T> valOrExcept, Func<T, R> func)
			=> valOrExcept.Success ? func(valOrExcept.Value) : new ValueOrException<R>(valOrExcept.Ex);

		/// <summary>
		/// Bind function for <see cref="ValueOrException{T}"/>
		/// </summary>
		/// <remarks>
		/// If the <see cref="ValueOrException{T}"/> object is in the Success state then the function being bound is called,
		/// passing the object's Value as the parameter.
		/// If the <see cref="ValueOrException{T}"/> object is in the Exception state then the object's <see cref="Exception"/>
		/// is returned as-is (in a <see cref="ValueOrException{R}"/>) and the Func is not called.
		/// </remarks>
		/// <typeparam name="T">The type of the value wrapped by the <see cref="ValueOrException{T}"/>.</typeparam>
		/// <typeparam name="R">The type of the value wrapped by the <see cref="ValueOrException{R}"/> returned by the function being bound.</typeparam>
		/// <param name="valOrExcept">The <see cref="ValueOrException{T}"/> object on which Bind is being called.</param>
		/// <param name="func">The function mapping a value of type T to a value of type <see cref="ValueOrException{R}"/>.</param>
		/// <returns>A <see cref="ValueOrException{T}"/> object that is the result of the Bind operation.</returns>
		public static ValueOrException<R> Bind<T, R>(this ValueOrException<T> valOrExcept, Func<T, ValueOrException<R>> func)
			=> valOrExcept.Success ? func(valOrExcept.Value) : new ValueOrException<R>(valOrExcept.Ex);

		/// <summary>
		/// ForEach function for <see cref="ValueOrException{T}"/>
		/// </summary>
		/// <remarks>
		/// Although the name ForEach suggests it operates on multiple values, a ValueOrException instance only ever wraps a single value
		/// so only that value is ever passed to the supplied Action (and then only when the <see cref="ValueOrException{T}"/> is
		/// in a <b>Success</b> state).
		/// The name ForEach has been used in order to maintain consistency with other FunctionalLib types.
		/// </remarks>
		/// <typeparam name="T">The type of the value wrapped by the <see cref="ValueOrException{T}"/>.</typeparam>
		/// <param name="valOrExcept">The <see cref="ValueOrException{T}"/> object on which ForEach is being called.</param>
		/// <param name="act">The method to be called on the value wrapped by the <see cref="ValueOrException{T}"/> instance (when
		/// in a <b>Success</b> state).</param>
		/// <returns>A <see cref="ValueOrException{Unit}"/> (as an Action has no return value).</returns>
		public static ValueOrException<Unit> ForEach<T>(this ValueOrException<T> valOrExcept, Action<T> act)
			=> Map(valOrExcept, act.ToUnitFunc());

		/// <summary>
		/// Applies a supplied <see cref="ValueOrException{T}"/> to a function that is itself wrapped in a <see cref="ValueOrException{T}"/> instance.
		/// </summary>
		/// <remarks>
		/// If either <see cref="ValueOrException{T}"/> instance is in the <b>Exception</b> state then the wrapped <see cref="Exception"/> object is
		/// propogated to the returned <see cref="ValueOrException{R}"/>. Only when both <see cref="ValueOrException{T}"/> instances are in the
		/// <b>Success</b> state is the argument applied to the supplied function.
		/// </remarks>
		/// <typeparam name="T">The type of the value wrapped by the <see cref="ValueOrException{T}"/> argument.</typeparam>
		/// <typeparam name="R">The type of the value wrapped by the <see cref="ValueOrException{R}"/> returned by the function.</typeparam>
		/// <param name="valOrExceptFn">A <see cref="ValueOrException{T}"/> wrapping a function that maps a value of type T to
		/// a value of type R.</param>
		/// <param name="arg">The <see cref="ValueOrException{T}"/> instance to be applied to the function.</param>
		/// <returns>A <see cref="ValueOrException{R}"/> object that is the result of the Apply operation.</returns>
		public static ValueOrException<R> Apply<T, R>(this ValueOrException<Func<T, R>> valOrExceptFn, ValueOrException<T> arg)
			=> valOrExceptFn.Match(
				exceptionFn: ex => ex,
				successFn: func => arg.Match(
					exceptionFn: ex => ex,
					successFn: t => new ValueOrException<R>(func(t))));
	}
}
