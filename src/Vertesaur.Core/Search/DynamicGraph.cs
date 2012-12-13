using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Vertesaur.Search
{

	/// <summary>
	/// Defines an operation that generates neighbor node and edge information.
	/// </summary>
	/// <typeparam name="TNode">The node type.</typeparam>
	/// <typeparam name="TCost">The edge cost type.</typeparam>
	/// <typeparam name="TEdge">The edge data type.</typeparam>
	/// <param name="node">The node to find neighbors for.</param>
	/// <param name="currentCost">The current cost to get to the given node.</param>
	/// <returns>Neighbor node info.</returns>
	public delegate IEnumerable<DynamicGraphNodeData<TNode, TCost, TEdge>> GetDynamicGraphNeighborInfo<TNode, TCost, TEdge>(TNode node, TCost currentCost);

	/// <summary>
	/// Holds information about graph nodes.
	/// </summary>
	/// <typeparam name="TNode">The node type.</typeparam>
	/// <typeparam name="TCost">The edge cost type.</typeparam>
	/// <typeparam name="TEdge">The edge data type.</typeparam>
	public sealed class DynamicGraphNodeData<TNode, TCost, TEdge>
	{
		/// <summary>
		/// Creates a default node data class.
		/// </summary>
		public DynamicGraphNodeData() { }

		/// <summary>
		/// Creates a node data class with the given values.
		/// </summary>
		/// <param name="node">The node the data relates to.</param>
		/// <param name="cost">The cost related to the edge.</param>
		/// <param name="edge">The edge data related to the node.</param>
		public DynamicGraphNodeData(TNode node, TCost cost, TEdge edge) {
			// ReSharper disable CompareNonConstrainedGenericWithNull
			if(null == node) throw new ArgumentNullException("node");
			// ReSharper restore CompareNonConstrainedGenericWithNull
			Contract.EndContractBlock();
			Node = node;
			Cost = cost;
			Edge = edge;
		}
		
		/// <summary>
		/// The node the data related to.
		/// </summary>
		public TNode Node { get; set; }
		/// <summary>
		/// The cost related to the edge.
		/// </summary>
		/// <remarks>
		/// The cost value often represents the total cost up to this point within the graph.
		/// </remarks>
		public TCost Cost { get; set; }
		/// <summary>
		/// The edge data related to the node.
		/// </summary>
		public TEdge Edge { get; set; }

	}

	/// <summary>
	/// Static helper methods for traversing a dynamic graph.
	/// </summary>
	/// <remarks>
	/// These helpers exist primarily to enable generic type inference.
	/// </remarks>
	public static class DynamicGraph
	{

		/// <summary>
		/// Finds the optimal path between the two given nodes.
		/// </summary>
		/// <typeparam name="TNode">The node type.</typeparam>
		/// <typeparam name="TCost">The edge cost type.</typeparam>
		/// <typeparam name="TEdge">The edge data type.</typeparam>
		/// <param name="start">The starting node.</param>
		/// <param name="target">The target node.</param>
		/// <param name="generateNeighborInfo">Generates neighbor information.</param>
		/// <returns>An optimal path or null on failure.</returns>
		public static ReadOnlyCollection<DynamicGraphNodeData<TNode, TCost, TEdge>> FindPath<TNode, TCost, TEdge>(
			TNode start,
			TNode target,
			GetDynamicGraphNeighborInfo<TNode, TCost, TEdge> generateNeighborInfo
		) {
			// ReSharper disable CompareNonConstrainedGenericWithNull
			if (null == start) throw new ArgumentNullException("start");
			if (null == target) throw new ArgumentNullException("target");
			// ReSharper restore CompareNonConstrainedGenericWithNull
			if (null == generateNeighborInfo) throw new ArgumentNullException("generateNeighborInfo");
			Contract.EndContractBlock();
			
			return FindPath(start, target, generateNeighborInfo, null, null);
		}

		/// <summary>
		/// Finds the optimal path between the two given nodes.
		/// </summary>
		/// <typeparam name="TNode">The node type.</typeparam>
		/// <typeparam name="TCost">The edge cost type.</typeparam>
		/// <typeparam name="TEdge">The edge data type.</typeparam>
		/// <param name="start">The starting node.</param>
		/// <param name="target">The target node.</param>
		/// <param name="generateNeighborInfo">Generates neighbor information.</param>
		/// <param name="costComparer">Compares the costs.</param>
		/// <param name="nodeComparer">Node equality comparer.</param>
		/// <returns>An optimal path or null on failure.</returns>
		public static ReadOnlyCollection<DynamicGraphNodeData<TNode, TCost, TEdge>> FindPath<TNode, TCost, TEdge>(
			TNode start,
			TNode target,
			GetDynamicGraphNeighborInfo<TNode, TCost, TEdge> generateNeighborInfo,
			IComparer<TCost> costComparer,
			IEqualityComparer<TNode> nodeComparer 
		) {
			// ReSharper disable CompareNonConstrainedGenericWithNull
			if (null == start) throw new ArgumentNullException("start");
			if (null == target) throw new ArgumentNullException("target");
			// ReSharper restore CompareNonConstrainedGenericWithNull
			if(null == generateNeighborInfo) throw new ArgumentNullException("generateNeighborInfo");
			Contract.EndContractBlock();

			return new DynamicGraph<TNode, TCost, TEdge>(generateNeighborInfo, costComparer, nodeComparer)
				.FindPath(start, target);
		}

	}

	/// <summary>
	/// A dynamic graph that can provide an optimal path between two nodes.
	/// </summary>
	/// <typeparam name="TNode">The node type.</typeparam>
	/// <typeparam name="TCost">The edge cost type.</typeparam>
	/// <typeparam name="TEdge">The edge data type.</typeparam>
	public class DynamicGraph<TNode, TCost, TEdge> : DynamicGraphBase<TNode, TCost, TEdge>
	{

		private readonly GetDynamicGraphNeighborInfo<TNode, TCost, TEdge> _generateNeighborInfo;

		/// <summary>
		/// Creates a new dynamic graph using the given neighbor data.
		/// </summary>
		/// <param name="generateNeighborInfo">Generates neighbor information for a given node.</param>
		public DynamicGraph(GetDynamicGraphNeighborInfo<TNode, TCost, TEdge> generateNeighborInfo)
			: this(generateNeighborInfo, null, null)
		{
			Contract.Requires(generateNeighborInfo != null);
			Contract.EndContractBlock();
		}

		/// <summary>
		/// Creates a new dynamic graph using the given neighbor data.
		/// </summary>
		/// <param name="generateNeighborInfo">Generates neighbor information for a given node.</param>
		/// <param name="costComparer">Used to compare node cost values.</param>
		/// <param name="nodeComparer">Compares nodes to determine equality.</param>
		public DynamicGraph(GetDynamicGraphNeighborInfo<TNode, TCost, TEdge> generateNeighborInfo, IComparer<TCost> costComparer,  IEqualityComparer<TNode> nodeComparer)
			: base(costComparer, nodeComparer) 
		{
			if(null == generateNeighborInfo) throw new ArgumentNullException("generateNeighborInfo");
			Contract.EndContractBlock();
			_generateNeighborInfo = generateNeighborInfo;
		}

		/// <inheritdoc/>
		protected override IEnumerable<DynamicGraphNodeData<TNode, TCost, TEdge>> GetNeighborInfo(TNode node, TCost currentCost) {
			return _generateNeighborInfo(node, currentCost) ?? Enumerable.Empty<DynamicGraphNodeData<TNode, TCost, TEdge>>();
		}

	}

	[ContractClassFor(typeof(DynamicGraphBase<,,>))]
	internal abstract class DynamicGraphBaseCodeContract<TNode, TCost, TEdge> : DynamicGraphBase<TNode, TCost, TEdge>
	{

		protected override IEnumerable<DynamicGraphNodeData<TNode, TCost, TEdge>> GetNeighborInfo(TNode node, TCost currentCost) {
			// ReSharper disable CompareNonConstrainedGenericWithNull
			Contract.Requires(null != node);
			Contract.Ensures(Contract.Result<IEnumerable<DynamicGraphNodeData<TNode, TCost, TEdge>>>() != null);
			return Enumerable.Empty<DynamicGraphNodeData<TNode, TCost, TEdge>>();
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}
	}

	/// <summary>
	/// A dynamic graph that can provide an optimal path between two nodes.
	/// </summary>
	/// <typeparam name="TNode">The node type.</typeparam>
	/// <typeparam name="TCost">The edge cost type.</typeparam>
	/// <typeparam name="TEdge">The edge data type.</typeparam>
	[ContractClass(typeof(DynamicGraphBaseCodeContract<,,>))]
	public abstract class DynamicGraphBase<TNode, TCost, TEdge>
	{

		/// <summary>
		/// Creates a default dynamic graph.
		/// </summary>
		protected DynamicGraphBase()
			: this(null, null) { }

		/// <summary>
		/// Creates a dynamic graph with the given comparers.
		/// </summary>
		/// <param name="costComparer">Used to compare node cost values.</param>
		/// <param name="nodeComparer">Compares nodes to determine equality.</param>
		protected DynamicGraphBase(IComparer<TCost> costComparer, IEqualityComparer<TNode> nodeComparer) {
			CostComparer = costComparer ?? Comparer<TCost>.Default;
			NodeComparer = nodeComparer ?? EqualityComparer<TNode>.Default;
		}

		/// <summary>
		/// Used to compare node cost values.
		/// </summary>
		public IComparer<TCost> CostComparer { get; private set; }
		/// <summary>
		/// Compares nodes to determine equality.
		/// </summary>
		public IEqualityComparer<TNode> NodeComparer { get; private set; } 

		/// <summary>
		/// Generates neighbor information with the calculated complete costs from start to current node.
		/// </summary>
		/// <param name="node">The node to generate neighbor information for.</param>
		/// <param name="currentCost">The current cost required to navigate to the given node.</param>
		/// <returns>Generated neighbor information.</returns>
		protected abstract IEnumerable<DynamicGraphNodeData<TNode, TCost, TEdge>> GetNeighborInfo(TNode node, TCost currentCost);

		private void FindSmallestNodeData(
			HashSet<TNode> keys,
			Dictionary<TNode, DynamicGraphNodeData<TNode, TCost, TEdge>> lookUp,
			out TNode smallestNode,
			out DynamicGraphNodeData<TNode, TCost, TEdge> smallestEdge
		) {
			Contract.Requires(keys != null);
			Contract.Requires(keys.Count > 0);
			Contract.Requires(lookUp != null);
			Contract.Requires(lookUp.Count > 0);
			Contract.Requires(lookUp.Count >= keys.Count);
			// ReSharper disable CompareNonConstrainedGenericWithNull
			Contract.Requires(Contract.ForAll(keys, key => key != null && lookUp.ContainsKey(key)));
			Contract.Ensures(Contract.ValueAtReturn(out smallestNode) != null);
			// ReSharper restore CompareNonConstrainedGenericWithNull
			Contract.Ensures(Contract.ValueAtReturn(out smallestEdge) != null);
			Contract.EndContractBlock();

			using(var enumerator = keys.GetEnumerator()) {
				if (!enumerator.MoveNext())
					throw new ArgumentException("keys is empty","keys");
				var currentNode = enumerator.Current;

				// ReSharper disable AssignNullToNotNullAttribute
				//var smallest = new KeyValuePair<TNode, DynamicGraphNodeData<TNode, TCost, TEdge>>(currentNode, lookUp[currentNode]);
				smallestNode = currentNode;
				smallestEdge = lookUp[currentNode];
				// ReSharper restore AssignNullToNotNullAttribute
				
				while(enumerator.MoveNext()) {
					currentNode = enumerator.Current;
					
					// ReSharper disable AssignNullToNotNullAttribute
					var currentData = lookUp[currentNode];
					// ReSharper restore AssignNullToNotNullAttribute
					
					if(CostComparer.Compare(smallestEdge.Cost, currentData.Cost) > 0) {
						smallestNode = currentNode;
						smallestEdge = currentData;
					}
				}
			}
			// ReSharper disable CompareNonConstrainedGenericWithNull
			if (null == smallestNode) throw new ArgumentException("null keys are not allowed", "keys");
			if (null == smallestEdge) throw new ArgumentException("null edge contained in edge look-up", "lookUp");
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}

		/// <summary>
		/// Finds a graph path from the <paramref name="start"/> node to the <paramref name="target"/> node.
		/// </summary>
		/// <param name="start">The node to begin the path search from.</param>
		/// <param name="target">The target node of the search.</param>
		/// <returns>The shortest path from the start node to the target node if one exists.</returns>
		/// <exception cref="System.ArgumentException">Thrown if a node or edge encountered within the graph is <c>null</c>.</exception>
		public ReadOnlyCollection<DynamicGraphNodeData<TNode, TCost, TEdge>> FindPath(TNode start, TNode target) {
			// ReSharper disable CompareNonConstrainedGenericWithNull
			if(null == start) throw new ArgumentNullException("start");
			if(null == target) throw new ArgumentNullException("target");
			// ReSharper restore CompareNonConstrainedGenericWithNull
			Contract.EndContractBlock();

			// initialize the look-ups
			var nodeDataCache = new Dictionary<TNode, DynamicGraphNodeData<TNode, TCost, TEdge>>(NodeComparer){
				{start,new DynamicGraphNodeData<TNode, TCost, TEdge>(start,default(TCost),default(TEdge))}
			};
			var visitRequired = new HashSet<TNode>(NodeComparer) { start }; // NOTE: in order for a node to be in this collection it must have a corresponding key in the pathData dictionary.
			DynamicGraphNodeData<TNode, TCost, TEdge> nodeData;
			TNode currentNode;
			var bestCompleteCost = default(TCost);
			var completeRouteFound = false;

			// generate the dynamic path information and find the shortest path
			while(visitRequired.Count != 0) {
				DynamicGraphNodeData<TNode, TCost, TEdge> currentNodeData;
				Contract.Assume(visitRequired.Count > 0);
				Contract.Assume(nodeDataCache.Count >= visitRequired.Count);
				FindSmallestNodeData(visitRequired, nodeDataCache, out currentNode, out currentNodeData);
				visitRequired.Remove(currentNode);

				// logic to see if we can short out of checking this node due to it being too long
				if(completeRouteFound) {
					if(CostComparer.Compare(bestCompleteCost, currentNodeData.Cost) <= 0) {
						continue; // this path is larger than or equal to the best found complete path
					}
					if(NodeComparer.Equals(currentNode, target)){
						bestCompleteCost = currentNodeData.Cost;
					}
				}
				else if (NodeComparer.Equals(currentNode, target)) {
					bestCompleteCost = currentNodeData.Cost;
					completeRouteFound = true;
				}

				foreach(var neighborInfo in GetNeighborInfo(currentNode, currentNodeData.Cost)) {
					if(ReferenceEquals(null, neighborInfo) || ReferenceEquals(null, neighborInfo.Node))
						continue;

					if(!nodeDataCache.TryGetValue(neighborInfo.Node, out nodeData)) {
						nodeDataCache.Add(neighborInfo.Node, new DynamicGraphNodeData<TNode, TCost, TEdge>(currentNode, neighborInfo.Cost, neighborInfo.Edge));
						visitRequired.Add(neighborInfo.Node);
					}
					else if(CostComparer.Compare(neighborInfo.Cost, nodeData.Cost) < 0) {
						nodeData.Node = currentNode;
						nodeData.Cost = neighborInfo.Cost;
						nodeData.Edge = neighborInfo.Edge;
						visitRequired.Add(neighborInfo.Node);
					}
				}
			}

			// build the final result path
			if(nodeDataCache.TryGetValue(target, out nodeData)) {
				var pathResult = new List<DynamicGraphNodeData<TNode, TCost, TEdge>> {
					new DynamicGraphNodeData<TNode, TCost, TEdge>(target, nodeData.Cost, nodeData.Edge)
				};
				currentNode = nodeData.Node;
				// ReSharper disable CompareNonConstrainedGenericWithNull
				if (null == currentNode)
					return null;
				// ReSharper restore CompareNonConstrainedGenericWithNull
				while(nodeDataCache.TryGetValue(currentNode, out nodeData)) {
					pathResult.Add(new DynamicGraphNodeData<TNode, TCost, TEdge>(currentNode, nodeData.Cost, nodeData.Edge));
					if(Equals(currentNode, start))
						break;
					currentNode = nodeData.Node;
					// ReSharper disable CompareNonConstrainedGenericWithNull
					if (null == currentNode)
						return null;
					// ReSharper restore CompareNonConstrainedGenericWithNull
				}
				pathResult.Reverse();
				return new ReadOnlyCollection<DynamicGraphNodeData<TNode, TCost, TEdge>>(pathResult);
			}

			return null; // no path was found

		}

		[ContractInvariantMethod]
		private void CodeContractInvariant() {
			Contract.Invariant(CostComparer != null);
			Contract.Invariant(NodeComparer != null);
		}

	}

}
