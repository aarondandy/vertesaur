using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{
	/// <summary>
	/// Generates vector related expressions.
	/// </summary>
	[Export(typeof(IExpressionGenerator))]
	public class VectorExpressionGenerator : IExpressionGenerator
	{
		public Expression GenerateExpression(IExpressionGenerationRequest request) {
			if (null == request) throw new ArgumentNullException("request");
			Contract.EndContractBlock();

			var expressionName = request.ExpressionName;
			var inputExpressions = request.InputExpressions;
			var topLevelGenerator = request.TopLevelGenerator;

			if (inputExpressions.Count > 0){
				if (StringComparer.OrdinalIgnoreCase.Equals(expressionName, "MAGNITUDE"))
					return new MagnitudeExpression(inputExpressions, topLevelGenerator);
				if (StringComparer.OrdinalIgnoreCase.Equals(expressionName, "SQUAREDMAGNITUDE"))
					return new SquaredMagnitudeExpression(inputExpressions, topLevelGenerator);

				if ((inputExpressions.Count % 2) == 0) {
					if (StringComparer.OrdinalIgnoreCase.Equals(expressionName, "DOTPRODUCT"))
						return new DotProductExpression(inputExpressions, topLevelGenerator);
					if (StringComparer.OrdinalIgnoreCase.Equals(expressionName, "DISTANCE"))
						return new DistanceExpression(inputExpressions, topLevelGenerator);
					if (StringComparer.OrdinalIgnoreCase.Equals(expressionName, "SQUAREDDISTANCE"))
						return new SquaredDistanceExpression(inputExpressions, topLevelGenerator);
				}
			}

			return null;
		}
	}
}
