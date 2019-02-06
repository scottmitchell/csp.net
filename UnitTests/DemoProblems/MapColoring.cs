using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csp.Constraints;

namespace Csp.UnitTests.DemoProblems
{
    class MapColoring
    {
        public static Problem<string, int> CreateProblem(int k = 3)
        {
            var colors = Enumerable.Range(1, k).ToList();

            var adjacencies = new Dictionary<string, List<string>> {
                { "Western Australia", new List<string> { "Northern Territory", "South Australia" } },
                { "Northern Territory", new List<string> { "Western Australia", "South Australia", "Queensland" } },
                { "New South Wales", new List<string> { "Queensland", "South Australia", "Victoria" } },
                { "Queensland", new List<string> { "Northern Territory", "South Australia", "New South Wales" } },
                { "South Australia", new List<string> { "Western Australia", "Northern Territory", "New South Wales", "Queensland", "Victoria" } },
                { "Victoria", new List<string> { "New South Wales", "South Australia" } },
                { "Tasmania", new List<string> { } }
            };

            var territories = adjacencies.Keys.ToList();

            var variables = territories.Select(t => new Variable<string, int>(t, colors)).ToList(); ;

            var constraints = new List<IConstraint<string, int>>();
            for (int i = 0; i < territories.Count; i++)
            {
                var neighbors = adjacencies[territories[i]];
                foreach (string n in neighbors)
                {
                    var neighborIndex = territories.IndexOf(n);
                    constraints.Add(new AllDifferentConstraint<string, int>(new List<Variable<string, int>> { variables[i], variables[neighborIndex] } ));
                }
            }

            return new Problem<string, int>(variables, constraints);
        }
    }
}
