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

namespace Vertesaur
{
    /// <summary>
    /// Provides functionality to calculate the intersection with another object.
    /// </summary>
    /// <typeparam name="TObject">The object type to calculate the intersection with.</typeparam>
    /// <typeparam name="TResult">the intersection result type.</typeparam>
    public interface IHasIntersectionOperation<in TObject, out TResult>
    {
        /// <summary>
        /// Calculates the intersection between this instance and <paramref name="other">another</paramref>.
        /// </summary>
        /// <param name="other">The object to calculate the intersection with.</param>
        /// <returns>The intersection result.</returns>
        TResult Intersection(TObject other);
    }
}
