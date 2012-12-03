// ===============================================================================
//
// Copyright (c) 2012 Aaron Dandy 
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
using JetBrains.Annotations;
using Vertesaur.Contracts;
using System.Linq.Expressions;

namespace Vertesaur.Transformation
{

	/// <summary>
	/// A concatenated transformation that chains all component transformations at run-time into a single typed transformation operation.
	/// </summary>
	/// <typeparam name="TFrom">The type to transform from.</typeparam>
	/// <typeparam name="TTo">The type to transform to.</typeparam>
	public class CompiledConcatenatedTransformation<TFrom,TTo> : ConcatenatedTransformation<TFrom, TTo>
	{

		[NotNull]
		private Expression BuildSingleTransformExpression([NotNull] Expression input) {
			Contract.Requires(input != null);
			Contract.Ensures(Contract.Result<Expression>() != null);
			Contract.EndContractBlock();

			var fromType = typeof(TFrom);
			var toType = typeof(TTo);
			if(TransformationPath.Count == 0)
				return (fromType == toType) ? input : Expression.Convert(input, toType);

			var exp = input;
			for (int i = 0; i < TransformationPath.Count; i++) {
				var txInfo = TransformationPath[i];
				exp = Expression.Call(Expression.Constant(txInfo.Core), txInfo.GetTransformValueMethod(), new[] {exp});
			}
			return exp;
		}

		private readonly Func<TFrom, TTo> _singleTransform; // TODO: lazy?

		/// <summary>
		/// Creates a new concatenated transformation that is JIT compiled into a single typed expression.
		/// </summary>
		/// <param name="transformations"></param>
		public CompiledConcatenatedTransformation([NotNull, InstantHandle] IEnumerable<ITransformation> transformations)
			: base(transformations)
		{
			Contract.Requires(transformations != null);
			Contract.EndContractBlock();

			var singleParam = Expression.Parameter(typeof(TFrom), "x");
			_singleTransform = Expression.Lambda<Func<TFrom, TTo>>(
				BuildSingleTransformExpression(singleParam),
				singleParam
			).Compile();
		}

		/// <inheritdoc/>
		public override TTo TransformValue(TFrom value) {
			return _singleTransform(value);
		}

		/// <inheritdoc/>
		public override IEnumerable<TTo> TransformValues(IEnumerable<TFrom> values) {
			return values.Select(_singleTransform);
		}

		/// <inheritdoc/>
		[NotNull]
		public new CompiledConcatenatedTransformation<TTo, TFrom> GetInverse() {
			Contract.Requires(HasInverse);
			Contract.Ensures(Contract.Result<CompiledConcatenatedTransformation<TTo, TFrom>>() != null);
			Contract.EndContractBlock();
			return (CompiledConcatenatedTransformation<TTo, TFrom>)CreateInverseConcatenatedOperation();
		}

		/// <inheritdoc/>
		protected override ConcatenatedTransformation CreateInverseConcatenatedOperation() {
			return new CompiledConcatenatedTransformation<TTo, TFrom>(CreateInverseOperations());
		}

	}

	/// <summary>
	/// A concatenated transformation that chains all component transformations at run-time into a single typed transformation operation.
	/// </summary>
	/// <typeparam name="TValue">The element type to convert from and to.</typeparam>
	public class CompiledConcatenatedTransformation<TValue> : CompiledConcatenatedTransformation<TValue, TValue>, ITransformation<TValue>
	{

		/// <summary>
		/// Constructs a new concatenated transformation.
		/// </summary>
		/// <param name="transformations">The transformations that will compose the concatenated operation.</param>
		public CompiledConcatenatedTransformation([NotNull, InstantHandle] IEnumerable<ITransformation> transformations)
			: base(transformations)
		{
			Contract.Requires(transformations != null);
			Contract.EndContractBlock();
		}

		/// <inheritdoc/>
		public void TransformValues(TValue[] values) {
			for(int i = 0; i < values.Length; i++)
				values[i] = TransformValue(values[i]);
		}

		/// <inheritdoc/>
		protected override ConcatenatedTransformation CreateInverseConcatenatedOperation() {
			return new CompiledConcatenatedTransformation<TValue>(CreateInverseOperations());
		}

		/// <inheritdoc/>
		[NotNull]
		public new CompiledConcatenatedTransformation<TValue> GetInverse() {
			Contract.Requires(HasInverse);
			Contract.Ensures(Contract.Result<CompiledConcatenatedTransformation<TValue>>() != null);
			Contract.EndContractBlock();

			return (CompiledConcatenatedTransformation<TValue>)CreateInverseConcatenatedOperation();
		}

		/// <inheritdoc/>
		ITransformation<TValue> ITransformation<TValue>.GetInverse() {
			return GetInverse();
		}
	}

}
