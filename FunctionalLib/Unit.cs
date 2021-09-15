using System;

namespace PendleCodeMonkey.FunctionalLib
{
	/// <summary>
	/// A type that has only one possible value (conceptually the same as the F# Unit type.)
	/// </summary>
	public sealed class UnitHJB : IEquatable<UnitHJB>
	{
		#region Constructor

		// Private constructor to prevent instantiation.
		// The only way to obtain an instance of this class is by using the static Value property.
		private UnitHJB()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets an instance of the <see cref="UnitHJB"> class.
		/// </summary>
		public static UnitHJB Value => default;

		#endregion

		#region Methods

		public override string ToString() => "Unit";

		public override int GetHashCode() => 0;

		// Equals operator - always returns true because two UnitHJB objects are guaranteed to be equal.
		public static bool operator ==(UnitHJB a, UnitHJB b) => true;

		// Not equals operator - always returns false because two UnitHJB objects are guaranteed to be equal.
		public static bool operator !=(UnitHJB a, UnitHJB b) => false;

		// Always returns true because another UnitHJB is guaranteed to be equal to this instance.
		public bool Equals(UnitHJB other) => true;

		// Only returns true if the supplied object is a UnitHJB (in which case the two objects are guaranteed to be equal)
		public override bool Equals(object obj) => obj is UnitHJB;

		#endregion
	}
}
