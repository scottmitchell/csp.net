using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csp
{
    /// <summary>
    /// Represents a constraint in a constraint-satisfaction problem.
    /// </summary>
    public interface ISoftConstraint<TVar, TVal> : IConstraint<TVar, TVal>
    {
        /// <summary>
        /// The variables involved in this constraint. This should 
        /// always include at least two variables (since instead of having a 
        /// constraint on one variable, the invalid values can just be removed 
        /// from the variable's domain).
        /// </summary>
        IReadOnlyCollection<Variable<TVar, TVal>> Variables { get; }

        /// <summary>
        /// Returns the cost of the given assignment.
        /// </summary>
        /// 
        /// <param name="assignment">the assignment to check</param>
        /// 
        /// <returns>cost of the assignment</returns>
        double Cost(Assignment<TVar, TVal> assignment);
    }
}
