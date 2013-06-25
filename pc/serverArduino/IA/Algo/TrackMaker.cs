using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbee.Communication;
using utils.Events;
using utils;
using IA.Algo;
using IA.Algo.AStarAlgo;

namespace IA.Algo
{
    public class TrackMaker
    {
        // Emplacement des cubes sur le terraim
        private List<Objectif> _Cubes;
        public List<Objectif> Cubes
        {
            get { return _Cubes; }
        }

        // Emplacement des zones
        private List<Zone> _ZonesDepose;
        public List<Zone> ZonesDepose
        {
            get { return _ZonesDepose; }
        }

        // Liste des Tracés déjà créer
        //private List<Track> _Tracks;

        private PositionZoneTravail _ZoneTravail;
        public PositionZoneTravail ZoneTravail
        {
            get { return _ZoneTravail; }
        }

        public TrackMaker()
        {
            _Cubes = new List<Objectif>();
            _ZonesDepose = new List<Zone>();
        }

        // Met a jour ou ajoute un cube
        public void updateCube(Objectif cube)
        {
            if (_Cubes.Exists(Objectif.ById(cube.id)))
            {
                int index = _Cubes.FindIndex(Objectif.ById(cube.id));
                if (cube.position.X == -1 || cube.position.Y == -1) // position invalide , suppression
                {
                    _Cubes.RemoveAt(index);
                }
                else
                {
                    _Cubes[index] = cube;
                }
            }
            else
                _Cubes.Add(cube);

        }

        // Met a jour ou ajoute une zone
        public void updateZone(Zone zone)
        {
            if (_ZonesDepose.Exists(Zone.ById(zone.id)))
            {
                int index = _ZonesDepose.FindIndex(Zone.ById(zone.id));
                /*Zone o = _ZonesDepose[index];
                o.setPosition(zone.position);*/
                _ZonesDepose[index] = zone;
            }
            else
                _ZonesDepose.Add(zone);
        }

        // Calcul de l'objectif en fonction de l'etat du robot 
        public Track CalculerObjectif(ArduinoBotIA robot)
        {
            if (robot.Saisie)
            {
                Zone o = _ZonesDepose.Find(Zone.ById(robot.Cube.idZone));
                Track t = CreerAstarDepose(robot, o).CalculerTrajectoire();
                robot.SetZoneDepose(o);
                robot.SetTrace(t);
                return t;
            }
            else
            {
                Objectif o;
                if (robot.Cube != null) // On a deja un objectif
                {
                    o = robot.Cube;
                }
                else // Trouver un objectif
                {
                    o = findObjectifNear(robot.Position);
                }
                
                Track t = CreerAstarCube(robot, o).CalculerTrajectoire();
                robot.SetObjectif(o);
                o.Robot = robot;
                robot.SetTrace(t);
                return t;
            }
        }

        // Trouve l'objectif le plus près de la position (objectif non effectué )
        private Objectif findObjectifNear(PositionElement p)
        {
            Objectif sortie = null;

            double MinDist = -1;
            foreach (Objectif ob in _Cubes)
            {
                // Objectif non fait et non en cours de traitement
                if (ob.Done == false && ob.Robot == null)
                {
                    double dist = UtilsMath.DistanceManhattan(p, ob.position);
                    if (sortie == null) // On a pas encore d'objectif ?
                    {
                        sortie = ob;
                        MinDist = dist;
                    }
                    else
                    {
                        if (MinDist > dist)
                        {
                            sortie = ob;
                            MinDist = dist;
                        }
                    }
                }
            }
            return sortie;
        }

        public void SetZoneTravail(PositionZoneTravail position)
        { 
            this._ZoneTravail = position;
        }

        #region  #### Creation AStar ####
        // Creer un astar avec les zones et les positions correctes
        private AStar CreerAstarCube(ArduinoBotIA robot, Objectif Cube)
        {
            AStar AS = new AStar(robot.Position, Cube.position, _ZoneTravail);
            // Ajout des autres cubes en obstacles
            foreach (Objectif o in _Cubes)
                if (o.id != Cube.id)
                    if(o.Robot != null)
                        AS.AddObstacle(o.position);

            // Ajout des Zones en Obstacles
            foreach (Zone o in _ZonesDepose)
                AS.AddObstacle(UtilsMath.CentreRectangle(o.position));

            return AS;
        }
        private AStar CreerAstarDepose(ArduinoBotIA robot, Zone Depose)
        {
            PositionElement PositionCentreZone = UtilsMath.CentreRectangle(Depose.position);
            AStar AS = new AStar(robot.Position, PositionCentreZone, _ZoneTravail);
            // Ajout des autres cubes en obstacles
            foreach (Objectif o in _Cubes)
                if (o.Robot != null)
                AS.AddObstacle(o.position);

            // Ajout des Zones en Obstacles
            foreach (Zone o in _ZonesDepose)
                if(o.id != Depose.id)
                    AS.AddObstacle(UtilsMath.CentreRectangle(o.position));

            return AS;
        }
        public AStar CreerAstarQuadriallage()
        {
            return new AStar(new PositionElement(), new PositionElement(), _ZoneTravail);
        }
        #endregion
    }
}
