// ===============================================================================
//
// Copyright (c) 2011 Aaron Dandy 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using JetBrains.Annotations;

namespace Vertesaur.PolygonOperation
{

	/// <summary>
	/// A hierarchy of rings grouped using their boundaries.
	/// </summary>
	public class RingBoundaryTree
	{

		/// <summary>
		/// A collection of ring nodes.
		/// </summary>
		public class NodeCollection : Collection<Node>
		{
			private readonly List<Node> _nodes;

			/// <summary>
			/// Constructs a new node collection from the given nodes.
			/// </summary>
			/// <param name="nodes"></param>
			public NodeCollection([CanBeNull] List<Node> nodes)
				: base(nodes ?? new List<Node>())
			{
				_nodes = nodes;
			}

			/// <summary>
			/// Determines if the nodes are holes, fills, or unknown.
			/// </summary>
			public bool? NodesAreHole {
				get {
					foreach (var node in _nodes) {
						if(node.Ring.Hole.HasValue) {
							return node.Ring.Hole;
						}
					}
					return null;
				}
			}

			/// <summary>
			/// Determines if all nodes are valid.
			/// </summary>
			/// <param name="holeExpected">flag weather the nodes are expected to be flagged as fills or holes.</param>
			/// <returns>true if all nodes are valid.</returns>
			public bool NodesAreValid(bool holeExpected)
			{
				return _nodes.All(node => node.Ring != null && (!node.Ring.Hole.HasValue || node.Ring.Hole.Value == holeExpected));
			}

			/// <summary>
			/// Determines if the given ring which does not intersect the boundary of any node is contained within the nodes.
			/// </summary>
			/// <param name="other"></param>
			/// <returns>true if the ring with contained by the nodes.</returns>
			/// <remarks>
			/// The logic assumes the given ring does not intersect the boundary of any node within this collection or any node deeper in the tree.
			/// </remarks>
			public bool NonIntersectingContains([NotNull] Ring2 other) {
				if(null == other)
					throw new ArgumentNullException("other");
				Contract.EndContractBlock();

				var nodeHoleFlag = NodesAreHole;
				return nodeHoleFlag.HasValue && nodeHoleFlag.Value // nodes are hole nodes?
					? NonIntersectingHolesContains(other)
					: NonIntersectingFillsContains(other);
			}

			private bool NonIntersectingFillsContains([NotNull] Ring2 other) {
				Contract.Requires(other != null);
				Contract.EndContractBlock();
				for (int i = 0; i < _nodes.Count; i++) {
					var node = _nodes[i];
					if (other.NonIntersectingWithin(node.Ring))
						return node.Count <= 0 || node.NonIntersectingContains(other);
				}
				return false;
			}

			private bool NonIntersectingHolesContains([NotNull] Ring2 other) {
				Contract.Requires(other != null);
				Contract.EndContractBlock();

				foreach (var node in _nodes) {
					if (!other.NonIntersectingWithin(node.Ring))
						return node.Count > 0 && node.NonIntersectingContains(other);
				}
				return Count > 0;
			}

			/// <summary>
			/// Removes all nodes satisfying the given predicate.
			/// </summary>
			/// <param name="predicate">determines if a node should be removed.</param>
			public void RemoveAll([NotNull, InstantHandle] Predicate<Node> predicate) {
				if(null == predicate)
					throw new ArgumentNullException("predicate");
				Contract.EndContractBlock();

				_nodes.RemoveAll(predicate);
			}

			[ContractAnnotation("null=>false")]
			private bool IsNodeValid(Node item) {
				if (null == item || item.Ring == null)
					return false;
				var holeFlag = NodesAreHole;
				if (holeFlag.HasValue && item.Ring.Hole.HasValue && holeFlag.Value != item.Ring.Hole.Value)
					return false;
				return true;
			}

			/// <summary>
			/// Sets the node if it is valid for the collection.
			/// </summary>
			/// <param name="index">The index to set.</param>
			/// <param name="item">The item to store at the index.</param>
			protected sealed override void SetItem(int index, Node item) {
				if (!IsNodeValid(item))
					throw new ArgumentException("item is not valid for this collection.","item");
				base.SetItem(index, item);
			}

			/// <summary>
			/// Inserts the node if it is valid for the collection.
			/// </summary>
			/// <param name="index">The index to insert at.</param>
			/// <param name="item">The item to store at the index.</param>
			protected sealed override void InsertItem(int index, Node item) {
				if(!IsNodeValid(item))
					throw new ArgumentException("item is not valid for this collection.","item");
				base.InsertItem(index, item);
			}

		}

