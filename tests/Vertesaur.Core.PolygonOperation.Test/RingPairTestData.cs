using System.Collections.Generic;

#pragma warning disable 1591

namespace Vertesaur.PolygonOperation.Test
{

    /// <summary>
    /// An object storing test data that is to be used in other tests.
    /// </summary>
    public class RingPairTestData
    {

        public RingPairTestData(string name, Ring2 a, Ring2 b) {
            Name = name;
            A = a;
            B = b;
        }

        public string Name { get; private set; }

        public Ring2 A { get; private set; }

        public Ring2 B { get; private set; }

        public List<Point2> CrossingPoints { get; set; }

        public override string ToString() {
            return Name;
        }

        internal RingPairTestData Reverse() {
            return new RingPairTestData(Name + " (Reverse)", B, A);
        }

    }

}

#pragma warning restore 1591