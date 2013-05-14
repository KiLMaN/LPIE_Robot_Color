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
        private const int       _ThreadDelay = 50;

        private XbeeAPI         _XbeeAPI;
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
            byte[] datas = args.trameBytes;
            int i = 0;
            // Parse les données
            while(i < datas.Length && !_TrameDecoder.parseIncomingData(datas[i]))
            { i++; }
            // Condition si la trame est finie ou pas
            TrameProtocole TrameFinale = _TrameDecoder.getDecodedTrame();

            /* Ajout dans la liste des trames recus */
            //_ListTramesRecues.Add(TrameFinale);
            PushTrameRecus(TrameFinale);

        }
        #endregion

        #region #### Trames Reçus ####
        /* Recuperrer une trame depuis la liste */
        public TrameProtocole PopTrameRecus()
        {
            return _ListTramesRecues.Find(TrameProtocole.TrameAFaire());
        }
        /* Ajouter une trame à traiter */
        public void PushTrameRecus(TrameProtocole trame)
        {
            trame.state = 0;
            _ListTramesRecues.Add(trame);
        }
        /* Des trames à traiter ? */
        public bool TrameRecusDisponible()
        {
            return _ListTramesRecues.Exists(TrameProtocole.TrameAFaire());
        }
        /* Marquer la trame comme traitée */
        public void TrameFaite(ushort num)
        {
            // Marquer la trame comme faite 
            // TODO la supprimer
            // Recupere la trame Numéroté num
            /*TrameProtocole t = _ListTramesRecues.Find(TrameProtocole.TrameByNum(num));
            t.state = 1; // La marque comme faite*/

            int pos = _ListTramesRecues.FindIndex(TrameProtocole.TrameByNum(num));
            TrameProtocole t = _ListTramesRecues[pos];
            t.state = 1; // La marque comme faite
            _ListTramesRecues[pos] = t;
        }
        #endregion

        #region #### Trame A Envoyer ####
        /* Recuperrer une trame à envoyer */
        public TrameProtocole PopTrameToSend()
        {
            return _ListTramesToSend.Find(TrameProtocole.TrameAFaire());
        }
        /* Ajoute une trame à envoyer dans la liste */
        public void PushTrameToSend(TrameProtocole trame)
        {
            trame.state = 0;
            _ListTramesToSend.Add(trame);
        }
        /* Des trames à envoyer disponnible ? */
        public bool TrameToSendDisponible()
        {
            return _ListTramesToSend.Exists(TrameProtocole.TrameAFaire());
        }
        /* Marquer la trame comme faite */
        public void TrameSend(ushort num)
        {
            // Marquer la trame comme faite 
            // TODO la supprimer
            // Recupere la trame Numéroté num
            int pos = _ListTramesToSend.FindIndex(TrameProtocole.TrameByNum(num));
            TrameProtocole t = _ListTramesToSend[pos];           
            t.state = 1; // La marque comme faite
            _ListTramesToSend[pos] = t;
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
                if (TrameToSendDisponible())
                {
                    TrameProtocole trame = PopTrameToSend(); // recuperer une trame
                    _XbeeAPI.sendApiFrame(trame.dst, _TrameEncoder.MakeTrameBinaryWithEscape(trame));
                    TrameSend(trame.num); // La marquer comme faite
                }
                Thread.Sleep(_ThreadDelay);
            }
        }
        #endregion

        #region #### Encoder / Decoder ####
        /* Decode la trame en Message */
        public MessageProtocol DecodeTrame(TrameProtocole trame)
        {
            return _TrameDecoder.DecodeTrame(trame);
        }
        /* Encore le message en Trame */
        public TrameProtocole EncodeTrame(byte src, byte dst,MessageProtocol message)
        {
            return _TrameEncoder.EncodeTrame(message, src,dst);
        }
        #endregion

        
    }
}
