using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using JetBrains.Annotations;
using Vertesaur.CodeContracts;
using Vertesaur.SegmentOperation;
using Vertesaur.Utility;

namespace Vertesaur.PolygonOperation
{
	internal struct PolygonPointCrossingGenerator
	{

		private struct Segment2Data
		{
			public Segment2Data([NotNull] Segment2 segment, int segmentIndex, int ringIndex) {
				Contract.Requires(segment != null);
				Contract.Requires(segmentIndex >= 0);
				Contract.Requires(ringIndex >= 0);

				Segment = segment;
				SegmentIndex = segmentIndex;
				RingIndex = ringIndex;
			}

			[NotNull] public readonly Segment2 Segment;
			public readonly int SegmentIndex;
			public readonly int RingIndex;
		}

		private readonly Polygon2 _a;
		private readonly Polygon2 _b;

#if (SILVERLIGHT)
		private readonly Dictionary<Ring2, int[]> _sortedRingSegmentIndices;

		public PolygonPointCrossingGenerator([NotNull] Polygon2 a, [NotNull] Polygon2 b) {
			if(null == a)
				throw new ArgumentNullException("a");
			if(null == b)
				throw new ArgumentNullException("b");
			Contract.EndContractBlock();

			_a = a;
			_b = b;
			_sortedRingSegmentIndices = new Dictionary<Ring2, int[]>();
		}

		[Obsolete]
		private int[] GetSortedRingSegmentIndices([NotNull] Ring2 ring) {
#warning This method is not thread safe.
			// TODO: make thread safe? only if used in a parallel way...
			int[] result;
			if(!_sortedRingSegmentIndices.TryGetValue(ring, out result)) {
				result = GenerateSortedRingSegmentIndices(ring);
				_sortedRingSegmentIndices.Add(ring, result);
			}
			return result;
		}
#else
		private readonly System.Collections.Concurrent.ConcurrentDictionary<Ring2, int[]> _sortedRingSegmentIndices;

		public PolygonPointCrossingGenerator([NotNull] Polygon2 a, [NotNull] Polygon2 b) {
			if(null == a)
				throw new ArgumentNullException("a");
			if(null == b)
				throw new ArgumentNullException("b");
			Contract.EndContractBlock();

			_a = a;
			_b = b;
			_sortedRingSegmentIndices = new System.Collections.Concurrent.ConcurrentDictionary<Ring2, int[]>();
		}

		private int[] GetSortedRingSegmentIndices([NotNull] Ring2 ring) {
			return _sortedRingSegmentIndices.GetOrAdd(ring, GenerateSortedRingSegmentIndices);
		}
#endif

		[NotNull]
		public List<PolygonCrossing> GenerateCrossings() {
			Contract.Ensures(Contract.Result<List<PolygonCrossing>>() != null);
			Contract.EndContractBlock();

			return GenerateCrossingsSerial();
			/*return A.Count > 8 || B.Count > 8
				? GenerateCrossingsParallel()
				: GenerateCrossingsSerial();*/
		}

		[NotNull]
		private static int[] GenerateSortedRingSegmentIndices([NotNull] Ring2 ring) {
			Contract.Requires(ring != null);
			Contract.Ensures(Contract.Result<int[]>() != null);
			Contract.EndContractBlock();

			var indices = new int[ring.Count];
			for (int i = 0; i < indices.Length; i++)
				indices[i] = i;
			Array.Sort(indices, (a, b) => Compare(a, b, ring));
			return indices;
		}

		private static int Compare(int indexAStart, int indexBStart, [NotNull] Ring2 ring) {
			Contract.Requires(ring != null);
			Contract.Requires(indexAStart >= 0);
			Contract.Requires(indexAStart < ring.Count);
			Contract.Requires(indexBStart >= 0);
			Contract.Requires(indexBStart < ring.Count);
			Contract.EndContractBlock();

			var a = ring[indexAStart].X;
			var b = ring[IterationUtils.AdvanceLoopingIndex(indexAStart, ring.Count)].X;
			var c = ring[indexBStart].X;
			var d = ring[IterationUtils.AdvanceLoopingIndex(indexBStart, ring.Count)].X;
			int compareResult;
			if (d < c) {
				compareResult = Math.Min(a,b).CompareTo(d);
				return compareResult != 0 ? compareResult : Math.Max(a,b).CompareTo(c);
			}
			compareResult = Math.Min(a, b).CompareTo(c);
			return compareResult != 0 ? compareResult : Math.Max(a, b).CompareTo(d);
		}

