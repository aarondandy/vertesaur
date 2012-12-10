using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{
	public class MefCombinedExpressionGenerator : IExpressionGenerator
	{

		static MefCombinedExpressionGenerator(){
			Default = new MefCombinedExpressionGenerator();
		}

		public static MefCombinedExpressionGenerator Default { get; private set; }

		public MefCombinedExpressionGenerator(){
			ComposeFromAssemblies();
		}

		[ImportMany(typeof(IExpressionGenerator))]
		public IEnumerable<IExpressionGenerator> AllExpressionGenerators { get; private set; }

		private void ComposeFromAssemblies(){
			var assemblies = new[]{
				Assembly.GetExecutingAssembly(),
				GetType().Assembly
			}.Distinct().ToArray();
			ComposeFromAssemblies(assemblies);
		}

		private void ComposeFromAssemblies(Assembly[] assemblies){
			ComposablePartCatalog catalog;

			if(assemblies.Length == 0)
				catalog = new AssemblyCatalog(assemblies[0]);
			else
				catalog = new AggregateCatalog(assemblies.Select(x => new AssemblyCatalog(x)));

			Compose(catalog);
		}

		private void Compose(ComposablePartCatalog catalog){
			var container = new CompositionContainer(catalog);
			container.ComposeParts(this);
		}

		/// <inheritdoc/>
		public Expression GenerateExpression(IExpressionGenerationRequest request) {
			if(null == request) throw new ArgumentNullException("request");
			Contract.EndContractBlock();
			return AllExpressionGenerators
				.Select(x => x.GenerateExpression(request))
				.FirstOrDefault(x => null != x);
		}
	}
}
