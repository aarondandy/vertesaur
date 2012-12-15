using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Vertesaur.Generation.Expressions
{
	public static class ExpressionUtilityMethods
	{

		public static bool IsMemoryLocationOrConstant(this Expression expression) {
			if (null == expression)
				return false;
			return expression is ParameterExpression
				|| expression is ConstantExpression
				|| (expression is MemberExpression && ((MemberExpression)expression).Member.MemberType == MemberTypes.Field);
		}

	}
}