		[NotNull]
		private List<PolygonCrossing> GenerateCrossingsSerial() {
			Contract.Ensures(Contract.Result<List<PolygonCrossing>>() != null);
			Contract.EndContractBlock();

			var result = new List<PolygonCrossing>();
			for (int ringIndexA = 0; ringIndexA < _a.Count; ringIndexA++) {
				var ringA = _a[ringIndexA];
				var ringAMbr = ringA.GetMbr();
				for (int ringIndexB = 0; ringIndexB < _b.Count; ringIndexB++) {
					var ringB = _b[ringIndexB];
					if (ringAMbr.Intersects(ringB.GetMbr()))
						result.AddRange(GenerateRingCrossings(ringA, ringB, ringIndexA, ringIndexB));
				}
			}
			return result;
		}

#if (SILVERLIGHT)
		[NotNull] private List<PolygonCrossing> GenerateCrossingsParallel() {
			Contract.Ensures(Contract.Result<List<PolygonCrossing>>() != null);
			Contract.EndContractBlock();

			throw new NotImplementedException();
		}
#else
		[NotNull] private List<PolygonCrossing> GenerateCrossingsParallel() {
			Contract.Ensures(Contract.Result<List<PolygonCrossing>>() != null);
			Contract.EndContractBlock();

			return GenerateRingCrossingGenerationTasks()
				.AsParallel().AsOrdered()
				.WithMergeOptions(ParallelMergeOptions.FullyBuffered)
				.SelectMany(Process).ToList();
		}
#endif

		private sealed class RingCrossingGenerationTask
		{
			public RingCrossingGenerationTask([NotNull] Ring2 ringA, [NotNull] Ring2 ringB, int ringIndexA, int ringIndexB) {
				CodeContractHelper.RequiresListIndexValid(ringA, ringIndexA);
				CodeContractHelper.RequiresListIndexValid(ringB, ringIndexB);
				Contract.EndContractBlock();

				RingA = ringA;
				RingB = ringB;
				RingIndexA = ringIndexA;
				RingIndexB = ringIndexB;
			}

			[NotNull] public Ring2 RingA { get; private set; }
			[NotNull] public Ring2 RingB { get; private set; }
			public int RingIndexA { get; private set; }
			public int RingIndexB { get; private set; }

			[ContractInvariantMethod]
			private void CodeContractInvariant() {
				Contract.Invariant(RingA != null);
				Contract.Invariant(RingA.Count > 0);
				Contract.Invariant(RingIndexA >= 0);
				Contract.Invariant(RingIndexA < RingA.Count);
				Contract.Invariant(RingB != null);
				Contract.Invariant(RingB.Count > 0);
				Contract.Invariant(RingIndexB >= 0);
				Contract.Invariant(RingIndexB < RingB.Count);
			}
		}

		[NotNull]
		private List<PolygonCrossing> Process([NotNull] RingCrossingGenerationTask task) {
			Contract.Requires(task != null);
			Contract.Ensures(Contract.Result<List<PolygonCrossing>>() != null);
			Contract.EndContractBlock();

			return GenerateRingCrossings(task.RingA, task.RingB, task.RingIndexA, task.RingIndexB);
		}

		[NotNull]
		private IEnumerable<RingCrossingGenerationTask> GenerateRingCrossingGenerationTasks() {
			Contract.Ensures(Contract.Result<IEnumerable<RingCrossingGenerationTask>>() != null);
			Contract.EndContractBlock();

			for (int ringIndexA = 0; ringIndexA < _a.Count; ringIndexA++) {
				var ringA = _a[ringIndexA];
				var ringAMbr = ringA.GetMbr();
				for (int ringIndexB = 0; ringIndexB < _b.Count; ringIndexB++) {
					var ringB = _b[ringIndexB];
					if (ringAMbr.Intersects(ringB.GetMbr()))
						yield return new RingCrossingGenerationTask(
							ringA,
							ringB,
							ringIndexA,
							ringIndexB
						);
				}
			}
		}

