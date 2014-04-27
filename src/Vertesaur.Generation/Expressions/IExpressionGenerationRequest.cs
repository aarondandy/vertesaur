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
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// An expression generation request.
    /// </summary>
    [ContractClass(typeof(ExpressionGenerationRequestCodeContract))]
    public interface IExpressionGenerationRequest
    {

        /// <summary>
        /// The generator that can be used to generate sub expressions.
        /// </summary>
        IExpressionGenerator TopLevelGenerator { get; }
        /// <summary>
        /// The name of the expression that is to be generated.
        /// </summary>
        string ExpressionName { get; }
        /// <summary>
        /// The function inputs for the expression.
        /// </summary>
        ReadOnlyCollection<Expression> InputExpressions { get; }
        /// <summary>
        /// The optional desired result type for the generated expression.
        /// </summary>
        Type DesiredResultType { get; }

    }

    [ContractClassFor(typeof(IExpressionGenerationRequest))]
    internal abstract class ExpressionGenerationRequestCodeContract : IExpressionGenerationRequest
    {

        public IExpressionGenerator TopLevelGenerator {
            get {
                Contract.Ensures(Contract.Result<IExpressionGenerator>() != null);
                throw new NotImplementedException();
            }
        }

        public string ExpressionName {
            get {
                Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));
                throw new NotImplementedException();
            }
        }

        public ReadOnlyCollection<Expression> InputExpressions {
            get {
                Contract.Ensures(Contract.Result<ReadOnlyCollection<Expression>>() != null);
                Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<Expression>>(), x => x != null));
                throw new NotImplementedException();
            }
        }

        public abstract Type DesiredResultType { get; }
    }

}
