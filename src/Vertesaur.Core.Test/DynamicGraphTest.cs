using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Vertesaur.Search;

namespace Vertesaur.Core.Test
{
	[TestFixture]
	public class DynamicGraphTest
	{

		private struct DualKey<TKey> :
			IEquatable<DualKey<TKey>>
			where TKey : IEquatable<TKey>
		{
			public readonly TKey A;
			public readonly TKey B;

			public DualKey(TKey a, TKey b) {
				A = a;
				B = b;
			}

			public bool Equals(DualKey<TKey> other) {
				return A.Equals(other.A) && B.Equals(other.B);
			}

			public override bool Equals(object obj) {
				return obj is DualKey<TKey> && Equals((DualKey<TKey>)obj);
			}

			public override int GetHashCode() {
				return A.GetHashCode() ^ -(B.GetHashCode());
			}

		}

		[Test]
		public void Wikipedia_graph_sample() {

			var nodes = new[] {1,2,3,4,5,6};
			var edges = new Dictionary<DualKey<int>, double> {
				{new DualKey<int>(1,2), 7},
				{new DualKey<int>(1,3), 9},
				{new DualKey<int>(1,6), 14},
				{new DualKey<int>(2,3), 10},
				{new DualKey<int>(2,4), 15},
				{new DualKey<int>(3,4), 11},
				{new DualKey<int>(3,6), 2},
				{new DualKey<int>(4,5), 6},
				{new DualKey<int>(5,6), 9}
			};

			// add the reverse edges
			foreach(var reversedEdgeData in edges.Select(x => new{Key = new DualKey<int>(x.Key.B,x.Key.A), Value = x.Value}).ToList()){
				edges.Add(reversedEdgeData.Key, reversedEdgeData.Value);
			}

			var result = DynamicGraph.FindPath(
				1, 5,
				(int node, double curCost) => edges
					.Where(x => x.Key.A == node)
					.Select(edge => new DynamicGraphNodeData<int, double, KeyValuePair<DualKey<int>,double>>(edge.Key.B, curCost + edge.Value, edge))
			);

			Assert.AreEqual(new[] { 1, 3, 6, 5 }, result.Select(x => x.Node).ToArray());
			Assert.AreEqual(20, result.Last().Cost);

		}

	}
}
