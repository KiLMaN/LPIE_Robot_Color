using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;


namespace DebugProtocolArduino
{
    class SerialXbee
    {
        private const byte COMPUTER_ID      = 0xFE;
        private const byte BROADCAST_ID     = 0xFF;


        private SerialPort _PortSerie;
        private Protocol _protocol = new Protocol();
        private List<ArduinoBots> FlotteRobots = new List<ArduinoBots>(); // Stockage des arduinos
        Thread _ThreadTimeOut = null;

        public SerialPort PortSerie
        {
            get
            {
                return _PortSerie;
            }
            set
            {
                _PortSerie = value;
            }
        }



        public SerialXbee(SerialPort SerialPort)
        {
            _PortSerie = SerialPort;
            _protocol.PortSerie = _PortSerie;
            //Thread _ThreadListen = new Thread(new ThreadStart(_ThreadListenF));
            _ThreadTimeOut = new Thread(new ThreadStart(_ThreadTimeOutF));
            _PortSerie.DataReceived +=new SerialDataReceivedEventHandler(_PortSerie_DataReceived); // declaration d'un eventHandler
            _PortSerie.ErrorReceived += new SerialErrorReceivedEventHandler(_PortSerie_ErrorReceived); 

        
        }
        ~SerialXbee()
        {
        }

        #region #### TimeOut Managment ####
        /* Verifie les timeOuts de chacun des arduinoBots de la liste */
        public void _ThreadTimeOutF()
        {
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

            TrameProtocole trame =  _protocol.getTrame(); // recuperre la trame reçue
            if (Protocol.crc16_protocole(trame) == trame.crc) // Verification du CRC
            {
                Logger.GlobalLogger.debug("CRC OK");

                byte idSource = trame.src;
                ArduinoBots robot = FlotteRobots.Find(ArduinoBots.ById(idSource));
                if (robot == null) // Robot inconnu
                {
                    Logger.GlobalLogger.info("Robot inconnu, Creation Nouveau robot");
                    robot = new ArduinoBots(idSource);
                    FlotteRobots.Add(robot);
                }

                if (trame.dst == BROADCAST_ID) // Broadcast 
                {
                   
                }
                else if (trame.dst == COMPUTER_ID) // Ordi 
                {
                }
                else
                {
                }

            }
            else
            {
                Logger.GlobalLogger.error("CRC FAILED");
            }
        }
        #endregion


        public void StartListenSerial()
        {
            try
            {
                Logger.GlobalLogger.debug("Openning Serial Port");
                if(!_PortSerie.IsOpen)
                    _PortSerie.Open();

                Logger.GlobalLogger.debug("Stating timeOut thread");
                _ThreadTimeOut.Start();
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
                Logger.GlobalLogger.debug("Closing Serial Port");
                if (_PortSerie.IsOpen)
                    _PortSerie.Close();

                Logger.GlobalLogger.debug("Ending timeOut thread");
                _ThreadTimeOut.Abort();
            }
            catch (Exception e)
            {
                Logger.GlobalLogger.error("[SerialXbee] Error Closing Serial Port: " + e.Message);
            }
        }
    }
}
