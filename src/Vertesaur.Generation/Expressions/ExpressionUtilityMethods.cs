using System.Linq;
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
            if (expression is ParameterExpression || expression is ConstantExpression)
                return true;

            var memberExpression = expression as MemberExpression;
            if (memberExpression != null) {
#if NETFX_CORE
                var member = memberExpression.Member;
                var memberType = member.DeclaringType;
                return memberType.GetTypeInfo().DeclaredFields.Any(x => x.Name == member.Name);
#else
                return memberExpression.Member.MemberType == MemberTypes.Field;
#endif
            }

            return false;

        }

    }
}
