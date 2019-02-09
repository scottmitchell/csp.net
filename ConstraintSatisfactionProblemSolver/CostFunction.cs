using Csp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csp
{
    /// <summary>
    /// Represents a collection of soft constraints
    /// </summary>
    public class CostFunction<TVar, TVal>
    {
        private readonly IImmutableList<ISoftConstraint<TVar, TVal>> softConstraints;
        
        public CostFunction(params ISoftConstraint<TVar, TVal>[] softConstraints)
            : this(ImmutableCollectionUtils.AsImmutableList(softConstraints)) { }
        
        public CostFunction(IEnumerable<ISoftConstraint<TVar, TVal>> softConstraints)
            : this(ImmutableCollectionUtils.AsImmutableList(softConstraints)) { }
        
        private CostFunction(IImmutableList<ISoftConstraint<TVar, TVal>> softConstraints)
        {
            this.softConstraints = softConstraints ?? throw new ArgumentNullException("softConstraints");
        }

        public double Cost(Assignment<TVar, TVal> assignment)
        {
            var cost = 0.0;
            foreach(ISoftConstraint<TVar, TVal> sc in softConstraints)
            {
                cost += sc.Cost(assignment);
            }
            return cost;
        }
    }
}
