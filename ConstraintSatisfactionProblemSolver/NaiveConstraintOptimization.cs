using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Csp
{
    public class NaiveConstraintOptimization<TVar, TVal>
    {
        private readonly IVariableSelectionStrategy<TVar, TVal> variableSelectionStrategy;
        private readonly IDomainSortStrategy<TVar, TVal> domainSortStrategy;
        private double best;
        private CostFunction<TVar, TVal> costFunction;

        /// <summary>
        /// Constructs a backtrack solver using the given strategies.
        /// </summary>
        /// 
        /// <param name="variableSelectionStrategy">the strategy for selecting the next unassigned variable, or null to use the default strategy</param>
        /// 
        /// <param name="domainSortStrategy">the strategy for ordering the domain before assignment values, or null to use the default strategy</param>
        public NaiveConstraintOptimization(IVariableSelectionStrategy<TVar, TVal> variableSelectionStrategy = null, IDomainSortStrategy<TVar, TVal> domainSortStrategy = null)
        {
            this.variableSelectionStrategy = variableSelectionStrategy ?? new DefaultVariableSelectionStrategy<TVar, TVal>();
            this.domainSortStrategy = domainSortStrategy ?? new DefaultDomainSortStrategy<TVar, TVal>();
            this.best = double.MaxValue;
        }

        /// <summary>
        /// The stragy used by this solver for selecting unassigned variables.
        /// </summary>
        public IVariableSelectionStrategy<TVar, TVal> VariableSelectionStrategy { get { return variableSelectionStrategy; } }

        /// <summary>
        /// The strategy used by this solver for ordering the domain of a variable.
        /// </summary>
        public IDomainSortStrategy<TVar, TVal> DomainSortStrategy { get { return domainSortStrategy; } }

        public Assignment<TVar, TVal> Optimize(OptimizationProblem<TVar, TVal> problem)
        {
            var currentAssignment = Solve(problem);
            var nextAssignment = currentAssignment;

            while (nextAssignment != null)
            {
                this.best = this.costFunction.Cost(currentAssignment);
                nextAssignment = Solve(problem);
                if (nextAssignment != null)
                {
                    currentAssignment = nextAssignment;
                }
            }

            return currentAssignment;
        }

        /// <summary>
        /// Attempts to solve the specified constraint satisfaction problem.
        /// If the problem could not be solved then this will return null.
        /// </summary>
        /// <param name="problem">the problem to solve</param>
        /// <returns>a complete assignment or null</returns>
        /// <exception cref="ArgumentNullException">if the problem is null</exception>
        public Assignment<TVar, TVal> Solve(OptimizationProblem<TVar, TVal> problem)
        {
            return Solve(problem, CancellationToken.None);
        }

        /// <summary>
        /// Attempts to solve the specified constraint satisfaction problem.
        /// If the problem could not be solved then this will return null.
        /// </summary>
        /// <param name="problem">the problem to solve</param>
        /// <param name="cancellationToken">a token that can be used to cancel the solve operation</param>
        /// <returns>a complete assignment or null</returns>
        /// <exception cref="OperationCanceledException">if the token is cancelled</exception>
        /// <exception cref="ArgumentNullException">if the problem is null</exception>
        public Assignment<TVar, TVal> Solve(OptimizationProblem<TVar, TVal> problem, CancellationToken cancellationToken)
        {
            if (problem == null) throw new ArgumentNullException("problem");
            this.costFunction = new CostFunction<TVar, TVal>(problem.SoftConstraints);
            return RecursiveBacktrack(problem.InitialAssignment, problem, cancellationToken);
        }

        private Assignment<TVar, TVal> RecursiveBacktrack(Assignment<TVar, TVal> currentAssignment, OptimizationProblem<TVar, TVal> problem, CancellationToken cancellationToken)
        {
            // This is based on an algorithm from 
            // "Artificial Intelligence: A Modern Approach" 2nd Edition by Stuart Russel and Perter Norvig (page 142).

            if (currentAssignment.IsComplete(problem.Variables))
            {
                return currentAssignment;
            }

            cancellationToken.ThrowIfCancellationRequested();

            Variable<TVar, TVal> variable = VariableSelectionStrategy.SelectUnassignedVariable(problem.Variables, currentAssignment, new Problem<TVar, TVal>(problem));
            IEnumerable<TVal> domain = DomainSortStrategy.GetOrderedDomain(variable, currentAssignment, new Problem<TVar, TVal>(problem));
            foreach (var value in domain)
            {
                var nextAssignment = currentAssignment.Assign(variable, value);
                if (this.costFunction.Cost(nextAssignment) >= this.best)
                {
                    continue;
                }
                if (nextAssignment.IsConsistent(problem.Constraints))
                {
                    var result = RecursiveBacktrack(nextAssignment, problem, cancellationToken);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }
    }
}
