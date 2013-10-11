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

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Vertesaur.Transformation
{
    /// <summary>
    /// A transformation.
    /// </summary>
    [ContractClass(typeof(CodeContractITransformation))]
    public interface ITransformation
    {
        /// <summary>
        /// Determines if this transformation has an inverse.
        /// </summary>
        bool HasInverse { get; }
        /// <summary>
        /// Gets the inverse of this transformation if one exists.
        /// </summary>
        /// <returns>A transformation.</returns>
        ITransformation GetInverse();
        /// <summary>
        /// Transforms a compatible value.
        /// </summary>
        /// <param name="value">The value to transform from.</param>
        /// <returns>The value after transformation.</returns>
        object TransformValue(object value);
        /// <summary>
        /// Transforms a set if compatible values.
        /// </summary>
        /// <param name="values"></param>
        /// <returns>A transformed set of values.</returns>
        IEnumerable<object> TransformValues(IEnumerable<object> values);
        /// <summary>
        /// Gets the allowable type mappings for this conversion.
        /// </summary>
        /// <returns>The types that can be converted from and to.</returns>
        IEnumerable<KeyValuePair<Type, Type[]>> GetTypeMappings();

    }

    /// <summary>
    /// A transformation from one value and type to another value and type.
    /// </summary>
    /// <typeparam name="TFrom">The type to transform from.</typeparam>
    /// <typeparam name="TTo">The type to transform to.</typeparam>
    [ContractClass(typeof(CodeContractITransformation<,>))]
    public interface ITransformation<TFrom, TTo> :
        ITransformation
    {
        /// <summary>
        /// Transforms a value.
        /// </summary>
        /// <param name="value">The value to transform from.</param>
        /// <returns>The value after transformation.</returns>
        TTo TransformValue(TFrom value);
        /// <summary>
        /// Transforms a set of values.
        /// </summary>
        /// <param name="values">The values transform from.</param>
        /// <returns>A transformed set of values.</returns>
        IEnumerable<TTo> TransformValues(IEnumerable<TFrom> values);
        /// <summary>
        /// Gets the inverse of this transformation if one exists.
        /// </summary>
        /// <returns>A transformation.</returns>
        new ITransformation<TTo, TFrom> GetInverse();
    }

    /// <summary>
    /// Transforms from one value to another where both are the same type.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to transform.</typeparam>
    [ContractClass(typeof(CodeContractITransformation<>))]
    public interface ITransformation<TValue> :
        ITransformation<TValue, TValue>
    {
        /// <summary>
        /// Transforms a set of values.
        /// </summary>
        /// <param name="values">The values to be transformed in place.</param>
        void TransformValues(TValue[] values);

        /// <summary>
        /// Gets the inverse of this transformation if one exists.
        /// </summary>
        /// <returns>A transformation.</returns>
        new ITransformation<TValue> GetInverse();
    }

    [ContractClassFor(typeof(ITransformation))]
    internal abstract class CodeContractITransformation : ITransformation
    {

        public abstract bool HasInverse { [Pure] get; }

        public ITransformation GetInverse() {
            Contract.Requires(HasInverse);
            Contract.Ensures(Contract.Result<ITransformation>() != null);
            throw new NotImplementedException();
        }

        public abstract object TransformValue(object value);

        public virtual IEnumerable<object> TransformValues(IEnumerable<object> values) {
            Contract.Requires(values != null);
            Contract.Ensures(Contract.Result<IEnumerable<object>>() != null);
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<Type, Type[]>> GetTypeMappings() {
            Contract.Ensures(Contract.Result<IEnumerable<Tuple<Type, Type[]>>>() != null);
            throw new NotImplementedException();
        }
    }

    [ContractClassFor(typeof(ITransformation<,>))]
    internal abstract class CodeContractITransformation<TFrom, TTo> : ITransformation<TFrom, TTo>
    {

        public abstract TTo TransformValue(TFrom value);

        public IEnumerable<TTo> TransformValues(IEnumerable<TFrom> values) {
            Contract.Requires(values != null);
            Contract.Ensures(Contract.Result<IEnumerable<TTo>>() != null);
            throw new NotImplementedException();
        }

        public ITransformation<TTo, TFrom> GetInverse() {
            Contract.Requires(HasInverse);
            Contract.Ensures(Contract.Result<ITransformation<TTo, TFrom>>() != null);
            throw new NotImplementedException();
        }

        public abstract bool HasInverse { get; }

        ITransformation ITransformation.GetInverse() {
            throw new NotImplementedException();
        }

        public abstract object TransformValue(object value);

        public abstract IEnumerable<object> TransformValues(IEnumerable<object> values);

        public abstract IEnumerable<KeyValuePair<Type, Type[]>> GetTypeMappings();
    }

    [ContractClassFor(typeof(ITransformation<>))]
    internal abstract class CodeContractITransformation<TValue> : ITransformation<TValue>
    {

        public void TransformValues(TValue[] values) {
            Contract.Requires(values != null);
            throw new NotImplementedException();
        }

        public ITransformation<TValue> GetInverse() {
            Contract.Requires(HasInverse);
            Contract.Ensures(Contract.Result<ITransformation<TValue>>() != null);
            throw new NotImplementedException();
        }

        public abstract TValue TransformValue(TValue value);

        public abstract IEnumerable<TValue> TransformValues(IEnumerable<TValue> values);

        ITransformation<TValue, TValue> ITransformation<TValue, TValue>.GetInverse() {
            throw new NotImplementedException();
        }

        public abstract bool HasInverse { get; }

        ITransformation ITransformation.GetInverse() {
            throw new NotImplementedException();
        }

        public abstract object TransformValue(object value);

        public abstract IEnumerable<object> TransformValues(IEnumerable<object> values);

        public abstract IEnumerable<KeyValuePair<Type, Type[]>> GetTypeMappings();
    }


}
