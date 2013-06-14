using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using utils;
using xbee.Communication.Events;

namespace xbee.Communication
{
    public class AutomateCommunication : IDisposable
    {
        /*private List<MessageEnAttente> _MessagesEnAttenteEnvoi;
        private struct MessageEnAttente
        {
            public ArduinoBot robot;
            public MessageProtocol message;
        };*/

        #region #### Evenement Sortant ####
        //Le délégué pour stocker les références sur les méthodes
        public delegate void NewTrameArduinoReceivedEventHandler(object sender, NewTrameArduinoReceveidEventArgs e);
        public delegate void ArduinoTimeoutEventHandler(object sender, ArduinoTimeoutEventArgs e);
        //L'évènement
        public event NewTrameArduinoReceivedEventHandler OnNewTrameArduinoReceived;
        public event ArduinoTimeoutEventHandler OnArduinoTimeout;
        #endregion 

        private byte _IdPc = 0xFE; // 254 : Id du PC
        public byte IdPc
        {
            get{return _IdPc;}
            set{_IdPc = value;}
        }

        // Connecteur Xbee sur port Serie
        private SerialXbee _SerialXbee;

        private const int _ThreadMessageRecuDelay = 10; // Delais entre les verification de nouveau messages recus en millisecondes
        private Thread _ThreadMessagesRecus;

        private const int _ThreadKeepAliveDelay = 10; // verification des keepalive
        private const int _TimeOutKeepAlive = 30; // Envoi d'un KeepAlive (ping) toutes les 30 secondes sans messages
        private Thread _ThreadMessagesEnvois; // Envoi / KeepAlive / Rejeux

        private ArduinoManager _ArduinoManager;
        public ArduinoManager ArduinoManager
        {
            get { return _ArduinoManager; }
            //set { _ArduinoManager = value; }
        }

        private const int _DelayRejeu = 10; // Temps d'attente en les rejeux
        private const int _MaxRejeu = 3; // Nombre max de rejeux avant timeout (sans conter l'envoi initial)


        // Messages en attente d'envois car l'on a pas recus d'ack de l'arduino
       
        

        public AutomateCommunication(string portSerie,bool XbeeApiMode,ArduinoManager ArduinoManager)
        {
            // liste des Arduinos
            _ArduinoManager = ArduinoManager;

            // Communication
            _SerialXbee = new SerialXbee(portSerie, XbeeApiMode);
            //_SerialXbee.OnArduinoTimeout += new SerialXbee.NewArduinoTimeoutEventHandler(_OnArduinoTimeout);

            // Thread de gestion des messages 
            _ThreadMessagesRecus = new Thread(new ThreadStart(_ThreadCheckMessageRecus));
            _ThreadMessagesRecus.Start();

            // Thread de Envoi / KeepAlive / Rejeux
            _ThreadMessagesEnvois = new Thread(new ThreadStart(_ThreadCheckEnvoi));
            _ThreadMessagesEnvois.Start();
        }
        public void Dispose()
        {
            _ThreadMessagesEnvois.Abort();
            _ThreadMessagesRecus.Abort();
            _SerialXbee.Dispose();
        }
        public void setXbeeApiMode(bool state)
        {
            Logger.GlobalLogger.info("Passage du mode Xbee API en :" + state);
            _SerialXbee.setXbeeApiMode(state);
        }

        #region #### Evenements ####
        private void _OnArduinoTimeout(ArduinoBot bot)
        {
            // Deconnection et suppression des messages en attente
            _ArduinoManager.disconnectArduinoBot(bot.id);

            ArduinoTimeoutEventArgs e = new ArduinoTimeoutEventArgs(_ArduinoManager.getArduinoBotById(bot.id));
            OnArduinoTimeout(this, e);
        }
        #endregion

        #region #### Gestion de le connexionSerial ####
        public void OpenSerialPort(string portSerie)
        {
            _SerialXbee.SetSerialName(portSerie);
            _SerialXbee.SetSerialConnexion(true);
        }
        public void CloseSerialPort()
        {
            _SerialXbee.SetSerialConnexion(false);
        }
        public bool IsSerialPortOpen()
        {
            return _SerialXbee.GetSerialState();
        }
        #endregion

