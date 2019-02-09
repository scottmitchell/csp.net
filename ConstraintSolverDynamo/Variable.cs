using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstraintSolverDynamo
{
    public class Variable
    {
        public static Csp.Variable<int, int> ByNameDomain(int name, List<int> domain)
        {
            return new Csp.Variable<int, int>(name, domain);
        }
    }
}
