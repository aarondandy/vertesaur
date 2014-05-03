
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// Manages local variables during the dynamic generation of expression blocks.
    /// </summary>
    public class LocalExpressionVariableManager
    {

        /// <summary>
        /// A variable container that returns the expression variable back to an unused pool when disposed.
        /// </summary>
        public sealed class VariableUsage : IDisposable
        {

            private readonly LocalExpressionVariableManager _manager;
            private readonly ParameterExpression _variable;

            internal VariableUsage(LocalExpressionVariableManager manager, ParameterExpression variable) {
                Contract.Requires(null != manager);
                Contract.Requires(null != variable);
                _manager = manager;
                _variable = variable;
            }

            [ContractInvariantMethod]
            private void CodeContractInvariants() {
                Contract.Invariant(_manager != null);
                Contract.Invariant(_variable != null);
            }

            /// <summary>
            /// The parameter expression that is to be used as a local variable.
            /// </summary>
            public ParameterExpression Variable {
                get {
                    Contract.Ensures(Contract.Result<ParameterExpression>() != null);
                    return _variable;
                }
            }

            /// <summary>
            /// Demotes the contained variable expression to the pending variable pool.
            /// </summary>
            public void Dispose() {
                _manager.ReleaseVariable(_variable);
            }

        }

        private int _variableIdCounter;
        private readonly LinkedList<ParameterExpression> _inUse;
        private readonly LinkedList<ParameterExpression> _pending;

        /// <summary>
        /// Manages the lifetimes and reuse of parameter expressions which are used as local variables.
        /// </summary>
        public LocalExpressionVariableManager() {
            _inUse = new LinkedList<ParameterExpression>();
            _pending = new LinkedList<ParameterExpression>();
        }

        [ContractInvariantMethod]
        private void CodeContractInvariants() {
            Contract.Invariant(_inUse != null);
            Contract.Invariant(_pending != null);
        }

        /// <summary>
        /// Returns all of the variables that are in use or that have been used.
        /// </summary>
        public IEnumerable<ParameterExpression> AllVariables {
            get {
                Contract.Ensures(Contract.Result<IEnumerable<ParameterExpression>>() != null);
                return _inUse.Concat(_pending);
            }
        }

        private int GetNewVariableNumber() {
            return Interlocked.Increment(ref _variableIdCounter);
        }

        private string GetNewVariableName() {
            Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));
            var result = "t" + GetNewVariableNumber().ToString(CultureInfo.InvariantCulture);
            Contract.Assume(!String.IsNullOrEmpty(result));
            return result;
        }

        private ParameterExpression RemoveFirstPending(Type type) {
            var node = _pending.First;
            while (null != node) {
                Contract.Assume(node.Value != null);
                if (node.Value.Type == type) {
                    _pending.Remove(node);
                    return node.Value;
                }
                node = node.Next;
            }
            return null;
        }

        /// <summary>
        /// Manually requests an unused variable from the pending pool or creates a new variable.
        /// </summary>
        /// <param name="type">The type of the variable to create.</param>
        /// <returns>A new variable that can be used.</returns>
        public ParameterExpression GetVariable(Type type) {
            if (null == type) throw new ArgumentNullException("type");
            Contract.Ensures(Contract.Result<ParameterExpression>() != null);

            var var = RemoveFirstPending(type)
                ?? Expression.Variable(type, GetNewVariableName());
            _inUse.AddFirst(var);
            return var;
        }

        /// <summary>
        /// Manually releases a variable that is currently in use so it can be demoted to the pending variable pool.
        /// </summary>
        /// <param name="variable">The variable to manually release.</param>
        /// <returns>True when the variable was released to the pending pool.</returns>
        public bool ReleaseVariable(ParameterExpression variable) {
            if (null == variable) throw new ArgumentNullException("variable");
            Contract.EndContractBlock();
            if (_inUse.Remove(variable)) {
                _pending.AddFirst(variable);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Manually releases multiple variables that are currently in use to they can be demoted to the pending variable pool.
        /// </summary>
        /// <param name="variables">The variables to manually release.</param>
        /// <returns>True when any of the given variables has been released.</returns>
        public bool ReleaseVariables(IEnumerable<ParameterExpression> variables) {
            if (null == variables) throw new ArgumentNullException("variables");
            Contract.EndContractBlock();
            var ok = false;
            foreach (var variable in variables.Where(x => null != x)) {
                Contract.Assume(null != variable);
                ok |= ReleaseVariable(variable);
            }
            return ok;
        }

        /// <summary>
        /// Creates a new expression variable or reuses an unused expression variable.
        /// </summary>
        /// <param name="type">The type of the variable to create or reuse.</param>
        /// <returns>A new or unused variable expression container.</returns>
        public VariableUsage Use(Type type) {
            Contract.Requires(type != null);
            Contract.Ensures(Contract.Result<VariableUsage>() != null);
            return new VariableUsage(this, GetVariable(type));
        }

    }
}
