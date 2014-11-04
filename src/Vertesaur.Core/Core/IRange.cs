using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Collections;

namespace Vertesaur
{

    /// <summary>
    /// A range which encompasses all values between two values, inclusive.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <remarks>
    /// A range where the high and low values are equal is a range that covers only one value.
    /// </remarks>
    [ContractClass(typeof(CodeContractRange<>))]
    public interface IRange<out TValue> :
        IHasMagnitude<TValue>
    {
        /// <summary>
        /// The lowest value of the range.
        /// </summary>
        TValue Low { get; }
        /// <summary>
        /// The highest value of the range.
        /// </summary>
        TValue High { get; }
        /// <summary>
        /// The average of the lowest and highest value.
        /// </summary>
        TValue Mid { get; }
    }

    [ContractClassFor(typeof(IRange<>))]
    internal abstract class CodeContractRange<TValue> : IRange<TValue>
    {

        public abstract TValue Low { get; }

        public abstract TValue High { get; }

        public abstract TValue Mid { get; }

        public abstract TValue GetMagnitude();

        public abstract TValue GetMagnitudeSquared();

        [ContractInvariantMethod]
        [Conditional("CONTRACTS_FULL")]
        private void CodeContractInvariant() {
            Contract.Invariant(Comparer.Default.Compare(Low, High) <= 0);
            Contract.Invariant(Comparer.Default.Compare(Low, Mid) <= 0);
            Contract.Invariant(Comparer.Default.Compare(Mid, High) <= 0);
        }
    }

}
