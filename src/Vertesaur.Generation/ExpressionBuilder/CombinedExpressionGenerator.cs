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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{

	/// <summary>
	/// Combines various generic operation providers together.
	/// Provides a requested expression from the first provider to successfully build one.
	/// </summary>
	public class CombinedExpressionGenerator : IExpressionGenerator {

		private readonly IExpressionGenerator[] _expressionGenerators;

		/// <summary>
		/// Constructs a new combined expression generators from the given operation providers.
		/// </summary>
		/// <param name="expressionGenerators">The ordered list of generators to combine.</param>
		public CombinedExpressionGenerator([NotNull] IEnumerable<IExpressionGenerator> expressionGenerators) {
			if (null == expressionGenerators) throw new ArgumentNullException("expressionGenerators");
			Contract.Ensures(_expressionGenerators != null);
			Contract.EndContractBlock();
			_expressionGenerators = expressionGenerators.ToArray();

		}

		/// <inheritdoc/>
		public Expression GenerateExpression(IExpressionGenerationRequest request) {
			if (null == request) throw new ArgumentNullException("request");
			Contract.EndContractBlock();
			return _expressionGenerators
				.Select(x => x.GenerateExpression(request))
				.FirstOrDefault(x => null != x);
		}
	}
}
