using System;
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
		/// Creates a new instance of <see cref="Either.Left{TLeft}"/> with the specified value.
		/// </summary>
		/// <typeparam name="TLeft">The type of the value wrapped by the <see cref="Either.Left{TLeft}"/> instance.</typeparam>
		/// <param name="val">The value to be wrapped.</param>
		/// <returns>A <see cref="Either.Left{TLeft}"/> initialized with the supplied value.</returns>
		public static Either.Left<TLeft> Left<TLeft>(TLeft val) => new Either.Left<TLeft>(val);

		/// <summary>
		/// Creates a new instance of <see cref="Either.Right{TRight}"/> with the specified value.
		/// </summary>
		/// <typeparam name="TRight">The type of the value wrapped by the <see cref="Either.Right{TRight}"/> instance.</typeparam>
		/// <param name="val">The value to be wrapped.</param>
		/// <returns>A <see cref="Either.Right{TRight}"/> initialized with the supplied value.</returns>
		public static Either.Right<TRight> Right<TRight>(TRight val) => new Either.Right<TRight>(val);
	}

	/// <summary>
	/// Implementation of the <see cref="Either{TLeft, TRight}"/> struct.
	/// </summary>
	/// <typeparam name="TLeft">Type of the <b>Left</b> value of the <see cref="Either{TLeft, TRight}"/> instance.</typeparam>
	/// <typeparam name="TRight">Type of the <b>Right</b> value of the <see cref="Either{TLeft, TRight}"/> instance.</typeparam>
	public struct Either<TLeft, TRight>
	{
		/// <summary>
		/// Gets the Left value of the <see cref="Either{TLeft, TRight}"/> instance.
		/// </summary>
		internal TLeft Left { get; }

		/// <summary>
		/// Gets the Right value of the <see cref="Either{TLeft, TRight}"/> instance.
		/// </summary>
		internal TRight Right { get; }

		/// <summary>
		/// Gets a value indicating if the <see cref="Either{TLeft, TRight}"/> is a <b>Right</b> type.
		/// </summary>
		private bool IsRight { get; }

		/// <summary>
		/// Gets a value indicating if the <see cref="Either{TLeft, TRight}"/> is a <b>Left</b> type.
		/// </summary>
		private bool IsLeft => !IsRight;

		/// <summary>
		/// Initializes an instance of <see cref="Either{TLeft, TRight}"/> in the <b>Left</b> state.
		/// </summary>
		/// <param name="left">The value (of type <see cref="TLeft"/>) to be assigned to the <see cref="Either{TLeft, TRight}"/> instance.</param>
		internal Either(TLeft left)
		{
			IsRight = false;
			Left = left;
			Right = default;
		}

		/// <summary>
		/// Initializes an instance of <see cref="Either{TLeft, TRight}"/> in the <b>Right</b> state.
		/// </summary>
		/// <param name="right">The value (of type <see cref="TRight"/>) to be assigned to the <see cref="Either{TLeft, TRight}"/> instance.</param>
		internal Either(TRight right)
		{
			IsRight = true;
			Right = right;
			Left = default;
		}

		/// <summary>
		/// Deconstruct the <see cref="Either{TLeft, TRight}"/> instance into its Left and Right values.
		/// </summary>
		/// <param name="leftValue">The <see cref="TLeft"/> that will be populated with the Left value of this <see cref="Either{TLeft, TRight}"/> instance.</param>
		/// <param name="rightValue">The <see cref="TRight"/> that will be populated with the Right value of this <see cref="Either{TLeft, TRight}"/> instance.</param>
		public void Deconstruct(out TLeft leftValue, out TRight rightValue)
			=> (leftValue, rightValue) = (Left, Right);

		/// <summary>
		/// Operator that allows a <see cref="Either{TLeft, TRight}"/> instance to be implicitly created from a <see cref="TLeft"/> value.
		/// </summary>
		/// <param name="value">The <see cref="TLeft"/> value from which the <see cref="Either{TLeft, TRight}"/> will be created.</param>
		public static implicit operator Either<TLeft, TRight>(TLeft value) => new Either<TLeft, TRight>(value);

		/// <summary>
		/// Operator that allows a <see cref="Either{TLeft, TRight}"/> instance to be implicitly created from a <see cref="TRight"/> value.
		/// </summary>
		/// <param name="value">The <see cref="TRight"/> value from which the <see cref="Either{TLeft, TRight}"/> will be created.</param>
		public static implicit operator Either<TLeft, TRight>(TRight value) => new Either<TLeft, TRight>(value);

		/// <summary>
		/// Operator that allows a <see cref="Either{TLeft, TRight}"/> instance to be implicitly created from a <see cref="Either.Left{TLeft}"/> value.
		/// </summary>
		/// <param name="left">The <see cref="Either.Left{TLeft}"/> value from which the <see cref="Either{TLeft, TRight}"/> will be created.</param>
		public static implicit operator Either<TLeft, TRight>(Either.Left<TLeft> left) => new Either<TLeft, TRight>(left.Value);

		/// <summary>
		/// Operator that allows a <see cref="Either{TLeft, TRight}"/> instance to be implicitly created from a <see cref="Either.Left{TRight}"/> value.
		/// </summary>
		/// <param name="right">The <see cref="Either.Left{TRight}"/> value from which the <see cref="Either{TLeft, TRight}"/> will be created.</param>
		public static implicit operator Either<TLeft, TRight>(Either.Right<TRight> right) => new Either<TLeft, TRight>(right.Value);

		/// <summary>
		/// Check if the specified object is equal to this <see cref="Either{TLeft, TRight}"/> instance.
		/// </summary>
		/// <param name="obj">The object being checked for equality.</param>
		/// <returns><c>true</c> if the specified object is equal to this <see cref="Either{TLeft, TRight}"/> instance; otherwise <c>false</c>.</returns>
		public override bool Equals(object obj) => obj is Either<TLeft, TRight> either && EithersAreEqual(this, either);

		/// <summary>
		/// Returns the hash code for this <see cref="Either{TLeft, TRight}"/> instance.
		/// </summary>
		/// <returns>The 32-bit hash code.</returns>
		public override int GetHashCode() => IsLeft ? Left.GetHashCode() : Right.GetHashCode();

		/// <summary>
		/// Determines if two <see cref="Either{TLeft, TRight}"/> instances are equal.
		/// </summary>
		/// <param name="x">The first <see cref="Either{TLeft, TRight}"/> instance to be compared.</param>
		/// <param name="y">The second <see cref="Either{TLeft, TRight}"/> instance to be compared.</param>
		/// <returns><c>true</c> if the two <see cref="Either{TLeft, TRight}"/> instances are equal; otherwise <c>false</c>.</returns>
		public static bool operator ==(Either<TLeft, TRight> x, Either<TLeft, TRight> y) => EithersAreEqual(x, y);

		/// <summary>
		/// Determines if two <see cref="Either{TLeft, TRight}"/> instances are not equal.
		/// </summary>
		/// <param name="x">The first <see cref="Either{TLeft, TRight}"/> instance to be compared.</param>
		/// <param name="y">The second <see cref="Either{TLeft, TRight}"/> instance to be compared.</param>
		/// <returns><c>true</c> if the two <see cref="Either{TLeft, TRight}"/> instances are not equal; otherwise <c>false</c>.</returns>
		public static bool operator !=(Either<TLeft, TRight> x, Either<TLeft, TRight> y) => !EithersAreEqual(x, y);

		/// <summary>
		/// Match function for <see cref="Either{TLeft, TRight}"/>.
		/// Executes one of two supplied functions depending on the state of the <see cref="Either{TLeft, TRight}"/> instance.
		/// </summary>
		/// <typeparam name="TResult">The type of the returned value.</typeparam>
		/// <param name="leftFn">Function that is executed when the <see cref="Either{TLeft, TRight}"/> is in the <b>Left</b> state.</param>
		/// <param name="rightFn">Function that is executed when the <see cref="Either{TLeft, TRight}"/> is in the <b>Right</b> state.</param>
		/// <returns>The value (of type TResult) returned by the function (leftFn or rightFn) that was executed.</returns>
		public TResult Match<TResult>(Func<TLeft, TResult> leftFn, Func<TRight, TResult> rightFn)
			=> IsLeft ? leftFn(Left) : rightFn(Right);

		/// <summary>
		/// Match function for <see cref="Either{TLeft, TRight}"/>. 
		/// Executes one of two supplied methods depending on the state of the <see cref="Either{TLeft, TRight}"/> instance.
		/// </summary>
		/// <param name="leftAction">Method that is executed when the <see cref="Either{TLeft, TRight}"/> is in the <b>Left</b> state.</param>
		/// <param name="rightAction">Method that is executed when the <see cref="Either{TLeft, TRight}"/> is in the <b>Left</b> state.</param>
		/// <returns>A <see cref="Unit"/> (i.e. no return value - because Action delegates return no value).</returns>
		public Unit Match(Action<TLeft> leftAction, Action<TRight> rightAction)
			=> Match(leftAction.ToUnitFunc(), rightAction.ToUnitFunc());

		/// <summary>
		/// Returns a string representation of the <see cref="Either{TLeft, TRight}"/> object's state.
		/// </summary>
		/// <returns>String representation of the <see cref="Either{TLeft, TRight}"/> object's state.</returns>
		public override string ToString() => Match(l => $"Left({l})", r => $"Right({r})");

		/// <summary>
		/// Determines if tow specified <see cref="Either{TLeft, TRight}"/> instances are equal.
		/// </summary>
		/// <param name="x">The first <see cref="Either{TLeft, TRight}"/> instance to be compared.</param>
		/// <param name="y">The second <see cref="Either{TLeft, TRight}"/> instance to be compared.</param>
		/// <returns><c>true</c> if the two <see cref="Either{TLeft, TRight}"/> instances are equal; otherwise <c>false</c>.</returns>
		private static bool EithersAreEqual(Either<TLeft, TRight> x, Either<TLeft, TRight> y)
		{
			if (x.IsLeft != y.IsLeft)
			{
				return false;
			}

			if (x.IsLeft)
			{
				return x.Left.Equals(y.Left);
			}

			return x.Right.Equals(y.Right);
		}
	}

	/// <summary>
	/// Implementation of the static <see cref="Either"/> class.
	/// </summary>
	public static class Either
	{
		/// <summary>
		/// Implementation of the <see cref="Left{TLeft}"/> struct.
		/// </summary>
		/// <typeparam name="TLeft">Type of the value wrapped by the <see cref="Left{TLeft}"/> instance.</typeparam>
		public struct Left<TLeft>
		{
			/// <summary>
			/// Gets the value wrapped by the <see cref="Left{TLeft}"/> instance.
			/// </summary>
			internal TLeft Value { get; }

			/// <summary>
			/// Initializes an instance of the <see cref="Left{TLeft}"/> struct with the specified value.
			/// </summary>
			/// <param name="value">The value with which the <see cref="Left{TLeft}"/> instance should be initialized.</param>
			internal Left(TLeft value) { Value = value; }

			/// <summary>
			/// Returns a string representation of the <see cref="Left{TLeft}"/> instance's state.
			/// </summary>
			/// <returns>String representation of the <see cref="Left{TLeft}"/> instance's state.</returns>
			public override string ToString() => $"Left({Value})";
		}

		/// <summary>
		/// Implementation of the <see cref="Right{TRight}"/> struct.
		/// </summary>
		/// <typeparam name="TRight">Type of the value wrapped by the <see cref="Right{TRight}"/> instance.</typeparam>
		public struct Right<TRight>
		{
			/// <summary>
			/// Gets the value wrapped by the <see cref="Right{TRight}"/> instance.
			/// </summary>
			internal TRight Value { get; }

			/// <summary>
			/// Initializes an instance of the <see cref="Right{TRight}"/> struct with the specified value.
			/// </summary>
			/// <param name="value">The value with which the <see cref="Right{TRight}"/> instance should be initialized.</param>
			internal Right(TRight value) { Value = value; }

			/// <summary>
			/// Returns a string representation of the <see cref="Right{TRight}"/> instance's state.
			/// </summary>
			/// <returns>String representation of the <see cref="Right{TRight}"/> instance's state.</returns>
			public override string ToString() => $"Right({Value})";

//			public Right<TRightResult> Map<TLeft, TRightResult>(Func<TRight, TRightResult> f) => Right(f(Value));
//			public Either<TLeft, TRightResult> Bind<TLeft, TRightResult>(Func<TRight, Either<TLeft, TRightResult>> f) => f(Value);
		}
	}
}
