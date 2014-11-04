using System.Diagnostics.Contracts;

namespace Vertesaur.Utility
{
    internal static class IterationUtils
    {
        // TODO: better as an out param?
        [Pure]
        public static int AdvanceLoopingIndex(int index, int count) {
            Contract.Requires(index >= 0);
            Contract.Ensures(Contract.Result<int>() >= 0);
            Contract.Ensures(Contract.Result<int>() < count);
            Contract.EndContractBlock();
            Contract.Assume(count > 0);

            index++;
            return index >= count ? 0 : index;
        }

        // TODO: better as an out param?
        [Pure]
        public static int RetreatLoopingIndex(int index, int count) {
            Contract.Requires(index < count);
            Contract.Ensures(Contract.Result<int>() >= 0);
            Contract.Ensures(Contract.Result<int>() < count);
            Contract.EndContractBlock();
            Contract.Assume(count > 0);

            index--;
            return index < 0 ? count - 1 : index;
        }



    }
}
