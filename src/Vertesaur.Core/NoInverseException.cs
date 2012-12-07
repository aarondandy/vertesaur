using System;
using JetBrains.Annotations;

namespace Vertesaur
{

	/// <summary>
	/// An exception that is thrown when a request for an inverse transformation is not available.
	/// </summary>
	/// <remarks>
	/// Often a property such as <c>HasInverse</c> should be checked before attempting to get an inverse of a transformation.
	/// </remarks>
	public class NoInverseException : Exception
	{

		private const string DefaultMessage = "There is no valid inverse.";

		/// <summary>
		/// Throw a default no inverse exception.
		/// </summary>
		public NoInverseException() : base(null) { }

		/// <summary>
		/// Throw a no inverse exception with a custom message.
		/// </summary>
		/// <param name="message"></param>
		public NoInverseException([CanBeNull] string message) : base(message ?? DefaultMessage) { }

	}
}
