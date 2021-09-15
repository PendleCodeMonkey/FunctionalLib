using System;
using Unit = System.ValueTuple;

namespace PendleCodeMonkey.FunctionalLib
{
	using static FnLib;

	/// <summary>
	/// Implementation of extension methods for the <see cref="Either{TLeft, TRight}"/> struct.
	/// </summary>
	public static class EitherExtensions
	{
		/// <summary>
		/// Map function for <see cref="Either{TLeft, TRight}"/>.
		/// </summary>
		/// <typeparam name="TLeft">Type of the <b>Left</b> value of the <see cref="Either{TLeft, TRight}"/> instance.</typeparam>
		/// <typeparam name="TRight">Type of the <b>Right</b> value of the <see cref="Either{TLeft, TRight}"/> instance.</typeparam>
		/// <typeparam name="TRightResult">Type of the <b>Right</b> result value.</typeparam>
		/// <param name="thisEither">The <see cref="Either{TLeft, TRight}"/> instance to which Map is being applied.</param>
		/// <param name="f">The function to be called when the <see cref="Either{TLeft, TRight}"/> is a <b>Right</b> type.</param>
		/// <returns>A <see cref="Either{TLeft, TRightResult}"/> containing the <b>Left</b> value or the mapped <b>Right</b> value.</returns>
		public static Either<TLeft, TRightResult> Map<TLeft, TRight, TRightResult>(this Either<TLeft, TRight> thisEither, Func<TRight, TRightResult> f)
			=> thisEither.Match<Either<TLeft, TRightResult>>(
				l => Left(l),
				r => Right(f(r)));

		/// <summary>
		/// Map function for <see cref="Either{TLeft, TRight}"/>.
		/// </summary>
		/// <typeparam name="TLeft">Type of the <b>Left</b> value of the <see cref="Either{TLeft, TRight}"/> instance.</typeparam>
		/// <typeparam name="TLeftResult">Type of the <b>Left</b> result value.</typeparam>
		/// <typeparam name="TRight">Type of the <b>Right</b> value of the <see cref="Either{TLeft, TRight}"/> instance.</typeparam>
		/// <typeparam name="TRightResult">Type of the <b>Right</b> result value.</typeparam>
		/// <param name="thisEither">The <see cref="Either{TLeft, TRight}"/> instance to which Map is being applied.</param>
		/// <param name="leftFn">The function to be called when the <see cref="Either{TLeft, TRight}"/> is a <b>Left</b> type.</param>
		/// <param name="rightFn">The function to be called when the <see cref="Either{TLeft, TRight}"/> is a <b>Right</b> type.</param>
		/// <returns>A <see cref="Either{TLeftResult, TRightResult}"/> containing the mapped <b>Left</b> or <b>Right</b> value.</returns>
		public static Either<TLeftResult, TRightResult> Map<TLeft, TLeftResult, TRight, TRightResult>
			(this Either<TLeft, TRight> thisEither, Func<TLeft, TLeftResult> leftFn, Func<TRight, TRightResult> rightFn)
				=> thisEither.Match<Either<TLeftResult, TRightResult>>(
					l => Left(leftFn(l)),
					r => Right(rightFn(r)));

		/// <summary>
		/// ForEach function for <see cref="Either{TLeft, TRight}"/>.
		/// </summary>
		/// <typeparam name="TLeft">Type of the <b>Left</b> value of the <see cref="Either{TLeft, TRight}"/> instance.</typeparam>
		/// <typeparam name="TRight">Type of the <b>Right</b> value of the <see cref="Either{TLeft, TRight}"/> instance.</typeparam>
		/// <param name="thisEither">The <see cref="Either{TLeft, TRight}"/> instance to which ForEach is being applied.</param>
		/// <param name="act">The method to be called on the <b>Left</b> or <b>Right</b> value.</param>
		/// <returns>A <see cref="Either{TLeft, Unit}"/>.</returns>
		public static Either<TLeft, Unit> ForEach<TLeft, TRight>(this Either<TLeft, TRight> thisEither, Action<TRight> act)
			=> Map(thisEither, act.ToUnitFunc());

		/// <summary>
		/// Bind function for <see cref="Either{TLeft, TRight}"/>.
		/// </summary>
		/// <typeparam name="TLeft">Type of the <b>Left</b> value of the <see cref="Either{TLeft, TRight}"/> instance.</typeparam>
		/// <typeparam name="TRight">Type of the <b>Right</b> value of the <see cref="Either{TLeft, TRight}"/> instance.</typeparam>
		/// <typeparam name="TRightResult">Type of the <b>Right</b> result value.</typeparam>
		/// <param name="thisEither">The <see cref="Either{TLeft, TRight}"/> instance to which Bind is being applied.</param>
		/// <param name="f">The function to be called when the <see cref="Either{TLeft, TRight}"/> is a <b>Right</b> type.</param>
		/// <returns>A <see cref="Either{TLeft, TRightResult}"/> containing the <b>Left</b> value or the bound <b>Right</b> value.</returns>
		public static Either<TLeft, TRightResult> Bind<TLeft, TRight, TRightResult>(this Either<TLeft, TRight> thisEither, Func<TRight, Either<TLeft, TRightResult>> f)
			=> thisEither.Match(
				l => Left(l),
				r => f(r));
	}
}
