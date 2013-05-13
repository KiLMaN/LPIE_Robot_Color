﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using utils;
using xbee.Communication.Events;

namespace xbee.Communication
{
    class AutomateCommunication
    {
        #region #### Evenement ####
        //Le délégué pour stocker les références sur les méthodes
        public delegate void NewTrameArduinoReceivedEventHandler(object sender, NewTrameArduinoReceveidEventArgs e);
        //L'évènement
        public event NewTrameArduinoReceivedEventHandler OnNewTrameArduinoReceived;
        #endregion 

        private const byte _IdPc = 0xFE; // 254 : Id du PC

        private SerialXbee _SerialXbee;

        private const int _ThreadDelay = 50;
        private Thread _ThreadMessagesRecus;

        private ArduinoManager _ArduinoManager;

        public ArduinoManager ArduinoManager
        {
            get { return _ArduinoManager; }
            //set { _ArduinoManager = value; }
        }

        public AutomateCommunication(string portSerie,bool XbeeApiMode,ArduinoManager ArduinoManager)
        {
            //_ArduinoManager = new ArduinoManager(); // initalisé depuis le niveau suppérieur 

            _ArduinoManager = ArduinoManager;

            _SerialXbee = new SerialXbee(portSerie, XbeeApiMode);

            // Thread de gestion des messages 
            _ThreadMessagesRecus = new Thread(new ThreadStart(_ThreadCheckMessageRecus));
            _ThreadMessagesRecus.Start();
        }

        #region #### Gestion de le connexionSerial ####
        public void OpenSerialPort()
        {
            _SerialXbee.SetSerialConnexion(true);
        }
        public void CloseSerialPort()
        {
            _SerialXbee.SetSerialConnexion(false);
        }
        #endregion

