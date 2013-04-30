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
using Vertesaur.Contracts;

namespace Vertesaur
{
    /// <summary>
    /// An equatorial spheroid defined by the semi-major axis and the flattening value.
    /// </summary>
    public struct SpheroidEquatorialF :
        ISpheroid<double>,
        IEquatable<ISpheroid<double>>,
        IEquatable<SpheroidEquatorialF>
    {

        /// <inheritdoc/>
        [Pure]
        public static bool operator ==(SpheroidEquatorialF a, SpheroidEquatorialF b) {
            return a.Equals(b);
        }

        /// <inheritdoc/>
        [Pure]
        public static bool operator !=(SpheroidEquatorialF a, SpheroidEquatorialF b) {
            return !a.Equals(b);
        }

        /// <summary>
        /// The semi-major axis.
        /// </summary>
        public readonly double A;

        /// <summary>
        /// The flattening value.
        /// </summary>
        public readonly double F;

        /// <summary>
        /// Creates a new equatorial spheroid defined by the given semi-major axis and flattening value.
        /// </summary>
        /// <param name="a">Semi-major axis.</param>
        /// <param name="f">Flattening value.</param>
        public SpheroidEquatorialF(double a, double f) {
            A = a;
            F = f;
        }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.A { get { return A; } }

        /// <inheritdoc/>
        public double B { [Pure] get { return A * (1.0 - F); } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.F { get { return F; } }

        /// <inheritdoc/>
        public double InvF {
            [Pure] get { return 1.0 / F; }
        }

        /// <inheritdoc/>
        public double E {
            [Pure] get { return F * Math.Sqrt((2.0 / F) - 1); }
        }

        /// <inheritdoc/>
        public double ESquared {
            [Pure] get { return F * F * ((2 / F) - 1); }
        }

        /// <inheritdoc/>
        public double ESecond {
            [Pure] get {
                return Math.Sqrt((2 / F) - 1)
                    / ((1 / F) - 1);
            }
        }

        /// <inheritdoc/>
        public double ESecondSquared {
            [Pure] get {
                var x = (2 / F) - 1;
                return x / ((1 / (F * F)) - x);
            }
        }

        /// <inheritdoc/>
        [Pure] public bool Equals(ISpheroid<double> other) {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return !ReferenceEquals(null, other)
                && A == other.A
                && (
                    F == other.F
                    || B == other.B
                );
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) {
            if (obj is SpheroidEquatorialF)
                return Equals((SpheroidEquatorialF)obj);
            return Equals(obj as ISpheroid<double>);
        }

        /// <inheritdoc/>
        [Pure] public override int GetHashCode() {
            return A.GetHashCode();
        }

        /// <inheritdoc/>
        [Pure] public bool Equals(SpheroidEquatorialF other) {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return A == other.A && F == other.F;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }
    }
}
