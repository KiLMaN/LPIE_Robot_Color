using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils;

namespace IA.Algo.AStar
{
    class ASMap
    {
        // Carte
        private ASCase[,] _map;
        private ASCase _start;
        private ASCase _end;

        private List<ASCase> _obstacles;

        private int _lignes, _colonnes;

        public ASCase Start
        {
            get { return _start; }
        }
        public ASCase End
        {
            get { return _end; }
        }


        public ASMap(int lignes,int colonnes)
        {
            _map = new ASCase[lignes,colonnes];
            _lignes = lignes;
            _colonnes = colonnes;
            _obstacles = new List<ASCase>();
            // Intialisation des indexs 
            for (int x = 0; x < lignes; x++)
            {
                for (int y = 0; y < colonnes; y++)
                {
                    _map[x, y] = new ASCase(x,y);
                }
            }
        }

        public void setStart(int pLigne, int pColonne)
        {
            _start = _map[pLigne,pColonne];
            //_start.Contenu = ASCaseState.START;
            //_start.Visited = false;
        }

        public void setEnd(int pLigne, int pColonne)
        {
            _end = _map[pLigne, pColonne];
            //_end.Contenu = ASCaseState.END;
            //_end.Visited = false;
        }
        public void AjouterObstacle(ASCase Case)
        {
            _obstacles.Add(Case);
        }
        /*public void setVisited(int pLigne, int pColonne)
        {
            _map[pLigne, pColonne].Visited = true;
        }*/


        public ASCase getCase(int pLigne, int pColonne)
        {
            return _map[pLigne, pColonne];
        }

        public bool inMap(ASPoint point)
        {
            return 
                (point.X >= 0)&&
                (point.Y >= 0)&&
                (point.X < _lignes)&&
                (point.Y < _colonnes);
        }
        public bool NearFree(ASPoint point)
        {
            return !_obstacles.Exists(ASCase.Proche(point, 1));
        }
        public bool NearMovementFree(ASPoint point)
        {
            return MovementFree(point) && NearFree(point);
        }

        public bool MovementFree(ASPoint point)
        {
            return !_obstacles.Exists(ASCase.byPos(point));
        }

        public List<ASCase> getAdjCase(ASCase current,bool bdiagonales = false)
        {
            List<ASCase> list = new List<ASCase>();
            ASPoint Point = new ASPoint();

            // Haut
            Point.X = current.Point.X;
            Point.Y = current.Point.Y + 1;
            if (inMap(Point) && NearMovementFree(Point))
            {
               ASCase Case =  new ASCase(Point.Clone(), current);
               Case.CalculH(_end);
               Case.G = current.G + 1;
               list.Add(Case);
            }

            // Droite
            Point.X = current.Point.X +1;
            Point.Y = current.Point.Y;
            if (inMap(Point) && NearMovementFree(Point))
            {
                ASCase Case = new ASCase(Point.Clone(), current);
                Case.CalculH(_end);
                Case.G = current.G + 1;
                list.Add(Case);
            }

            // Gauche
            Point.X = current.Point.X -1 ;
            Point.Y = current.Point.Y;
            if (inMap(Point) && NearMovementFree(Point))
            {
                ASCase Case = new ASCase(Point.Clone(), current);
                Case.CalculH(_end);
                Case.G = current.G + 1;
                list.Add(Case);
            }

            // Bas
            Point.X = current.Point.X ;
            Point.Y = current.Point.Y -1;
            if (inMap(Point) && NearMovementFree(Point))
            {
                ASCase Case = new ASCase(Point.Clone(), current);
                Case.CalculH(_end);
                Case.G = current.G + 1;
                list.Add(Case);
            }

            if (bdiagonales)
            {
                // Haut gauche
                Point.X = current.Point.X - 1 ;
                Point.Y = current.Point.Y + 1;
                if (inMap(Point) && MovementFree(Point))
                {
                    ASCase Case = new ASCase(Point.Clone(), current);
                    Case.CalculH(_end);
                    Case.G = current.G +2;
                    list.Add(Case);
                }

                // Haut Droite
                Point.X = current.Point.X + 1;
                Point.Y = current.Point.Y + 1;
                if (inMap(Point) && MovementFree(Point))
                {
                    ASCase Case = new ASCase(Point.Clone(), current);
                    Case.CalculH(_end);
                    Case.G = current.G + 2;
                    list.Add(Case);
                }

                // Bas Gauche
                Point.X = current.Point.X - 1;
                Point.Y = current.Point.Y - 1;
                if (inMap(Point) && MovementFree(Point))
                {
                    ASCase Case = new ASCase(Point.Clone(), current);
                    Case.CalculH(_end);
                    Case.G = current.G + 2;
                    list.Add(Case);
                }

                // Bas Droite
                Point.X = current.Point.X + 1 ;
                Point.Y = current.Point.Y - 1;
                if (inMap(Point) && MovementFree(Point))
                {
                    ASCase Case = new ASCase(Point.Clone(), current);
                    Case.CalculH(_end);
                    Case.G = current.G + 2;
                    list.Add(Case);
                }
            }

            return list;
               
        }
    }
}
