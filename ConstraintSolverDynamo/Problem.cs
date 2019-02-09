using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csp;

namespace ConstraintSolverDynamo
{
    public class Problem
    {
        public static Csp.Problem<int, int> FromVariablesConstraints(
            List<Variable<int,int>> variables,
            List<Csp.Constraints.AllDifferentConstraint<int, int>> constraints
            )
        {
            return new Csp.Problem<int, int>(variables, constraints);
        }
        
        public static List<int> Solve(Csp.Problem<int, int> problem)
        {
            var solver = new Csp.RecursiveBacktrackSolver<int, int>();
            var solution = solver.Solve(problem);
            var assigned = new List<int>();
            foreach (Csp.Variable<int, int> v in problem.Variables)
            {
                assigned.Add(solution.GetValue(v));
            }
            return assigned;
        }

    }
}
