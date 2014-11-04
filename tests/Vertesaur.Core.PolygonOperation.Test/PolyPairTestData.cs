using System.Collections.Generic;
using System.Linq;

#pragma warning disable 1591

namespace Vertesaur.PolygonOperation.Test
{

    /// <summary>
    /// An object storing test data that is to be used in other tests.
    /// </summary>
    public class PolyPairTestData
    {

        public PolyPairTestData(string name, Polygon2 a, Polygon2 b) {
            Name = name;
            A = a;
            B = b;
        }

        public PolyPairTestData(string name, Polygon2 a, Polygon2 b, Polygon2 r) {
            Name = name;
            A = a;
            B = b;
            R = r;
        }

        public PolyPairTestData(RingPairTestData ringData) : this(ringData, null) { }

        public PolyPairTestData(PolyPairTestData data, Polygon2 result) {
            Name = data.Name;
            A = new Polygon2(data.A);
            B = new Polygon2(data.B);
            R = null == result ? data.R : new Polygon2(result);
            CrossingPoints = (data.CrossingPoints ?? Enumerable.Empty<Point2>()).ToList();
        }

        public PolyPairTestData(RingPairTestData data, Polygon2 result) {
            Name = data.Name;
            A = new Polygon2(data.A);
            B = new Polygon2(data.B);
            R = null == result ? null : new Polygon2(result);
            CrossingPoints = (data.CrossingPoints ?? Enumerable.Empty<Point2>()).ToList();
        }

        public string Name { get; private set; }

        public Polygon2 A { get; private set; }

        public Polygon2 B { get; private set; }

        /// <summary>
        /// The expected result.
        /// </summary>
        public Polygon2 R { get; private set; }

        public List<Point2> CrossingPoints { get; set; }

        public override string ToString() {
            return Name;
        }

    }

}

#pragma warning restore 1591