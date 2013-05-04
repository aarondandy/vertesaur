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

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Vertesaur
{
    /// <summary>
    /// An equatorial spheroid defined by the semi-major axis and inverse flattening value.
    /// </summary>
    public struct SpheroidEquatorialInvF :
        ISpheroid<double>,
        IEquatable<ISpheroid<double>>,
        IEquatable<SpheroidEquatorialInvF>
    {

        /// <inheritdoc/>
        public static bool operator ==(SpheroidEquatorialInvF a, SpheroidEquatorialInvF b) {
            return a.Equals(b);
        }

        /// <inheritdoc/>
        public static bool operator !=(SpheroidEquatorialInvF a, SpheroidEquatorialInvF b) {
            return !a.Equals(b);
        }

        /// <summary>
        /// The semi-major axis.
        /// </summary>
        public readonly double A;
        /// <summary>
        /// The inverse flattening value.
        /// </summary>
        public readonly double InvF;

        /// <summary>
        /// Creates an equatorial spheroid defined by the given semi-major axis and inverse flattening value.
        /// </summary>
        /// <param name="a">Semi-major axis.</param>
        /// <param name="invF">Inverse flattening value.</param>
        public SpheroidEquatorialInvF(double a, double invF) {
            A = a;
            InvF = invF;
        }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.A { get { return A; } }

        /// <inheritdoc/>
        public double B { [Pure] get { return A * (1.0 - (1.0 / InvF)); } }

        /// <inheritdoc/>
        public double F { [Pure] get { return 1.0 / InvF; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.InvF { get { return InvF; } }

        /// <inheritdoc/>
        public double E { [Pure] get { return Math.Sqrt((2 * InvF) - 1) / InvF; } }

        /// <inheritdoc/>
        public double ESquared { [Pure] get { return ((2 * InvF) - 1) / (InvF * InvF); } }

        /// <inheritdoc/>
        public double ESecond { [Pure] get { return Math.Sqrt((2 * InvF) - 1) / (InvF - 1); } }

        /// <inheritdoc/>
        public double ESecondSquared {
            [Pure] get {
                double im1 = InvF - 1;
                return ((2.0 * InvF) - 1.0) / (im1 * im1);
            }
        }

        /// <inheritdoc/>
        [Pure] public bool Equals(ISpheroid<double> other) {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return !ReferenceEquals(null, other)
                && A == other.A
                && (
                    InvF == other.InvF
                    || B == other.B
                );
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <inheritdoc/>
        [Pure] public override bool Equals(object obj) {
            if (obj is SpheroidEquatorialInvF)
                return Equals((SpheroidEquatorialInvF)obj);
            return Equals(obj as ISpheroid<double>);
        }

        /// <inheritdoc/>
        [Pure] public override int GetHashCode() {
            return A.GetHashCode();
        }

        /// <inheritdoc/>
        [Pure] public bool Equals(SpheroidEquatorialInvF other) {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return A == other.A && InvF == other.InvF;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }
    }
}
