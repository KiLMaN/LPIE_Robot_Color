using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IA.Algo.Astar
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Represents a MapPoint object.
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    public class MapPoint
    {
        private int _x = 0;
        private int _y = 0;

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Create a new MapPoint.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public MapPoint()
        {
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Create a new MapPoint.
        /// </summary>
        /// <param name="x"> The x-coordinate. </param>
        /// <param name="y"> The x-coordinate. </param>
        /// ----------------------------------------------------------------------------------------
        public MapPoint(int x, int y)
        {
            this._x = x;
            this._y = y;
        }

        #region Properties

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get an invalid point.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public static MapPoint InvalidPoint
        {
            get { return new MapPoint(-1, -1); }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get the x-coordinate.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public int X
        {
            get { return this._x; }
            internal set { this._x = value; }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get the y-coordinate.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public int Y
        {
            get { return this._y; }
            internal set { this._y = value; }
        }

        #endregion

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Operator override ! Now : value comparison.
        /// </summary>
        /// <param name="labyPt1"> The 1st point. </param>
        /// <param name="labyPt2"> The 2nd point. </param>
        /// <returns> True if the points are equals (by value!). </returns>
        /// ----------------------------------------------------------------------------------------
        public static bool operator ==(MapPoint labyPt1, MapPoint labyPt2)
        {
            return (labyPt1.X == labyPt2.X && labyPt1.Y == labyPt2.Y);
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Operator override ! Now : value comparison.
        /// </summary>
        /// <param name="point1"> The 1st point. </param>
        /// <param name="point2"> The 2nd point. </param>
        /// <returns> True if the points are equals (by value!). </returns>
        /// ----------------------------------------------------------------------------------------
        public static bool operator !=(MapPoint point1, MapPoint point2)
        {
            return !(point1 == point2);
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Value comparison.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns> True if the points are equals (by value!). </returns>
        /// ----------------------------------------------------------------------------------------
        public override bool Equals(object obj)
        {
            if (!(obj is MapPoint)) return false;
            MapPoint point = (MapPoint)obj;
            return point == this;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// This is the same implementation than System.Drawing.Point.
        /// </summary>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------
        public override int GetHashCode()
        {
            return (this._x ^ this._y);
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Clone the current object.
        /// </summary>
        /// <returns> A new instance with the same content. </returns>
        /// ----------------------------------------------------------------------------------------
        public MapPoint Clone()
        {
            return new MapPoint(this._x, this._y);
        }
    }
}
