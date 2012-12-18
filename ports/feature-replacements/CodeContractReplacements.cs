using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Vertesaur
{
	/// <summary>
	/// A placeholder for the .NET pure attribute
	/// </summary>
	internal class PureAttribute : Attribute
	{

	}

	[Conditional("IGNORE")]
	internal class ContractClassAttribute : Attribute
	{
		public ContractClassAttribute(Type _) { }
	}

	[Conditional("IGNORE")]
	internal class ContractClassForAttribute : Attribute
	{
		public ContractClassForAttribute(Type _) { }
	}

	[Conditional("IGNORE")]
	internal class ContractInvariantMethodAttribute : Attribute
	{
		
	}

	internal static class Contract
	{
		[Conditional("IGNORE")]
		public static void Assume(bool _) { }

		[Conditional("IGNORE")]
		public static void Ensures(bool _) { }

		[Conditional("IGNORE")]
		public static void Requires(bool _) { }

		[Conditional("IGNORE")]
		public static void Invariant(bool _) { }

		public static T OldValue<T>(T o) {
			return o;
		}

		public static T ValueAtReturn<T>(out T o) {
			o = default(T);
			return default(T);
		}

		public static T Result<T>() {
			return default(T);
		}

		[Conditional("IGNORE")]
		public static void EndContractBlock() { }

		public static bool ForAll<T>(IEnumerable<T> _1, Predicate<T> _2) {
			return false;
		}

		public static bool ForAll(int i1, int i2, Predicate<int> _) {
			return false;
		}
	}

}