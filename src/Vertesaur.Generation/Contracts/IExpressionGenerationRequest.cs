using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Vertesaur.Generation.Contracts
{
	/// <summary>
	/// An expression generation request.
	/// </summary>
	[ContractClass(typeof(ExpressionGenerationRequestCodeContract))]
	public interface IExpressionGenerationRequest
	{

		/// <summary>
		/// The generator that can be used to generate sub expressions.
		/// </summary>
		IExpressionGenerator TopLevelGenerator { get; }
		/// <summary>
		/// The name of the expression that is to be generated.
		/// </summary>
		string ExpressionName { get; }
		/// <summary>
		/// The function inputs for the expression.
		/// </summary>
		ReadOnlyCollection<Expression> InputExpressions { get; }
		/// <summary>
		/// The optional desired result type for the generated expression.
		/// </summary>
		Type DesiredResultType { get; }

	}

	[ContractClassFor(typeof(IExpressionGenerationRequest))]
	internal abstract class ExpressionGenerationRequestCodeContract : IExpressionGenerationRequest
	{

		public IExpressionGenerator TopLevelGenerator {
			get{
				Contract.Ensures(Contract.Result<IExpressionGenerator>() != null);
				Contract.EndContractBlock();
				throw new NotImplementedException();
			}
		}

		public string ExpressionName {
			get{
				Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));
				Contract.EndContractBlock();
				throw new NotImplementedException();
			}
		}

		public ReadOnlyCollection<Expression> InputExpressions {
			get{
				Contract.Ensures(Contract.Result<ReadOnlyCollection<Expression>>() != null);
				Contract.EndContractBlock();
				throw new NotImplementedException();
			}
		}

		public Type DesiredResultType {
			get { throw new NotImplementedException(); }
		}
	}

}
