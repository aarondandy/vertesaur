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

namespace Vertesaur.Generation.ExpressionBuilder
{

	/// <summary>
	/// Constant generic operation types.
	/// </summary>
	public enum GenericConstantOperationType {

		// constants

		/// <summary>
		/// Constant for the value, 0.
		/// </summary>
		ValueZero = 0,
		/// <summary>
		/// Constant for the value, 1.
		/// </summary>
		ValueOne = 1,
		/// <summary>
		/// Constant for the value, 2.
		/// </summary>
		ValueTwo = 2,
		/// <summary>
		/// Constant for the value, pi.
		/// </summary>
		ValuePi,
		/// <summary>
		/// Constant for the value, e.
		/// </summary>
		ValueE,

	}

	/// <summary>
	/// Unary generic operation types.
	/// </summary>
	public enum GenericUnaryOperationType {

		// conversions

		/// <summary>
		/// Convert a generic value to a Double value.
		/// </summary>
		ConvertToDouble,
		/// <summary>
		/// Convert a Double value to a generic value.
		/// </summary>
		ConvertFromDouble,
		/// <summary>
		/// Convert a generic value to an Int32 value.
		/// </summary>
		ConvertToInt,
		/// <summary>
		/// Convert an Int32 value to a generic value.
		/// </summary>
		ConvertFromInt,

		// arithmetic

		/// <summary>
		/// Calculates the negative value.
		/// </summary>
		Negate,

	}

	/// <summary>
	/// Binary generic operation types.
	/// </summary>
	public enum GenericBinaryOperationType {

		// arithmetic

		/// <summary>
		/// Adds to values.
		/// </summary>
		Add,
		/// <summary>
		/// Subtracts the later value from another.
		/// </summary>
		Subtract,
		/// <summary>
		/// Multiplies two values.
		/// </summary>
		Multiply,
		/// <summary>
		/// Divides two values.
		/// </summary>
		Divide,

		// comparison

		/// <summary>
		/// Determines equality between two values.
		/// </summary>
		Equal,
		/// <summary>
		/// Determines inequality between two values.
		/// </summary>
		NotEqual,
		/// <summary>
		/// Determines if one value is less than the other value.
		/// </summary>
		Less,
		/// <summary>
		/// Determines if one values is less than or equal to another value.
		/// </summary>
		LessEqual,
		/// <summary>
		/// Determines if one value is greater than the other value.
		/// </summary>
		Greater,
		/// <summary>
		/// Determines if one values is greater than or equal to another value.
		/// </summary>
		GreaterEqual,
		/// <summary>
		/// Determines the smallest of two values.
		/// </summary>
		Min,
		/// <summary>
		/// Determines the largest of two values.
		/// </summary>
		Max,
		/// <summary>
		/// Determines which value compares higher than another. Returns an Int32 value similar to the IComparable interface.
		/// </summary>
		CompareTo,

	}

}
