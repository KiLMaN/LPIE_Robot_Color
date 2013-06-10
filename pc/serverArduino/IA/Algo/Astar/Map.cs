using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IA.Algo.Astar
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Represents a map
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    public class Map
    {
        private int[,] _costs = null;
        private byte[,] _map = null;
        private MapPoint _startPt = MapPoint.InvalidPoint;
        private MapPoint _endPt = MapPoint.InvalidPoint;

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Create a new map.
        /// </summary>
        /// <param name="fullMapFile"> The map's path to load. </param>
        /// ----------------------------------------------------------------------------------------
        public Map(string fullMapFile)
        {
            // Load the map and assign the costs
            this.LoadLabyrinthe(fullMapFile);
            this.AssignCost();
        }

        #region Properties

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get the length of the map.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public int Length
        {
            get { return this._map.GetLength(0); }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get the size of the map.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public int Size
        {
            get { return this.Length * this.Length; }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get the start point.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public MapPoint StartPoint
        {
            get { return this._startPt; }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get the end Point.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public MapPoint EndPoint
        {
            get { return this._endPt; }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get the byte assign to a square.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public byte this[int x, int y]
        {
            get { return this._map[x, y]; }
        }

        #endregion

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Assign the costs.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private void AssignCost()
        {
            for (int i = 0; i < this.Length; i++)
            {
                for (int j = 0; j < this.Length; j++)
                {
                    int cost = 0;
                    switch (this._map[i, j])
                    {
                        case 0: cost = 1; break;    // Normal
                        case 1: cost = 2; break;    // Water
                        case 2: cost = 5; break;    // Fire
                        case 3: cost = -1; break;   // Wall
                        default: cost = 1; break;   // Normal
                    }
                    this._costs[j, i] = cost;
                }
            }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Load a map.
        /// </summary>
        /// <param name="fullMapFile"> The map's path to load. </param>
        /// ----------------------------------------------------------------------------------------
        public void LoadLabyrinthe(string fullMapFile)
        {
            try
            {
                // Start and endpoint
                this._startPt = MapPoint.InvalidPoint;
                this._endPt = MapPoint.InvalidPoint;
                bool isStartPointSet = false;
                // Read the file
                using (StreamReader sr = new StreamReader(fullMapFile, Encoding.Default))
                {
                    int line = 0;
                    string currentLine = sr.ReadLine();
                    if (!string.IsNullOrEmpty(currentLine))
                    {
                        int size = currentLine.Length;
                        this._map = new byte[size, size];  // Constraint : game must be square !
                        this._costs = new int[size, size]; // Constraint : costs must be square !

                        do
                        {
                            for (int i = 0; i < size; i++)
                            {
                                char c = currentLine[i];
                                if (char.IsNumber(c)) this._map[line, i] = Byte.Parse(currentLine[i].ToString());
                                else
                                {
                                    if (!isStartPointSet)
                                    {
                                        this._startPt = new MapPoint(i, line);
                                        isStartPointSet = true;
                                    }
                                    else this._endPt = new MapPoint(i, line);
                                }
                            }
                            line++;
                        }
                        while ((currentLine = sr.ReadLine()) != null);
                    }
                }
            }
            catch (Exception ex)
            {
                this._map = null;
                throw new ArgumentException("File cannot be loaded", "fullMapFile", ex.InnerException);
            }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Check if a point is valid.
        /// </summary>
        /// <param name="labyPt"> The point to check. </param>
        /// <returns> True if the point is valid, otherwise false. </returns>
        /// ----------------------------------------------------------------------------------------
        public bool IsPointValid(MapPoint labyPt)
        {
            return (this.Length > labyPt.X && labyPt.X >= 0 && labyPt.Y >= 0 && this.Length > labyPt.Y);
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Check if the current point is a wall (outside point = wall).
        /// </summary>
        /// <param name="labyPt"> The point. </param>
        /// <returns> True if it is a wall. </returns>
        /// ----------------------------------------------------------------------------------------
        public bool IsWall(MapPoint labyPt)
        {
            return this.GetCost(labyPt) < 0;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get the cost of a Point.
        /// </summary>
        /// <param name="labyPt"> The point. </param>
        /// <returns> The cost. </returns>
        /// ----------------------------------------------------------------------------------------
        public int GetCost(MapPoint labyPt)
        {
            if (this.IsPointValid(labyPt)) return this._costs[labyPt.X, labyPt.Y];
            return -2;
        }
    }
}
