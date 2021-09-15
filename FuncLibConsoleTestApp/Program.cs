using PendleCodeMonkey.FunctionalLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static PendleCodeMonkey.FunctionalLib.FnLib;

namespace PendleCodeMonkey.FuncLibConsoleTestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			bool quit = false;
			while (!quit)
			{
				Console.WriteLine();
				Console.WriteLine("1 - Test Option");
				Console.WriteLine("2 - Test Memoization");
				Console.WriteLine("3 - Test Validation");
				Console.WriteLine("4 - Test ValueOrException");
				Console.WriteLine("5 - Test Either");
				Console.WriteLine("X - EXIT");
				Console.WriteLine();
				var input = Console.ReadLine();

				switch (input)
				{
					case "1":
						TestOption();
						break;
					case "2":
						TestMemoize();
						break;
					case "3":
						TestValidation();
						break;
					case "4":
						TestValueOrException();
						break;
					case "5":
						TestEither();
						break;
					case "x":
					case "X":
						quit = true;
						break;
				}
			}
		}

		static void OutputSeparatorLine()
		{
			Console.WriteLine();
			Console.WriteLine("----------------------------------------------------------------------------------");
			Console.WriteLine();
		}

		static void TestOption()
		{
			Console.WriteLine("OPTION TESTS");
			Console.WriteLine();

			static int DoubleValue(int x) => x * 2;

			static string OutputResult(Option<int> opt)
				=> opt.Match(
					NoneFn: () => "No value",
					SomeFn: x => $"Doubled value is: {x}");

			Option<int> optValue1 = Some(20);
			Option<int> optValue2 = None;
			var test = optValue1.Map(DoubleValue);
			string msg = OutputResult(test);
			Console.Write("Option with value of 20 - ");
			Console.WriteLine(msg);

			test = optValue2.Map(DoubleValue);
			msg = OutputResult(test);
			Console.Write("Option with no value - ");
			Console.WriteLine(msg);

			OutputSeparatorLine();
		}

		static void TestMemoize()
		{
			Console.WriteLine("MEMOIZATION TESTS");
			Console.WriteLine();

			// Recursive function that calculates the n'th value in the Fibonacci sequence (an excellent candidate for memoization)
			Func<int, int> fibonacci = null;
			fibonacci = n => n > 1 ? fibonacci(n - 1) + fibonacci(n - 2) : n;

			double frequency = Stopwatch.Frequency;
			double microsecPerTick = (1000 * 1000) / frequency;

			int count = 40;         // value to be passed to the fibonacci function.

			// Perform (and time) the calculation without memoization (i.e. calling the fibonacci function as-is)
			var sw = Stopwatch.StartNew();
			var result1 = fibonacci(count);
			sw.Stop();
			Console.WriteLine($"Without memoization - Result: {result1}. Time taken: {(sw.ElapsedTicks * microsecPerTick):F3} (microseconds)");

			// Now memoize the fibonacci function.
			fibonacci = fibonacci.Memoize();

			// Perform (and time) the calculation with memoization (i.e. calling the memoized fibonacci function)
			sw = Stopwatch.StartNew();
			var result2 = fibonacci(count);
			sw.Stop();
			Console.WriteLine($"   With memoization - Result: {result2}. Time taken: {(sw.ElapsedTicks * microsecPerTick):F3} (microseconds)");

			OutputSeparatorLine();
		}

		static void TestValidation()
		{
			Console.WriteLine("VALIDATION TESTS");
			Console.WriteLine();

			static Validation<int> ValidateValueLessThan100(int value)
			{
				if (value > 100)
				{
					return Invalid("Value out of range.");
				}
				else return value;
			}

			static Validation<int> ParseInteger(string value)
			{
				if (int.TryParse(value, out int intVal))
				{
					return intVal;
				}

				return Invalid("Unable to parse the string as an integer value.");
			}

			static void WriteResult(Validation<int> validate)
			{
				string msg = validate.Match(
					invalidFn: error => $"Error: {error.Head().Value}",
					validFn: value => $"Value: {value}");

				Console.WriteLine(msg);
			}

			Console.Write("Checking if value of 110 is in range - ");
			var validate = ValidateValueLessThan100(110);
			WriteResult(validate);

			Console.Write("Parsing string '123' - ");
			validate = ParseInteger("123");
			WriteResult(validate);

			Console.Write("Parsing string 'ABCD' - ");
			validate = ParseInteger("ABCD");
			WriteResult(validate);

			OutputSeparatorLine();
		}

		static void TestValueOrException()
		{
			Console.WriteLine("VALUE OR EXCEPTION TESTS");
			Console.WriteLine();

			static ValueOrException<int> Divide(int value1, int value2)
			{
				try
				{
					return value1 / value2;
				}
				catch (Exception ex)
				{
					return ex;
				}
			}

			static void WriteResult(ValueOrException<int> valOrEx)
			{
				string msg = valOrEx.Match(
					exceptionFn: ex => $"Exception: {ex.Message}",
					successFn: value => $"Value: {value}");

				Console.WriteLine(msg);
			}

			Console.Write("100/0 - ");
			WriteResult(Divide(100, 0));

			Console.Write("100/5 - ");
			WriteResult(Divide(100, 5));

			OutputSeparatorLine();
		}

		static void TestEither()
		{
			Console.WriteLine("EITHER TESTS");
			Console.WriteLine();

			var squaresCache = new Dictionary<int, int>()
			{
				{ 2, 4 },
				{ 5, 25 },
				{ 10, 100 }
			};

			// Gets the square of the supplied integer value.
			// Returns an Either<int, int> with the Left value containing the result if the value was retrieved from the dictionary
			// otherwise the Right contains the calculated result.
			Either<int, int> Square(int x)
			{
				// If the value is found in the dictionary then return the cached value in the 'Left' of the Either object.
				if (squaresCache.ContainsKey(x))
				{
					return Left(squaresCache[x]);
				}

				// Otherwise calculate the square of x and return it in the 'Right' of the Either object.
				return Right(x * x);
			}

			void PerformCalc_OutputUsingMatch(int x)
			{
				var message = Square(x).Match(
					leftFn: cached => $"Result retrieved from cache: {cached}",
					rightFn: calc => $"Calculated result: {calc}");

				Console.WriteLine($"Square of {x} - {message}");
			}

			// Call using a value that is in the dictionary.
			PerformCalc_OutputUsingMatch(5);

			// Call using a value that is not in the dictionary.
			PerformCalc_OutputUsingMatch(6);

			OutputSeparatorLine();
		}


	}
}
