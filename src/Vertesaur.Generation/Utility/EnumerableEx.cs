using System.Diagnostics.Contracts;

namespace Vertesaur.Generation.Utility
{
    internal static class EnumerableEx
    {

        [ContractVerification(false)] // TODO: remove when CC bugs are fixed
        [Pure]
        public static bool ContainsNull<T>(this T[] items) where T : class {
            Contract.Requires(items != null);
            Contract.Ensures(Contract.Result<bool>() == !Contract.ForAll(items, x => x != null));
            foreach (var item in items) {
                if (item == null) {
                    return true;
                }
            }
            return false;
        }

    }
}
