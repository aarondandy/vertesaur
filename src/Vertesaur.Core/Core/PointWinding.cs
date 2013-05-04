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

namespace Vertesaur
{
    /// <summary>
    /// Describes the point winding for a closed path or ring
    /// </summary>
    /// <remarks>
    /// Many Vertesaur algorithms use CounterClockwise as the standard point ordering or winding
    /// when describing fills for polygons and rings while other systems may use Clockwise.
    /// Winding must be considered when operating on polygons or rings.
    /// It is best to explicitly state if each individual ring or polygon is a fill or hole by
    /// using the <see cref="Vertesaur.Ring2.Hole"/> property of the <see cref="Vertesaur.Ring2"/> class
    /// or equivalent methods or properties.
    /// </remarks>
    /// <seealso cref="Vertesaur.Ring2"/>
    /// <seealso cref="Vertesaur.Polygon2"/>
    public enum PointWinding : byte
    {
        /// <summary>
        /// Winding is unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Clockwise point ordering
        /// </summary>
        Clockwise = 1,
        /// <summary>
        /// Counter clockwise point ordering
        /// </summary>
        CounterClockwise = 2
    }
}
