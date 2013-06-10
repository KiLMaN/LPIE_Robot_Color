using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;

/*
 *  Author : Bidou (http://www.csharpfr.com/auteurdetail.aspx?ID=13319)
 *  Blog   : http://blogs.developpeur.org/bidou/
 *  Date   : January 2007
 */

namespace IA.Algo.Astar
{
    class AStar
    {
        private Map _map = null;
        private SortedNodeList<Node> _open = new SortedNodeList<Node>();
        private NodeList<Node> _close = new NodeList<Node>();

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Create a new AStar object.
        /// </summary>
        /// <param name="map"> The map. </param>
        /// ----------------------------------------------------------------------------------------
        public AStar(Map map)
        {
            if (map == null) throw new ArgumentException("map cannot be null");
            this._map = map;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Calculate the shortest path between the start point and the end point.
        /// </summary>
        /// <remarks> The path is reversed. </remarks>
        /// <returns> The shortest path. </returns>
        /// ----------------------------------------------------------------------------------------
        public List<MapPoint> CalculateBestPath()
        {
            Node.Map = this._map;
            Node startNode = new Node(null, this._map.StartPoint);
            this._open.Add(startNode);

            while (this._open.Count > 0)
            {
                Node best = this._open.RemoveFirst();           // This is the best node
                if (best.MapPoint == this._map.EndPoint)        // We are finished
                {
                    List<MapPoint> sol = new List<MapPoint>();  // The solution
                    while (best.Parent != null)
                    {
                        sol.Add(best.MapPoint);
                        best = best.Parent;
                    }
                    return sol; // Return the solution when the parent is null (the first point)
                }
                this._close.Add(best);
                this.AddToOpen(best, best.GetPossibleNode());
            }
            // No path found
            return null;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Add a list of nodes to the open list if needed.
        /// </summary>
        /// <param name="current"> The current nodes. </param>
        /// <param name="nodes"> The nodes to add. </param>
        /// ----------------------------------------------------------------------------------------
        private void AddToOpen(Node current, IEnumerable<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                if (!this._open.Contains(node))
                {
                    if (!this._close.Contains(node)) this._open.AddDichotomic(node);
                }
                // Else really nedded ?
                else
                {
                    if (node.CostWillBe() < this._open[node].Cost) node.Parent = current;
                }
            }
        }
    }
}
