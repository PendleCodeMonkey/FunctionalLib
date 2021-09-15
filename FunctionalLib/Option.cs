using System;
using System.Collections.Generic;

namespace PendleCodeMonkey.FunctionalLib
{
	using static FnLib;

	/// <summary>
	/// Partial implementation of the FnLib class.
	/// </summary>
	public static partial class FnLib
	{
		/// <summary>
		/// Gets an Option in the <b>Some</b> state (i.e. an Option containing the specified value)
		/// </summary>
		/// <typeparam name="T">The type of the value wrapped in this Option.</typeparam>
		/// <param name="value">The value wrapped in this Option.</param>
		/// <returns>An Option in the <b>Some</b> state that wraps the specified value.</returns>
		public static Option<T> Some<T>(T value) => new Option.Some<T>(value);

		/// <summary>
		/// Gets an Option in the <b>None</b> state (i.e. an Option containing no value)
		/// </summary>
		public static Option.None None => Option.None.Default;
	}

	/// <summary>
	/// Implementation of the Option{T} struct.
	/// </summary>
	/// <typeparam name="T">The type of the value wrapped in this Option (when in the <b>Some</b> state).</typeparam>
	public struct Option<T> : IEquatable<Option<T>>, IEquatable<Option.None>
	{
		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Option{T}" /> struct.
		/// </summary>
		/// <remarks>
		/// Note: This is a private constructor as it is only possible to
		/// create instances of this struct using the implicit operators below.
		/// Also note that, if this constructor is not called (i.e. default construction is performed) then
		/// the Option contains no value (i.e. is in the <b>None</b> state)
		/// </remarks>
		/// <param name="value">The value that is wrapped in the <b>Some</b> of this Option.</param>
		private Option(T value)
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}

