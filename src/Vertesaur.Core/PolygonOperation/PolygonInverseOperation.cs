// ===============================================================================
//
// Copyright (c) 2011 Aaron Dandy 
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

using System.Diagnostics.Contracts;
using System.Linq;

namespace Vertesaur.PolygonOperation
{
	/// <summary>
	/// An operation that will 
	/// </summary>
	public static class PolygonInverseOperation
	{

		/// <summary>
		/// Inverts a polygon.
		/// </summary>
		/// <param name="polygon">The polygon to get the inverse of.</param>
		/// <returns>An inverted polygon.</returns>
		public static Polygon2 Invert(Polygon2 polygon) {
			Contract.Ensures(polygon == null ? Contract.Result<Polygon2>() == null : Contract.Result<Polygon2>() != null);
			return null == polygon ? null : new Polygon2(polygon.Select(Invert));
		}

		/// <summary>
		/// Inverts a ring.
		/// </summary>
		/// <param name="ring">The ring to get the inverse of.</param>
		/// <returns>An inverted polygon.</returns>
		public static Ring2 Invert(Ring2 ring) {
			Contract.Ensures(ring == null ? Contract.Result<Ring2>() == null : Contract.Result<Ring2>() != null);
			return null == ring ? null : ring.GetInverse();
		}
	}
}
