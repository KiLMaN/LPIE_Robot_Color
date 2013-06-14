using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using utils;
using xbee.Communication.Events;

namespace xbee.Communication
{
    class SerialXbee : IDisposable
    {
        private Thread          _ThreadEnvoi;
        private const int       _ThreadDelay = 10;

        private XbeeAPI         _XbeeAPI;
        //private ArduinoManager  _ArduinoManager;
        private TrameDecoder    _TrameDecoder;
        private TrameEncoder    _TrameEncoder;

        // Listes des messages en attente de traitement 
        private List<TrameProtocole> _ListTramesRecues;
        private List<TrameProtocole> _ListTramesToSend;

        public SerialXbee(string PortSerie,bool XbeeApiEnabled)
        {
            // Creation des listes 
            _ListTramesRecues = new List<TrameProtocole>();
            _ListTramesToSend = new List<TrameProtocole>();

            // Création des décodeurs/encodeurs
            _TrameDecoder = new TrameDecoder();
            _TrameEncoder = new TrameEncoder();

            //_ArduinoManager = AM;

            // Connection Xbee 
            _XbeeAPI = new XbeeAPI(PortSerie, XbeeApiEnabled);
            // Nouvelle Trame Reçue 
            _XbeeAPI.OnNewTrameReceived += new XbeeAPI.NewTrameReceivedEventHandler(_XbeeAPI_OnNewTrameReceived);

            // Thread Pour l'envoi des messagess
            _ThreadEnvoi = new Thread(new ThreadStart(_ThreadEnvoiTrames));
            _ThreadEnvoi.Start();
        }
        ~SerialXbee()
        {
            StopThreadEnvoi();
        }
        public void Dispose()
        {
            StopThreadEnvoi();
            _XbeeAPI.Dispose();
        }

        public void setXbeeApiMode(bool state)
        {
            _XbeeAPI.ApiEnabled = state;
        }

        #region #### Gestion Port Série ####
        public void SetSerialConnexion(bool state)
        {
            _XbeeAPI.SetSerialConnexion(state);
        }
        public bool GetSerialState()
        {
            return _XbeeAPI.GetSerialState();
        }
        public void SetSerialName(string name)
        {
            _XbeeAPI.SetSerialName(name);
        }
        #endregion

        #region #### Evenement ####
        // Nouvelle trame 
        private void _XbeeAPI_OnNewTrameReceived(object sender, NewTrameReceivedEventArgs args)
        {
            Logger.GlobalLogger.debug("Données reçues ");
            byte[] datas = args.trameBytes;
            int i = 0;
            // Parse les données
            while(i < datas.Length && !_TrameDecoder.parseIncomingData(datas[i]))
            { i++; }
            Logger.GlobalLogger.debug("Parsing terminé etat :  " + _TrameDecoder.bIsCompleted);
            // Condition si la trame est finie ou pas
            if (!_TrameDecoder.bIsCompleted) // Trame non complette on attends
                return;
           
            TrameProtocole TrameFinale = _TrameDecoder.getDecodedTrame();
            /* Ajout dans la liste des trames recus */
            //_ListTramesRecues.Add(TrameFinale);
            PushTrameRecus(TrameFinale);
            Logger.GlobalLogger.debug("Decodage et ajout en liste");
        }
        #endregion

        #region #### Trames Reçus ####
        // Recuperrer une trame depuis la liste et la supprime //
        public TrameProtocole PopTrameRecus()
        {
            TrameProtocole t = default(TrameProtocole);
            if (_ListTramesRecues.Count > 0)
            {
                t = _ListTramesRecues[0];
                _ListTramesRecues.Remove(t);
            }
            return t;
        }
        // Ajouter une trame à traiter //
        public void PushTrameRecus(TrameProtocole trame)
        {
            Logger.GlobalLogger.debug("Reception d'une trame (" + trame.ToString() + ") ", 1);
            _ListTramesRecues.Add(trame);
        }
        // Des trames à traiter ? //
        public bool TrameRecusDisponible()
        {
            return _ListTramesRecues.Count > 0;
            //return _ListTramesRecues.Exists(TrameProtocole.TrameAFaire());
        }
        #endregion

        #region #### Trame A Envoyer ####
        // Recuperrer une trame depuis la liste et la supprime //
        public TrameProtocole PopTrameAEnvoyer()
        {
            TrameProtocole t = default(TrameProtocole);
            if (_ListTramesToSend.Count > 0)
            {
                t = _ListTramesToSend[0];
                _ListTramesToSend.Remove(t);
            }
            return t;
        }
        // Ajouter une trame à traiter //
        public void PushTrameAEnvoyer(TrameProtocole trame)
        {
            _ListTramesToSend.Add(trame);
        }
        // Des trames à traiter ? //
        public bool TrameAEnvoyerDisponible()
        {
            return _ListTramesToSend.Count > 0;
        }
        #endregion

        #region #### Thread Envoi ####
        public void StopThreadEnvoi()
        {
            _ThreadEnvoi.Abort();
        }
        private void _ThreadEnvoiTrames()
        {
            while (true)
            {

                if (TrameAEnvoyerDisponible())
                {
                    /* Envoi */
                   //List<byte> Envoyes = new List<byte>();
                    TrameProtocole trame = PopTrameAEnvoyer();
                    Logger.GlobalLogger.debug("Envoi d'une trame ("+trame.ToString()+") ", 1);
                    _XbeeAPI.sendApiFrame(trame.dst, _TrameEncoder.MakeTrameBinaryWithEscape(trame));
                    Logger.GlobalLogger.debug("OK");
                }

                Thread.Sleep(_ThreadDelay);
            }
        }
        #endregion

        #region #### Encoder / Decoder ####
        // Decode la trame en Message //
        public MessageProtocol DecodeTrame(TrameProtocole trame)
        {
            return _TrameDecoder.DecodeTrame(trame);
        }
        // Encore le message en Trame//
        public TrameProtocole EncodeTrame(byte src, byte dst,ushort num ,MessageProtocol message)
        {
            return _TrameEncoder.EncodeTrame(message, src, dst, num);
        }
        #endregion
        
    }
}
