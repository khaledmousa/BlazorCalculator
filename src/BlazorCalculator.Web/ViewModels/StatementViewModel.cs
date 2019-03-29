using RuntimeExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RuntimeExpressions.ParseResult;

namespace BlazorCalculator.Web.ViewModels
{
    public class StatementViewModel<T> : IEquatable<StatementViewModel<T>>
    {
        private string _id;
        private RuntimeExpressionTree.Expression _expression;
        private EvaluationEngine _engine;        

        public StatementViewModel(string statement, SimpleAcyclicDirectedGraph<string> variableGraph, IVariableEvaluator parentEvaluator)
        {            
            Statement = statement;

            var graphVariableEvaluator = new GraphBuilderVariableEvaluator(variableGraph, null);
            _engine = new EvaluationEngine(new IVariableEvaluator[] { graphVariableEvaluator, parentEvaluator }, null);

            ParseStatement(statement);
            graphVariableEvaluator.AssociateVariable(_id);
            RefreshResult();
        }

        private void ParseStatement(string statement)
        {
            ParseResult parseResult = null;
            try
            {
                parseResult = _engine.ParseString(statement);
            }
            catch { throw new ParseException(statement); }

            switch (parseResult)
            {
                case AssignmentResult assignment:
                    _id = assignment.Item.Variable;
                    _expression = assignment.Item.Expression;
                    break;
                default:
                    throw new ParseException($"Statement {statement} is not an assignment statement. Cannot use Expressions directly.");
            }
        }

        public void RefreshResult()
        {
            try
            {
                var result = _engine.EvaluateExpression<T>(_expression);
                EvaluatedResult = result;
                IsEvaluated = true;
            }
            catch //not ideal .. maybe change the F# engine to return an evaluation result with true/false for IsEvaluated
            {
                EvaluatedResult = default(T);
                IsEvaluated = false;
            }
        }

        public bool Equals(StatementViewModel<T> other)
        {
            return this.Id?.Equals(other?.Id, StringComparison.InvariantCultureIgnoreCase) == true;
        }

        public override bool Equals(object obj) => (obj is StatementViewModel<T> other) && Equals(other);
        public override int GetHashCode() => this.Id?.GetHashCode() ?? int.MinValue;

        public string Statement { get; private set; }
        public string Id => _id;

        public T EvaluatedResult { get; set; }
        public bool IsEvaluated { get; set; }
    }
}
