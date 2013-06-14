using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;
using utils;

namespace IA.Algo.AStar
{
    public struct QuadrillageCoord
    {
        public PositionElement A;
        public PositionElement B;
    }

    public class AStar
    {
        /*
        // Positions pour le calcul
        private PositionElement         _positionDepartRobot;
        private PositionElement         _positionArriveeRobot;

        // Distance d'evitement Minimal en cm
        private int _ecartMinAEviter = 30;

        // Autres cubes a eviter
        private List<PositionElement>   _positionCubeAEviter;
        
        // Eviter de passer dans les zones
        private List<PositionZone>            _positionZoneAEviter;

        // Zone à ne pas dépasser
        private PositionZoneTravail     _zoneTravail;

        // Pas pour la trajectoire
        private int                     _pasTrajectoire = 10;
        // Distance maximale 
        private int                     _distanceMax = 10;

        // Trajectoire calculée
        private Track                   _trajectoire;


        public AStar(PositionElement Depart, PositionElement Arrivee,PositionZoneTravail ZoneTravail)
        {
            _positionDepartRobot = Depart;
            _positionArriveeRobot = Arrivee;
            _zoneTravail = ZoneTravail;
            _positionCubeAEviter = new List<PositionElement>();
            _positionZoneAEviter = new List<PositionZone>();

            _trajectoire = new Track();
        }

        public void AddListCubeAEviter(List<PositionElement> ListeCube)
        {
            _positionCubeAEviter.AddRange(ListeCube);
        }
        public void AddListZoneAEviter(List<PositionZone> ListeZone)
        {
            _positionZoneAEviter.AddRange(ListeZone);
        }


        public Track calculerTrajectoire()
        {
            _trajectoire = (calculerProchainPointAmeliore(_positionDepartRobot));
            
            return _trajectoire;
        }
        private PositionElement calculerCoordoneePoint(PositionElement point, double angle)
        {
            PositionElement nouveaupoint;
            nouveaupoint.X = point.X;
            nouveaupoint.Y = point.Y;

            nouveaupoint.X += (int)(Math.Cos(angle) * _pasTrajectoire);
            nouveaupoint.Y += (int)(Math.Sin(angle) * _pasTrajectoire);
            return nouveaupoint;
        }
        // Detecte la proximité d'un obstacle //
        private Boolean isPointProxiObstacle(PositionElement point)
        {
            foreach (PositionElement cubesAEviter in _positionCubeAEviter)
            {
                // On est trop proche d'un autre cube
                if (Math.Sqrt(Math.Pow(point.X - cubesAEviter.X, 2) + Math.Pow(point.Y - cubesAEviter.Y, 2)) < _ecartMinAEviter)
                {
                    return true;
                }
            }
            return false;
        }
        // Calcul a base de sinus / cosinus / Tangeante
        private List<PositionElement> calculerProchainPoint(PositionElement PositionActuelle)
        {
            // trajectoire calculée 
            List<PositionElement> TrajectoireSortie = new List<PositionElement>();
            TrajectoireSortie.Add(PositionActuelle);
            // Copie de la position acutelle
            PositionElement tmp = new PositionElement();
            tmp.X = PositionActuelle.X;
            tmp.Y = PositionActuelle.Y;

            double deltaX = (double)tmp.X - (double)_positionArriveeRobot.X;
            
            double deltaY = (double)tmp.Y - (double)_positionArriveeRobot.Y;

            //double delta =  deltaY / deltaX;

            // Calcul de l'angle
            double alpha = Math.Atan2(-deltaY, -deltaX);

            //return new List<PositionElement>();
            // On est éloigné de la fin, on doit donc avancer
            if (Math.Sqrt(
                Math.Pow(deltaY, 2) +
                Math.Pow(deltaX, 2)
                ) > _distanceMax) 
            {
                tmp = calculerCoordoneePoint(tmp, alpha);

                // On est face a un obstacle
                if (isPointProxiObstacle(tmp))
                {
                    List<PositionElement> TrajectoireDroite = null, TrajectoireGauche = null, TrajectoireOptimale;
                    PositionElement Gauche = calculerCoordoneePoint(tmp, alpha - (Math.PI / 4));
                    PositionElement Droite = calculerCoordoneePoint(tmp, alpha + (Math.PI / 4));
                    if (!isPointProxiObstacle(Droite))
                    {
                        TrajectoireDroite = calculerProchainPoint(Droite);
                    }
                    if (!isPointProxiObstacle(Gauche))
                    {
                        TrajectoireGauche = calculerProchainPoint(Gauche);
                    }
                    if (TrajectoireDroite != null && TrajectoireGauche != null)
                    {
                        TrajectoireOptimale = (TrajectoireDroite.Count < TrajectoireGauche.Count) ? TrajectoireDroite : TrajectoireGauche;
                    }
                    else
                        TrajectoireOptimale = TrajectoireDroite ?? TrajectoireGauche;

                    TrajectoireSortie.AddRange(TrajectoireOptimale);
                }
                else
                {
                    // Si on est encore éloigné, alors on calcul un autre point 
                    if (Math.Sqrt(
                    Math.Pow(tmp.Y - _positionArriveeRobot.Y, 2) +
                    Math.Pow(tmp.X - _positionArriveeRobot.X, 2)
                    )
                        > _distanceMax * 1.5)
                        TrajectoireSortie.AddRange(calculerProchainPoint(tmp));
                }
            }
            return TrajectoireSortie;
        }



        // Calcul a base de sinus / cosinus / Tangeante
        private Track calculerProchainPointAmeliore(PositionElement PositionActuelle)
        {
            // trajectoire calculée 
            Track TrajectoireSortie = new Track();
            TrajectoireSortie.ajouterPoint(PositionActuelle);

            double deltaX = (double)PositionActuelle.X - (double)_positionArriveeRobot.X;

            double deltaY = (double)PositionActuelle.Y - (double)_positionArriveeRobot.Y;

            //double delta =  deltaY / deltaX;

            // Calcul de l'angle
            double alpha = Math.Atan2(-deltaY, -deltaX);

            // Position suivante
            PositionElement suivante = calculerCoordoneePoint(PositionActuelle, alpha);
            // Si on peu y aller ?
            bool bRetour = false;
            if (!isPointProxiObstacle(suivante))
            {
                if (Math.Sqrt(
                Math.Pow((double)suivante.X - (double)_positionArriveeRobot.X, 2) +
                Math.Pow((double)suivante.Y - (double)_positionArriveeRobot.Y, 2)
                ) > _distanceMax)
                {
                    Track Trajectoire = calculerProchainPointAmeliore(suivante);
                    if (Trajectoire.Valide) // La trajectoire est valide
                    {
                        TrajectoireSortie.Valide = true;
                        TrajectoireSortie.ajouterPoints(Trajectoire.Positions);
                    }
                    else
                        bRetour = true;
                }
                else
                {
                    TrajectoireSortie.Valide = true;
                }
            }
            
            // On doit determiner un autre chemin
            if(isPointProxiObstacle(suivante) || bRetour)
            {
                // TODO : faire attention au cercle trigo et du tour du cercle
                PositionElement SuivanteGauche = calculerCoordoneePoint(PositionActuelle, alpha + (Math.PI / 4));
                PositionElement SuivanteDroite = calculerCoordoneePoint(PositionActuelle, alpha - (Math.PI / 4));

                Track TrajectoireGauche = calculerProchainPointAmeliore(SuivanteGauche);
                Track TrajectoireDroite = calculerProchainPointAmeliore(SuivanteDroite);

                if (!TrajectoireDroite.Valide)
                    if (!TrajectoireGauche.Valide)// On est dans un cul de sac, demi tour
                        TrajectoireSortie.Valide = false;
                    else
                    {
                        TrajectoireSortie.Valide = true;
                        TrajectoireSortie.ajouterPoints(TrajectoireGauche.Positions);
                    }

                else
                {
                    
                    if (TrajectoireGauche.Valide)
                    {
                        if(TrajectoireGauche.Positions.Count < TrajectoireDroite.Positions.Count)
                            TrajectoireSortie.ajouterPoints(TrajectoireGauche.Positions);
                        else
                            TrajectoireSortie.ajouterPoints(TrajectoireDroite.Positions);
                    }
                    else
                        TrajectoireSortie.ajouterPoints(TrajectoireDroite.Positions) ;

                    TrajectoireSortie.Valide = true;
                }

            }


            //return new List<PositionElement>();
            // On est éloigné de la fin, on doit donc avancer
            //if (Math.Sqrt(
                Math.Pow(deltaY, 2) +
                Math.Pow(deltaX, 2)
                ) > _distanceMax)
            {
                tmp = calculerCoordoneePoint(tmp, alpha);

                // On est face a un obstacle
                if (isPointProxiObstacle(tmp))
                {
                    List<PositionElement> TrajectoireDroite = null, TrajectoireGauche = null, TrajectoireOptimale;
                    PositionElement Gauche = calculerCoordoneePoint(tmp, alpha - (Math.PI / 4));
                    PositionElement Droite = calculerCoordoneePoint(tmp, alpha + (Math.PI / 4));
                    if (!isPointProxiObstacle(Droite))
                    {
                        TrajectoireDroite = calculerProchainPoint(Droite);
                    }
                    if (!isPointProxiObstacle(Gauche))
                    {
                        TrajectoireGauche = calculerProchainPoint(Gauche);
                    }
                    if (TrajectoireDroite != null && TrajectoireGauche != null)
                    {
                        TrajectoireOptimale = (TrajectoireDroite.Count < TrajectoireGauche.Count) ? TrajectoireDroite : TrajectoireGauche;
                    }
                    else
                        TrajectoireOptimale = TrajectoireDroite ?? TrajectoireGauche;

                    TrajectoireSortie.AddRange(TrajectoireOptimale);
                }
                else
                {
                    // Si on est encore éloigné, alors on calcul un autre point 
                    if (Math.Sqrt(
                    Math.Pow(tmp.Y - _positionArriveeRobot.Y, 2) +
                    Math.Pow(tmp.X - _positionArriveeRobot.X, 2)
                    )
                        > _distanceMax * 1.5)
                        TrajectoireSortie.AddRange(calculerProchainPoint(tmp));
                }
            }//

            
            // On est trop loin encore
            
            return TrajectoireSortie;
        }*/

