using Csp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csp.Constraints
{
    /// <summary>
    /// A constraint that is violated if any two variables have the same value.
    /// </summary>
    public class NumUniqueConstraint<TVar, TVal> : ISoftConstraint<TVar, TVal>
    {
        private readonly IImmutableList<Variable<TVar, TVal>> variables;
        private readonly int maxUnique;

        /// <summary>
        /// Constructs a constraint for ensuring that all of the specified variable have different values.
        /// </summary>
        /// <param name="variables">the variables</param>
        /// <exception cref="ArgumentException">if fewer than two variables are provided</exception>
        public NumUniqueConstraint(int maxUnique, params Variable<TVar, TVal>[] variables)
            : this(maxUnique, ImmutableCollectionUtils.AsImmutableList(variables)) { }

        /// <summary>
        /// Constructs a constraint for ensuring that all of the specified variable have different values.
        /// </summary>
        /// <param name="variables">the variables</param>
        /// <exception cref="ArgumentException">if fewer than two variables are provided</exception>
        public NumUniqueConstraint(int maxUnique, IEnumerable<Variable<TVar, TVal>> variables)
            : this(maxUnique, ImmutableCollectionUtils.AsImmutableList(variables)) { }

        private NumUniqueConstraint(int maxUnique, IImmutableList<Variable<TVar, TVal>> variables)
        {
            if (variables == null) throw new ArgumentNullException("variables");
            if (variables.Count < 2) throw new ArgumentException("Must have at least two variables");
            if (maxUnique == null) throw new ArgumentNullException("variables");
            if (maxUnique < 1) throw new ArgumentException("Number of unique values must be at least one");
            this.variables = variables;
            this.maxUnique = maxUnique;
        }

        /// <summary>
        /// The variables that must all have different values.
        /// </summary>
        public IReadOnlyCollection<Variable<TVar, TVal>> Variables
        {
            get { return variables; }
        }

        /// <summary>
        /// Returns true if any two of the variables included in this constraint have the same values in the specified assignment.
        /// </summary>
        public bool IsViolated(Assignment<TVar, TVal> assignment)
        {
            var assignedVariables = variables.Where(v => assignment.HasValue(v));
            var assignedValues = assignedVariables.Select(v => assignment.GetValue(v));
            return assignedValues.Distinct().Count() >= maxUnique;
        }

        /// <summary>
        /// Returns true if any two of the variables included in this constraint have the same values in the specified assignment.
        /// </summary>
        public double Cost(Assignment<TVar, TVal> assignment)
        {
            var assignedVariables = variables.Where(v => assignment.HasValue(v));
            var assignedValues = assignedVariables.Select(v => assignment.GetValue(v));
            return assignedValues.Distinct().Count();
        }
    }
}

