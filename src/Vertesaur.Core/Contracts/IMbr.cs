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

using System.Diagnostics.Contracts;
using System.Collections;

namespace Vertesaur.Contracts {

	/// <summary>
	/// A 2D axis aligned minimum bounding rectangle, sometimes known as an envelope.
	/// </summary>
	/// <typeparam name="TValue">The element type.</typeparam>
	[ContractClass(typeof(CodeContractMbr<>))]
	public interface IMbr<out TValue> :
		IPlanarGeometry
	{
		/// <summary>
		/// The minimum encompassed value on the x-axis. 
		/// </summary>
		TValue XMin { get; }
		/// <summary>
		/// The maximum encompassed value on the x-axis. 
		/// </summary>
		TValue XMax { get; }
		/// <summary>
		/// The minimum encompassed value on the y-axis. 
		/// </summary>
		TValue YMin { get; }
		/// <summary>
		/// The maximum encompassed value on the y-axis. 
		/// </summary>
		TValue YMax { get; }
		/// <summary>
		/// The width of the MBR.
		/// </summary>
		TValue Width { get; }
		/// <summary>
		/// The height of the MBR.
		/// </summary>
		TValue Height { get; }

	}

	[ContractClassFor(typeof(IMbr<>))]
	internal abstract class CodeContractMbr<TValue> : IMbr<TValue>
	{

		private CodeContractMbr() { }

		public TValue XMin {
			get { throw new System.NotImplementedException(); }
		}

		public TValue XMax {
			get { throw new System.NotImplementedException(); }
		}

		public TValue YMin {
			get { throw new System.NotImplementedException(); }
		}

		public TValue YMax {
			get { throw new System.NotImplementedException(); }
		}

		public TValue Width {
			get { throw new System.NotImplementedException(); }
		}

		public TValue Height {
			get { throw new System.NotImplementedException(); }
		}

		[ContractInvariantMethod]
		private void CodeContractInvariant() {
			Contract.Invariant(Comparer.Default.Compare(XMin, XMax) <= 0);
			Contract.Invariant(Comparer.Default.Compare(YMin, YMax) <= 0);
			Contract.Invariant(Comparer.Default.Compare(0, Width) <= 0);
			Contract.Invariant(Comparer.Default.Compare(0, Height) <= 0);
		}

	}

}
