using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IA.Algo.AStar
{
    public class ASPoint
    {
        private int _x = 0;
        private int _y = 0;

        #region #### Constructeurs ####
        public ASPoint()
        {
        }
        public ASPoint(int x, int y)
        {
            this._x = x;
            this._y = y;
        }
        #endregion

        #region #### Propriétés ####
        public static ASPoint InvalidPoint
        {
            get { return new ASPoint(-1, -1); }
        }
        public int X
        {
            get { return this._x; }
            set { this._x = value; }
        }
        public int Y
        {
            get { return this._y; }
            set { this._y = value; }
        }
        #endregion

        #region #### Operateurs ####
        public static bool operator ==(ASPoint pt1, ASPoint pt2)
        {
            return (pt1.X == pt2.X && pt1.Y == pt2.Y);
        }
        public static bool operator !=(ASPoint pt1, ASPoint pt2)
        {
            return !(pt1 == pt2);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is ASPoint)) return false;
            ASPoint pt = (ASPoint)obj;
            return pt == this;
        }
        public override int GetHashCode()
        {
            return (this._x ^ this._y);
        }
        public ASPoint Clone()
        {
            return new ASPoint(this._x, this._y);
        }
        #endregion
    }

    public class ASCase
    {
        public ASPoint Point;

        private int _H = 0; // Distance pour atteindre l'objectif
        private int _G = 0; // Distance deja parcourue
        //private int _F = 0; // Total des deux si dessus

        private ASCase _Parent = null; // Neux Parent

        public int H
        {
            get {return _H; }
        }
        public int G
        {
            get { return _G; }
            set { this._G = value; }
        }
        public int F
        {
            get {return this.G + this.H; }
        }
        public ASCase Parent
        {
            get { return _Parent; }
            set { this._Parent = value; }
        }

        #region #### Constructeurs ####
       
        public ASCase(int X, int Y)
        {
            this.Point = new ASPoint(X, Y);
        }

        public ASCase(int X, int Y, int G)
            : this(X,Y)
        {
            this._G = (G);
        }

        public ASCase(int X, int Y, int G, ASCase end)
            :this(X,Y,G)
        {
            this.CalculH(end);
        }

        public ASCase(ASPoint point, ASCase parent)
        {
            this.Point = point;
            this.Parent = parent;
        }
        #endregion

        /*public void SetG(int G)
        {
            this._G = G;
        }*/
        /*public void CalculHF(ASCase End)
        {
            CalculH(End);
            this._F = this._G + this._H;
        }*/
        public int CalculH(ASCase End)
        {
            this._H = SquareDistanceTo(this, End);
            return this._H;
        }
        //Calcul du coup pour le deplacement du point actuel vers un autre point (peut prendre en compte des poid differents acutellemnt 1)
        public int CalculCout()
        {
            int poidDeplacement = 1;
           /* if (this.Point.X != this.Parent.Point.X && this.Point.Y != this.Parent.Point.Y) // Deplacement en diagonale
            {
                poidDeplacement = 3;
            }*/
            return (this._Parent != null ? this._Parent.G + poidDeplacement : 0);
        }
        /* Distance Euclidienne
         * Retourne la distance euclidienne au carré, qui permet de gagner du temps car on ne calcul pas la racine 
         */
        static public int SquareDistanceTo(ASCase CaseA,ASCase CaseB)
        {
            return SquareDistanceTo(CaseA.Point, CaseB.Point);
                /*(int)Math.Pow(Case.Point.X - CaseA.Point.X, 2) +
                (int)Math.Pow(Case.Point.Y - CaseA.Point.Y, 2);*/
        }
        static public int SquareDistanceTo(ASPoint pointA, ASPoint pointB)
        {
            return
                (int)Math.Pow(pointA.X - pointB.X, 2) +
                (int)Math.Pow(pointA.Y - pointB.Y, 2);
        }
        public static Predicate<ASCase> byPos(ASPoint point)
        {
            return delegate(ASCase o)
            {
                return o.Point == point;
            };
        }
        public static Predicate<ASCase> Proche(ASPoint point,int distance)
        {
            return delegate(ASCase o)
            {
                int deltaX= Math.Abs(o.Point.X - point.X);
                int deltaY= Math.Abs(o.Point.Y - point.Y);
                return (deltaX + deltaY) < distance;
            };
        } 
    }

    /* Liste de noeud, ajout de fonctions utilies */
    public class NodeList<T> : List<T> where T : ASCase
    {
        public T RemoveFirst()
        {
            T First = this[0];
            this.RemoveAt(0);
            return First;
        }
        public new bool Contains(T node)
        {
            return this[node] != null;
        }
        public T this[T node]
        {
            get
            {
                foreach (T n in this)
                {
                    if (n.Point == node.Point) return n;
                }
                return default(T);
            }
        }
    }

    public class SortedNodeList<T> : NodeList<T> where T : ASCase
    {
        // Ajout avec dichotomatie ( le plus proche en premier )
        public void AddDichotomatic(T Node)
        {
            int left = 0;
            int right = this.Count - 1;
            int center = 0;
            while (left <= right)
            {
                center = (left + right) / 2;
                if (Node.F < this[center].F) right = center - 1;
                else if (Node.F > this[center].F) left = center + 1;
                else { left = center; break; }
            }
            this.Insert(left,Node);
        }
    }
}
