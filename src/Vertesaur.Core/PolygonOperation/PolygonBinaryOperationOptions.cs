using System;
using System.Diagnostics.Contracts;

namespace Vertesaur.PolygonOperation
{

    /// <summary>
    /// Options to be used by various polygon operations.
    /// </summary>
    public class PolygonBinaryOperationOptions : PolygonOperationOptions
    {

        /// <summary>
        /// Creates a copy of the given object or the default object when given <c>null</c>.
        /// </summary>
        /// <param name="options">The object to clone.</param>
        /// <returns>A new object.</returns>
        public static PolygonBinaryOperationOptions CloneOrDefault(PolygonBinaryOperationOptions options) {
            Contract.Ensures(Contract.Result<PolygonBinaryOperationOptions>() != null);
            return null == options
                ? new PolygonBinaryOperationOptions()
                : new PolygonBinaryOperationOptions(options);
        }

        /// <summary>
        /// Default constructor initializes all options as defaults.
        /// </summary>
        public PolygonBinaryOperationOptions() { }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="options">The object to copy.</param>
        public PolygonBinaryOperationOptions(PolygonBinaryOperationOptions options)
            : base(options) {
            if (null == options) throw new ArgumentNullException("options");
            Contract.EndContractBlock();

            InvertLeftHandSide = options.InvertLeftHandSide;
            InvertRightHandSide = options.InvertRightHandSide;
        }

        /// <summary>
        /// When <c>true</c>, all polygons and rings supplied as the left hand parameter to
        /// a binary operation are to be treated as negated.
        /// </summary>
        public bool InvertLeftHandSide { get; set; }

        /// <summary>
        /// When <c>true</c>, all polygons and rings supplied as the right hand parameter to
        /// a binary operation are to be treated as negated.
        /// </summary>
        public bool InvertRightHandSide { get; set; }

    }
}