        #region #### Thread Messages Recus ####
        private void _ThreadCheckMessageRecus()
        {
            while (true)
            {
                if (_SerialXbee.TrameRecusDisponible())
                {
                    TrameProtocole trame = _SerialXbee.PopTrameRecus();

                    if(TraiteTrameRecue(trame))
                    {
                        // Envoi au couches supérrieures 
                        NewTrameArduinoReceveidEventArgs arg = new NewTrameArduinoReceveidEventArgs(trame);
                        OnNewTrameArduinoReceived(this, arg);
                    }
                    _SerialXbee.TrameFaite(trame.num); // Marque la trame comme traitée
                }
                Thread.Sleep(_ThreadDelay);
            }
        }
        /* Traite la trame et retourne si la trame doit remonter a la couche supérieure (true) */
        private bool TraiteTrameRecue(TrameProtocole trame)
        {
            MessageProtocol message = _SerialXbee.DecodeTrame(trame);
            ArduinoBot robot = _ArduinoManager.getArduinoBotById(trame.src);

            // TODO : verifier que 'is' marche bien
            if(message is EMBtoPCMessageAskConn)
            {
                if (robot == null) // Le robot n'as jamais été céer, on le créer
                {
                    robot = new ArduinoBot(trame.src);
                    _ArduinoManager.addArduinoBot(robot);
                }

                if (!robot.Connected) // Robot non connecté, on l'ajoute dans la liste des connectés
                {
                    robot.Connected = true;
                    robot.DateLastMessageReceived = DateTime.Now;
                   // robot.DateLastMessageSend = DateTime.Now;
                    robot.stateBot = StateArduinoBot.STATE_ARDUINO_NONE;
                    robot.stateComm = StateArduinoComm.STATE_COMM_NONE;

                    PCtoEMBMessageRespConn response = new PCtoEMBMessageRespConn();
                    response.state = 0x01; // Valide 

                    // Ajout a la liste à envoyer 
                    _SerialXbee.PushTrameToSend(_SerialXbee.EncodeTrame(_IdPc, trame.src, message));
                    return true; // Notification a l'application
                }
                else
                {
                    Logger.GlobalLogger.error("Reception d'une demande de connexion d'un bot déjà connecté");
                    return false;
                }
            }
            else if(message is EMBtoPCMessageGlobalAck)
            {
                if (robot == null)
                {
                    Logger.GlobalLogger.error("Robot Inconnu in EMBtoPCMessageGlobalAck");
                    return false; // Marquer comme traité
                }


                if (robot.Connected) // Robot connecté
                {
                    // TODO : Se charger de la gestion des messages (envoi en cous, et envoi ackité)
                    robot.DateLastMessageReceived = DateTime.Now;
                    if (robot.stateComm == StateArduinoComm.STATE_COMM_WAIT_ACK) // On attendais un ACK
                        robot.stateComm = StateArduinoComm.STATE_COMM_NONE;

                    return false;
                }
                else
                {
                    Logger.GlobalLogger.error("Reception d'un ACK alors que le robot n'est pas connecté !");
                    return false;
                }
                    
            }
            else if(message is EMBtoPCMessageRespPing)
            {
                if (robot == null)
                {
                    Logger.GlobalLogger.error("Robot Inconnu in EMBtoPCMessageRespPing");
                    return false; // Marquer comme traité
                }

                if (robot.Connected) // Robot connecté
                {
                    robot.DateLastMessageReceived = DateTime.Now;
                    if (robot.stateComm == StateArduinoComm.STATE_COMM_WAIT_PING) // On attendais un ACK
                        robot.stateComm = StateArduinoComm.STATE_COMM_NONE;

                    return false;
                }
                else
                {
                    Logger.GlobalLogger.error("Reception d'un ACK alors que le robot n'est pas connecté !");
                    return false;
                }
                    
            }
            else if (message is EMBtoPCMessageRespSensor)
            {
                if (robot == null)
                {
                    Logger.GlobalLogger.error("Robot Inconnu in EMBtoPCMessageRespSensor");
                    return false; // Marquer comme traité
                }

                if (robot.Connected) // Robot connecté
                {
                    robot.DateLastMessageReceived = DateTime.Now;
                    if (robot.stateComm == StateArduinoComm.STATE_COMM_WAIT_SENSOR) // On attendais un ACK
                        robot.stateComm = StateArduinoComm.STATE_COMM_NONE;

                    return true; // Envoi a l'apllication pour traitement
                }
                else
                {
                    Logger.GlobalLogger.error("Reception d'un ACK alors que le robot n'est pas connecté !");
                    return false;
                }
            }
            else
            {
                if (robot == null)
                {
                    Logger.GlobalLogger.error("Robot Inconnu");
                }
                Logger.GlobalLogger.error("Message de type Inconnu !");
                return false; // Marquer comme traité
            }
        }
        #endregion

        #region #### Envoi de messages ####
        public void SendMessageToArduino(MessageProtocol mess, ArduinoBot bot)
        {
            if (bot.Connected)
            {
                if (mess is PCtoEMBMessageAskSensor)
                {
                    bot.stateComm = StateArduinoComm.STATE_COMM_WAIT_SENSOR;
                }
                else if (mess is PCtoEMBMessageCloseClaw || mess is PCtoEMBMessageOpenClaw)
                {
                    bot.stateComm = StateArduinoComm.STATE_COMM_WAIT_ACK;
                    bot.stateBot = StateArduinoBot.STATE_ARDUINO_CLAW;
                }
                else if (mess is PCtoEMBMessageTurn || mess is PCtoEMBMessageMove)
                {
                    bot.stateComm = StateArduinoComm.STATE_COMM_WAIT_ACK;
                    bot.stateBot = StateArduinoBot.STATE_ARDUINO_MOVE;
                }
                else if (mess is PCtoEMBMessagePing)
                {
                    bot.stateComm = StateArduinoComm.STATE_COMM_WAIT_PING;
                }
                else if (mess is PCtoEMBMessageRespConn)
                {
                    bot.stateComm = StateArduinoComm.STATE_COMM_NONE;
                    bot.stateBot = StateArduinoBot.STATE_ARDUINO_NONE;
                }
                else
                {
                    Logger.GlobalLogger.error("Envoi d'un message non connu !");
                }

                _SerialXbee.PushTrameToSend(_SerialXbee.EncodeTrame(_IdPc, bot.id, mess));
            }
            else
            {
                Logger.GlobalLogger.error("Envoi d'un message à un robot non connecté :(");
            }
        }
        #endregion

    }
}