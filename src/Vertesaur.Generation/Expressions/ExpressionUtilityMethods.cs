using System.Linq.Expressions;
using System.Reflection;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// Various utility methods and extension methods related to expression trees.
	/// </summary>
	public static class ExpressionUtilityMethods
	{

		/// <summary>
		/// Determines if the expression is a constant or memory location that can be evaluated multiple times without requiring extra computation.
		/// </summary>
		/// <param name="expression">The expression to test.</param>
		/// <returns>True when the expression is a constant or value stored in memory.</returns>
		public static bool IsMemoryLocationOrConstant(this Expression expression) {
			if (null == expression)
				return false;
			return expression is ParameterExpression
				|| expression is ConstantExpression
				|| (expression is MemberExpression && ((MemberExpression)expression).Member.MemberType == MemberTypes.Field);
		}

	}
}
