using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// Generates vector related expressions.
    /// </summary>
#if !NO_MEF
    [System.ComponentModel.Composition.Export(typeof(IExpressionGenerator))]
#endif
    public class VectorExpressionGenerator : IExpressionGenerator
    {
        /// <summary>
        /// Generates a vector related expression given a supported expression generation request.
        /// </summary>
        /// <param name="request">The expression generation request to be processed.</param>
        /// <returns>A new expression or null if the request is not supported.</returns>
        public Expression Generate(IExpressionGenerationRequest request) {
            if (null == request) throw new ArgumentNullException("request");
            Contract.EndContractBlock();

            var expressionName = request.ExpressionName;
            var inputExpressions = request.InputExpressions;
            var topLevelGenerator = request.TopLevelGenerator;

            if (inputExpressions.Count > 0) {
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
