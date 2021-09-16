# A C# Functional Programming Library #

This repo contains the code for a C# functional programming library, implementing classes for many functional programming constructs as well as some other useful utility functionality.

The FunctionalLib solution consists of the following projects:

- **FuncLibConsoleTestApp**: A very basic console application that demonstrates some functionality of the library.
- **FunctionalLib**: The code for the library itself.
- **FunctionalLibTests**: A handful of unit tests.

Some of the functionality in this library is quite heavily based on Enrico Buonanno's excellent book "Functional Programming in C#", published by Manning.

### Prerequisites

The **FunctionalLib** project is a .NET Standard 2.0 class library; therefore, any version of .NET that implements .NET Standard 2.0 can be used for client applications.  
The **FuncLibConsoleTestApp** and **FunctionalLibTests** projects are both .NET Core 3.1 Console applications.
  
<br>

## The main constructs implemented in the library are: 

| Construct | Description
|---:| ---
| Option | Can be in one of two states - `Some` (which wraps a value) or `None` (which indicates that no value is present).
| Either | A type that can be used to represent two possible outcomes - `Left` or `Right`.
| Validation | Can be in one of two states - `Valid` (and therefore holds a value) or `Invalid` (and holds a collection of validation errors).
| ValueOrException | Can be in one of two states - `Success` (which wraps a value) or `Exception` (and holds the Exception object).
| Union | A type that can represent a value of one of a pre-determined set of types.
| Memoize | Can be applied to expensive function calls, returning a cached result when the same inputs occur again.
  
<br>

The library also implements functionality to enable `Currying` and `Partial Application` of function parameters.