		[NotNull]
		private List<PolygonCrossing> GenerateRingCrossings([NotNull] Ring2 ringA, [NotNull] Ring2 ringB, int ringIndexA, int ringIndexB) {
			CodeContractHelper.RequiresListIndexValid(ringA, ringIndexA);
			CodeContractHelper.RequiresListIndexValid(ringB, ringIndexB);
			Contract.Ensures(Contract.Result<List<PolygonCrossing>>() != null);
			Contract.EndContractBlock();

			return GenerateRingCrossingsBruteForce(ringA, ringB, ringIndexA, ringIndexB);
			/*return ringA.Count > 32 && ringB.Count > 32
				? GenerateRingCrossingsSorted(ringA, ringB, ringIndexA, ringIndexB)
				: GenerateRingCrossingsBruteForce(ringA, ringB, ringIndexA, ringIndexB);*/
		}

		/// <summary>
		/// Determines the points that would need to be inserted into the resulting
		/// intersection geometry between the two given rings, at the location where
		/// their boundaries cross.
		/// </summary>
		/// <param name="ringA">The first ring to test.</param>
		/// <param name="ringB">The second ring to test.</param>
		/// <param name="ringIndexA">The ring index on polygon a.</param>
		/// <param name="ringIndexB">The ring index on polygon b.</param>
		[NotNull]
		private List<PolygonCrossing> GenerateRingCrossingsSorted([NotNull] Ring2 ringA, [NotNull] Ring2 ringB, int ringIndexA, int ringIndexB) {
			CodeContractHelper.RequiresListIndexValid(ringA, ringIndexA);
			CodeContractHelper.RequiresListIndexValid(ringB, ringIndexB);
			Contract.Ensures(Contract.Result<List<PolygonCrossing>>() != null);
			Contract.EndContractBlock();

			int minSegmentSearchIndicesB = 0;
			var segmentSearchIndicesA = GetSortedRingSegmentIndices(ringA);
			var segmentSearchIndicesB = GetSortedRingSegmentIndices(ringB);
			var countA = ringA.Count;
			var countB = ringB.Count;
			var crossings = new List<PolygonCrossing>();
			for (var segmentSearchIndexA = 0; segmentSearchIndexA < segmentSearchIndicesA.Length; segmentSearchIndexA++) {
				double temp;
				var segmentStartIndexA = segmentSearchIndicesA[segmentSearchIndexA];
				var a = ringA[segmentStartIndexA];
				var smallestXValueOnA = a.X;
				var b = ringA[IterationUtils.AdvanceLoopingIndex(segmentStartIndexA, countA)];
				var largestXValueOnA = b.X;
				if (largestXValueOnA < smallestXValueOnA) {
					temp = largestXValueOnA;
					largestXValueOnA = smallestXValueOnA;
					smallestXValueOnA = temp;
				}
				var segmentDataA = new Segment2Data(
					new Segment2(a, b),
					segmentStartIndexA,
					ringIndexA
				);
				var segmentAMbr = segmentDataA.Segment.GetMbr();
				for (int segmentSearchIndexB = minSegmentSearchIndicesB; segmentSearchIndexB < segmentSearchIndicesB.Length; segmentSearchIndexB++) {
					int segmentStartIndexB = segmentSearchIndicesB[segmentSearchIndexB];
					var c = ringB[segmentStartIndexB];
					var smallestXValueOnC = c.X;
					var d = ringB[IterationUtils.AdvanceLoopingIndex(segmentStartIndexB, countB)];
					var largestXValueOnD = d.X;
					if (largestXValueOnD < smallestXValueOnC) {
						temp = largestXValueOnD;
						largestXValueOnD = smallestXValueOnC;
						smallestXValueOnC = temp;
					}
					if (largestXValueOnD < smallestXValueOnA) {
						minSegmentSearchIndicesB = segmentSearchIndexB + 1;
						continue;
					}
					if (smallestXValueOnC > largestXValueOnA)
						break;

					if (!segmentAMbr.Intersects(c, d))
						continue;

					AddPointCrossings(
						crossings,
						segmentDataA,
						new Segment2Data(
							new Segment2(c, d),
							segmentStartIndexB,
							ringIndexB
						)
					);
				}

				if (minSegmentSearchIndicesB >= segmentSearchIndicesB.Length)
					break; // wont do anything else
			}

			return crossings;
		}

