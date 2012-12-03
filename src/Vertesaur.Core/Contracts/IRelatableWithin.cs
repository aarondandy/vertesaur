// ===============================================================================
//
// Copyright (c) 2011,2012 Aaron Dandy 
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

using JetBrains.Annotations;

namespace Vertesaur.Contracts {
	/// <summary>
	/// Functionality to determine if an object can be contained within another object.
	/// </summary>
	/// <typeparam name="TObject">The object type which may contain this instance.</typeparam>
	public interface IRelatableWithin<in TObject>
	{
		/// <summary>
		/// Determines if the given object contains this instance and this instance
		/// does not intersect the <paramref name="other"/> exterior.
		/// </summary>
		/// <param name="other">The object to test.</param>
		/// <returns>True if this instance is within the given object.</returns>
		/// <remarks>
		/// An object is within another if there is an intersection between interiors
		/// and there is no intersection between the interior of this instance and the
		/// exterior of the other instance.
		/// </remarks>
		bool Within([CanBeNull] TObject other);
	}
}
