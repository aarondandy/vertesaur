using System;
using System.Diagnostics.Contracts;

namespace Vertesaur.PolygonOperation
{

    /// <summary>
    /// Options to be used by various polygon operations.
    /// </summary>
    public class PolygonOperationOptions
    {

        /// <summary>
        /// Creates a copy of the given object or the default object when given <c>null</c>.
        /// </summary>
        /// <param name="options">The object to clone.</param>
        /// <returns>A new object.</returns>
        public static PolygonOperationOptions CloneOrDefault(PolygonOperationOptions options) {
            Contract.Ensures(Contract.Result<PolygonOperationOptions>() != null);

            return null == options
                ? new PolygonOperationOptions()
                : new PolygonOperationOptions(options);
        }

        /// <summary>
        /// Default constructor initializes all options as defaults.
        /// </summary>
        public PolygonOperationOptions() { }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="options">Object to copy.</param>
        public PolygonOperationOptions(PolygonOperationOptions options) {
            if (null == options) throw new ArgumentNullException("options");
            Contract.EndContractBlock();
            InvertResult = options.InvertResult;
        }

        /// <summary>
        /// When true the result should be inverted.
        /// </summary>
        public bool InvertResult { get; set; }

    }
}
