using Csp.UnitTests.DemoProblems;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Csp.Constraints;

namespace Csp.UnitTests.DemoProblems
{
    class TestMapColoring
    {
        [Test]
        public void Solve_BacktrackSolver()
        {
            var problem = MapColoring.CreateProblem(5);
            var solver = new RecursiveBacktrackSolver<string, int>();
            var solution = solver.Solve(problem, CancellationToken.None);

            var assigned = new Dictionary<string, int>();
            foreach (Variable<string, int> v in problem.Variables)
            {
                assigned[v.UserObject] = solution.GetValue(v);
            }


            var adjacencies = new Dictionary<string, List<string>> {
                { "Western Australia", new List<string> { "Northern Territory", "South Australia" } },
                { "Northern Territory", new List<string> { "Western Australia", "South Australia", "Queensland" } },
                { "New South Wales", new List<string> { "Queensland", "South Australia", "Victoria" } },
                { "Queensland", new List<string> { "Northern Territory", "South Australia", "New South Wales" } },
                { "South Australia", new List<string> { "Western Australia", "Northern Territory", "New South Wales", "Queensland", "Victoria" } },
                { "Victoria", new List<string> { "New South Wales", "South Australia" } },
                { "Tasmania", new List<string> { } }
            };

            
            foreach (string t in adjacencies.Keys)
            {
                var neighbors = adjacencies[t];
                foreach (string n in neighbors)
                {
                    Assert.AreNotEqual(assigned[t], assigned[n]);
                }
            }
        }

        [Test]
        public void Solve_NaiveOpimitizationSolver()
        {
            var problem = MapColoring.CreateProblem(5);
            var softConstraints = new List<ISoftConstraint<string, int>> { new NumUniqueConstraint<string, int>(3, problem.Variables)};
            var optimizationProblem = new OptimizationProblem<string, int>(problem.Variables, problem.Constraints, softConstraints, problem.InitialAssignment);

            var solver = new NaiveConstraintOptimization<string, int>();
            var solution = solver.Optimize(optimizationProblem);

            var assigned = new Dictionary<string, int>();
            foreach (Variable<string, int> v in problem.Variables)
            {
                assigned[v.UserObject] = solution.GetValue(v);
            }


            var adjacencies = new Dictionary<string, List<string>> {
                { "Western Australia", new List<string> { "Northern Territory", "South Australia" } },
                { "Northern Territory", new List<string> { "Western Australia", "South Australia", "Queensland" } },
                { "New South Wales", new List<string> { "Queensland", "South Australia", "Victoria" } },
                { "Queensland", new List<string> { "Northern Territory", "South Australia", "New South Wales" } },
                { "South Australia", new List<string> { "Western Australia", "Northern Territory", "New South Wales", "Queensland", "Victoria" } },
                { "Victoria", new List<string> { "New South Wales", "South Australia" } },
                { "Tasmania", new List<string> { } }
            };


            foreach (string t in adjacencies.Keys)
            {
                var neighbors = adjacencies[t];
                foreach (string n in neighbors)
                {
                    Assert.AreNotEqual(assigned[t], assigned[n]);
                }
            }
            Assert.AreEqual(assigned.Values.Distinct().ToList().Count, 3);
        }
    }
}
