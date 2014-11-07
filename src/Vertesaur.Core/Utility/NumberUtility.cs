using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Vertesaur.Utility
{
    internal static class NumberUtility
    {

        // TODO: [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Swap(ref double a, ref double b) {
            var t = a;
            a = b;
            b = t;
        }

    }
}
