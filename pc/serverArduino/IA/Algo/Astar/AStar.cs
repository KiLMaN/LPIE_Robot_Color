using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;
using utils;

namespace IA.Algo.AStarAlgo
{
    public struct QuadrillageCoord
    {
        public PositionElement A;
        public PositionElement B;
    }

    public class AStar
    {
        private ASMap _map;

        private SortedNodeList<ASCase> _open; // Liste des Cases a visiter
        private NodeList<ASCase> _close; // Liste des cases Visitée 

        private int _NumCol = 20;
        private int _NumRow = 70;

        private float _UnitByCol;
        private float _UnitByRow;

        public float UnitCol
        {
            get { return _UnitByCol; }
        }
        public float UnitRow
        {
            get { return _UnitByRow; }
        }

        public AStar(PositionElement Depart, PositionElement Arrivee,PositionZoneTravail ZoneTravail)
        {
            _open = new SortedNodeList<ASCase>();
            _close = new NodeList<ASCase>();
            
            _UnitByCol = (float)Math.Abs(ZoneTravail.A.X - ZoneTravail.B.X) / _NumCol;
            _UnitByRow = (float)Math.Abs(ZoneTravail.A.Y - ZoneTravail.B.Y) / _NumRow;

            _map = new ASMap(_NumCol,_NumRow);

            ASCase Start = ConvertToCase(Depart);
            ASCase End = ConvertToCase(Arrivee);

            _map.setStart(Start.Point.X, Start.Point.Y);
            _map.setEnd(End.Point.X, End.Point.Y);
        }

        public ASCase ConvertToCase(PositionElement point)
        {
            ASCase Case = new ASCase((point.X / (int)_UnitByCol), (point.Y / (int)_UnitByRow));
           /* Case.Visited = false;
            Case.Contenu = ASCaseState.NONE;
            Case.X = point.X / _UnitByCol;
            Case.Y = point.Y / _UnitbyRow;*/
            return Case;
        }
        public PositionElement ConvertFromCase(ASCase Case)
        {
            PositionElement Position = new PositionElement();
            Position.X = (int)(Case.Point.X * _UnitByCol);
            Position.Y = (int)(Case.Point.Y * _UnitByRow);
            return Position;
        }