		/// <summary>
		/// A ring node in a ring boundary tree.
		/// </summary>
		public sealed class Node : NodeCollection
		{
			private readonly Ring2 _ring;

			internal Node([NotNull] Ring2 ring, [CanBeNull] IEnumerable<Node> children)
				: this(ring, null == children ? null : new List<Node>(children))
			{
				if (null == ring)
					throw new ArgumentNullException("ring");
				Contract.EndContractBlock();
			}

			private Node([NotNull] Ring2 ring, [CanBeNull] List<Node> children)
				: base(children)
			{
				if (null == ring)
					throw new ArgumentNullException("ring");
				Contract.EndContractBlock();

				_ring = ring;
				
				if(!NodesAreValid(!Hole))
					throw new ArgumentException("Fill/Hole mismatch.","children");
			}

			/// <summary>
			/// The ring the node represents.
			/// </summary>
			public Ring2 Ring { get { return _ring; } }

			/// <summary>
			/// True when the ring is explicitly set to being a hole.
			/// </summary>
			public bool Hole { get { return _ring.Hole.HasValue && _ring.Hole.Value; } }

		}

		[NotNull]
		private static List<Node> BuildTree([NotNull, InstantHandle] IEnumerable<Ring2> rings) {
			Contract.Requires(rings != null);
			Contract.Ensures(Contract.Result<List<Node>>() != null);
			Contract.EndContractBlock();

			var roots = new List<Node>();
			foreach (var ring in rings) {
				var parent = FindParent(ring, roots);
				if (null == parent) {
					parent = new Node(ring, BoundBy(roots, ring));
					roots.RemoveAll(parent.Contains);
					roots.Add(parent);
				}
				else {
					var newNode = new Node(ring, BoundBy(parent, ring));
					parent.RemoveAll(newNode.Contains);
					parent.Add(newNode);
				}
			}
			return roots;
		}

		[NotNull]
		private static IEnumerable<Node> BoundBy([NotNull] IEnumerable<Node> nodes, [NotNull] Ring2 ring) {
			Contract.Requires(nodes != null);
			Contract.Requires(ring != null);
			Contract.EndContractBlock();
			return nodes.Where(n => RingIsNonIntersectingBoundBy(n.Ring, ring));
		}

		[CanBeNull]
		private static Node FindParent([NotNull] Ring2 ring, [NotNull, InstantHandle] IEnumerable<Node> roots) {
			Contract.Requires(ring != null);
			Contract.Requires(roots != null);
			Contract.EndContractBlock();
			foreach (var node in roots) {
				if (RingIsNonIntersectingBoundBy(ring, node.Ring))
					return FindParent(ring, node) ?? node;
			}
			return null;
		}

		private static bool RingIsNonIntersectingBoundBy([NotNull] Ring2 ring, [NotNull] Ring2 container) {
			Contract.Requires(null != ring);
			Contract.Requires(null != container);
			Contract.EndContractBlock();
			if (ring.GetMbr().Within(container.GetMbr())) {
				for (var i = 0; i < ring.Count; i++) {
					var intersectionCount = container.IntersectionPositiveXRayCount(ring[i]);
					if (intersectionCount > 0)
						return 1 == (intersectionCount % 2);
				}
			}
			return false;
		}

		[NotNull] private readonly NodeCollection _roots;

		/// <summary>
		/// Constructs a ring boundary tree from a collection of non-intersecting rings.
		/// </summary>
		/// <param name="rings">A collection of non-intersecting rings.</param>
		public RingBoundaryTree([NotNull, InstantHandle] IEnumerable<Ring2> rings)
		{
			if(null == rings)
				throw new ArgumentNullException("rings");
			Contract.EndContractBlock();

			_roots = new NodeCollection(BuildTree(rings));
			var hole = _roots.NodesAreHole;
			if (!_roots.NodesAreValid(hole.HasValue && hole.Value))
				throw new ArgumentException("Root fill/hole mismatch.","rings");
		}

		/// <summary>
		/// Determines if the given ring which does not intersect the boundary of any node is contained within the nodes.
		/// </summary>
		/// <param name="other"></param>
		/// <returns>true if the ring is contained by the nodes.</returns>
		/// <remarks>
		/// The logic assumes the given ring does not intersect the boundary of any node within this collection or any node deeper in the tree.
		/// </remarks>
		public bool NonIntersectingContains(Ring2 other) {
			return null != other && _roots.NonIntersectingContains(other);
		}

		/// <summary>
		/// The root nodes for the tree.
		/// </summary>
		[NotNull] public IEnumerable<Node> Roots { get { return new ReadOnlyCollection<Node>(_roots); } }

	}
}
