using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Vertesaur.Utility
{
	internal static class ArrayUtility
	{

		public static void Sort<T>(this T[] array, Comparison<T> comparison) {
			Contract.Requires(null != array);
			Contract.Requires(null != comparison);
			Array.Sort(array,comparison);
		}

		public static ReadOnlyCollection<T> AsReadOnly<T>(this T[] array) {
			Contract.Requires(null != array);
#if NETFX_CORE
			return new ReadOnlyCollection<T>(array);
#else
			return Array.AsReadOnly(array);
#endif
		}

#if NETFX_CORE || WINDOWS_PHONE || SILVERLIGHT
		public static TTo[] ConvertAll<TFrom, TTo>(this TFrom[] array, Func<TFrom, TTo> converter) {
			Contract.Requires(null != array);
			Contract.Requires(null != converter);
			var result = new TTo[array.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = converter(array[i]);
			}
			return result;
		}
#else
		public static TTo[] ConvertAll<TFrom, TTo>(this TFrom[] array, Converter<TFrom, TTo> converter) {
			Contract.Requires(null != array);
			Contract.Requires(null != converter);
			return Array.ConvertAll(array, converter);
		}
#endif

	}
}
