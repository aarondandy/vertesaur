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
using System.Diagnostics.Contracts;

namespace Vertesaur.PolygonOperation
{

    /// <summary>
    /// Options to be used by various polygon operations.
    /// </summary>
    public class PolygonOperationOptions
    {

        /// <summary>
        /// Creates a copy of the given object or the default object when given <c>null</c>.
        /// </summary>
        /// <param name="options">The object to clone.</param>
        /// <returns>A new object.</returns>
        public static PolygonOperationOptions CloneOrDefault(PolygonOperationOptions options) {
            Contract.Ensures(Contract.Result<PolygonOperationOptions>() != null);

            return null == options
                ? new PolygonOperationOptions()
                : new PolygonOperationOptions(options);
        }

        /// <summary>
        /// Default constructor initializes all options as defaults.
        /// </summary>
        public PolygonOperationOptions() { }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="options">Object to copy.</param>
        public PolygonOperationOptions(PolygonOperationOptions options) {
            if (null == options) throw new ArgumentNullException("options");
            Contract.EndContractBlock();
            InvertResult = options.InvertResult;
        }

        /// <summary>
        /// When true the result should be inverted.
        /// </summary>
        public bool InvertResult { get; set; }

    }
}
