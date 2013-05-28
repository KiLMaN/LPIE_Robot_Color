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
        #region #### Evenement ####
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

        private const int _ThreadMessageRecuDelay = 50; // Delais entre les verification de nouveau messages recus en millisecondes
        private Thread _ThreadMessagesRecus;

        private const int _ThreadKeepAliveDelay = 100; // verification des keepalive
        private const int _TimeOutKeepAlive = 30; // Envoi d'un KeepAlive (ping) toutes les 30 secondes sans messages
        private Thread _ThreadKeepAlive;

        private ArduinoManager _ArduinoManager;
        public ArduinoManager ArduinoManager
        {
            get { return _ArduinoManager; }
            //set { _ArduinoManager = value; }
        }

        // Messages en attente d'envois car l'on a pas recus d'ack de l'arduino
        private struct MessageEnAttente
        {
            public ArduinoBot robot;
            public MessageProtocol message;
        };
        private List<MessageEnAttente> _MessagesEnAttenteEnvoi;

        public AutomateCommunication(string portSerie,bool XbeeApiMode,ArduinoManager ArduinoManager)
        {
            _MessagesEnAttenteEnvoi = new List<MessageEnAttente>();

            _ArduinoManager = ArduinoManager;

            _SerialXbee = new SerialXbee(portSerie, XbeeApiMode);
            _SerialXbee.OnArduinoTimeout += new SerialXbee.NewArduinoTimeoutEventHandler(_OnArduinoTimeout);

            // Thread de gestion des messages 
            _ThreadMessagesRecus = new Thread(new ThreadStart(_ThreadCheckMessageRecus));
            _ThreadMessagesRecus.Start();

            // Thread de keepAlive
            _ThreadKeepAlive = new Thread(new ThreadStart(_ThreadCheckKeepAlive));
            _ThreadKeepAlive.Start();
        }
        public void Dispose()
        {
            _ThreadKeepAlive.Abort();
            _ThreadMessagesRecus.Abort();
            _SerialXbee.Dispose();
        }
        public void setXbeeApiMode(bool state)
        {
            _SerialXbee.setXbeeApiMode(state);
        }

        #region #### Evenements ####
        public void _OnArduinoTimeout(object sender, NewArduinoTimeoutEventArgs e)
        {
            _ArduinoManager.disconnectArduinoBot(e.Id);

            // Supressions des messages en attente pour l'arduino qui viens de se deonnecter
            for (int i = _MessagesEnAttenteEnvoi.Count -1; i >= 0; i--)
            {
                if (_MessagesEnAttenteEnvoi[i].robot.id == e.Id)
                {
                    _MessagesEnAttenteEnvoi.Remove(_MessagesEnAttenteEnvoi[i]);
                }
            }
            ArduinoTimeoutEventArgs e2 = new ArduinoTimeoutEventArgs(_ArduinoManager.getArduinoBotById(e.Id));
            OnArduinoTimeout(this, e2);
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
                if (_SerialXbee.TrameRecusDisponible())
                {
                    TrameProtocole trame = _SerialXbee.PopTrameRecus();

                    if(TraiteTrameRecue(trame))
                    {
                        // Envoi au couches supérrieures 
                         MessageProtocol message = _SerialXbee.DecodeTrame(trame);
                        ArduinoBot robot = ArduinoManager.getArduinoBotById(trame.src);
                        NewTrameArduinoReceveidEventArgs arg = new NewTrameArduinoReceveidEventArgs(message,robot);

                        OnNewTrameArduinoReceived(this, arg);
                    }
                    _SerialXbee.TrameFaite(trame.num); // Marque la trame comme traitée
                }

                /* Verification message en attente envoi */
                if (_MessagesEnAttenteEnvoi.Count > 0)
                {
                    int count = _MessagesEnAttenteEnvoi.Count;
                    for(int i=0 ; i< count;i++)
                    {
                        if (_MessagesEnAttenteEnvoi[i].robot.stateComm == StateArduinoComm.STATE_COMM_NONE)
                        {
                            // L'arduino est libre, on peut envoyer
                            SendMessageToArduino(_MessagesEnAttenteEnvoi[i].message, _MessagesEnAttenteEnvoi[i].robot);
                        }
                    }
                   
                }
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
                    _ArduinoManager.addArduinoBot(robot);
                }

                if (!robot.Connected) // Robot non connecté, on l'ajoute dans la liste des connectés
                {
                    robot.Connected = true;
                    robot.DateLastMessageReceived = DateTime.Now;
                   // robot.DateLastMessageSend = DateTime.Now;
                    robot.stateBot = StateArduinoBot.STATE_ARDUINO_NONE;
                    robot.stateComm = StateArduinoComm.STATE_COMM_NONE;
                    robot.CountSend = trame.num;

                    MessageProtocol reponse = MessageBuilder.createRespConnMessage(0x01);

                    // Ajout a la liste à envoyer 
                    TrameProtocole trameRet = _SerialXbee.EncodeTrame(_IdPc, trame.src, reponse);
                    trameRet.num = robot.CountSend++;
                    _SerialXbee.PushTrameToSend(trameRet);
                    
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

                        _SerialXbee.DeleteTrame(msg.idTrame);
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

        #region #### Thread KeepAlive ####
        public void _ThreadCheckKeepAlive()
        {
            while (true)
            {
                List<ArduinoBot> listeArduino = _ArduinoManager.ListeArduino;
                for (int i = 0; i < listeArduino.Count; i++)
                {
                    if(listeArduino[i].Connected)
                    {
                        // Pas de messages depuis _timeOut
                        if (DateTime.Now - listeArduino[i].DateLastMessageReceived > TimeSpan.FromSeconds(_TimeOutKeepAlive))
                        {
                            // On n'est pas en attente d'un ACK
                            if (listeArduino[i].stateComm == StateArduinoComm.STATE_COMM_NONE)
                            {
                                MessageProtocol reponse = MessageBuilder.createAskPingMessage();

                                // Ajout a la liste à envoyer 
                                //TrameProtocole trameRet = _SerialXbee.EncodeTrame(_IdPc, listeArduino[i].id, reponse);
                                //trameRet.num = listeArduino[i].CountSend++;
                                //_SerialXbee.PushTrameToSend(trameRet);
                                SendMessageToArduino(reponse, listeArduino[i]);
                            }
                        }
                    }
                }
                Thread.Sleep(_ThreadKeepAliveDelay);
            }
        }
        #endregion

        #region #### Envoi de messages ####
        public void SendMessageToArduino(MessageProtocol mess, ArduinoBot bot)
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

                    TrameProtocole trame = _SerialXbee.EncodeTrame(_IdPc, bot.id, mess);
                    trame.num = bot.CountSend++;
                    _SerialXbee.PushTrameToSend(trame);
                    
                }
                else
                {
                    //TrameProtocole trame = _SerialXbee.EncodeTrame(_IdPc, bot.id, mess);
                    //trame.num = bot.CountSend++;
                    //MessageEnAttente.Add(trame);
                    MessageEnAttente MEA = new MessageEnAttente();
                    MEA.message = mess;
                    MEA.robot = bot;
                    _MessagesEnAttenteEnvoi.Add(MEA);

                }
                
            }
            else
            {
                Logger.GlobalLogger.error("Envoi d'un message à un robot non connecté :(");
            }
        }
        #endregion

    }
}
