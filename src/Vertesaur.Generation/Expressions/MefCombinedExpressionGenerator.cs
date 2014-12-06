#if !NO_MEF

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// A combined expression generator using MEF to locate all available expression generators at run-time.
    /// </summary>
    public class MefCombinedExpressionGenerator : IExpressionGenerator
    {

        static MefCombinedExpressionGenerator() {
            Default = new MefCombinedExpressionGenerator();
        }

        /// <summary>
        /// The default MEF combined expression generator.
        /// </summary>
        public static MefCombinedExpressionGenerator Default { get; private set; }

        /// <summary>
        /// A default MEF combined expression generator loading generators from the default assemblies.
        /// </summary>
        public MefCombinedExpressionGenerator() {
            ComposeFromAssemblies();
        }

        /// <summary>
        /// All expression generators known to this combined expression generator.
        /// </summary>
        [ImportMany(typeof(IExpressionGenerator))]
        public IEnumerable<IExpressionGenerator> AllExpressionGenerators { get; private set; }

        private void ComposeFromAssemblies() {
            var assemblies = new[]{
                Assembly.GetExecutingAssembly(),
                GetType().Assembly
            }.Distinct().ToArray();
            Contract.Assume(Contract.ForAll(assemblies, x => x != null));
            Contract.Assume(Contract.ForAll(assemblies, x => !x.ReflectionOnly));
            ComposeFromAssemblies(assemblies);
        }

        private void ComposeFromAssemblies(Assembly[] assemblies) {
            Contract.Requires(assemblies != null);
            Contract.Requires(Contract.ForAll(assemblies, x => x != null));
            Contract.Requires(Contract.ForAll(assemblies, x => !x.ReflectionOnly));

            ComposablePartCatalog catalog;
            if (assemblies.Length == 1) {
                Contract.Assume(!assemblies[0].ReflectionOnly);
                catalog = new AssemblyCatalog(assemblies[0]);
            }
            else {
                catalog = new AggregateCatalog(Array.ConvertAll<Assembly, ComposablePartCatalog>(assemblies, x => new AssemblyCatalog(x)));
            }

            Compose(catalog);
        }

        private void Compose(ComposablePartCatalog catalog) {
            Contract.Requires(null != catalog);
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        /// <inheritdoc/>
        public Expression Generate(IExpressionGenerationRequest request) {
            if (null == request) throw new ArgumentNullException("request");
            Contract.EndContractBlock();
            return AllExpressionGenerators
                .Select(x => x.Generate(request))
                .FirstOrDefault(x => null != x);
        }
    }
}

#endif