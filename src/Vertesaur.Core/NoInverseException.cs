// ===============================================================================
//
// Copyright (c) 2012 Aaron Dandy 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// ===============================================================================

using System;

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
		public NoInverseException(string message) : base(message ?? DefaultMessage) { }

	}
}
