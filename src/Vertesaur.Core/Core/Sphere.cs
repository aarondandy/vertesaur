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
using System.Globalization;

namespace Vertesaur
{
    /// <summary>
    /// A perfect sphere defined by a radius.
    /// </summary>
    public struct Sphere :
        ISpheroid<double>,
        IEquatable<ISpheroid<double>>,
        IEquatable<Sphere>
    {

        /// <inheritdoc/>
        public static bool operator ==(Sphere a, Sphere b) {
            return a.Equals(b);
        }

        /// <inheritdoc/>
        public static bool operator !=(Sphere a, Sphere b) {
            return !a.Equals(b);
        }

        /// <summary>
        /// The radius of the sphere.
        /// </summary>
        public readonly double Radius;

        /// <summary>
        /// Creates a new sphere defined by the given <paramref name="radius"/>.
        /// </summary>
        /// <param name="radius">The radius of the sphere.</param>
        public Sphere(double radius) {
            Radius = radius;
        }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.A { get { return Radius; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.B { get { return Radius; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.F { get { return 0.0; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.InvF { get { return Double.PositiveInfinity; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.E { get { return 0.0; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.ESquared { get { return 0.0; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.ESecond { get { return 0.0; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.ESecondSquared { get { return 0.0; } }

        /// <inheritdoc/>
        public override bool Equals(object obj) {
            if (obj is Sphere)
                return Equals((Sphere)obj);
            return Equals(obj as ISpheroid<double>);
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            return Radius.GetHashCode();
        }

        /// <inheritdoc/>
        public bool Equals(ISpheroid<double> other) {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return (
                !ReferenceEquals(null, other)
                && Radius == other.A
                && (
                    Radius == other.B
                    || other.F == 0
                )
            );
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <inheritdoc/>
        public bool Equals(Sphere other) {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return Radius == other.Radius;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <inheritdoc/>
        public override string ToString() {
            Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));
            var result = String.Format("r:" + Radius.ToString(CultureInfo.InvariantCulture));
            Contract.Assume(!String.IsNullOrEmpty(result));
            return result;
        }

    }
}
