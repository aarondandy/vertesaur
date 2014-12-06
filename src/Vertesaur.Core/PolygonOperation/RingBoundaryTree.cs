using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Vertesaur.Utility;

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
            public NodeCollection(List<Node> nodes)
                : base(nodes ?? (nodes = new List<Node>())) {
                _nodes = nodes;
            }

            [ContractInvariantMethod]
            [Conditional("CONTRACTS_FULL")]
            private void CodeContractInvariant() {
                Contract.Invariant(_nodes != null);
            }

            /// <summary>
            /// Determines if the nodes are holes, fills, or unknown.
            /// </summary>
            public bool? NodesAreHole {
                get {
                    foreach (var node in _nodes) {
                        if (node.Ring.Hole.HasValue) {
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
            public bool NodesAreValid(bool holeExpected) {
                return _nodes.All(node => !node.Ring.Hole.HasValue || node.Ring.Hole.Value == holeExpected);
            }

            /// <summary>
            /// Determines if the given ring which does not intersect the boundary of any node is contained within the nodes.
            /// </summary>
            /// <param name="other"></param>
            /// <returns>true if the ring with contained by the nodes.</returns>
            /// <remarks>
            /// The logic assumes the given ring does not intersect the boundary of any node within this collection or any node deeper in the tree.
            /// </remarks>
            public bool NonIntersectingContains(Ring2 other) {
                if (null == other) throw new ArgumentNullException("other");
                Contract.EndContractBlock();

                var nodeHoleFlag = NodesAreHole;
                return nodeHoleFlag.HasValue && nodeHoleFlag.Value // nodes are hole nodes?
                    ? NonIntersectingHolesContains(other)
                    : NonIntersectingFillsContains(other);
            }


            private bool NonIntersectingFillsContains(Ring2 other) {
                Contract.Requires(other != null);

                foreach(var node in _nodes){
                    if (other.NonIntersectingWithin(node.Ring))
                        return node.Count <= 0 || node.NonIntersectingContains(other);
                }
                return false;
            }

            private bool NonIntersectingHolesContains(Ring2 other) {
                Contract.Requires(other != null);

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
            public void RemoveAll(Predicate<Node> predicate) {
                if (null == predicate) throw new ArgumentNullException("predicate");
                Contract.EndContractBlock();

                _nodes.RemoveAll(predicate);
            }

            [Pure] private bool IsNodeValid(Node item) {
                if (null == item)
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
            /// <exception cref="System.ArgumentException">Item is not valid for this collection.</exception>
            protected sealed override void SetItem(int index, Node item) {
                Contract.EndContractBlock();
                if (!IsNodeValid(item))
                    throw new ArgumentException("item is not valid for this collection.", "item");

                base.SetItem(index, item);
            }

            /// <summary>
            /// Inserts the node if it is valid for the collection.
            /// </summary>
            /// <param name="index">The index to insert at.</param>
            /// <param name="item">The item to store at the index.</param>
            /// <exception cref="System.ArgumentException">Item is not valid for this collection.</exception>
            protected sealed override void InsertItem(int index, Node item) {
                Contract.EndContractBlock();
                if (!IsNodeValid(item))
                    throw new ArgumentException("item is not valid for this collection.", "item");

                base.InsertItem(index, item);
            }

        }

        /// <summary>
        /// A ring node in a ring boundary tree.
        /// </summary>
        public sealed class Node : NodeCollection
        {

            internal Node(Ring2 ring, IEnumerable<Node> children)
                : this(ring, null == children ? null : new List<Node>(children)) {
                Contract.Requires(null != ring);
                Contract.EndContractBlock();
            }

            private Node(Ring2 ring, List<Node> children)
                : base(children) {
                if (null == ring) throw new ArgumentNullException("ring");
                Contract.EndContractBlock();

                Ring = ring;

                if (!NodesAreValid(!Hole))
                    throw new ArgumentException("Fill/Hole mismatch.", "children");
            }

            [ContractInvariantMethod]
            [Conditional("CONTRACTS_FULL")]
            private void CodeContractInvariant() {
                Contract.Invariant(Ring != null);
            }

            /// <summary>
            /// The ring the node represents.
            /// </summary>
            public Ring2 Ring { get; private set; }

            /// <summary>
            /// True when the ring is explicitly set to being a hole.
            /// </summary>
            public bool Hole { get { return Ring.Hole.HasValue && Ring.Hole.Value; } }

        }

        private static List<Node> BuildTree(IEnumerable<Ring2> rings) {
            if (null == rings) throw new ArgumentNullException("rings");
            Contract.Ensures(Contract.Result<List<Node>>() != null);

            var roots = new List<Node>();
            foreach (var ring in rings.Where(x => null != x)) {
                Contract.Assume(null != ring);
                ApplyRingToRoots(roots, ring);
            }
            return roots;
        }

        private static List<Node> BuildTree(Polygon2 polygon) {
            if (null == polygon) throw new ArgumentNullException("polygon");
            Contract.Ensures(Contract.Result<List<Node>>() != null);

            var roots = new List<Node>();
            for (int i = 0; i < polygon.Count; i++) {
                Contract.Assume(null != polygon[i]);
                ApplyRingToRoots(roots, polygon[i]);
            }
            return roots;
        }

        private static void ApplyRingToRoots(List<Node> roots, Ring2 ring) {
            Contract.Requires(null != roots);
            Contract.Requires(null != ring);

            Contract.Assume(Contract.ForAll(roots, x => null != x));
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

        private static IEnumerable<Node> BoundBy(IEnumerable<Node> nodes, Ring2 ring) {
            Contract.Requires(nodes != null);
            Contract.Requires(ring != null);
            Contract.Ensures(Contract.Result<IEnumerable<Node>>() != null);

            return nodes.Where(n => RingIsNonIntersectingBoundBy(n.Ring, ring));
        }

        private static Node FindParent(Ring2 ring, IEnumerable<Node> roots) {
            Contract.Requires(ring != null);
            Contract.Requires(roots != null);

            foreach (var node in roots) {
                if (RingIsNonIntersectingBoundBy(ring, node.Ring)) {
                    return FindParent(ring, node) ?? node;
                }
            }
            return null;
        }

        private static bool RingIsNonIntersectingBoundBy(Ring2 ring, Ring2 container) {
            Contract.Requires(null != ring);
            Contract.Requires(null != container);

            var ringMbr = ring.GetMbr();
            if (ringMbr != null){
                if(ringMbr.Within(container.GetMbr())) {
                    for (var i = 0; i < ring.Count; i++) {
                        var intersectionCount = container.IntersectionPositiveXRayCount(ring[i]);
                        if (intersectionCount > 0)
                            return 1 == (intersectionCount % 2);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Constructs a ring boundary tree from a collection of non-intersecting rings.
        /// </summary>
        /// <param name="rings">A collection of non-intersecting rings.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="rings"/> is null.</exception>
        public RingBoundaryTree(IEnumerable<Ring2> rings)
            : this(BuildTree(rings)) {
            Contract.Requires(null != rings);
            Contract.EndContractBlock();
        }

        /// <summary>
        /// Constructs a ring boundary tree from a polygon.
        /// </summary>
        /// <param name="polygon">A collection of non-intersecting rings.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="polygon"/> is null.</exception>
        public RingBoundaryTree(Polygon2 polygon)
            : this(BuildTree(polygon)) {
            Contract.Requires(null != polygon);
        }

        private RingBoundaryTree(List<Node> roots) {
            Contract.Requires(null != roots);

            Roots = new NodeCollection(roots);
            var hole = Roots.NodesAreHole;
            if (!Roots.NodesAreValid(hole.HasValue && hole.Value))
                throw new ArgumentException("Root fill/hole mismatch.", "roots");
        }

        [ContractInvariantMethod]
        [Conditional("CONTRACTS_FULL")]
        private void CodeContractInvariant() {
            Contract.Invariant(null != Roots);
        }

        /// <summary>
        /// The root nodes for the tree.
        /// </summary>
        public NodeCollection Roots { get; private set; }

        /// <summary>
        /// Determines if the given ring which does not intersect the boundary of any node is contained within the nodes.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true if the ring is contained by the nodes.</returns>
        /// <remarks>
        /// The logic assumes the given ring does not intersect the boundary of any node within this collection or any node deeper in the tree.
        /// </remarks>
        public bool NonIntersectingContains(Ring2 other) {
            return other != null && Roots.NonIntersectingContains(other);
        }

    }
}
