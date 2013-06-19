using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;
using xbee.Communication;
using utils;
using System.Threading;

namespace IA.Algo
{
    public class Follower
    {
        // Disance maximum pour le recalcul de l'itinéraire
        private const int _DistanceMaximumRecal = 20;

        // Distance pour passer a la suite d'un tracé
        private const int _RadiusNextItineraire = 10;

        // Fabrication des tracés 
        private TrackMaker _TrackMaker;

        // Liste des robots Partie Comm
        private ArduinoManagerComm _ArduinoManager;
        private AutomateCommunication _AutomateComm;
        // Liste des robot Partie IA
        private List<ArduinoBotIA> _ListArduino;

        private Thread _ThreadIA;


        public Follower(ArduinoManagerComm AM,AutomateCommunication AUTO)
        {
            // Partie communication
            this._ArduinoManager = AM;
            this._AutomateComm = AUTO;

            _ListArduino = new List<ArduinoBotIA>();

            // Createur de trajectoire
            _TrackMaker = new TrackMaker();

            
        }
        // Demarer l'automate
        public void Start()
        {
            if (_AutomateComm.IsSerialPortOpen())
            {
                _ThreadIA = new Thread(new ThreadStart(_ThreadPrincipalIA));
                _ThreadIA.Start();
            }
            else
            {
                Logger.GlobalLogger.error("Automate de communication non connecté au port Serie Démarrage impossible ");
            }
        }
        //Stopper l'automate
        public void Stop()
        {
            if(_ThreadIA != null)
                _ThreadIA.Abort();
            _ThreadIA = null;
        }

        public void _ThreadPrincipalIA()
        {
            while (true) 
            { 
            
            }
        }

        #region #### Itinéraire ####

        // Supprime les point avant le point donne
        private void removeBeforePoint(Track tr,PositionElement p)
        {
            tr.removeBefore(p);
        }

        // Verifie si le robot est proche d'un point de l'itinéraire pour changer passer a la suite 
        private bool checkSuiteItineraire(byte idRobot , out PositionElement NearestPosition)
        {
            NearestPosition = new PositionElement();
             

            // On a bien un robot avec ce numéro
             if (_ListArduino.Exists(ArduinoBotIA.ById(idRobot)))
             {
                 ArduinoBotIA Robot = _ListArduino.Find(ArduinoBotIA.ById(idRobot));

                // Le robot a bien un tracé définit
                 if (Robot.Trace != null)
                 {
                     Track tr = Robot.Trace;
                    for (int i = 0; i < tr.Positions.Count - 1; i++)
                    {
                        // On est assez proche du point, on passe à la suite 
                        if (UtilsMath.DistanceEuclidienne(tr.Positions[i], Robot.Position) < _RadiusNextItineraire)
                        {
                            NearestPosition = tr.Positions[i];
                            return true;
                        }
                    }
                 }
             }
             return false;
        }

        // Verifie si un robot est bien proche du tracé qu'il doit effectuer
        private bool checkProximiteTrace(byte idRobot)
        {

            // On a bien un robot avec ce numéro
            if (_ListArduino.Exists(ArduinoBotIA.ById(idRobot)))
            {
                ArduinoBotIA Robot = _ListArduino.Find(ArduinoBotIA.ById(idRobot));

                // Le robot a bien un tracé définit
                if (Robot.Trace != null)
                {
                    Track tr = Robot.Trace;
                    double minDistance = -1;
                    for (int i = 0; i < tr.Positions.Count - 1; i++)
                    {
                        // Trouve la distance la plus petite entre la position actuelle et les traits du tracé
                        double tmp = UtilsMath.FindDistanceToSegment(Robot.Position, tr.Positions[i], tr.Positions[i + 1]);
                        if (minDistance == -1 || minDistance > tmp)
                        {
                            minDistance = tmp;
                        }
                    }
                    if (minDistance >= _DistanceMaximumRecal)
                    {
                        Logger.GlobalLogger.info("Robot trop éloigné de son tracé, Recalcul de l'itinéraire ");
                        return true;
                    }

                }
            }
            return false;
        }
        #endregion


        #region #### Evenements Images ####
        public void UpdatePositionRobots(List<PositionRobot> Positions)
        {
            
        }
        public void UpdatePositionCubes(List<PositionCube> Cubes)
        {

            //_TrackMaker.updateCube(Cubes);
        }
        public void UpdatePositionZones(List<PositionZone> Zone)
        {
        }
        public void UpdatePositionZoneTravail(PositionZoneTravail Zone)
        {
        }
        #endregion

        #region #### Evenements Comm ####
        // TODO : Ajouter dand _ListArduino a la connexion
        #endregion
    }
}
