using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using utils;
using Communication.Arduino.protocol;
using Communication.Events;


namespace Communication.Arduino.Xbee
{
    class SerialXbee
    {


        //Le délégué pour stocker les références sur les méthodes
        public delegate void NewInfosArduinoBotEventHandler(object sender, NewTrameReceiveEventArgs e);
        //L'évènement
        public static event NewInfosArduinoBotEventHandler OnNewTrameReceived;

         

        private int delayThreadSend = 100; // Delay entre les executions des envois en millisecondes

        private List<TrameProtocole> _stackReceive;     // Liste des messages reçus a traiter //
        private List<TrameProtocole> _stackToSend;      // Liste des messages à envoyer //

        private const byte COMPUTER_ID      = 0xFE;     // Constante identifiant l'ordinateur //
        private const byte BROADCAST_ID     = 0xFF;     // Constante identifiant un broadcast //

        private SerialPort _PortSerie;

        private bool _XbeeModeApi;
        private Protocol _protocol = new Protocol();
        //private List<ArduinoBots> FlotteRobots = new List<ArduinoBots>(); // Stockage des arduinos
        Thread _ThreadSendMessage = null;

        /* Mutateurs */ 
        public SerialPort PortSerie
        {
            get
            {
                return _PortSerie;
            }
            /*set
            {
                _PortSerie = value;
            }*/
        }
        public int BaudRate
        {
            get { return _PortSerie.BaudRate; }
            set { if (!_PortSerie.IsOpen) _PortSerie.BaudRate = value; }
            /*get { return _BAUD_RATE; }
            set { _BAUD_RATE = value; }*/
        }
        public String ComPort
        {
            get { return _PortSerie.PortName; }
            set { if(!_PortSerie.IsOpen) _PortSerie.PortName = value; }
        }
        public bool ModeApi
        {
            set { _XbeeModeApi = value; }
            get { return _XbeeModeApi; }
        }

        /* Constructor */
        public SerialXbee()
        {
            // Initialisation des stacks
            _stackReceive = new List<TrameProtocole>();
            _stackToSend = new List<TrameProtocole>();

            // Initialisation du port Serie
            _PortSerie = new SerialPort();
            //_PortSerie.PortName = ComPort;
            _PortSerie.BaudRate = 9600;
            _PortSerie.DataReceived += new SerialDataReceivedEventHandler(_PortSerie_DataReceived); // declaration d'un eventHandler
            _PortSerie.ErrorReceived += new SerialErrorReceivedEventHandler(_PortSerie_ErrorReceived); 

            //_PortSerie = SerialPort;
            _protocol.PortSerie = _PortSerie; // Fournit le port au protocole pour pouvoir lire dessus

            //Thread _ThreadListen = new Thread(new ThreadStart(_ThreadListenF));
            //_ThreadSendMessage = new Thread(new ThreadStart(_ThreadSendMessageF));

            //StartListenSerial();
        }
        ~SerialXbee()
        {
            StopListenSerial();
        }



        #region #### Sending Thread Managment ####
        /* Verifie la présence d'un message à  envoyer et l'envoi si necessaire */
        public void _ThreadSendMessageF()
        {
            while (System.Threading.Thread.CurrentThread.ThreadState == ThreadState.Running)
            {
                if (_stackToSend.Count > 0) // On doit envoyer un message
                {
                    //foreach (TrameProtocole trame in _stackToSend)
                    for(int i = _stackToSend.Count-1 ; i >= 0 ; i--)
                    {
                        if (_stackToSend[i].state == 0) // on doit l'envoyer
                        {
                            TrameProtocole trame = _stackToSend[i];
                            _protocol.SendTrame(trame, _XbeeModeApi);
                            trame.state = 1;
                            _stackToSend[i] = trame; // Force écrasement de la valeur
                        }
                    }
                }
                System.Threading.Thread.Sleep(delayThreadSend);
            }
        }
        #endregion

        #region #### Serial Managment ####
        public void _PortSerie_ErrorReceived(object sender, SerialErrorReceivedEventArgs arg)
        {
            Logger.GlobalLogger.error("_PortSerie_ErrorReceived : " + arg.ToString());
        }
        public void _PortSerie_DataReceived(object sender, SerialDataReceivedEventArgs arg)
        {
            Logger.GlobalLogger.debug("_PortSerie_DataReceived");
            try
            {
                TrameProtocole trame = _protocol.getIncomingTrame(_XbeeModeApi); // recuperre la trame reçue
                if (trame.src == 0)
                    Logger.GlobalLogger.error("Parsing Error : No SRC !");
                else if (Protocol.crc16_protocole(trame) == trame.crc) // Verification du CRC
                {
                    Logger.GlobalLogger.debug("CRC OK");

                    //byte idSource = trame.src;
                    //ArduinoBots robot = FlotteRobots.Find(ArduinoBots.ById(idSource));
                    /*if (robot == null) // Robot inconnu
                    {
                        Logger.GlobalLogger.info("Robot inconnu, Creation Nouveau robot");
                        robot = new ArduinoBots(idSource);
                        FlotteRobots.Add(robot);
                    }*/

                    /*if (trame.dst == BROADCAST_ID) // Broadcast 
                    {
                   
                    }
                    else if (trame.dst == COMPUTER_ID) // Ordi 
                    {
                    }
                    else
                    {
                    }*/

                    Logger.GlobalLogger.debug("Add in stack");
                    _stackReceive.Add(trame); // ajout le la trame reçus dans la stack à traité

                    // envoi de l'evenement
                    NewTrameReceiveEventArgs e = new NewTrameReceiveEventArgs(trame);
                    OnNewTrameReceived(this, e);


                }
                else
                {
                    Logger.GlobalLogger.error("CRC FAILED");
                }
            }
            catch (Exception e)
            {
                Logger.GlobalLogger.error(e.ToString());
                return;
            }
        }
        public void StartListenSerial()
        {
            try
            {
                Logger.GlobalLogger.debug("Openning Serial Port");
                if (!_PortSerie.IsOpen)
                    _PortSerie.Open();

                Logger.GlobalLogger.debug("Starting Sending thread");
                _ThreadSendMessage = new Thread(new ThreadStart(_ThreadSendMessageF));
                _ThreadSendMessage.Start();
            }
            catch (Exception e)
            {
                Logger.GlobalLogger.error("Error Openning Serial Port: " + e.Message);
            }
        }
        public void StopListenSerial()
        {
            try
            {
                Logger.GlobalLogger.debug("Ending Sending thread");
                _ThreadSendMessage.Abort();

                Logger.GlobalLogger.debug("Closing Serial Port");
                if (_PortSerie.IsOpen)
                    _PortSerie.Close();



            }
            catch (Exception e)
            {
                Logger.GlobalLogger.error("Error Closing Serial Port: " + e.Message);
            }
        }
        #endregion

        public void addMessageToSend(TrameProtocole trame)
        {
            _stackToSend.Add(trame);
        }

    }
}
