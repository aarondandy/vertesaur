using System;
using System.Collections.Generic;

namespace Vertesaur
{
	internal static class SilverlightUtils
	{

		public static void RemoveAll<T>(this List<T> list, Predicate<T> test) {
			for(int i = list.Count-1; i >= 0; i--) {
				if(test(list[i]))
					list.RemoveAt(i);
			}
		}


	}
}
