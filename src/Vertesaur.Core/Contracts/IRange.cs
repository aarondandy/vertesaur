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

using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Collections;

namespace Vertesaur.Contracts
{

    /// <summary>
    /// A range which encompasses all values between two values, inclusive.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <remarks>
    /// A range where the high and low values are equal is a range that covers only one value.
    /// </remarks>
    [ContractClass(typeof(CodeContractRange<>))]
    public interface IRange<out TValue> :
        IHasMagnitude<TValue>
    {
        /// <summary>
        /// The lowest value of the range.
        /// </summary>
        TValue Low { get; }
        /// <summary>
        /// The highest value of the range.
        /// </summary>
        TValue High { get; }
        /// <summary>
        /// The average of the lowest and highest value.
        /// </summary>
        TValue Mid { get; }
    }

    [ContractClassFor(typeof(IRange<>))]
    internal abstract class CodeContractRange<TValue> : IRange<TValue>
    {

        public abstract TValue Low { get; }

        public abstract TValue High { get; }

        public abstract TValue Mid { get; }

        public abstract TValue GetMagnitude();

        public abstract TValue GetMagnitudeSquared();

        [ContractInvariantMethod]
        [Conditional("CONTRACTS_FULL")]
        private void CodeContractInvariant() {
            Contract.Invariant(Comparer.Default.Compare(Low, High) <= 0);
            Contract.Invariant(Comparer.Default.Compare(Low, Mid) <= 0);
            Contract.Invariant(Comparer.Default.Compare(Mid, High) <= 0);
        }
    }

}
