﻿using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Collections;

namespace Vertesaur
{

    /// <summary>
    /// A 2D axis aligned minimum bounding rectangle, sometimes known as an envelope.
    /// </summary>
    /// <typeparam name="TValue">The coordinate value type.</typeparam>
    [ContractClass(typeof(CodeContractMbr<>))]
    public interface IMbr<out TValue> :
        IPlanarGeometry
    {
        /// <summary>
        /// The minimum encompassed value on the x-axis. 
        /// </summary>
        TValue XMin { get; }
        /// <summary>
        /// The maximum encompassed value on the x-axis. 
        /// </summary>
        TValue XMax { get; }
        /// <summary>
        /// The minimum encompassed value on the y-axis. 
        /// </summary>
        TValue YMin { get; }
        /// <summary>
        /// The maximum encompassed value on the y-axis. 
        /// </summary>
        TValue YMax { get; }
        /// <summary>
        /// The width of the MBR.
        /// </summary>
        TValue Width { get; }
        /// <summary>
        /// The height of the MBR.
        /// </summary>
        TValue Height { get; }

    }

    [ContractClassFor(typeof(IMbr<>))]
    internal abstract class CodeContractMbr<TValue> : IMbr<TValue>
    {

        public abstract TValue XMin { get; }

        public abstract TValue XMax { get; }

        public abstract TValue YMin { get; }

        public abstract TValue YMax { get; }

        public abstract TValue Width { get; }

        public abstract TValue Height { get; }

        [ContractInvariantMethod]
        [Conditional("CONTRACTS_FULL")]
        private void CodeContractInvariant() {
            Contract.Invariant(Comparer.Default.Compare(XMin, XMax) <= 0);
            Contract.Invariant(Comparer.Default.Compare(YMin, YMax) <= 0);
            Contract.Invariant(Comparer.Default.Compare(0, Width) <= 0);
            Contract.Invariant(Comparer.Default.Compare(0, Height) <= 0);
        }

    }

}
