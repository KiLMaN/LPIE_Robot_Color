using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;
using xbee.Communication;
using utils;
using System.Threading;
using xbee.Communication.Events;

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
        public List<ArduinoBotIA> ListArduino
        {
            get { return _ListArduino; }
        }

        //private Thread _ThreadIA;

        // Avons nous toute les informations en provenance de l'image afin de faire nos calculs ?
        private bool _bInfosPosition = false;
        private bool _bInfosCubes = false;
        private bool _bInfosZone = false;
        private bool _bInfosZoneTravail = false;

        public Follower(ArduinoManagerComm AM,AutomateCommunication AUTO)
        {
            // Partie communication
            this._ArduinoManager = AM;
            this._AutomateComm = AUTO;

            _ListArduino = new List<ArduinoBotIA>();

            // Createur de trajectoire
            _TrackMaker = new TrackMaker();

            this._AutomateComm.OnArduinoTimeout += new AutomateCommunication.ArduinoTimeoutEventHandler(_AutomateComm_OnArduinoTimeout);
            this._AutomateComm.OnNewTrameArduinoReceived += new AutomateCommunication.NewTrameArduinoReceivedEventHandler(_AutomateComm_OnNewTrameArduinoReceived);
        }

        // Demarer l'automate
        public bool Start()
        {
            if (_AutomateComm == null)
                return false;
            if (_AutomateComm.IsSerialPortOpen())
            {
                return true;
            }
            else
            {
                Logger.GlobalLogger.error("Automate de communication non connecté au port Serie Démarrage impossible ");
                return false;
            }
        }
        //Stopper l'automate
        public void Stop()
        {
            /*if(_ThreadIA != null)
                _ThreadIA.Abort();
            _ThreadIA = null;*/
        }

   
        private void tickIA() // Fonction principale
        {
            if (_bInfosPosition && _bInfosCubes && _bInfosZone && _bInfosZoneTravail) // On a recus tout le necessaire depuis l'image
            {
                foreach (ArduinoBotIA Robot in _ListArduino)
                {
                    ArduinoBotComm RobotComm = _ArduinoManager.getArduinoBotById(Robot.ID);
                    if (RobotComm != null && RobotComm.Connected) // Le robot est déja connecté 
                    {
                        // Faire des verification et envoi des messages
                        if (Robot.PositionValide) // On a bien une position valide
                        {
                            if (Robot.Trace != null) // On a un tracé attribué au robot 
                            {

                                PositionElement NearestPositionTroncon;
                                if (checkSuiteItineraire(Robot.ID, out NearestPositionTroncon)) // On est proche du point de passage ?
                                {
                                    removeBeforePoint(Robot.Trace, NearestPositionTroncon); // On supprime les anciens points
                                }

                                // Calcul de la différence d'orientation 
                                if (Math.Abs(diffOrientation(Robot, Robot.Trace)) > 15) // Différence suppérieur de 15 degreé
                                {
                                    // Tourner pour se placer dans le bon sens
                                }

                                if (!checkProximiteTrace(Robot.ID)) // On est proche du tracé ? 
                                {
                                    // Si oui on continue a se deplacer
                                }
                                else
                                {
                                    // Si non on recalcule 

                                }
                            }
                            else
                            {
                                // Calcul d'un itineraire
                            }
                        }
                        else
                        {// Pas de position valide
                            // Envoyer un stop pour ne pas bouger
                            MessageProtocol mess = MessageBuilder.createMoveMessage(true,0x00,0x00);
                            _AutomateComm.PushSendMessageToArduino(mess, RobotComm);
                        }
                    }
                }
            }
        }

        #region #### Itinéraire ####
        // Supprime les point avant le point donne
        private void removeBeforePoint(Track tr,PositionElement p)
        {
            tr.removeBefore(p);
        }
        // Verifie si le robot est proche d'un point de l'itinéraire pour changer passer a la suite 
        private bool checkSuiteItineraire(byte idRobot , out PositionElement NearestPositionTroncon)
        {
            NearestPositionTroncon = new PositionElement();
             
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
                            NearestPositionTroncon = tr.Positions[i];
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
        // Retourne la différence entre l'orientation du robot et l'axe du tracé (premier et second point)
        private double diffOrientation(ArduinoBotIA robot, Track tr)
        {
            if (tr.Positions.Count < 2)
                return 0;

            double VXA, VYA, VXB, VYB;
            // Vecteur Normal vertical 
            VXA = 0 - 1;
            VYA = 0 - 0;
            // Vecteur de déplacement
            VXB = tr.Positions[0].X - tr.Positions[1].X;
            VYB = tr.Positions[0].Y - tr.Positions[1].Y;
            // Calcul des normes
            double NormeA = Math.Sqrt(VXA * VXA + VYA * VYA);
            double NormeB = Math.Sqrt(VXB * VXB + VYB * VYB);
            double C = ((VXA * VXB) + (VYA * VYB)) / (NormeA * NormeB);
            double S = ((VXA * VXB) - (VYA * VYB));
            double angleDeplacement = Math.Sign(S) * Math.Acos(C);

            return angleDeplacement - robot.Angle;
        }
        #endregion

        #region #### Evenements Images ####
        public void UpdatePositionRobots(List<PositionRobot> Positions)
        {
            _bInfosPosition = true;
            // Mettre a jour la liste des robots avec les infos en provenance de l'image
            foreach(PositionRobot p in Positions)
            {
                ArduinoBotIA Robot = _ListArduino.Find(ArduinoBotIA.ById((byte)p.Identifiant));
                if (Robot != null)
                {
                    int index = _ListArduino.IndexOf(Robot);
                    ArduinoBotIA Robot2 = _ListArduino[index];
                    Robot2.Position = p.Position;
                    Robot2.Angle = p.Angle;
                    _ListArduino[index] = Robot2;
                }
                else
                {
                    ArduinoBotIA Robot2 = new ArduinoBotIA((byte)p.Identifiant);
                    Robot2.Position = p.Position;
                    Robot2.Angle = p.Angle;
                    _ListArduino.Add(Robot2);
                }
            }
            // Declencher un tick de l'IA
            tickIA();
        }
        public void UpdatePositionCubes(List<PositionCube> Cubes)
        {
            _bInfosCubes = true;
            foreach (PositionCube p in Cubes)
            {
                _TrackMaker.updateCube(new Objectif(p.ID, p.Position, p.IDZone));
            }
            tickIA();
        }
        public void UpdatePositionZones(List<PositionZone> Zone)
        {
            _bInfosZone = true;
            foreach (PositionZone p in Zone)
            {
                _TrackMaker.updateZone(new Zone(p.ID,p));
            }
            tickIA();
        }
        public void UpdatePositionZoneTravail(PositionZoneTravail Zone)
        {
            _bInfosZoneTravail = true;
            _TrackMaker.SetZoneTravail(Zone);
            tickIA();
        }
        #endregion

        #region #### Evenements Comm ####
        void _AutomateComm_OnNewTrameArduinoReceived(object sender, NewTrameArduinoReceveidEventArgs e)
        {
            byte IDRobot = e.Source.id;

            ArduinoBotIA Rob;
            if(_ListArduino.Exists(ArduinoBotIA.ById(IDRobot)))
            {
                 Rob = _ListArduino.Find(ArduinoBotIA.ById(IDRobot));
            }
            else
            {
                Rob = new ArduinoBotIA(IDRobot);
                _ListArduino.Add(Rob);
            }

            // TODO : Traitement de la trame recu
            tickIA();
        }
        void _AutomateComm_OnArduinoTimeout(object sender, ArduinoTimeoutEventArgs e)
        {
            byte IDRobot = e.Bot.id;

            ArduinoBotIA Rob = null;
            if (_ListArduino.Exists(ArduinoBotIA.ById(IDRobot)))
            {
                Rob = _ListArduino.Find(ArduinoBotIA.ById(IDRobot));
            }
            else
            {
                return;
            }
            int index = _ListArduino.IndexOf(Rob);
            
            Rob.PositionValide = false;
            _ListArduino[index] = Rob;
            tickIA();
        }
        #endregion
    }
}
