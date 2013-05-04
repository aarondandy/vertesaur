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
    /// Provides functionality to calculate the distance to another object.
    /// </summary>
    /// <typeparam name="TObject">The object type to find the distance to.</typeparam>
    /// <typeparam name="TValue">The distance result value type.</typeparam>
    public interface IHasDistance<in TObject, out TValue>
    {

        /// <summary>
        /// Calculates the distance between this instance and another.
        /// </summary>
        /// <param name="other">The object to calculate distance to.</param>
        /// <returns>The distance to the other object.</returns>
        /// <ensures csharp="!(result &lt; 0)">Result is not negative.</ensures>
        /// <remarks>
        /// If squared distance is required use <see cref="Vertesaur.IHasDistance{TObject,TValue}.DistanceSquared(TObject)"/>.
        /// </remarks>
        TValue Distance(TObject other);

        /// <summary>
        /// Calculates the squared distance between this instance and another.
        /// </summary>
        /// <param name="other">The object to calculate squared distance to.</param>
        /// <returns>The squared distance to the other object.</returns>
        /// <remarks>
        /// While most users of this library will be interested in the
        /// <see cref="Vertesaur.IHasDistance{TObject,TValue}.Distance(TObject)">distance</see>
        /// between two objects some algorithms and situations will require or
        /// benefit from the squared distance between them. The squared distance
        /// often requires less operations and may be able to avoid a call to the
        /// square-root function. For some situations such as ordering objects by
        /// their distance to another object the squared distance may offer some
        /// benefit.
        /// </remarks>
        /// <ensures csharp="!(result &lt; 0)">Result is not negative.</ensures>
        TValue DistanceSquared(TObject other);

    }
}
