using System;
using System.Collections.Generic;
using System.Linq;
using Unit = System.ValueTuple;

namespace PendleCodeMonkey.FunctionalLib
{
	using static FnLib;

	/// <summary>
	/// Partial implementation of the FnLib class.
	/// </summary>
	public static partial class FnLib
	{
		/// <summary>
		/// Create a Validation object in the Valid state with the specified value.
		/// </summary>
		/// <typeparam name="T">The type of the value associated with this Validation object.</typeparam>
		/// <param name="value">The value associated with this Validation object.</param>
		/// <returns>A <see cref="Validation{T}"/> in the Valid state.</returns>
		public static Validation<T> Valid<T>(T value) => new Validation<T>(value);

		/// <summary>
		/// Create a Validation.Invalid object with the specified errors.
		/// </summary>
		/// <param name="errors">Array of <see cref="Error"/> objects.</param>
		/// <returns>A <see cref="Validation.Invalid"/> containing the list of validation errors.</returns>
		public static Validation.Invalid Invalid(params Error[] errors) => new Validation.Invalid(errors);

		/// <summary>
		/// Create a Validation object in the Invalid state with the specified errors.
		/// </summary>
		/// <typeparam name="T">The type of the value associated with the Validation object.</typeparam>
		/// <param name="errors">Array of <see cref="Error"/> objects.</param>
		/// <returns>A <see cref="Validation{T}"/> in the invalid state.</returns>
		public static Validation<T> Invalid<T>(params Error[] errors) => new Validation.Invalid(errors);

		/// <summary>
		/// Create a Validation.Invalid object with the specified errors.
		/// </summary>
		/// <param name="errors">A collection of <see cref="Error"/> objects.</param>
		/// <returns>A <see cref="Validation.Invalid"/> containing the list of validation errors.</returns>
		public static Validation.Invalid Invalid(IEnumerable<Error> errors) => new Validation.Invalid(errors);

		/// <summary>
		/// Create a Validation object in the Invalid state with the specified errors.
		/// </summary>
		/// <typeparam name="T">The type of the value associated with the Validation object.</typeparam>
		/// <param name="errors">A collection of <see cref="Error"/> objects.</param>
		/// <returns>A <see cref="Validation{T}"/> in the invalid state.</returns>
		public static Validation<T> Invalid<T>(IEnumerable<Error> errors) => new Validation.Invalid(errors);
	}

