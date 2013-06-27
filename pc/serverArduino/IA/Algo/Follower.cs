using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;
using xbee.Communication;
using utils;
using System.Threading;
using xbee.Communication.Events;
using System.Drawing;

namespace IA.Algo
{
    public class Follower
    {
        private const int _ConversionUnit = 10; // Facteur de conversion entre unité de l'image et unité du robot

        // Disance maximum pour le recalcul de l'itinéraire
        private const int _DistanceMaximumRecal = 5 * _ConversionUnit; // 5 Cm

        // Distance pour passer a la suite d'un tracé
        private const int _RadiusNextItineraire = 5 * _ConversionUnit; // 1 cm

        // Seuil pour le passage en autonome et la depose
        private const int _seuilProximiteObjectif = 30 * _ConversionUnit; // 30 cm

        // seuil pour la detection de proximité d'un autre robot
        private const int _seuilProximiteRobot = 20 * _ConversionUnit;

        // Angle maximum de différence pour l'orientation du robot
        private const int _differenceMaxOrientation = 20;

       

        // Fabrication des tracés 
        private TrackMaker _TrackMaker;
        public TrackMaker TrackMaker
        {
            get {return _TrackMaker;}
        }

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



   
        private void tickIA() // Fonction principale
        {
            if (_bInfosPosition && _bInfosCubes && _bInfosZone && _bInfosZoneTravail) // On a recus tout le necessaire depuis l'image
            {
               // foreach (ArduinoBotIA Robot in _ListArduino)
                for(int indexRobot = 0 ; indexRobot < _ListArduino.Count ; indexRobot ++)
                {
                    ArduinoBotIA Robot = _ListArduino[indexRobot];
                    ArduinoBotComm RobotComm = _ArduinoManager.getArduinoBotById(Robot.ID);

                    if (RobotComm != null && RobotComm.Connected) // Le robot est déja connecté 
                    {
                        // Faire des verification et envoi des messages
                        if (Robot.PositionValide) // On a bien une position valide
                        {
                            if (Robot.LastAction == ActionRobot.ROBOT_AUTONOME)
                                continue; // Le robot est en mode autonome (on devrais verifier si il ne séloigne pas de l'objectif )
                            if (Robot.Trace != null) // On a un tracé attribué au robot 
                            {
                                if(!Robot.Saisie && !_TrackMaker.Cubes.Exists(Objectif.ById(Robot.Cube.id))) // Le cube n'existe plus ( téléportation), et on ne l'as pas saisis
                                {
                                    // Reinit pour recalcul de trajectoire
                                    Robot.SetZoneDepose(null);
                                    Robot.SetObjectif(null);
                                    Robot.SetTrace(null);
                                    Robot.Saisie = false;
                                }

                                if (checkProximiteObjectif(Robot)) // Proximité de l'objectif ?
                                {
                                    Logger.GlobalLogger.debug("Proximité detectée ! ");
                                    if (Robot.Saisie) // On a un cube ? si oui on le dépose
                                    {
                                        MessageProtocol mess = MessageBuilder.createOpenClawMessage();
                                        _AutomateComm.PushSendMessageToArduino(mess, RobotComm);
                                        Robot.LastAction = ActionRobot.ROBOT_PINCE;
                                       
                                        Robot.SetZoneDepose(null);
                                        Robot.SetObjectif(null);
                                        Robot.SetTrace(null);
                                        Robot.Saisie = false;
                                    }
                                    else
                                    {
                                        // Passer en Autonome 
                                        MessageProtocol mess = MessageBuilder.createModeAutoMessage();
                                        _AutomateComm.PushSendMessageToArduino(mess, RobotComm);
                                        Robot.LastAction = ActionRobot.ROBOT_AUTONOME;

                                    }
                                    Robot.LastActionTime = DateTime.Now;
                                }

                                else
                                {
                                    Logger.GlobalLogger.debug("Proximité non detectée ! ");
                                    PositionElement NearestPositionTroncon;
                                    if (checkSuiteItineraire(Robot.ID, out NearestPositionTroncon)) // On est proche du point de passage ?
                                    {
                                        Logger.GlobalLogger.debug("Point Suivant ");
                                        removeBeforePoint(Robot.Trace, NearestPositionTroncon); // On supprime les anciens points
                                    }

                                    

                                    if (!checkProximiteTrace(Robot.ID)) // On est proche du tracé ? 
                                    {
                                        // Calcul de la différence d'orientation 
                                        if (Math.Abs(diffOrientation(Robot, Robot.Trace)) > _differenceMaxOrientation) // Différence suppérieur de 15 degreé entre le robot et l'angle de la droite
                                        {
                                            Logger.GlobalLogger.debug("Orientation différente ");
                                            if (Robot.LastAction != ActionRobot.ROBOT_TOURNER || (DateTime.Now - Robot.LastActionTime) > TimeSpan.FromSeconds(1)) // On etait pas en train de tourner ou ça fait plus de 5 secondes
                                            {
                                                // Faire trouner le robot 
                                                double angle = diffOrientation(Robot, Robot.Trace);
                                                
                                                if (angle > 180) // Si suppérieur a 180 ° alors tourner a gauche
                                                {
                                                   angle =  360 - angle;
                                                    MessageProtocol mess = MessageBuilder.createTurnMessage(true, (byte)angle);
                                                    _AutomateComm.PushSendMessageToArduino(mess, RobotComm);
                                                }
                                                else // sinon touner a droite
                                                {
                                                    MessageProtocol mess = MessageBuilder.createTurnMessage(false, (byte)angle);
                                                    _AutomateComm.PushSendMessageToArduino(mess, RobotComm);
                                                }
                                                Logger.GlobalLogger.info("Changement d'angle : " + angle);
                                                Robot.LastAction = ActionRobot.ROBOT_TOURNER;
                                                Robot.LastActionTime = DateTime.Now;
                                            }
                                            // Tourner pour se placer dans le bon sens
                                        }
                                        else
                                        {
                                            // Si oui on continue a se deplacer
                                            foreach (ArduinoBotIA RobotProche in _ListArduino) // tester la presence d'autre robot a proximité
                                            {
                                                if (RobotProche.ID == Robot.ID) // Soi meme
                                                    continue; // suivant

                                                if (UtilsMath.DistanceEuclidienne(Robot.Position, RobotProche.Position) < _seuilProximiteRobot) // On est trop proche d'un autre robot
                                                {
                                                    if (RobotProche.LastAction != ActionRobot.ROBOT_ARRET) // L'autre robot n'est pas en arret
                                                    {
                                                        // nous arreter
                                                        MessageProtocol mess = MessageBuilder.createMoveMessage(true, (byte)0, (byte)0); // STOP
                                                        _AutomateComm.PushSendMessageToArduino(mess, RobotComm);
                                                        Robot.LastAction = ActionRobot.ROBOT_ARRET;
                                                        Robot.LastActionTime = DateTime.Now;
                                                        break; // sortie de boucle 
                                                    }
                                                }
                                            }
                                            if (Robot.LastAction != ActionRobot.ROBOT_DEPLACER || (DateTime.Now - Robot.LastActionTime) > TimeSpan.FromSeconds(1)) // On etait pas en train de se deplacer ou ça fait plus de 10 secondes
                                            {
                                                double distance = UtilsMath.DistanceEuclidienne(Robot.Position, Robot.Trace.Positions[1]);
                                                //Logger.GlobalLogger.debug("Distance : " + (byte)(distance / _ConversionUnit), 5);
                                                MessageProtocol mess = MessageBuilder.createMoveMessage(true, (byte)100, (byte)(distance / _ConversionUnit)); // Avancer a 50% de vitesse
                                                _AutomateComm.PushSendMessageToArduino(mess, RobotComm);

                                                Robot.LastAction = ActionRobot.ROBOT_DEPLACER;
                                                Robot.LastActionTime = DateTime.Now;
                                            }
                                            else
                                            {
                                                // ne pas renvoyer d'ordre, le robot est en train de se déplacer
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // Si non on recalcule 
                                        _TrackMaker.CalculerObjectif(Robot);
                                    }
                                }
                            }
                            else
                            {
                                // Calcul d'un itineraire
                                _TrackMaker.CalculerObjectif(Robot);
                            }
                        }
                        else
                        {// Pas de position valide
                            // Envoyer un stop pour ne pas bouger
                            MessageProtocol mess = MessageBuilder.createMoveMessage(true,0x00,0x00);
                            _AutomateComm.PushSendMessageToArduino(mess, RobotComm);
                            Robot.LastAction = ActionRobot.ROBOT_ARRET;
                            Robot.LastActionTime = DateTime.Now;
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
                     for (int i = tr.Positions.Count - 1; i >=0; i--) // On parcours le tracé depuis la fin pour trouver le plus proche de lobjectif
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
        // Proximité de l'objectif ou de la zone 
        private bool checkProximiteObjectif(ArduinoBotIA Robot)
        {
            if (Robot.Saisie) // le robot à un cube de saisie
            {
                Zone Depose = Robot.DeposeZone;
                if(UtilsMath.DistanceEuclidienne(Robot.Position,UtilsMath.CentreRectangle(Depose.position)) < _seuilProximiteObjectif)
                {
                    Logger.GlobalLogger.info("Robot proche de la zone de dépose ! id : " +Robot.ID);
                    return true;
                }
            }
            else
            {
                Objectif Cube = Robot.Cube;
                if(UtilsMath.DistanceEuclidienne(Robot.Position,Cube.position) < _seuilProximiteObjectif)
                {
                    Logger.GlobalLogger.info("Robot proche du cube ! id : " +Robot.ID);
                    return true;
                }
            }
            return false;
        }
        // Retourne la différence entre l'orientation du robot et l'axe du tracé (premier et second point)
        private double diffOrientation(ArduinoBotIA robot, Track tr)
        {
            if (tr.Positions.Count < 2)
                return 0;

            double angleTrace = 0;
            /* Vecteur de comparaison */
            PointF A = new PointF(0, 10);
            PointF B = new PointF(0, 0);

            /* Vecteur de mouvement */
            PointF Start = new PointF(tr.Positions[0].X, tr.Positions[0].Y);
            PointF Stop = new PointF(tr.Positions[1].X, tr.Positions[1].Y);

            angleTrace = UtilsMath.TrueAngleBetweenVectors(A, B, Start, Stop);

            Logger.GlobalLogger.debug("Angle déplacement :" + angleTrace + "robot.Angle :" + robot.Angle);
            return angleTrace - robot.Angle;
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
                    if (p.Position.X == -1 || p.Position.Y == -1)
                    {
                        _ListArduino.RemoveAt(index);
                    }
                    else
                    {
                        ArduinoBotIA Robot2 = _ListArduino[index];
                        Robot2.Position = p.Position;
                        Robot2.Angle = p.Angle;
                        _ListArduino[index] = Robot2;
                    }
                   
                }
                else
                {
                    if (p.Position.X != -1 && p.Position.Y != -1)
                    {
                        ArduinoBotIA Robot2 = new ArduinoBotIA((byte)p.Identifiant);
                        Robot2.Position = p.Position;
                        Robot2.Angle = p.Angle;
                        _ListArduino.Add(Robot2);
                    }
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
            switch (e.Message.headerMess)
            {
                case (byte)EMBtoPCmessHeads.ASK_CONN :
                    // Rien de spécial sur la demande de connexion
                    break;
                case (byte)EMBtoPCmessHeads.AUTO_MODE_OFF:
                    // Fin de mode auto / le robot doit avoir pris le cube
                    if (Rob.LastAction == ActionRobot.ROBOT_AUTONOME)
                    {
                        Rob.LastAction = ActionRobot.ROBOT_ARRET;
                        Rob.Saisie = true;
                        Rob.SetTrace(null);
                        // Une fois le cube saisie
                    }
                    else
                        Logger.GlobalLogger.error("Fin de mode autnome qui n'as pas été lancé par le serveur !"+Rob.ID);
                    break;
                case (byte)EMBtoPCmessHeads.RESP_SENSOR:
                    Logger.GlobalLogger.info("Réponse de capteur ? : : " + e.Message.ToString());
                    break;
                default:
                    Logger.GlobalLogger.error("Message reçu qui ne devrais pas être remonter dans L'IA : " + e.Message.ToString());
                    break;
            }
            tickIA();
        }
        void _AutomateComm_OnArduinoTimeout(object sender, ArduinoTimeoutEventArgs e)
        {
            /*byte IDRobot = e.Bot.id;

            ArduinoBotIA Rob = null;
            if (_ListArduino.Exists(ArduinoBotIA.ById(IDRobot)))
            {
                Rob = _ListArduino.Find(ArduinoBotIA.ById(IDRobot));
            }
            else
            {
                return;
            }
            int index = _ListArduino.IndexOf(Rob);*/
            // Robot déconnecté par la couche basse donc rien a faire
            Logger.GlobalLogger.info("Robot déconnecté id :" + e.Bot.id);
            tickIA();
        }
        #endregion
    }
}