		[NotNull]
		private static List<PolygonCrossing> GenerateRingCrossingsBruteForce([NotNull] Ring2 ringA, [NotNull] Ring2 ringB, int ringIndexA, int ringIndexB) {
			CodeContractHelper.RequiresListIndexValid(ringA, ringIndexA);
			CodeContractHelper.RequiresListIndexValid(ringB, ringIndexB);
			Contract.Ensures(Contract.Result<List<PolygonCrossing>>() != null);
			Contract.EndContractBlock();

			var ringACount = ringA.Count;
			var ringBCount = ringB.Count;
			var segmentStartIndexA = ringACount - 1;
			var b = ringA[segmentStartIndexA];
			var crossings = new List<PolygonCrossing>();
			for (int segmentEndIndexA = 0; segmentEndIndexA < ringACount; segmentStartIndexA = segmentEndIndexA++) {
				var a = b;
				b = ringA[segmentEndIndexA];
				var segmentDataA = new Segment2Data(
					new Segment2(a, b),
					segmentStartIndexA,
					ringIndexA
				);
				var segmentMbrA = segmentDataA.Segment.GetMbr();
				var segmentStartIndexB = ringBCount - 1;
				var d = ringB[segmentStartIndexB];
				for (int segmentEndIndexB = 0; segmentEndIndexB < ringBCount; segmentStartIndexB = segmentEndIndexB++) {
					var c = d;
					d = ringB[segmentEndIndexB];
					if (!segmentMbrA.Intersects(c, d))
						continue;

					AddPointCrossings(
						crossings,
						segmentDataA,
						new Segment2Data(
							new Segment2(c, d),
							segmentStartIndexB,
							ringIndexB
						)
					);
				}
			}
			return crossings;
		}

		private static void AddPointCrossings([NotNull] List<PolygonCrossing> results, Segment2Data segmentDataA, Segment2Data segmentDataB) {
			Contract.Requires(results != null);
			Contract.Ensures(results.Count >= Contract.OldValue(results).Count);
			Contract.EndContractBlock();

			Contract.Assume(null != segmentDataA.Segment);
			Contract.Assume(null != segmentDataB.Segment);

			var intersectionDetails = SegmentIntersectionOperation.IntersectionDetails(segmentDataA.Segment, segmentDataB.Segment);
			if (intersectionDetails is SegmentIntersectionOperation.PointResult) {
				var pointResult = (SegmentIntersectionOperation.PointResult)intersectionDetails;
				if (IsNotHead(pointResult))
					results.Add(CreatePolygonCrossing(pointResult, segmentDataA, segmentDataB));
			}
			else if (intersectionDetails is SegmentIntersectionOperation.SegmentResult) {
				var segmentResult = intersectionDetails as SegmentIntersectionOperation.SegmentResult;
				var bIsNotHead = IsNotHead(segmentResult.B);
				if (IsNotHead(segmentResult.A)) {
					var resultA = CreatePolygonCrossing(segmentResult.A, segmentDataA, segmentDataB);
					if (bIsNotHead)
						results.AddRange(new[] { resultA, CreatePolygonCrossing(segmentResult.B, segmentDataA, segmentDataB) });
					else
						results.Add(resultA);
				}
				else if (bIsNotHead)
					results.Add(CreatePolygonCrossing(segmentResult.B, segmentDataA, segmentDataB));
			}
		}

		[NotNull]
		private static PolygonCrossing CreatePolygonCrossing(
			SegmentIntersectionOperation.PointResult pointResult,
			Segment2Data segmentDataA, Segment2Data segmentDataB
		) {
			Contract.Ensures(Contract.Result<PolygonCrossing>() != null);
			Contract.EndContractBlock();

			Contract.Assume(segmentDataA.RingIndex >= 0);
			Contract.Assume(segmentDataA.SegmentIndex >= 0);
			Contract.Assume(segmentDataB.RingIndex >= 0);
			Contract.Assume(segmentDataB.SegmentIndex >= 0);

			return new PolygonCrossing(
				pointResult.P,
				new PolygonBoundaryLocation(segmentDataA.RingIndex, segmentDataA.SegmentIndex, pointResult.S),
				new PolygonBoundaryLocation(segmentDataB.RingIndex, segmentDataB.SegmentIndex, pointResult.T)
			);
		}

		private static bool IsNotHead(SegmentIntersectionOperation.PointResult pointResult) {
			return 0 == ((pointResult.TypeA | pointResult.TypeB) & SegmentIntersectionOperation.SegmentIntersectionType.Head);
		}
	}
}
