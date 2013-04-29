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

namespace Vertesaur.Contracts
{
    /// <summary>
    /// Defines operations to calculate the distance between two objects.
    /// </summary>
    /// <typeparam name="TValue">The type for a distance result.</typeparam>
    /// <typeparam name="TObject">The type to find the distance to.</typeparam>
    /// <remarks>
    /// Squared distance is also provided to possibly reduce error resulting
    /// from too many operations as well as to possibly increase performance
    /// when a caller uses the appropriate method.
    /// </remarks>
    public interface IHasDistance<in TObject, out TValue>
    {
        /// <summary>
        /// Calculates the distance between this instance and another.
        /// </summary>
        /// <param name="o">The object to calculate distance to.</param>
        /// <returns>The distance value.</returns>
        TValue Distance(TObject o);
        /// <summary>
        /// Calculates the squared distance between this instance and another.
        /// </summary>
        /// <param name="o">The object to calculate squared distance to.</param>
        /// <returns>The squared distance value.</returns>
        TValue DistanceSquared(TObject o);
    }
}