        public void AddObstacle(PositionElement point)
        {
            ASCase Case = ConvertToCase(point);
            _map.AjouterObstacle(Case);
        }
        public void AddObstacles(List<PositionElement> points)
        {
            foreach (PositionElement p in points)
                AddObstacle(p);
        }
        // Trouve la case la plus proche dans la bonne direction
        // Prend en compte les cases déja visitées 
        /*private ASCase FindNearestInDirection(ASCase current)
        {
            List<ASCase> ListValide = new List<ASCase>();
            ASCase CaseOut = null;

            // Parcours toute les direction
            for (int i = 0; i < 4; i++)
            {
                ASCase Case = _map.getAdjCase(current, i);
                if (Case == null)
                    continue;

                if( Case.Contenu == ASCaseState.END) // Fin
                {
                    ListValide.Add(Case);
                    return Case;
                }

                if (!Case.Visited) // Case valide 
                {
                    if (Case.Contenu == ASCaseState.OBSTACLE)
                        continue;
                    _map.setVisited(Case.X, Case.Y);
                    ListValide.Add(Case);
                }
            }

            foreach (ASCase Case in ListValide)
            {

                if (CaseOut == null)
                    CaseOut = Case;
                else
                {
                    double distance1 = CaseOut.DistanceTo(_map.End);
                    double distance2 = Case.DistanceTo(_map.End);
                    if (distance1 > distance2) // Elle est plus proche
                        CaseOut = Case;
                }
            }

            return CaseOut;
        }
        */
        /*public Track CalculerTrajectoire()
        {
            ASCase PositionCourante = _map.Start;
            List<ASCase> ListeDeplacement = new List<ASCase>();
            ListeDeplacement.Add(PositionCourante);
            Track Trajectoire = new Track();
            while (PositionCourante.Contenu != ASCaseState.END) // Tant qu'on a pas trouver
            {
                ASCase Case = FindNearestInDirection(PositionCourante);
                if (Case != null) // On a trouver une case , on l'ajoute et on avance
                {
                    //_map.setVisited(Case.X, Case.Y);
                    ListeDeplacement.Add(Case);
                    PositionCourante = Case;
                }
                else
                {
                    ASCase Precedente = ListeDeplacement.Last();
                    PositionCourante = Precedente; // On recommence a la derniere place
                    ListeDeplacement.Remove(Precedente); // On le retire de la liste de déplacement
                }
                
            }

            foreach (ASCase Ajout in ListeDeplacement)
                Trajectoire.ajouterPoint(ConvertFromCase(Ajout));
            //Trajectoire.ajouterPoints(ListeDeplacement);
            return Trajectoire;
        }

        */

       
        // Ajouter dans la liste des noeuds a visiter
        private void AddToOpen(ASCase courant, IEnumerable<ASCase> fils)
        {
            foreach (ASCase Casefille in fils)
            {
                if (!this._open.Contains(Casefille)) // Pas deja dans la liste
                {
                    if (!this._close.Contains(Casefille))  // Pas déja traité
                        this._open.AddDichotomatic(Casefille);// Ajout dans les traitemant furtur
                }
                else
                {
                    if (Casefille.CalculCout() < this._open[Casefille].G) // Si on a un chemin plus court
                    {
                        Casefille.Parent = courant; // Mettre a jour le parent si on y accede plus rapidement
                        Casefille.G = courant.G +1 ;
                    }
                }
            }
        }
        // Calcul de la trajectoire
        public Track CalculerTrajectoire()
        {
            this._open.Add(_map.Start);
            while (this._open.Count > 0)
            {
                ASCase best = this._open.RemoveFirst();
                
                    if (best.Point == _map.End.Point)
                    {
                        Track sol = new Track(); // Solution
                        while (best.Parent != null) // on remonte la hierarchie
                        {
                            sol.ajouterPoint(ConvertFromCase(best)); // Ajout dans la solution
                            best = best.Parent; // Remonte
                        }
                        return sol;
                    }
                
                this._close.Add(best);
                this.AddToOpen(best, _map.getAdjCase(best,true));
            }
            return null; // Pas de trouvé
        }

        public System.Drawing.Rectangle CalculerRectangle(PositionElement point)
        {
            ASCase p = this.ConvertToCase(point);
            //ASCase Case = _map.getCase(p.X, p.Y);
            PositionElement HautGauche = ConvertFromCase(p);
            p.Point.X++;
            p.Point.Y++;
            PositionElement BasDroite = ConvertFromCase(p);

            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(HautGauche.X,HautGauche.Y,BasDroite.X - HautGauche.X , BasDroite.Y - HautGauche.Y);
            return rect;
        }

        public List<QuadrillageCoord> CalculerQuadrillage()
        {
            List<QuadrillageCoord> Quad = new List<QuadrillageCoord>();
            // Pour chacune des collones
            for (int x = 0; x < _NumCol; x++)
            {
                // faire un Trait entre 
                QuadrillageCoord q = new QuadrillageCoord();
                q.A.Y = 0;
                q.B.Y = (int)(_NumRow * _UnitByRow);

                q.A.X = (int)(x * _UnitByCol);
                q.B.X = q.A.X;
                Quad.Add(q);
            }
            for (int y = 0; y < _NumRow; y++)
            {
                QuadrillageCoord q = new QuadrillageCoord();
                q.A.X = 0;
                q.B.X = (int)(_NumCol * _UnitByCol);

                q.A.Y = (int)(y * _UnitByRow);
                q.B.Y = q.A.Y;
                Quad.Add(q);
            }
            return Quad;
        }
    }
}
