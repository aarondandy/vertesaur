using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Vertesaur.Generation.Utility
{
    internal static class EnumerableEx
    {

        [Pure] public static bool ContainsNull<T>(this T[] items) where T : class {
            Contract.Requires(items != null);
            Contract.Ensures(!Contract.ForAll(items, x => x != null) == Contract.Result<bool>());
            foreach (var item in items) {
                if (item == null) {
                    Contract.Assume(!Contract.ForAll(items, x => x != null) == true);
                    Contract.Assume(!Contract.ForAll(items, x => x != null));
                    return true;
                }
            }
            Contract.Assume(!Contract.ForAll(items, x => x != null) == false);
            Contract.Assume(Contract.ForAll(items, x => x != null));
            return false;
        }

    }
}