        private ASMap _map;

        private SortedNodeList<ASCase> _open; // Liste des Cases a visiter
        private NodeList<ASCase> _close; // Liste des cases Visitée 

        private int _NumCol = 50;
        private int _NumRow = 100;

        private int _UnitByCol;
        private int _UnitbyRow;

        public int UnitCol
        {
            get { return _UnitByCol; }
        }
        public int UnitRow
        {
            get { return _UnitbyRow; }
        }

        public AStar(PositionElement Depart, PositionElement Arrivee,PositionZoneTravail ZoneTravail)
        {
            _open = new SortedNodeList<ASCase>();
            _close = new NodeList<ASCase>();

            _UnitByCol = Math.Abs(ZoneTravail.A.X - ZoneTravail.B.X) / _NumCol;
            _UnitbyRow = Math.Abs(ZoneTravail.A.Y - ZoneTravail.B.Y) / _NumRow;

            _map = new ASMap(_NumRow,_NumCol);

            ASCase Start = ConvertToCase(Depart);
            ASCase End = ConvertToCase(Arrivee);

            _map.setStart(Start.Point.X, Start.Point.Y);
            _map.setEnd(End.Point.X, End.Point.Y);
        }

        public ASCase ConvertToCase(PositionElement point)
        {
            ASCase Case = new ASCase((point.X / _UnitByCol),(point.Y / _UnitbyRow));
           /* Case.Visited = false;
            Case.Contenu = ASCaseState.NONE;
            Case.X = point.X / _UnitByCol;
            Case.Y = point.Y / _UnitbyRow;*/
            return Case;
        }
        public PositionElement ConvertFromCase(ASCase Case)
        {
            PositionElement Position = new PositionElement();
            Position.X = Case.Point.X * _UnitByCol;
            Position.Y = Case.Point.Y * _UnitbyRow;
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

       

        public void AddToOpen(ASCase courant, IEnumerable<ASCase> fils)
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
                this.AddToOpen(best, _map.getAdjCase(best,false));
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

            for (int x = 0; x < _NumCol; x++)
            {
                QuadrillageCoord q = new QuadrillageCoord();
                q.A.X = 0;
                q.B.X = _NumCol * _UnitByCol;

                q.A.Y = x * _UnitbyRow;
                q.B.Y = x * _UnitbyRow;
                Quad.Add(q);
            }
            for (int y = 0; y < _NumRow; y++)
            {
                QuadrillageCoord q = new QuadrillageCoord();
                q.A.Y = 0;
                q.B.Y = _NumRow * _UnitbyRow;

                q.A.X = y * _UnitByCol;
                q.B.X = y * _UnitByCol;
                Quad.Add(q);
            }
            return Quad;
        }
    }
}
