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

		public static bool ImplementsInterface(this Type targetType, Type interfaceType) {
			Contract.Requires(null != targetType);
			Contract.Requires(null != interfaceType);
#if NETFX_CORE
			return targetType.GetTypeInfo().ImplementedInterfaces.Contains(interfaceType);
#else
			return targetType.GetInterfaces().Contains(interfaceType);
#endif
		}

		public static IEnumerable<MethodInfo> GetPublicInstanceInvokableMethods(this Type targetType) {
			Contract.Requires(null != targetType);
#if NETFX_CORE
			return targetType.GetTypeInfo().DeclaredMethods.Where(m => m.IsPublic && !m.IsStatic && !m.IsConstructor);
#else
			return targetType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod);
#endif
		}

		public static IEnumerable<MethodInfo> GetPublicStaticInvokableMethods(this Type targetType) {
			Contract.Requires(null != targetType);
#if NETFX_CORE
			return targetType.GetTypeInfo().DeclaredMethods.Where(m => m.IsPublic && m.IsStatic && !m.IsConstructor);
#else
			return targetType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod);
#endif
		}

		private static bool TypesEqual(ParameterInfo[] parameterInfos, Type[] compareTypes) {
			if (ReferenceEquals(parameterInfos, null))
				return ReferenceEquals(compareTypes, null);
			if (ReferenceEquals(compareTypes, null))
				return false;
			if (parameterInfos.Length != compareTypes.Length)
				return false;
			for (int i = 0; i < parameterInfos.Length; i++) {
				if (parameterInfos[i].ParameterType != compareTypes[i])
					return false;
			}
			return true;
		}

		public static MethodInfo GetPublicInstanceInvokableMethod(this Type targetType, string methodName, params Type[] paramTypes) {
			Contract.Requires(null != targetType);
			Contract.Requires(!String.IsNullOrEmpty(methodName));
			Contract.Requires(null != paramTypes);
#if NETFX_CORE
			return GetPublicInstanceInvokableMethods(targetType).FirstOrDefault(m => 
				m.Name == methodName
				&& TypesEqual(m.GetParameters(),paramTypes)
			);
#else
			return targetType.GetMethod(
				methodName,
				BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod,
				null, paramTypes, null
			);
#endif
		}

		public static MethodInfo GetPublicStaticInvokableMethod(this Type targetType, string methodName, params Type[] paramTypes) {
			Contract.Requires(null != targetType);
			Contract.Requires(!String.IsNullOrEmpty(methodName));
			Contract.Requires(null != paramTypes);
#if NETFX_CORE
			return GetPublicStaticInvokableMethods(targetType).FirstOrDefault(m =>
				m.Name == methodName
				&& TypesEqual(m.GetParameters(), paramTypes)
			);
#else
			return targetType.GetMethod(
				methodName,
				BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod,
				null, paramTypes, null
			);
#endif
		}


#if NETFX_CORE
		public static IEnumerable<MethodInfo> GetMethods(this Type targetType) {
			Contract.Requires(null != targetType);
			return targetType.GetTypeInfo().DeclaredMethods;
		} 

		public static Type[] GetGenericArguments(this Type targetType) {
			Contract.Requires(null != targetType);
			return targetType.GetTypeInfo().GenericTypeArguments;
		}
#endif

	}
}
