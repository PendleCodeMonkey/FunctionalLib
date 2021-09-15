using System;

namespace PendleCodeMonkey.FunctionalLib
{
	using static FnLib;

	public static class LinqExtensions
	{
		//
		// Option<T> LINQ extension methods.
		//

		public static Option<R> Select<T, R>(this Option<T> opt, Func<T, R> func)
			=> opt.Map(func);

		public static Option<T> Where<T>(this Option<T> opt, Func<T, bool> predicate)
			=> opt.Match(
			() => None,
			(t) => predicate(t) ? opt : None);

		public static Option<RR> SelectMany<T, R, RR>(this Option<T> opt, Func<T, Option<R>> bind, Func<T, R, RR> project)
			=> opt.Match(
			() => None,
			(t) => bind(t).Match(
				() => None,
				(r) => Some(project(t, r))));


		//
		// Either<L, R> LINQ extension methods.
		//

		public static Either<L, R> Select<L, T, R>(this Either<L, T> either, Func<T, R> map)
			=> either.Map(map);

		public static Either<L, RR> SelectMany<L, T, R, RR>(this Either<L, T> either, Func<T, Either<L, R>> bind, Func<T, R, RR> project)
		   => either.Match(
			  leftFn: l => Left(l),
			  rightFn: t =>
				 bind(either.Right).Match<Either<L, RR>>(
					leftFn: l => Left(l),
					rightFn: r => project(t, r)));


		//
		// Validation<T> LINQ extension methods.
		//

		public static Validation<R> Select<T, R>(this Validation<T> valiidation, Func<T, R> map)
			=> valiidation.Map(map);

		public static Validation<RR> SelectMany<T, R, RR>(this Validation<T> valiidation, Func<T, Validation<R>> bind, Func<T, R, RR> project)
			=> valiidation.Match(
				invalidFn: (err) => Invalid(err),
				validFn: (t) => bind(t).Match(
					invalidFn: (err) => Invalid(err),
					validFn: (r) => Valid(project(t, r))));


		//
		// ValueOrException<T> LINQ extension methods.
		//

		public static ValueOrException<R> Select<T, R>(this ValueOrException<T> valOrExcept, Func<T, R> map)
			=> valOrExcept.Map(map);

		public static ValueOrException<RR> SelectMany<T, R, RR>(this ValueOrException<T> valOrExcept,
		   Func<T, ValueOrException<R>> bind, Func<T, R, RR> project)
		{
			if (valOrExcept.Exception)
			{
				return new ValueOrException<RR>(valOrExcept.Ex);
			}
			var bound = bind(valOrExcept.Value);
			return bound.Exception ? new ValueOrException<RR>(bound.Ex) : project(valOrExcept.Value, bound.Value);
		}
	}
}
