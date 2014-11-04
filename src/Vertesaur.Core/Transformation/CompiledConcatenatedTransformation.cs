using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

namespace Vertesaur.Transformation
{

    /// <summary>
    /// A concatenated transformation that chains all component transformations at run-time into a single typed transformation operation.
    /// </summary>
    /// <typeparam name="TFrom">The type to transform from.</typeparam>
    /// <typeparam name="TTo">The type to transform to.</typeparam>
    public class CompiledConcatenatedTransformation<TFrom, TTo> : ConcatenatedTransformation<TFrom, TTo>
    {

        /// <summary>
        /// Creates a new concatenated transformation that is JIT compiled into a single typed expression.
        /// </summary>
        /// <param name="transformations"></param>
        public CompiledConcatenatedTransformation(IEnumerable<ITransformation> transformations) : base(transformations) {
            Contract.Requires(transformations != null);
            Contract.Requires(Contract.ForAll(transformations, x => x != null));
            var singleParam = Expression.Parameter(typeof(TFrom), "x");
            _singleTransform = Expression.Lambda<Func<TFrom, TTo>>(
                BuildSingleTransformExpression(singleParam),
                singleParam
            ).Compile();
        }

        private readonly Func<TFrom, TTo> _singleTransform; // TODO: lazy?

        [ContractInvariantMethod]
        private void CodeContractInvariants() {
            Contract.Invariant(_singleTransform != null);
        }

        private Expression BuildSingleTransformExpression(Expression input) {
            Contract.Requires(input != null);
            Contract.Ensures(Contract.Result<Expression>() != null);

            var fromType = typeof(TFrom);
            var toType = typeof(TTo);
            if (TransformationPath.Length == 0)
                return (fromType == toType) ? input : Expression.Convert(input, toType);

            var exp = input;
            for (int i = 0; i < TransformationPath.Length; i++) {
                var txInfo = TransformationPath[i];
                Contract.Assume(txInfo != null);
                exp = Expression.Call(Expression.Constant(txInfo.Core), txInfo.GetTransformValueMethod(), new[] { exp });
            }
            return exp;
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
        public new CompiledConcatenatedTransformation<TTo, TFrom> GetInverse() {
            if (!HasInverse) throw new NoInverseException();
            Contract.Ensures(Contract.Result<CompiledConcatenatedTransformation<TTo, TFrom>>() != null);
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
        public CompiledConcatenatedTransformation(IEnumerable<ITransformation> transformations)
            : base(transformations) {
            Contract.Requires(transformations != null);
            Contract.Requires(Contract.ForAll(transformations, x => x != null));
        }

        /// <inheritdoc/>
        public void TransformValues(TValue[] values) {
            for (int i = 0; i < values.Length; i++)
                values[i] = TransformValue(values[i]);
        }

        /// <inheritdoc/>
        protected override ConcatenatedTransformation CreateInverseConcatenatedOperation() {
            return new CompiledConcatenatedTransformation<TValue>(CreateInverseOperations());
        }

        /// <inheritdoc/>
        public new CompiledConcatenatedTransformation<TValue> GetInverse() {
            if (!HasInverse) throw new NoInverseException();
            Contract.Ensures(Contract.Result<CompiledConcatenatedTransformation<TValue>>() != null);
            return (CompiledConcatenatedTransformation<TValue>)CreateInverseConcatenatedOperation();
        }

        /// <inheritdoc/>
        ITransformation<TValue> ITransformation<TValue>.GetInverse() {
            return GetInverse();
        }
    }

}
