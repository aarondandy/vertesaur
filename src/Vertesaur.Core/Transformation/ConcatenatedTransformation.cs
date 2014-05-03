﻿// ===============================================================================
//
// Copyright (c) 2012,2014 Aaron Dandy 
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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Collections;
using System.Diagnostics.Contracts;
using Vertesaur.Utility;

namespace Vertesaur.Transformation
{

    /// <summary>
    /// A transformation that is composed of a sequence of composite transformations.
    /// </summary>
    public class ConcatenatedTransformation : ITransformation
    {


        /// <summary>
        /// Creates a new concatenated transformation composed of a sequence of transformations.
        /// </summary>
        /// <param name="transformations">A sequence of transformations.</param>
        /// <exception cref="System.ArgumentException">Thrown if a transformation is <c>null</c>.</exception>
        public ConcatenatedTransformation(IEnumerable<ITransformation> transformations) {
            if (null == transformations) throw new ArgumentNullException("transformations");
            Contract.EndContractBlock();

            var txArray = transformations.ToArray();
            for (int i = 0; i < txArray.Length; i++)
                if (txArray[i] == null)
                    throw new ArgumentException("null transformations are not valid.");

            Transformations = txArray.AsReadOnly();

        }

        /// <summary>
        /// Creates a new concatenated transformation composed of a sequence of transformations.
        /// </summary>
        /// <param name="transformations">A sequence of transformations.</param>
        /// <remarks>
        /// The parameter is used as is so make sure to clone before passing it in.
        /// </remarks>
        private ConcatenatedTransformation(ITransformation[] transformations) {
            Contract.Requires(transformations != null);
            Contract.Requires(Contract.ForAll(transformations, x => x != null));
            Transformations = transformations.AsReadOnly();
        }

        [ContractInvariantMethod]
        [Conditional("CONTRACTS_FULL")]
        private void CodeContractInvariant() {
            Contract.Invariant(Transformations != null);
            Contract.Invariant(Contract.ForAll(Transformations, x => x != null));
            Contract.Invariant(Contract.ForAll(0, Transformations.Count, i => Transformations[i] != null));
        }

        /// <summary>
        /// Generates an inverse set of transformation operations that represent the inverse of this transformations..
        /// </summary>
        /// <returns></returns>
        protected ITransformation[] CreateInverseOperations() {
            if (!HasInverse) throw new NoInverseException();
            Contract.Ensures(Contract.Result<ITransformation[]>() != null);
            Contract.Ensures(Contract.ForAll(Contract.Result<ITransformation[]>(), x => x != null));

            var inverseTransformations = new ITransformation[Transformations.Count];
            for (int i = 0; i < inverseTransformations.Length; i++) {
                var tx = Transformations[Transformations.Count - 1 - i];
                Contract.Assume(tx != null);
                Contract.Assume(tx.HasInverse);
                var ix = tx.GetInverse();
                inverseTransformations[i] = ix;
            }
            return inverseTransformations;
        }

        /// <summary>
        /// Creates a new concatenated transformation that is the inverse of this transformation.
        /// </summary>
        /// <returns>The inverse transformation.</returns>
        protected virtual ConcatenatedTransformation CreateInverseConcatenatedOperation() {
            Contract.Requires(HasInverse);
            Contract.Ensures(Contract.Result<ConcatenatedTransformation>() != null);
            return new ConcatenatedTransformation(CreateInverseOperations());
        }

        /// <summary>
        /// Gets the transformations that make up this concatenated transformation.
        /// </summary>
        [Obsolete("May move to IEnumerable.")]
        public ReadOnlyCollection<ITransformation> Transformations { get; private set; }

        /// <summary>
        /// Creates a new concatenated transformation that is the inverse of this transformation.
        /// </summary>
        /// <returns>The inverse transformation.</returns>
        public ConcatenatedTransformation GetInverse() {
            Contract.Requires(HasInverse);
            Contract.Ensures(Contract.Result<ConcatenatedTransformation>() != null);
            return CreateInverseConcatenatedOperation();
        }

        ITransformation ITransformation.GetInverse() {
            return GetInverse();
        }

        /// <inheritdoc/>
        public bool HasInverse {
            get {
                if (Transformations.Count == 0)
                    return true;
                for (int i = 0; i < Transformations.Count; i++) {
                    Contract.Assume(Transformations[i] != null);
                    if (!Transformations[i].HasInverse)
                        return false;
                }
                return true;
            }
        }

        public object TransformValue(object value) {
            foreach (var tx in Transformations)
                value = tx.TransformValue(value);
            return value;
        }

        public IEnumerable<object> TransformValues(IEnumerable<object> values) {
            if(values == null) throw new ArgumentNullException("values");
            Contract.Ensures(Contract.Result<IEnumerable<object>>() != null);
            return values.Select(TransformValue);
        }

        public Type[] GetInputTypes() {
            if (Transformations.Count == 0)
                return EmptyArray<Type>.Value;
            Contract.Assume(Transformations[0] != null);
            return Transformations[0].GetInputTypes();
        }

        public Type[] GetOutputTypes(Type inputType) {
            var types = new []{inputType};
            foreach (var tx in Transformations) {
                if (types.Length == 0)
                    break;
                types = types
                    .SelectMany(tx.GetOutputTypes)
                    .Distinct()
                    .ToArray();
            }
            return types;
        }
    }

