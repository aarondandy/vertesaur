using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Vertesaur.Utility
{
    internal static class ReflectionUtils
    {

        public static IEnumerable<Type> GetInterfacesByGenericTypeDefinition(this Type targetType, Type genericTypeDefinition) {
            Contract.Requires(null != targetType);
            Contract.Requires(null != genericTypeDefinition);
            Contract.Ensures(Contract.Result<IEnumerable<Type>>() != null);
#if NETFX_CORE
            return targetType.GetTypeInfo()
                .ImplementedInterfaces
                .Select(t => t.GetTypeInfo())
                .Where(ti => ti.IsGenericParameter &&  ti.GetGenericTypeDefinition() == genericTypeDefinition)
                .Select(ti => ti.AsType());
#else
            return targetType
                .GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == genericTypeDefinition);
#endif

        }

        public static IEnumerable<MethodInfo> GetPublicInstanceInvokableMethods(this Type targetType) {
            Contract.Requires(null != targetType);
            Contract.Ensures(Contract.Result<IEnumerable<MethodInfo>>() != null);
#if NETFX_CORE
            return targetType.GetTypeInfo().DeclaredMethods.Where(m => m.IsPublic && !m.IsStatic);
#else
            return targetType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod);
#endif
        }


#if NETFX_CORE
        public static IEnumerable<MethodInfo> GetMethods(this Type targetType) {
            Contract.Requires(null != targetType);
            Contract.Ensures(Contract.Result<IEnumerable<MethodInfo>>() != null);
            return targetType.GetTypeInfo().DeclaredMethods;
        } 

        public static Type[] GetGenericArguments(this Type targetType) {
            Contract.Requires(targetType != null);
            Contract.Ensures(Contract.Result<Type[]>() != null)
            return targetType.GetTypeInfo().GenericTypeArguments;
        }
#endif

    }
}
