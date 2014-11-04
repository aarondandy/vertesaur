namespace Vertesaur
{

    /// <summary>
    /// Functionality to determine various geometric relations between two objects.
    /// </summary>
    /// <typeparam name="TObject">The other object type.</typeparam>
    public interface ISpatiallyRelatable<in TObject> :
        ISpatiallyEquatable<TObject>,
        IRelatableDisjoint<TObject>,
        IRelatableIntersects<TObject>,
        IRelatableTouches<TObject>,
        IRelatableCrosses<TObject>,
        IRelatableWithin<TObject>,
        IRelatableContains<TObject>,
        IRelatableOverlaps<TObject>
    //IRelatableCovers<TObject>,
    //IRelatableCoveredBy<TObject>
    {
    }

}