        #region #### Thread Messages Recus ####
        private void _ThreadCheckMessageRecus()
        {
            while (true)
            {
                /* Traitement des trames entrantes */
                if (_SerialXbee.TrameRecusDisponible())
                {
                    TrameProtocole trame = _SerialXbee.PopTrameRecus();
                    // Si traitement par l'application (données capteur ou autre )
                    // Les Ack et autre sont gerrer ici
                    if (TraiteTrameRecue(trame))
                    {
                        // Envoi au couches supérrieures 
                        MessageProtocol message = _SerialXbee.DecodeTrame(trame);
                        ArduinoBot robot = ArduinoManager.getArduinoBotById(trame.src);
                        NewTrameArduinoReceveidEventArgs arg = new NewTrameArduinoReceveidEventArgs(message, robot);

                        OnNewTrameArduinoReceived(this, arg);
                    }
                }
                

                /* Verification message en attente envoi */
                /*if (_MessagesEnAttenteEnvoi.Count > 0)
                {
                    
                    int count = _MessagesEnAttenteEnvoi.Count;
                    for(int i=0 ; i< count;i++)
                    {
                        if (_MessagesEnAttenteEnvoi[i].robot.stateComm == StateArduinoComm.STATE_COMM_NONE)
                        {
                            Logger.GlobalLogger.debug("Envoi d'un message en attente au robot :" + _MessagesEnAttenteEnvoi[i].robot + " Messsage : " + _MessagesEnAttenteEnvoi[i].message.GetType().ToString(),1);
                            // L'arduino est libre, on peut envoyer
                            SendMessageToArduino(_MessagesEnAttenteEnvoi[i].message, _MessagesEnAttenteEnvoi[i].robot);
                        }
                    }
                   
                }*/
                Thread.Sleep(_ThreadMessageRecuDelay);
            }
        }
        /* Traite la trame et retourne si la trame doit remonter a la couche supérieure (true) */
        private bool TraiteTrameRecue(TrameProtocole trame)
        {
            MessageProtocol message = _SerialXbee.DecodeTrame(trame);
            ArduinoBot robot = ArduinoManager.getArduinoBotById(trame.src);

            if(message is EMBtoPCMessageAskConn)
            {
                if (robot == null) // Le robot n'as jamais été céer, on le créer
                {
                    robot = new ArduinoBot(trame.src);
                    robot.DateLastMessageReceived = DateTime.Now;
                    _ArduinoManager.addArduinoBot(robot);
                }
                Logger.GlobalLogger.debug("Reception du message EMBtoPCMessageAskConn par " + robot.id);
                if (!robot.Connected) // Robot non connecté, on l'ajoute dans la liste des connectés
                {
                    robot.Connect();
                    robot.DateLastMessageReceived = DateTime.Now;
                    robot.stateBot = StateArduinoBot.STATE_ARDUINO_NONE;
                    robot.stateComm = StateArduinoComm.STATE_COMM_NONE;
                    robot.CountSend = trame.num; // Utilisation du compteur du robot

                    MessageProtocol reponse = MessageBuilder.createRespConnMessage(0x01);

                    // Ajout a la liste à envoyer 
                    //TrameProtocole trameRet = _SerialXbee.EncodeTrame(_IdPc, trame.src, robot.CountSend++, reponse);
                    //trameRet.num = robot.CountSend++;
                    robot.PushMessageAEnvoyer(reponse);
                    //_SerialXbee.PushTrameToSend(trameRet);
                    
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
                Logger.GlobalLogger.debug("Reception du message EMBtoPCMessageGlobalAck par " + robot.id);

                if (robot == null)
                {
                    Logger.GlobalLogger.error("Robot Inconnu in EMBtoPCMessageGlobalAck");
                    return false; // Marquer comme traité
                }


                if (robot.Connected) // Robot connecté
                {
                    robot.DateLastMessageReceived = DateTime.Now;

                    EMBtoPCMessageGlobalAck msg = ((EMBtoPCMessageGlobalAck)message);
                    if (msg.valueAck == 0x00) // !akitement negatif Message erreur
                    {
                        Logger.GlobalLogger.error("Ackitement Negatif !");
                    }
                    else if (msg.valueAck == 0x01)
                    {
                        if (robot.stateComm == StateArduinoComm.STATE_COMM_WAIT_ACK) // On attendais un ACK
                            robot.stateComm = StateArduinoComm.STATE_COMM_NONE;

                        robot.AckRecu(msg.idTrame);
                        //_SerialXbee.DeleteTrame(msg.idTrame);
                    }
                    else if (msg.valueAck == 0x02)
                    {
                        Logger.GlobalLogger.error("CRC corrompu, on r'envois !");
                    }
                    else
                    {
                        Logger.GlobalLogger.error("Ackitement inconnu !");
                    }
                    

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
                Logger.GlobalLogger.debug("Reception du message EMBtoPCMessageRespPing par " + robot.id);
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


                    /* Supprimer les demande de ping sans ackitements en provenance du robot */
                    foreach (MessageProtocol mp in robot.ListMessageAttenteAck())
                    {
                        if(mp.headerMess == (byte)PCtoEMBmessHeads.ASK_PING)
                        {
                            robot.SupprimerMessage(mp);
                        }
                    }
                    //List<TrameProtocole> Listtp = _SerialXbee.FetchTrameSentNoAck();
                    /*foreach (TrameProtocole tp in Listtp)
                    {
                       if (tp.data[0] == (byte)PCtoEMBmessHeads.ASK_PING)
                            if(tp.dst == robot.id)
                                _SerialXbee.DeleteTrame(tp.num);
                    }*/
                    
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
                Logger.GlobalLogger.debug("Reception du message EMBtoPCMessageRespSensor par " + robot.id);
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
                    
                    foreach (MessageProtocol mp in robot.ListMessageAttenteAck())
                    {
                        if(mp.headerMess == (byte)PCtoEMBmessHeads.ASK_SENSOR)
                        {
                            robot.SupprimerMessage(mp);
                        }
                    }

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

        #region #### Thread Envoi / KeepAlive / Rejeux ####
        public void _ThreadCheckEnvoi()
        {
            while (true)
            {
                // Chacun des robots
                List<ArduinoBot> listeArduino = _ArduinoManager.ListeArduino;
                for (int i = 0; i < listeArduino.Count; i++)
                {
                    // Si il est connecté
                    if(listeArduino[i].Connected)
                    {
                         /* Keep alive */
                        // Pas de messages depuis DateLastMessageReceived
                        if (DateTime.Now - listeArduino[i].DateLastMessageReceived > TimeSpan.FromSeconds(_TimeOutKeepAlive))
                        {
                            // On n'est pas en attente d'un ACK
                            if (listeArduino[i].stateComm == StateArduinoComm.STATE_COMM_NONE)
                            {
                                MessageProtocol reponse = MessageBuilder.createAskPingMessage();

                                Logger.GlobalLogger.debug("Envoi d'un Ping au robot " + listeArduino[i].id);
                                PushSendMessageToArduino(reponse, listeArduino[i]);
                                //SendMessageToArduino(reponse, listeArduino[i]);
                            }
                        }

                        /* Rejeux */
                        List<MessageProtocol> AttAck = listeArduino[i].ListMessageAttenteAck();
                        foreach (MessageProtocol mess in AttAck)
                        {
                            /* On doit en envoyer un rejeux */
                            if ((DateTime.Now - mess.time) > TimeSpan.FromSeconds(_DelayRejeu))
                            {
                                if (mess.countRejeu < _MaxRejeu)
                                {
                                    //listeArduino[i].ResendMessageAttenteAck(mess);
                                    listeArduino[i].AddRejeuxMessageAttenteAck(mess);
                                    listeArduino[i].UpdateDateEnvoiMessageAttenteAck(mess);

                                    Logger.GlobalLogger.debug("Declenchement Renvoi ", 1);

                                    //listeArduino[i].MessageAttenteAck(mess, listeArduino[i].CountSend);
                                    TrameProtocole trame = _SerialXbee.EncodeTrame(_IdPc, listeArduino[i].id, listeArduino[i].CountSend, mess);
                                    
                                    _SerialXbee.PushTrameAEnvoyer(trame);

                                   
                                    break;
                                }
                                else
                                {
                                    _OnArduinoTimeout(listeArduino[i]);
                                    Logger.GlobalLogger.info("Pas de réponses de l'arduino, suppression !");
                                    break;
                                     // Pas de réponse depuis longtemps ? on déconnect 
                                }
                            }
                        }

                        if(listeArduino[i].IsMessageAEnvoyer())
                        {
                            if(listeArduino[i].Connected)
                            {
                                // On n'est pas en attente pour ne pas spamer
                                if (listeArduino[i].stateComm == StateArduinoComm.STATE_COMM_NONE)
                                {
                                    MessageProtocol mess = listeArduino[i].PopMessageAEnvoyer();
                                    //_SerialXbee.PushTrameAEnvoyer(_SerialXbee.EncodeTrame(_IdPc,listeArduino[i].id,listeArduino[i].CountSend,mess));
                                    
                                    SendMessageToArduino(mess, listeArduino[i]);
                                    Logger.GlobalLogger.debug("Envoi d'un message");
                                    break;
                                }
                            }
                        }

                    }
                }
                Thread.Sleep(_ThreadKeepAliveDelay);
            }
        }
        #endregion

        #region #### Envoi de messages ####
        private void SendMessageToArduino(MessageProtocol mess, ArduinoBot bot)
        {
            if (bot == null)
            {
                Logger.GlobalLogger.error("Robot inconu !");
                return;
            }

            if (bot.Connected)
            {

                if (bot.stateComm == StateArduinoComm.STATE_COMM_NONE)
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
                        bot.stateComm = StateArduinoComm.STATE_COMM_WAIT_ACK;
                        bot.stateBot = StateArduinoBot.STATE_ARDUINO_NONE;
                    }
                    else
                    {
                        Logger.GlobalLogger.error("Envoi d'un message non connu !");
                    }

                    bot.MessageAttenteAck(mess, bot.CountSend);
                    TrameProtocole trame = _SerialXbee.EncodeTrame(_IdPc, bot.id, bot.CountSend++, mess);
                    //trame.num = bot.CountSend++;
                    _SerialXbee.PushTrameAEnvoyer(trame);

                }   
            }
            else
            {
                Logger.GlobalLogger.error("Envoi d'un message à un robot non connecté :(");
            }
        }
        public void PushSendMessageToArduino(MessageProtocol mess, ArduinoBot bot)
        {
            if(bot != null)
                bot.PushMessageAEnvoyer(mess);
        }
        #endregion

    }
}
