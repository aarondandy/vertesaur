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

namespace Vertesaur {
	
	/// <summary>
	/// A relative direction based on context.
	/// </summary>
	public enum RelativeDirectionType : short {

		/// <summary>
		/// No direction. This could also represent the relative origin.
		/// </summary>
		None = 0,
		/// <summary>
		/// A direction to the right of the origin.
		/// </summary>
		Right = 1,
		/// <summary>
		/// A direction to the left of the origin.
		/// </summary>
		Left = -1,
		/// <summary>
		/// A direction above the origin.
		/// </summary>
		Up = 2,
		/// <summary>
		/// A direction below the origin.
		/// </summary>
		Down = -2,
		/// <summary>
		/// A direction in front of the origin.
		/// </summary>
		Front = 3,
		/// <summary>
		/// A direction behind the origin.
		/// </summary>
		Back = -3 

	}

}
