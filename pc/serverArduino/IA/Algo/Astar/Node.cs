using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IA.Algo.Astar
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Define a node.
    /// </summary>
    /// <remarks> 
    /// Remember: F = Cost + Heuristic! 
    /// Read the html file in the documentation directory (AStarAlgo project) for more informations.
    /// </remarks>
    /// ----------------------------------------------------------------------------------------
    internal class Node : INode
    {
        // Represents the map
        private static Map _map = null;

        private int _costG = 0; // From start point to here
        private Node _parent = null;
        private MapPoint _currentPoint = MapPoint.InvalidPoint;

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Create a new Node.
        /// </summary>
        /// <param name="parent"> The parent node. </param>
        /// <param name="currentPoint"> The current point. </param>
        /// ----------------------------------------------------------------------------------------
        public Node(Node parent, MapPoint currentPoint)
        {
            this._currentPoint = currentPoint;
            this.SetParent(parent);
        }

        #region Properties

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get or set the Map.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public static Map Map
        {
            get { return _map; }
            set { _map = value; }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get or set the parent.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public Node Parent
        {
            get { return this._parent; }
            set { this.SetParent(value); }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get the cost.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public int Cost
        {
            get { return this._costG; }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get the F distance (Cost + Heuristic).
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public int F
        {
            get { return this._costG + this.GetHeuristic(); }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get the location of the node.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public MapPoint MapPoint
        {
            get { return this._currentPoint; }
        }

        #endregion

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Set the parent.
        /// </summary>
        /// <param name="parent"> The parent to set. </param>
        /// ----------------------------------------------------------------------------------------
        private void SetParent(Node parent)
        {
            this._parent = parent;
            // Refresh the cost : the cost of the parent + the cost of the current point
            if (parent != null) this._costG = this._parent.Cost + _map.GetCost(this._currentPoint);
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// The cost if you move to this.
        /// </summary>
        /// <returns> The futur cost. </returns>
        /// --------- -------------------------------------------------------------------------------
        public int CostWillBe()
        {
            return (this._parent != null ? this._parent.Cost + _map.GetCost(this._currentPoint) : 0);
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Calculate the heuristic. (absolute x and y displacement).
        /// </summary>
        /// <returns> The heuristic. </returns>
        /// ----------------------------------------------------------------------------------------
        public int GetHeuristic()
        {
            return (Math.Abs(this._currentPoint.X - _map.EndPoint.X) + Math.Abs(this._currentPoint.Y - _map.EndPoint.Y));
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get the possible node.
        /// </summary>
        /// <returns> A list of possible node. </returns>
        /// ----------------------------------------------------------------------------------------
        public List<Node> GetPossibleNode()
        {
            List<Node> nodes = new List<Node>();
            MapPoint mapPt = new MapPoint();

            // Top
            mapPt.X = _currentPoint.X;
            mapPt.Y = _currentPoint.Y + 1;
            if (!_map.IsWall(mapPt)) nodes.Add(new Node(this, mapPt.Clone()));

            // Right
            mapPt.X = _currentPoint.X + 1;
            mapPt.Y = _currentPoint.Y;
            if (!_map.IsWall(mapPt)) nodes.Add(new Node(this, mapPt.Clone()));

            // Left
            mapPt.X = _currentPoint.X - 1;
            mapPt.Y = _currentPoint.Y;
            if (!_map.IsWall(mapPt)) nodes.Add(new Node(this, mapPt.Clone()));

            // Bottom
            mapPt.X = _currentPoint.X;
            mapPt.Y = _currentPoint.Y - 1;
            if (!_map.IsWall(mapPt)) nodes.Add(new Node(this, mapPt.Clone()));

            return nodes;
        }
    }


    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Represents a collection of Nodes.
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    internal class NodeList<T> : List<T> where T : INode
    {
        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Remove and return the first node.
        /// </summary>
        /// <returns> The first Node. </returns>
        /// ----------------------------------------------------------------------------------------
        public T RemoveFirst()
        {
            T first = this[0];
            this.RemoveAt(0);
            return first;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Chek if the collection contains a Node (the MapPoint are compared by value!).
        /// </summary>
        /// <param name="node"> The node to check. </param>
        /// <returns> True if it's contained, otherwise false. </returns>
        /// ----------------------------------------------------------------------------------------
        public new bool Contains(T node)
        {
            return this[node] != null;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get a node from the collection (the MapPoint are compared by value!).
        /// </summary>
        /// <param name="node"> The node to get. </param>
        /// <returns> The node with the same MapPoint. </returns>
        /// ----------------------------------------------------------------------------------------
        public T this[T node]
        {
            get
            {
                foreach (T n in this)
                {
                    if (n.MapPoint == node.MapPoint) return n;
                }
                return default(T);
            }
        }
    }


    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Represents a collection of SortedNodes.
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    internal class SortedNodeList<T> : NodeList<T> where T : INode
    {
        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Insert the node in the collection with a dichotomic algorithm.
        /// </summary>
        /// <param name="node"> The node to add.</param>
        /// ----------------------------------------------------------------------------------------
        public void AddDichotomic(T node)
        {
            int left = 0;
            int right = this.Count - 1;
            int center = 0;

            while (left <= right)
            {
                center = (left + right) / 2;
                if (node.F < this[center].F) right = center - 1;
                else if (node.F > this[center].F) left = center + 1;
                else { left = center; break; }
            }
            this.Insert(left, node);
        }
    }
}
