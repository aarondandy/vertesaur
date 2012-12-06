
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading;

namespace Vertesaur.Generation.ExpressionBuilder
{
	public class IntermediaryVariableManager
	{

		public sealed class VariableUse : IDisposable
		{

			private readonly IntermediaryVariableManager _mgr;
			private readonly ParameterExpression _var;

			internal VariableUse(IntermediaryVariableManager mgr, ParameterExpression var) {
				_mgr = mgr;
				_var = var;
			}

			public ParameterExpression Variable { get { return _var; } }

			public void Dispose() {
				_mgr.ReleaseVariable(_var);
			}

		}

		private int _variableIdCounter;
		private readonly LinkedList<ParameterExpression> _inUse;
		private readonly LinkedList<ParameterExpression> _pending;

		public IntermediaryVariableManager() {
			_inUse = new LinkedList<ParameterExpression>();
			_pending = new LinkedList<ParameterExpression>();
		}

		private int GetNewVariableNumber() {
			return Interlocked.Increment(ref _variableIdCounter);
		}

		private ParameterExpression RemoveFirstPending(Type type) {
			var node = _pending.First;
			while(null != node) {
				if(node.Value.Type == type) {
					_pending.Remove(node);
					return node.Value;
				}
				node = node.Next;
			}
			return null;
		}

		public ParameterExpression GetVariable(Type type) {
			var var = RemoveFirstPending(type)
				?? Expression.Variable(type, GetNewVariableNumber().ToString(CultureInfo.InvariantCulture));
			_inUse.AddFirst(var);
			return var;
		}

		public bool ReleaseVariable(ParameterExpression variable) {
			if(_inUse.Remove(variable)) {
				_pending.AddFirst(variable);
				return true;
			}
			return false;
		}

		public VariableUse Use(Type type) {
			return new VariableUse(this, GetVariable(type));
		}

	}
}
