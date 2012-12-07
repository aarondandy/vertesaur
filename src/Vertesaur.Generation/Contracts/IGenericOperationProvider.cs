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

using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Vertesaur.Generation.ExpressionBuilder;

namespace Vertesaur.Generation.Contracts{

	/// <summary>
	/// Defines operations required to retrieve and build generic operations.
	/// </summary>
	/// <remarks>
	/// A method should return <c>null</c> if the requested expression cannot be built.
	/// </remarks>
	[ContractClass(typeof(GenericOperationProviderCodeContract))]
	public interface IGenericOperationProvider {

		/// <summary>
		/// Generates an expression for a constant value.
		/// </summary>
		/// <returns>An expression for a constant value.</returns>
		[CanBeNull] Expression GetConstantExpression(GenericConstantOperationType operationType);

		/// <summary>
		/// Generates an expression for a unary operation.
		/// </summary>
		/// <param name="input">The parameter for the operation.</param>
		/// <param name="operationType">The type of operation.</param>
		/// <returns>An expression for an operation, or <c>null</c>.</returns>
		[CanBeNull] Expression GetUnaryExpression([NotNull] Expression input, GenericUnaryOperationType operationType);

		/// <summary>
		/// Generates an expression for a binary operation.
		/// </summary>
		/// <param name="leftHandSide">The parameter for the left side of the operation.</param>
		/// <param name="rightHandSide">the parameter for the right side of the operation.</param>
		/// <param name="operationType">The type of operation.</param>
		/// <returns>An expression for an operation, or <c>null</c>.</returns>
		[CanBeNull] Expression GetBinaryExpression([NotNull] Expression leftHandSide, [NotNull] Expression rightHandSide, GenericBinaryOperationType operationType);

	}

	[ContractClassFor(typeof(IGenericOperationProvider))]
	internal abstract class GenericOperationProviderCodeContract : IGenericOperationProvider
	{

		public Expression GetConstantExpression(GenericConstantOperationType operationType) {
			throw new System.NotImplementedException();
		}

		public Expression GetUnaryExpression(Expression input, GenericUnaryOperationType operationType) {
			Contract.Requires(input != null);
			Contract.EndContractBlock();
			throw new System.NotImplementedException();
		}

		public Expression GetBinaryExpression(Expression leftHandSide, Expression rightHandSide, GenericBinaryOperationType operationType) {
			Contract.Requires(leftHandSide != null);
			Contract.Requires(rightHandSide != null);
			Contract.EndContractBlock();
			throw new System.NotImplementedException();
		}
	}

}
