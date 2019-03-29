using RuntimeExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCalculator.Web.ViewModels
{
    //Used to maintain the depency graph of variables. Everytime a variable is evaluated, it implies that the associatedVariable is dependent on it.
    public class GraphBuilderVariableEvaluator : IVariableEvaluator
    {
        public int Order => 0;

        private string _associatedVariable;
        private SimpleAcyclicDirectedGraph<string> _graph;

        public GraphBuilderVariableEvaluator(SimpleAcyclicDirectedGraph<string> graph, string associatedVariable)
        {
            _graph = graph;
            _associatedVariable = associatedVariable;
        }

        public void AssociateVariable(string newAssociatedVariable) => _associatedVariable = newAssociatedVariable;

        public EvaluationResult Evaluate(string value)
        {
            if (!string.IsNullOrEmpty(_associatedVariable)) _graph.AddVertex(value, _associatedVariable);
            return EvaluationResult.NotEvaluated;
        }
    }
}
