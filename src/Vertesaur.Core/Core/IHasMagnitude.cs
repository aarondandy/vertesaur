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
    /// Provides functionality to calculate the magnitude of an object.
    /// </summary>
    /// <typeparam name="TValue">The magnitude result value type.</typeparam>
    public interface IHasMagnitude<out TValue>
    {

        /// <summary>
        /// Calculates the magnitude of this instance.
        /// </summary>
        /// <returns>The magnitude.</returns>
        /// <remarks>
        /// <para>
        /// If squared magnitude is required use <see cref="Vertesaur.IHasMagnitude{TValue}.GetMagnitudeSquared()"/>.
        /// </para>
        /// <para>
        /// Note that this could also represent the length of an object or the perimeter of an object.
        /// </para>
        /// </remarks>
        TValue GetMagnitude();
        /// <summary>
        /// Calculates the squared magnitude of this instance.
        /// </summary>
        /// <returns>The squared magnitude.</returns>
        /// <remarks>
        /// <para>
        /// While most users of this library will be interested in the
        /// <see cref="Vertesaur.IHasMagnitude{TValue}.GetMagnitude()">magnitude</see>
        /// or length of an object some algorithms and situations will require or
        /// benefit from the squared magnitude of an object. The squared magnitude
        /// often requires less operations and may be able to avoid a call to the
        /// square-root function. For some situations such as ordering objects by
        /// magnitude the squared magnitude may offer some benefit.
        /// </para>
        /// <para>
        /// Note that this could also represent the squared length of an object or the squared perimeter of an object.
        /// </para>
        /// </remarks>
        TValue GetMagnitudeSquared();

    }

}
