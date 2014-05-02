using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Vertesaur.Search;
using Vertesaur.Utility;

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
        public static TransformationCastNode[] FindCastPath(IList<ITransformation> transformations, Type startType, Type endType) {
            if (null == transformations) throw new ArgumentNullException("transformations");
            if (null == startType) throw new ArgumentNullException("startType");
            if (null == endType) throw new ArgumentNullException("endType");
            if (transformations.Any(x => null == x)) throw new ArgumentException("transformations must all be non-null", "transformations");
            Contract.EndContractBlock();

            if (transformations.Count == 0)
                return null; // NO DATA

            // determine the valid start nodes
            Contract.Assume(transformations[0] != null);
            var startNodes = GenerateNodes(transformations[0])
                .Where(x => x.FromType == startType)
                .ToArray();
            Contract.Assume(Contract.ForAll(startNodes, x => x != null));

            // if there is only one transformation, just use that
            if (transformations.Count == 1) {
                var selectedTx = startNodes.FirstOrDefault(x => x.ToType == endType);
                return null != selectedTx ? new[] { selectedTx } : null;
            }

            // build the edges and nodes between the start and end types
            var previousNodes = startNodes;
            var edges = new Dictionary<TransformationCastNode, List<TransformationCastNode>>();
            for (int i = 1; i < transformations.Count; i++) {
                Contract.Assume(transformations[i] != null);
                var currentNodes = GenerateNodes(transformations[i]).ToArray();
                Contract.Assume(Contract.ForAll(currentNodes, x => x != null));
                foreach (var priorNode in previousNodes) {
                    foreach (var curNode in currentNodes) {
                        if (priorNode.ToType == curNode.FromType) {
                            List<TransformationCastNode> edgeData;
                            if (!edges.TryGetValue(priorNode, out edgeData)) {
                                edgeData = new List<TransformationCastNode>();
                                edges.Add(priorNode, edgeData);
                            }
                            Contract.Assume(edgeData != null);
                            edgeData.Add(curNode);
                        }
                    }
                }
                previousNodes = currentNodes;
            }

            // the end types are the last nodes that have matching end types
            var endNodes = previousNodes.Where(x => x.ToType == endType).ToArray();

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
        public static IEnumerable<TransformationCastNode> GenerateNodes(ITransformation core) {
            if (null == core) throw new ArgumentNullException("core");
            Contract.Ensures(Contract.Result<IEnumerable<TransformationCastNode>>() != null);
            Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<TransformationCastNode>>(), x => null != x));
            return core.GetType().GetInterfacesByGenericTypeDefinition(TxGenericType)
                .Select(t => {
                    var arguments = t.GetGenericArguments();
                    return new TransformationCastNode(core, arguments[0], arguments[1]);
                });
        }

        /// <summary>
        /// Create a new transformation cast node.
        /// </summary>
        /// <param name="core">The transformation the cast information is related to.</param>
        /// <param name="fromType">The selected from type.</param>
        /// <param name="toType">The selected to type.</param>
        public TransformationCastNode(ITransformation core, Type fromType, Type toType) {
            if (null == core) throw new ArgumentNullException("core");
            if (null == fromType) throw new ArgumentNullException("fromType");
            if (null == toType) throw new ArgumentNullException("toType");
            Contract.EndContractBlock();

            Core = core;
            FromType = fromType;
            ToType = toType;
        }

        [ContractInvariantMethod]
        [Conditional("CONTRACTS_FULL")]
        private void CodeContractInvariant() {
            Contract.Invariant(Core != null);
            Contract.Invariant(FromType != null);
            Contract.Invariant(ToType != null);
            Contract.Invariant(TxGenericType != null);
            Contract.Invariant(EnumerableGenericType != null);
        }

        /// <summary>
        /// The transformation that the cast information is related to.
        /// </summary>
        public ITransformation Core { get; private set; }
        /// <summary>
        /// The selected from type used to cast the transformation.
        /// </summary>
        public Type FromType { get; private set; }
        /// <summary>
        /// The selected to type used to case the transformation.
        /// </summary>
        public Type ToType { get; private set; }

        private Type MakeGenericTransformationType() {
            Contract.Assume(2 == TxGenericType.GetGenericArguments().Length);
            return TxGenericType.MakeGenericType(FromType, ToType);
        }

        private static Type MakeGenericEnumerableType(Type itemType) {
            Contract.Assume(1 == EnumerableGenericType.GetGenericArguments().Length);
            return EnumerableGenericType.MakeGenericType(itemType);
        }

        private bool IsValidTransformValueMethod(MethodInfo m) {
            Contract.Requires(null != m);
            if (TransformValueMethodName.Equals(m.Name) && m.ReturnType == ToType) {
                var parameters = m.GetParameters();
                Contract.Assume(parameters != null);
                if (parameters.Length == 1) {
                    Contract.Assume(parameters[0] != null);
                    if(parameters[0].ParameterType == FromType) {
                        return true;
                    }
                } 
            }
            return false;
        }

        private bool IsValidTransformEnumerableValuesMethod(MethodInfo m) {
            Contract.Requires(null != m);
            if (TransformValuesMethodName.Equals(m.Name) && m.ReturnType == MakeGenericEnumerableType(ToType)) {
                var parameters = m.GetParameters();
                Contract.Assume(parameters != null);
                if (parameters.Length == 1 && parameters[0].ParameterType == MakeGenericEnumerableType(FromType)) {
                    return true;
                }
            }
            return false;
        }

        private IEnumerable<MethodInfo> GetAllCandidateMethods() {
            Contract.Ensures(Contract.Result<IEnumerable<MethodInfo>>() != null);
            Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<MethodInfo>>(), x => null != x));
            return Core
                .GetType()
                .GetPublicInstanceInvokableMethods()
                .Concat(MakeGenericTransformationType().GetMethods());
        }

        /// <summary>
        /// Gets the method info for this specific cast of the transform value method.
        /// </summary>
        /// <returns>A method info object.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if the <c>TransformValue</c> method cannot be found at run time.</exception>
        public MethodInfo GetTransformValueMethod() {
            Contract.Ensures(Contract.Result<MethodInfo>() != null);
            var result = GetAllCandidateMethods().FirstOrDefault(IsValidTransformValueMethod);
            if (null == result)
                throw new InvalidOperationException("Could not find a TransformValue method.");
            return result;
        }

        /// <summary>
        /// Gets the method info for this specific cast of the transform values method.
        /// </summary>
        /// <returns>A method info object.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if the <c>TransformValues</c> method cannot be found at run time.</exception>
        public MethodInfo GetTransformEnumerableValuesMethod() {
            Contract.Ensures(Contract.Result<MethodInfo>() != null);
            var result = GetAllCandidateMethods().FirstOrDefault(IsValidTransformEnumerableValuesMethod);
            if (null == result)
                throw new InvalidOperationException("Could not find a TransformValues method.");
            return result;
        }

        /// <summary>
        /// Transforms a value using the current type selection.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The result of transforming the value.</returns>
        public object TransformValue(object value) {
            var method = GetTransformValueMethod();
            return method.Invoke(Core, new[] { value });
        }

        /// <summary>
        /// Transforms a set of values using the current type selection.
        /// </summary>
        /// <param name="values">The values to transform.</param>
        /// <returns>The result of transforming the values.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if one of the transformations returns a <c>null</c> enumerable.</exception>
        public IEnumerable TransformValues(IEnumerable values) {
            Contract.Requires(values != null);
            Contract.Ensures(Contract.Result<IEnumerable>() != null);
            var method = GetTransformEnumerableValuesMethod();
            var result = (IEnumerable)method.Invoke(Core, new object[] { values });
            if (null == result)
                throw new InvalidOperationException("A transformation returned a null enumerable given a non-null enumerable as input.");
            return result;
        }

    }
}
