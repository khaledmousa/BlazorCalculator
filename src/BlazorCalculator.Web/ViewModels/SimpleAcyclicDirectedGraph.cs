using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCalculator.Web.ViewModels
{
    /// <summary>
    /// Simplified acyclic directed graph
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleAcyclicDirectedGraph<T>
    {
        private Dictionary<T, Dictionary<T, bool>> _matrix;
        private List<T> _nodes;

        public SimpleAcyclicDirectedGraph()
        {
            _matrix = new Dictionary<T, Dictionary<T, bool>>();
            _nodes = new List<T>();
        }

        /// <summary>
        /// Number of nodes in the graph
        /// </summary>
        public int Count => _nodes.Count;

        /// <summary>
        /// Add a node if it doesn't exist
        /// </summary>
        /// <param name="node"></param>
        public void AddNode(T node)
        {
            if (!_nodes.Contains(node))
            {
                _nodes.Add(node);
                _matrix[node] = new Dictionary<T, bool>();

                foreach (var row in _matrix.Values) row[node] = false;
            }
        }

        /// <summary>
        /// Add a vertex between two nodes. If the nodes do not exists, they will be added
        /// </summary>
        /// <param name="node1">Source node</param>
        /// <param name="node2">Destination node</param>
        public void AddVertex(T node1, T node2)
        {
            if (node1.Equals(node2) || PathExists(node2, node1)) throw new CyclicVertexException($"Cannot add vertex from {node1.ToString()} to {node2.ToString()} because it will cause a cycle.");

            if (!_nodes.Contains(node1)) AddNode(node1);
            if (!_nodes.Contains(node2)) AddNode(node2);

            _matrix[node1][node2] = true;
        }

        /// <summary>
        /// Check if a path exists between two nodes
        /// </summary>        
        public bool PathExists(T sourceNode, T destinationNode) => PathExistsInternal(sourceNode, destinationNode, new List<T>());

        /// <summary>
        /// Get all nodes linked to the source node
        /// </summary>
        public IEnumerable<T> GetLinkedNodes(T sourceNode) => _nodes.Where(n => PathExists(sourceNode, n));

        private bool PathExistsInternal(T sourceNode, T destinationNode, List<T> visitedNodes)
        {
            if (visitedNodes.Contains(sourceNode)) return false;

            return _matrix.TryGetValue(sourceNode, out var row)
                && ((row.TryGetValue(destinationNode, out var value) && value) ||
                row.Where(kvp => kvp.Value).Any(kvp => PathExistsInternal(kvp.Key, destinationNode, visitedNodes.Concat(new[] { sourceNode }).ToList())));
        }
    }
}