			IsSome = true;
			Value = value;
		}

		#endregion

		#region properties

		/// <summary>
		/// The value of this <see cref="Option{T}" /> instance.
		/// </summary>
		/// <remarks>
		/// Only valid if this <see cref="Option{T}" /> is a <b>Some</b>.
		/// </remarks>
		public T Value { get; }

		/// <summary>
		/// A boolean value indicating if this <see cref="Option{T}" /> is a <b>Some</b> (i.e. has a value).
		/// </summary>
		public bool IsSome { get; }

		/// <summary>
		/// A boolean value indicating if this <see cref="Option{T}" /> is a <b>None</b> (i.e. has no value).
		/// </summary>
		public bool IsNone { get => !IsSome; }

		#endregion

		#region methods

		/// <summary>
		/// Implicitly convert an Option.None into an <see cref="Option{T}" /> in the <b>None</b> state.
		/// </summary>
		/// <param name="_">An Option.None.</param>
		public static implicit operator Option<T>(Option.None _) => new Option<T>();

		/// <summary>
		/// Implicitly convert an Option.Some{T} to an <see cref="Option{T}" /> in the <b>Some</b> state.
		/// </summary>
		/// <param name="some">The Option.Some{T} to be converted into an Option{T}.</param>
		public static implicit operator Option<T>(Option.Some<T> some) => new Option<T>(some.Value);

		/// <summary>
		/// Implicitly convert a value into an <see cref="Option{T}" />.
		/// </summary>
		/// <remarks>
		/// If the value is null then an <see cref="Option{T}" /> in the <b>None</b> state is created; otherwise, an
		/// <see cref="Option{T}" /> in the <b>Some</b> state is created which wraps the value.
		/// </remarks>
		/// <param name="value">The value to be converted to an <see cref="Option{T}" />.</param>
		public static implicit operator Option<T>(T value) => value == null ? None : Some(value);

		/// <summary>
		/// Calls a specified function depending on the state (i.e. Some or None) of the <see cref="Option{T}" />. 
		/// </summary>
		/// <typeparam name="R">The type of the return value of the functions that are called as a result of matching.</typeparam>
		/// <param name="NoneFn">The function to be called when the <see cref="Option{T}" /> is a <b>None</b>.</param>
		/// <param name="SomeFn">The function to be called when the <see cref="Option{T}" /> is a <b>Some</b>.</param>
		/// <returns>The value returned by the function that is called as a result of matching.</returns>
		public R Match<R>(Func<R> NoneFn, Func<T, R> SomeFn)
			=> IsSome ? SomeFn(Value) : NoneFn();

		/// <summary>
		/// Return this Option's Value as an IEnumerable (yielding no result if the <see cref="Option{T}" /> is a <b>None</b>).
		/// </summary>
		/// <returns>An IEnumerable yielding the Value (or no result if the <see cref="Option{T}" /> is a <b>None</b>)</returns>
		public IEnumerable<T> AsEnumerable()
		{
			if (IsSome)
			{
				yield return Value;
			}
		}

		/// <summary>
		/// Calculates the hash code for the current <see cref="Option{T}" /> instance.
		/// </summary>
		/// <returns>The hash code for the current <see cref="Option{T}" /> instance.</returns>
		public override int GetHashCode() => (IsSome, Value).GetHashCode();

		/// <summary>
		/// Determines whether the specified <see cref="Option{T}" /> object is equal to the current object.
		/// </summary>
		/// <param name="otherOpt">An <see cref="Option{T}" /> object with which to check for equality.</param>
		/// <returns><c>true</c> if the specified <see cref="Option{T}" /> object is equal to the current object; otherwise, <c>false</c>.</returns>
		public bool Equals(Option<T> otherOpt) => IsSome == otherOpt.IsSome && (IsNone || Value.Equals(otherOpt.Value));

		/// <summary>
		/// Determine if the current object is equal to the <b>None</b> Option (i.e. check if this <see cref="Option{T}" /> is a <b>None</b>)
		/// </summary>
		/// <param name="_">Unused parameter of type <see cref="FunctionalLib.Option.None"/>.</param>
		/// <returns><c>true</c> if the current object is equal to the <b>None</b> Option; otherwise, <c>false</c>.</returns>
		public bool Equals(Option.None _) => IsNone;

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <param name="obj">An object with which to check for equality.</param>
		/// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj) => this.ToString() == obj.ToString();   // unfortunately, a bit of a hack :-(

		/// <summary>
		/// Determine if the two specified <see cref="Option{T}" /> objects are equal.
		/// </summary>
		/// <param name="first">The first <see cref="Option{T}" /> object.</param>
		/// <param name="second">The second <see cref="Option{T}" /> object</param>
		/// <returns><c>true</c> if the two specified <see cref="Option{T}" /> objects are equal; otherwise, <c>false</c>.</returns>
		public static bool operator ==(Option<T> first, Option<T> second) => first.Equals(second);

		/// <summary>
		/// Determine if the two specified <see cref="Option{T}" /> objects are not equal.
		/// </summary>
		/// <param name="first">The first <see cref="Option{T}" /> object.</param>
		/// <param name="second">The second <see cref="Option{T}" /> object</param>
		/// <returns><c>true</c> if the two specified <see cref="Option{T}" /> objects are not equal; otherwise, <c>false</c>.</returns>
		public static bool operator !=(Option<T> first, Option<T> second) => !(first == second);

		/// <summary>
		/// Returns a string representation of the <see cref="Option{T}" /> object's state.
		/// </summary>
		/// <returns>A string representation of the <see cref="Option{T}" /> object's state.</returns>
		public override string ToString() => IsSome ? $"Some({Value})" : "None";

		#endregion
	}

	namespace Option
	{
		/// <summary>
		/// An Option in the <b>None</b> state.
		/// </summary>
		public struct None
		{
			internal static readonly None Default = new None();
		}

		/// <summary>
		/// An Option in the <b>Some</b> state.
		/// </summary>
		/// <typeparam name="T">The type of the value wrapped by the <see cref="Some{T}"/>.</typeparam>
		public struct Some<T>
		{
			/// <summary>
			/// The value being wrapped by this <see cref="Some{T}"/> instance.
			/// </summary>
			internal T Value { get; }

			/// <summary>
			/// Initialize an instance of the <see cref="Some{T}"/> struct with the specified value.
			/// </summary>
			/// <param name="value">The value wrapped by the <see cref="Some{T}"/>.</param>
			internal Some(T value)
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "A null value cannot be wrapped in a Option.Some; use Option.None instead");
				}

				Value = value;
			}
		}
	}
}
