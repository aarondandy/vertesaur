using System.Collections;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Vertesaur.CodeContracts
{

	internal static class CodeContractHelper
	{

		[ContractAbbreviator]
		[Conditional("CONTRACTS_FULL")]
		internal static void RequiresNotNullOrEmpty<T>(T list) where T: IList {
			// ReSharper disable CompareNonConstrainedGenericWithNull
			Contract.Requires(list != null);
			Contract.Requires(list.Count > 0);
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}

		[ContractAbbreviator]
		[Conditional("CONTRACTS_FULL")]
		internal static void RequiresListIndexValid<T>(T list, int index) where T : IList {
			RequiresNotNullOrEmpty(list);
			Contract.Requires(index >= 0);
			Contract.Requires(index < list.Count);
		}

	}
}