	/// <summary>
	/// Implementation of the <see cref="Validation{T}"/> struct.
	/// </summary>
	/// <typeparam name="T">The type of the value associated with the <see cref="Validation{T}"/> object.</typeparam>
	public struct Validation<T>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Validation{T}"/> struct in the Invalid state with
		/// the specified errors.
		/// </summary>
		/// <param name="errors">Collection of <see cref="Error"/> objects to be associated with the <see cref="Validation{T}"/> object.</param>
		private Validation(IEnumerable<Error> errors)
		{
			IsValid = false;
			Value = default;
			Errors = errors;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Validation{T}"/> struct in the Valid state with
		/// the specified associated value.
		/// </summary>
		/// <param name="value">The value associated with thee Validation object.</param>
		internal Validation(T value)
		{
			IsValid = true;
			Value = value;
			Errors = Enumerable.Empty<Error>();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the collection of errors associated with this Validation object.
		/// </summary>
		/// <remarks>
		/// This collection is only non-empty when the Validation object is in the Invalid state.
		/// </remarks>
		internal IEnumerable<Error> Errors { get; }

		/// <summary>
		/// Gets the value associated with this Validation object.
		/// </summary>
		/// <remarks>
		/// This value is the default value for its type when the Validation object is in the Invalid state, and
		/// therefore should only be used when the Validation object is in the Valid state.
		/// </remarks>
		internal T Value { get; }

		/// <summary>
		/// Gets a boolean value indicating if the Validation object is in the Valid state.
		/// </summary>
		public bool IsValid { get; }

		#endregion

		#region Methods

		/// <summary>
		/// The Return method.
		/// </summary>
		/// <remarks>
		/// Returns a Func delegate that takes a value of type T and returns a <see cref="Validation{T}"/> in the Valid state.
		/// </remarks>
		public static Func<T, Validation<T>> Return = t => Valid(t);

		/// <summary>
		/// Create a <see cref="Validation{T}"/> object in the Invalid state with the specified errors.
		/// </summary>
		/// <param name="errors">Collection of <see cref="Error"/> objects to be associated with the <see cref="Validation{T}"/> object.</param>
		/// <returns>A <see cref="Validation{T}"/> object in the Invalid state with the specified errors.</returns>
		public static Validation<T> FailWithErrors(IEnumerable<Error> errors) => new Validation<T>(errors);

		/// <summary>
		/// Create a <see cref="Validation{T}"/> object in the Invalid state with the specified errors.
		/// </summary>
		/// <param name="errors">An array of <see cref="Error"/> objects to be associated with the <see cref="Validation{T}"/> object.</param>
		/// <returns>A <see cref="Validation{T}"/> object in the Invalid state with the specified errors.</returns>
		public static Validation<T> FailWithErrors(params Error[] errors) => new Validation<T>(errors.AsEnumerable());

		/// <summary>
		/// Implicitly convert an <see cref="Error"/> into a <see cref="Validation{T}"/> in the Invalid state.
		/// </summary>
		/// <param name="error">The error to be associated with the <see cref="Validation{T}"/> object.</param>
		public static implicit operator Validation<T>(Error error) => new Validation<T>(new[] { error });

		/// <summary>
		/// Implicitly convert an <see cref="Validation.Invalid"/> into a <see cref="Validation{T}"/> in the Invalid state.
		/// </summary>
		/// <param name="invalid">The <see cref="Validation.Invalid"/> object to be converted.</param>
		public static implicit operator Validation<T>(Validation.Invalid invalid) => new Validation<T>(invalid.Errors);

		/// <summary>
		/// Implicitly convert a value into a <see cref="Validation{T}"/> in the Valid state.
		/// </summary>
		/// <param name="value">The value to be associated with the the <see cref="Validation{T}"/> object.</param>
		public static implicit operator Validation<T>(T value) => Valid(value);


		/// <summary>
		/// Calls a specified function depending on the state (i.e. Valid or Invalid) of the Validation object. 
		/// </summary>
		/// <typeparam name="TResult">The type of the return value of the functions that are called as a result of matching.</typeparam>
		/// <param name="invalidFn">The function to be called when the Validation object is in the Invalid state.</param>
		/// <param name="validFn">The function to be called when the Validation object is in the Valid state.</param>
		/// <returns>The value returned by the function that is called as a result of matching.</returns>
		public TResult Match<TResult>(Func<IEnumerable<Error>, TResult> invalidFn, Func<T, TResult> validFn)
			=> IsValid ? validFn(Value) : invalidFn(Errors);

		/// <summary>
		/// Calls a specified Action method depending on the state (i.e. Valid or Invalid) of the Validation object. 
		/// </summary>
		/// <remarks>
		/// Effectively the same as the Match method that works with Func delegates (see above) but allows Action delegates to
		/// be specified instead of Funcs.
		/// </remarks>
		/// <param name="invalidAction">The method to be called when the Validation object is in the Invalid state.</param>
		/// <param name="validAction">The method to be called when the Validation object is in the Valid state.</param>
		/// <returns>A <see cref="Unit"/> (as Action delegate methods have no return value).</returns>
		public Unit Match(Action<IEnumerable<Error>> invalidAction, Action<T> validAction)
			=> Match(invalidAction.ToUnitFunc(), validAction.ToUnitFunc());

		/// <summary>
		/// Returns the Validation object as an enumerable.
		/// </summary>
		/// <remarks>
		/// The returned enumerable will yield a single value when in the Valid state or no value at all when in the Invalid state.
		/// </remarks>
		/// <returns>An <see cref="IEnumerator{T}"/> that yields this Validation object's Value when in the Valid state, or yields
		/// nothing when in the Invalid state.</returns>
		public IEnumerator<T> AsEnumerable()
		{
			if (IsValid)
			{
				yield return Value;
			}
		}

		/// <summary>
		/// Returns a string representation of the <see cref="Validation{T}"/> object's state.
		/// </summary>
		/// <returns>A string representation of the <see cref="Validation{T}"/> object's state.</returns>
		public override string ToString()
			=> IsValid ? $"Valid({Value})" : $"Invalid([{string.Join(", ", Errors)}])";

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <param name="obj">An object with which to check for equality.</param>
		/// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj) => this.ToString() == obj.ToString();   // hack


		/// <summary>
		/// Calculates the hash code for the <see cref="Validation{T}"/> object.
		/// </summary>
		/// <returns>The hash code for the <see cref="Validation{T}"/> object.</returns>
		public override int GetHashCode() => (IsValid, Value, Errors).GetHashCode();

		#endregion
	}

	/// <summary>
	/// Implementation of the static <see cref="Validation"/> class.
	/// </summary>
	public static class Validation
	{
		/// <summary>
		/// Implementation of the static <see cref="Invalid"/> struct.
		/// </summary>
		public struct Invalid
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="Invalid"/> struct with the specified erorrs.
			/// </summary>
			/// <param name="errors">Collection of <see cref="Error"/> objects to be associated with the <see cref="Invalid"/> object.</param>
			public Invalid(IEnumerable<Error> errors) { Errors = errors; }

			/// <summary>
			/// Gets the collection of errors associated with this <see cref="Invalid"/> object.
			/// </summary>
			internal IEnumerable<Error> Errors { get; private set; }
		}
	}
}
