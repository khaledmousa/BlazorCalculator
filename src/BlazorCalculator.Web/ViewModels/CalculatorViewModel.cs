using RuntimeExpressions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCalculator.Web.ViewModels
{
    public class CalculatorViewModel : IVariableEvaluator
    {
        private SimpleAcyclicDirectedGraph<string> _variableGraph;
        private List<StatementViewModel<decimal>> _statements;                
        public ReadOnlyCollection<StatementViewModel<decimal>> Statements { get; }

        public CalculatorViewModel()
        {
            _variableGraph = new SimpleAcyclicDirectedGraph<string>();
            _statements = new List<StatementViewModel<decimal>>();
            Statements = new ReadOnlyCollection<StatementViewModel<decimal>>(_statements);
        }

        public void AddStatement() => AddStatement(StatementInput);

        public void AddStatement(string statement)
        {
            try
            {
                var statementViewModel = new StatementViewModel<decimal>(statement, _variableGraph, this);

                var existing = _statements.FirstOrDefault(s => s.Id.Equals(statementViewModel.Id, StringComparison.OrdinalIgnoreCase));
                if (existing != null) _statements.Remove(existing);

                _statements.Add(statementViewModel);
                RefreshAssociatedStatements(statementViewModel);
                ClearError();
                StatementInput = string.Empty;
            }
            catch (ParseException)
            {
                Error = Constants.Text.InvalidStatement;
                HasError = true;
            }
            catch (CyclicVertexException)
            {
                Error = Constants.Text.CyclicStatement;
                HasError = true;
            }
        }

        private void RefreshAssociatedStatements(StatementViewModel<decimal> addedStatement)
        {
            if (addedStatement.IsEvaluated)
            {
                foreach (var node in _variableGraph.GetLinkedNodes(addedStatement.Id).ToList())
                {
                    var statement = _statements.FirstOrDefault(s => s.Id.Equals(node, StringComparison.OrdinalIgnoreCase));
                    statement?.RefreshResult();
                }
            }
        }

        private void ClearError()
        {
            Error = string.Empty;
            HasError = false;
        }

        public EvaluationResult Evaluate(string value)
        {
            var matchingStatement = _statements.FirstOrDefault(s => s.Id.Equals(value, StringComparison.OrdinalIgnoreCase));
            if (matchingStatement?.IsEvaluated == true) return new EvaluationResult(matchingStatement.EvaluatedResult);
            else return EvaluationResult.NotEvaluated;
        }

        public string AddStatementCaption => Constants.Text.AddStatementCaption;
        public string StatementPlaceholder => Constants.Text.StatementPlaceholder;
        
        public string StatementInput { get; set; }        
        
        public string Error { get; set; }        
        public bool HasError { get; set; }

        private StatementViewModel<decimal> _selectedItem;
        public StatementViewModel<decimal> SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                if (_selectedItem != null) StatementInput = _selectedItem.Statement;
            }
        }

        public int Order => int.MaxValue;
    }
}
