using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstraintSolverDynamo
{
    public class Constraint
    {
        public static Csp.Constraints.AllDifferentConstraint<int, int> AllDifferent(List<Csp.Variable<int, int>> variables)
        {
            return new Csp.Constraints.AllDifferentConstraint<int, int>(variables);
        }
    }
}
