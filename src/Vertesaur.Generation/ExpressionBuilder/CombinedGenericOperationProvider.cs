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

using System.Collections.Generic;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{

	/// <summary>
	/// Combines various generic operation providers together.
	/// Provides a requested expression from the first provider to successfully build one.
	/// </summary>
	public class CombinedGenericOperationProvider : IGenericOperationProvider {

		private readonly List<IGenericOperationProvider> _operationProviders;

		/// <summary>
		/// Constructs a new combined operation provider from the given operation providers.
		/// </summary>
		/// <param name="operationProviders">The providers to combine.</param>
		public CombinedGenericOperationProvider(IEnumerable<IGenericOperationProvider> operationProviders) {
			_operationProviders = (null == operationProviders)
				? new List<IGenericOperationProvider>()
				: new List<IGenericOperationProvider>(operationProviders);
		}

		/// <summary>
		/// Returns the first constant expression that was successfully created by a provider.
		/// </summary>
		/// <param name="operationType">The operation type.</param>
		/// <returns>An operation expression, or <c>null</c> on failure.</returns>
		public Expression GetConstantExpression(GenericConstantOperationType operationType) {
			foreach(var prov in _operationProviders) {
				Expression tempExp;
				if (null != (tempExp = prov.GetConstantExpression(operationType))) {
					return tempExp;
				}
			}
			return null;
		}

		/// <summary>
		/// Returns the first unary expression that was successfully created by a provider.
		/// </summary>
		/// <param name="input">The unary parameter expression.</param>
		/// <param name="operationType">The operation type.</param>
		/// <returns>An operation expression, or <c>null</c> on failure.</returns>
		public Expression GetUnaryExpression(Expression input, GenericUnaryOperationType operationType) {
			foreach (var prov in _operationProviders) {
				Expression tempExp;
				if (null != (tempExp = prov.GetUnaryExpression(input, operationType))) {
					return tempExp;
				}
			}
			return null;
		}

		/// <summary>
		/// Returns the first binary expression that was successfully created by a provider.
		/// </summary>
		/// <param name="leftHandSide">The left hand side binary parameter expression.</param>
		/// <param name="rightHandSide">The right hand side binary parameter expression.</param>
		/// <param name="operationType">The operation type.</param>
		/// <returns>An operation expression, or <c>null</c> on failure.</returns>
		public Expression GetBinaryExpression(Expression leftHandSide, Expression rightHandSide, GenericBinaryOperationType operationType) {
			foreach (var prov in _operationProviders) {
				Expression tempExp;
				if (null != (tempExp = prov.GetBinaryExpression(leftHandSide,rightHandSide, operationType))) {
					return tempExp;
				}
			}
			return null;
		}
	}
}
