using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Vertesaur.Contracts;
using Vertesaur.Search;

namespace Vertesaur.Transformation
{
	/// <summary>
	/// Represents the from and to cast interpretations of a generically typed transformation.
	/// </summary>
	public class TransformationCastNode
	{

		/// <summary>
		/// Finds a path through all the given transformations and determines the required to and from types for each transformation.
		/// </summary>
		/// <param name="transformations">The transformations to find a casting path between.</param>
		/// <param name="startType">The from type of the first transformation.</param>
		/// <param name="endType">The to type of the last transformation.</param>
		/// <returns>Transformation nodes providing cast information for a path from start to finish.</returns>
		[CanBeNull]
		public static TransformationCastNode[] FindCastPath(
			[NotNull] IList<ITransformation> transformations,
			[NotNull] Type startType,
			[NotNull] Type endType
		) {
			if(null == transformations) throw new ArgumentNullException("transformations");
			if(null == startType) throw new ArgumentNullException("startType");
			if(null == endType) throw new ArgumentNullException("endType");
			if(transformations.Any(x => null == x)) throw new ArgumentException("transformations must all be non-null","transformations");
			Contract.EndContractBlock();

			if (transformations.Count == 0)
				return null; // NO DATA

			// determine the valid start nodes
			Contract.Assume(transformations[0] != null);
			var startNodes = GenerateNodes(transformations[0])
				.Where(x => x._fromType == startType)
				.ToArray();

			// if there is only one transformation, just use that
			if (transformations.Count == 1) {
				var selectedTx = startNodes.FirstOrDefault(x => x._toType == endType);
				return null != selectedTx ? new[] { selectedTx } : null;
			}

			// build the edges and nodes between the start and end types
			var previousNodes = startNodes;
			var edges = new Dictionary<TransformationCastNode, List<TransformationCastNode>>();
			for (int i = 1; i < transformations.Count; i++) {
				Contract.Assume(transformations[i] != null);
				var currentNodes = GenerateNodes(transformations[i]).ToArray();
				foreach (var priorNode in previousNodes) {
					foreach (var curNode in currentNodes) {
						if (priorNode._toType == curNode._fromType) {
							List<TransformationCastNode> edgeData;
							if (!edges.TryGetValue(priorNode, out edgeData)) {
								edgeData = new List<TransformationCastNode>();
								edges.Add(priorNode, edgeData);
							}
							edgeData.Add(curNode);
						}
					}
				}
				previousNodes = currentNodes;
			}

			// the end types are the last nodes that have matching end types
			var endNodes = previousNodes.Where(x => x._toType == endType).ToArray();

			// the neighborhood
			GetDynamicGraphNeighborInfo<TransformationCastNode, int, TransformationCastNode> getNeighborhood = (node, curCost) => {
				List<TransformationCastNode> edgeData;
				edges.TryGetValue(node, out edgeData);
				return (edgeData ?? Enumerable.Empty<TransformationCastNode>()).Select(x => new DynamicGraphNodeData<TransformationCastNode, int, TransformationCastNode>(x, curCost + 1, x));
			};

			// go through each combination of start and end types until something is found
			foreach (var startNode in startNodes) {
				Contract.Assume(startNode != null); // because GenerateNodes will not return a null element
				foreach (var endNode in endNodes) {
					Contract.Assume(endNode != null); // because GenerateNodes will not return a null element
					var result = DynamicGraph.FindPath(startNode, endNode, getNeighborhood);
					if (null != result)
						return result.Select(x => x.Node).ToArray(); // OK!
				}
			}

			return null; // no path found
		}

		private static readonly Type TxGenericType = typeof(ITransformation<,>);
		private static readonly Type EnumerableGenericType = typeof(IEnumerable<>);
		private const string TransformValueMethodName = "TransformValue";
		private const string TransformValuesMethodName = "TransformValues";

		/// <summary>
		/// Generates possible nodes for a given transformation.
		/// </summary>
		/// <param name="core">The transformation to generate cast information from.</param>
		/// <returns>Possible transformation cast nodes.</returns>
		[NotNull]
		public static IEnumerable<TransformationCastNode> GenerateNodes([NotNull] ITransformation core) {
			if(null == core) throw new ArgumentNullException("core");
			Contract.Ensures(Contract.Result<IEnumerable<TransformationCastNode>>() != null);
			Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<TransformationCastNode>>(), x => null != x));
			Contract.EndContractBlock();

