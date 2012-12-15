using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// A block expression builder that assists with the construction of block expressions.
	/// </summary>
	public class BlockExpressionBuilder : Collection<Expression>
	{

		private readonly LocalExpressionVariableManager _localManager;

		/// <summary>
		/// Constructs a new block expression builder.
		/// </summary>
		/// <param name="localManager">The local variable manager to use.</param>
		public BlockExpressionBuilder(LocalExpressionVariableManager localManager = null)
		{
			_localManager = localManager ?? new LocalExpressionVariableManager();
		}

		/// <summary>
		/// The local manager used to provision local variables.
		/// </summary>
		public LocalExpressionVariableManager LocalManager {
			get {
				Contract.Ensures(Contract.Result<LocalExpressionVariableManager>() != null);
				return _localManager;
			}
		}

		/// <summary>
		/// Builds a new block expression using the required local variables and statments.
		/// </summary>
		/// <returns></returns>
		public BlockExpression GetExpression() {
			return Expression.Block(_localManager.AllVariables, this);
		}

		/// <summary>
		/// Add a group of statment expressions to this block expression builder.
		/// </summary>
		/// <param name="expressions">The expressions to add.</param>
		/// <returns>A reference to this block expression builder for chaining.</returns>
		public BlockExpressionBuilder AddRange(IEnumerable<Expression> expressions) {
			if(null == expressions) throw new ArgumentNullException("expressions");
			Contract.Ensures(Contract.Result<BlockExpressionBuilder>() == this);
			Contract.EndContractBlock();
			foreach(var expression in expressions)
				Add(expression);

			return this;
		}

		/// <summary>
		/// Add expressions using a temporary local provided from the local manager.
		/// </summary>
		/// <param name="localType">The local variable type.</param>
		/// <param name="generator">The generator that will create the statment expressions.</param>
		/// <returns>A reference to this block expression builder for chaining.</returns>
		public BlockExpressionBuilder AddUsingLocal(Func<ParameterExpression, IEnumerable<Expression>> generator, Type localType) {
			if(null == localType) throw new ArgumentNullException("localType");
			if(null == generator) throw new ArgumentNullException("generator");
			Contract.Ensures(Contract.Result<BlockExpressionBuilder>() == this);
			Contract.EndContractBlock();
			using (var local = LocalManager.Use(localType)) {
				var expressions = generator(local.Variable);
				if(null == expressions)
					throw new InvalidOperationException("Generator may not return null.");
				AddRange(expressions);
			}
			return this;
		}

		/// <summary>
		/// Add expressions using temporary local variables provided by the local manager.
		/// </summary>
		/// <param name="generator">The generator that will create the statment expressions.</param>
		/// <param name="types">The types of the local variables to create.</param>
		/// <returns>A reference to this block expression builder for chaining.</returns>
		public BlockExpressionBuilder AddUsingLocals(Func<ParameterExpression[], IEnumerable<Expression>> generator, params Type[] types) {
			if(null == generator) throw new ArgumentNullException("generator");
			if(null == types) throw new ArgumentNullException("types");
			Contract.Ensures(Contract.Result<BlockExpressionBuilder>() == this);
			Contract.EndContractBlock();
			var locals = Array.ConvertAll(types, t => LocalManager.GetVariable(t));
			try {
				var expressions = generator(locals);
				if(null == expressions)
					throw new InvalidOperationException("Generator may not return null.");
				AddRange(expressions);
			}
			finally {
				LocalManager.ReleaseVariables(locals);
			}
			return this;
		}
		
		/// <summary>
		/// Add expressions using a captured expression as local variables provided by the local manager.
		/// </summary>
		/// <param name="generator">The generator that will create the statment expressions.</param>
		/// <param name="expressionToCapture">The expression to capture as a local variable.</param>
		/// <returns>A reference to this block expression builder for chaining.</returns>
		[Obsolete]
		public BlockExpressionBuilder AddUsingAssignedLocal(Func<ParameterExpression, IEnumerable<Expression>> generator, Expression expressionToCapture) {
			if (null == generator) throw new ArgumentNullException("generator");
			if (null == expressionToCapture) throw new ArgumentNullException("expressionToCapture");
			Contract.Ensures(Contract.Result<BlockExpressionBuilder>() == this);
			Contract.EndContractBlock();
			using (var local = LocalManager.Use(expressionToCapture.Type)) {
				Add(Expression.Assign(local.Variable, expressionToCapture));
				var expressions = generator(local.Variable);
				if (null == expressions)
					throw new InvalidOperationException("Generator may not return null.");
				AddRange(expressions);
			}
			return this;
		}

		public BlockExpressionBuilder AddUsingMemoryLocationOrConstant(Func<Expression, IEnumerable<Expression>> generator, Expression expressionToCapture) {
			if (null == generator) throw new ArgumentNullException("generator");
			if (null == expressionToCapture) throw new ArgumentNullException("expressionToCapture");
			Contract.Ensures(Contract.Result<BlockExpressionBuilder>() == this);
			Contract.EndContractBlock();
			LocalExpressionVariableManager.VariableUsage usage;
			Expression capturedExpression;
			if (expressionToCapture.IsMemoryLocationOrConstant()) {
				usage = null;
				capturedExpression = expressionToCapture;
			}
			else {
				usage = LocalManager.Use(expressionToCapture.Type);
				capturedExpression = usage.Variable;
				Add(Expression.Assign(usage.Variable, expressionToCapture));
			}
			try {
				var expressions = generator(capturedExpression);
				if(null == expressions)
					throw new InvalidOperationException("Generator may not return null.");
				AddRange(expressions);
			}
			finally {
				if(null != usage)
					usage.Dispose();
			}
			return this;
		}

		/// <summary>
		/// Add expressions using captured expressions as local variables provided by the local manager.
		/// </summary>
		/// <param name="generator">The generator that will create the statment expressions.</param>
		/// <param name="expressionsToCapture">The expressions to capture as local variables.</param>
		/// <returns>A reference to this block expression builder for chaining.</returns>
		[Obsolete]
		public BlockExpressionBuilder AddUsingAssignedLocals(Func<ParameterExpression[], IEnumerable<Expression>> generator, params Expression[] expressionsToCapture) {
			if (null == generator) throw new ArgumentNullException("generator");
			if (null == expressionsToCapture) throw new ArgumentNullException("expressionsToCapture");
			Contract.Ensures(Contract.Result<BlockExpressionBuilder>() == this);
			Contract.EndContractBlock();
			var locals = Array.ConvertAll(expressionsToCapture, e => LocalManager.GetVariable(e.Type));
			try {
				for (int i = 0; i < locals.Length; i++) {
					Add(Expression.Assign(locals[i], expressionsToCapture[i]));
				}
				var expressions = generator(locals);
				if (null == expressions)
					throw new InvalidOperationException("Generator may not return null.");
				AddRange(expressions);
			}
			finally {
				LocalManager.ReleaseVariables(locals);
			}
			return this;
		}

		public BlockExpressionBuilder AddUsingMemoryLocationsOrConstants(Func<Expression[], IEnumerable<Expression>> generator, params Expression[] expressionsToCapture) {
			if (null == generator) throw new ArgumentNullException("generator");
			if (null == expressionsToCapture) throw new ArgumentNullException("expressionsToCapture");
			Contract.Ensures(Contract.Result<BlockExpressionBuilder>() == this);
			Contract.EndContractBlock();
			var usages = Array.ConvertAll(expressionsToCapture, e => e.IsMemoryLocationOrConstant() ? null : LocalManager.Use(e.Type));
			try {
				var capturedExpressions = new Expression[expressionsToCapture.Length];
				for (int i = 0; i < capturedExpressions.Length; i++) {
					var usage = usages[i];
					if (null == usage) {
						capturedExpressions[i] = expressionsToCapture[i];
					}
					else {
						Add(Expression.Assign(usage.Variable, expressionsToCapture[i]));
						capturedExpressions[i] = usage.Variable;
					}
				}
				var expressions = generator(capturedExpressions);
				if (null == expressions)
					throw new InvalidOperationException("Generator may not return null.");
				AddRange(expressions);
			}
			finally {
				for (int i = 0; i < usages.Length; i++) {
					if(null != usages[i])
						usages[i].Dispose();
				}
			}
			return this;
		}

	}
}
