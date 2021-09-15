namespace PendleCodeMonkey.FunctionalLib
{
	/// <summary>
	/// Partial implementation of the FnLib class.
	/// </summary>
	public static partial class FnLib
	{
		/// <summary>
		/// Helper function to allow an instance of the <see cref="Error"/> class to be created by calling an Error
		/// method, supplying the error emssage text.
		/// </summary>
		/// <param name="message">The error message text.</param>
		/// <returns>A new instance of the <see cref="Error"/> class.</returns>
		public static Error Error(string message) => new Error(message);
	}

	/// <summary>
	/// Implementation of the <see cref="Error"/> class.
	/// </summary>
	public class Error
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Error"/> class.
		/// </summary>
		protected Error()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Error"/> class.
		/// </summary>
		/// <param name="message">The error message text.</param>
		internal Error(string message)
		{
			Message = message;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the error message text.
		/// </summary>
		public virtual string Message { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Implicitly convert a string to an <see cref="Error"/>
		/// </summary>
		/// <param name="message">The error message text.</param>
		public static implicit operator Error(string message) => new Error(message);

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString() => Message;

		#endregion
	}
}