			return core.GetType()
				.GetInterfaces()
				.Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == TxGenericType)
				.Select(t => {
					var arguments = t.GetGenericArguments();
					return new TransformationCastNode(core, arguments[0], arguments[1]);
				});
		}

		[NotNull] private readonly ITransformation _core;
		[NotNull] private readonly Type _fromType;
		[NotNull] private readonly Type _toType;

		/// <summary>
		/// Create a new transformation cast node.
		/// </summary>
		/// <param name="core">The transformation the cast information is related to.</param>
		/// <param name="fromType">The selected from type.</param>
		/// <param name="toType">The selected to type.</param>
		public TransformationCastNode([NotNull] ITransformation core, [NotNull] Type fromType, [NotNull] Type toType) {
			if(null == core) throw new ArgumentNullException("core");
			if(null == fromType) throw new ArgumentNullException("fromType");
			if(null == toType) throw new ArgumentNullException("toType");
			Contract.EndContractBlock();

			_core = core;
			_fromType = fromType;
			_toType = toType;
		}

		/// <summary>
		/// The transformation that the cast information is related to.
		/// </summary>
		[NotNull] public ITransformation Core { get { return _core; } }
		/// <summary>
		/// The selected from type used to cast the transformation.
		/// </summary>
		[NotNull] public Type FromType { get { return _fromType; } }
		/// <summary>
		/// The selected to type used to case the transformation.
		/// </summary>
		[NotNull] public Type ToType { get { return _toType; } }

		private Type MakeGenericTransformationType() {
			Contract.Assume(2 == TxGenericType.GetGenericArguments().Length);
			return TxGenericType.MakeGenericType(_fromType, _toType);
		}

		private static Type MakeGenericEnumerableType(Type itemType) {
			Contract.Assume(1 == EnumerableGenericType.GetGenericArguments().Length);
			return EnumerableGenericType.MakeGenericType(itemType);
		}

		private bool IsValidTransformValueMethod([NotNull] MethodInfo m) {
			Contract.Requires(null != m);
			Contract.EndContractBlock();

			if (TransformValueMethodName.Equals(m.Name) && m.ReturnType == _toType) {
				var parameters = m.GetParameters();
				if (parameters.Length == 1 && parameters[0].ParameterType == _fromType) {
					return true;
				}
			}
			return false;
		}

		private bool IsValidTransformEnumerableValuesMethod([NotNull] MethodInfo m) {
			Contract.Requires(null != m);
			Contract.EndContractBlock();
			if (TransformValuesMethodName.Equals(m.Name) && m.ReturnType == MakeGenericEnumerableType(_toType)) {
				var parameters = m.GetParameters();
				if (parameters.Length == 1 && parameters[0].ParameterType == MakeGenericEnumerableType(_fromType)) {
					return true;
				}
			}
			return false;
		}

		[NotNull]
		private IEnumerable<MethodInfo> GetAllCandidateMethods() {
			Contract.Ensures(Contract.Result<IEnumerable<MethodInfo>>() != null);
			Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<MethodInfo>>(), x => null != x));
			Contract.EndContractBlock();

			return _core
				.GetType()
				.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod)
				.Concat(MakeGenericTransformationType().GetMethods());
		}

		/// <summary>
		/// Gets the method info for this specific cast of the transfom value method.
		/// </summary>
		/// <returns>A method info object.</returns>
		/// <exception cref="System.InvalidOperationException">Thrown if the <c>TransformValue</c> method cannot be found at run time.</exception>
		[NotNull]
		public MethodInfo GetTransformValueMethod() {
			Contract.Ensures(Contract.Result<MethodInfo>() != null);
			Contract.EndContractBlock();
			var result = GetAllCandidateMethods().FirstOrDefault(IsValidTransformValueMethod);
			if(null == result)
				throw new InvalidOperationException("Could not find a TransformValue method.");
			return result;
		}

		/// <summary>
		/// Gets the method info for this specific cast of the transfom values method.
		/// </summary>
		/// <returns>A method info object.</returns>
		/// <exception cref="System.InvalidOperationException">Thrown if the <c>TransformValues</c> method cannot be found at run time.</exception>
		[NotNull]
		public MethodInfo GetTransformEnumerableValuesMethod() {
			Contract.Ensures(Contract.Result<MethodInfo>() != null);
			Contract.EndContractBlock();
			var result = GetAllCandidateMethods().FirstOrDefault(IsValidTransformEnumerableValuesMethod);
			if(null == result)
				throw new InvalidOperationException("Could not find a TransformValues method.");
			return result;
		}

		/// <summary>
		/// Transforms a value using the current type selection.
		/// </summary>
		/// <param name="value">The value to transfom.</param>
		/// <returns>The result of transforming the value.</returns>
		public object TransformValue(object value) {
			var method = GetTransformValueMethod();
			return method.Invoke(_core, new[] { value });
		}

		/// <summary>
		/// Transfoms a set of values using the current type selection.
		/// </summary>
		/// <param name="values">The values to transform.</param>
		/// <returns>The result of transforming the values.</returns>
		/// <exception cref="System.InvalidOperationException">Thrown if one of the transformations returns a <c>null</c> enumerable.</exception>
		public IEnumerable TransformValues([NotNull] IEnumerable values) {
			Contract.Requires(values != null);
			Contract.Ensures(Contract.Result<IEnumerable>() != null);
			Contract.EndContractBlock();

			var method = GetTransformEnumerableValuesMethod();
			var result = (IEnumerable)method.Invoke(_core, new object[] {values});
			if(null == result) throw new InvalidOperationException("A transformation returned a null enumerable given a non-null enumerable as input.");
			return result;
		}

	}
}