    /// <summary>
    /// A transformation that is composed of a sequence of transformations as a chained expression.
    /// </summary>
    public class ConcatenatedTransformation<TFrom, TTo> : ConcatenatedTransformation, ITransformation<TFrom, TTo>
    {

        /// <summary>
        /// Creates a new concatenated transformation composed of a sequence of transformations.
        /// </summary>
        /// <param name="transformations">A sequence of transformations.</param>
        /// <exception cref="System.InvalidOperationException">Thrown when a valid casting path can not be determined.</exception>
        public ConcatenatedTransformation(IEnumerable<ITransformation> transformations)
            : base(transformations) {
            Contract.Requires(transformations != null);
            Contract.Requires(Contract.ForAll(transformations, x => x != null));

            var path = TransformationCastNode.FindCastPath(Transformations, typeof(TFrom), typeof(TTo));
            if (null == path)
                throw new InvalidOperationException("A concatenated transformation casting path could not be found.");
            TransformationPath = path.AsReadOnly();
        }

        [ContractInvariantMethod]
        [Conditional("CONTRACTS_FULL")]
        private void CodeContractInvariant() {
            Contract.Invariant(null != TransformationPath);
            Contract.Invariant(Contract.ForAll(TransformationPath, x => x != null));
            Contract.Invariant(Contract.ForAll(0, TransformationPath.Count, i => TransformationPath[i] != null));
        }

        /// <summary>
        /// The chosen transformation cast path used when compiling the concatenated transformation.
        /// </summary>
        [Obsolete("May move to IEnumerable")]
        public ReadOnlyCollection<TransformationCastNode> TransformationPath { get; private set; }

        /// <inheritdoc/>
        public virtual TTo TransformValue(TFrom value) {
            if (TransformationPath.Count == 1) {
                Contract.Assume(TransformationPath[0] != null);
                return ((ITransformation<TFrom, TTo>) (TransformationPath[0].Core)).TransformValue(value);
            }

            object tempValue = value;
            for (int i = 0; i < TransformationPath.Count; i++) {
                Contract.Assume(TransformationPath[i] != null);
                tempValue = TransformationPath[i].TransformValue(tempValue);
            }
            if (tempValue == null)
                return default(TTo);
            return (TTo)tempValue;
        }

        /// <inheritdoc/>
        public virtual IEnumerable<TTo> TransformValues(IEnumerable<TFrom> values) {
            Contract.Ensures(Contract.Result<IEnumerable<TTo>>() != null);

            if (Transformations.Count == 1) {
                Contract.Assume(TransformationPath[0] != null);
                return ((ITransformation<TFrom, TTo>) (TransformationPath[0].Core)).TransformValues(values);
            }

            IEnumerable tempValues = values;
            for (int i = 0; i < TransformationPath.Count; i++) {
                Contract.Assume(TransformationPath[i] != null);
                tempValues = TransformationPath[i].TransformValues(tempValues);
            }
            return (IEnumerable<TTo>)tempValues;
        }

        /// <inheritdoc/>
        public new ConcatenatedTransformation<TTo, TFrom> GetInverse() {
            if (!HasInverse) throw new NoInverseException();
            Contract.Ensures(Contract.Result<ConcatenatedTransformation<TTo, TFrom>>() != null);
            return (ConcatenatedTransformation<TTo, TFrom>)CreateInverseConcatenatedOperation();
        }

        /// <inheritdoc/>
        ITransformation<TTo, TFrom> ITransformation<TFrom, TTo>.GetInverse() {
            Contract.Ensures(Contract.Result<ITransformation<TTo, TFrom>>() != null);
            return GetInverse();
        }

        /// <inheritdoc/>
        protected override ConcatenatedTransformation CreateInverseConcatenatedOperation() {
            return new ConcatenatedTransformation<TTo, TFrom>(CreateInverseOperations());
        }

    }

    /// <summary>
    /// A transformation that is composed of a sequence of transformations as a chained expression.
    /// </summary>
    public class ConcatenatedTransformation<TValue> : ConcatenatedTransformation<TValue, TValue>, ITransformation<TValue>
    {
        /// <summary>
        /// Creates a new concatenated transformation composed of the given transformations.
        /// </summary>
        /// <param name="transformations"></param>
        public ConcatenatedTransformation(IEnumerable<ITransformation> transformations)
            : base(transformations) {
            Contract.Requires(transformations != null);
            Contract.Requires(Contract.ForAll(transformations, x => x != null));
        }

        /// <inheritdoc/>
        public virtual void TransformValues(TValue[] values) {
            for (int i = 0; i < values.Length; i++)
                values[i] = TransformValue(values[i]);
        }

        /// <inheritdoc/>
        public new ConcatenatedTransformation<TValue> GetInverse() {
            if (!HasInverse) throw new NoInverseException();
            Contract.Ensures(Contract.Result<ConcatenatedTransformation<TValue>>() != null);
            return (ConcatenatedTransformation<TValue>)CreateInverseConcatenatedOperation();
        }

        /// <inheritdoc/>
        ITransformation<TValue> ITransformation<TValue>.GetInverse() {
            return GetInverse();
        }

        /// <inheritdoc/>
        protected override ConcatenatedTransformation CreateInverseConcatenatedOperation() {
            Contract.Ensures(Contract.Result<ConcatenatedTransformation>() != null);
            return new ConcatenatedTransformation<TValue>(CreateInverseOperations());
        }

    }

}