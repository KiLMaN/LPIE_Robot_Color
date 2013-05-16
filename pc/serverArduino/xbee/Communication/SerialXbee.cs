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
        //private List<>

        private const int _DelayRejeu = 10; // Temps d'attente en les rejeux
        private const int _MaxRejeu = 3; // Nombre max de rejeux avant timeout (sans conter l'envoi initial)

        #region #### Evenement ####
        //Le délégué pour stocker les références sur les méthodes
        public delegate void NewArduinoTimeoutEventHandler(object sender, NewArduinoTimeoutEventArgs e);
        //L'évènement
        public event NewArduinoTimeoutEventHandler OnArduinoTimeout;
        #endregion

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
            if (!_TrameDecoder.bIsCompleted) // Trame non complette on attends
                return;
           
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
            int pos = _ListTramesRecues.FindIndex(TrameProtocole.TrameByNum(num));
            TrameProtocole t = _ListTramesRecues[pos];
            t.state = 1; // La marque comme faite
            _ListTramesRecues[pos] = t;
            _ListTramesRecues.Remove(_ListTramesRecues.Find(TrameProtocole.TrameByNum(num)));
        }
        #endregion

        #region #### Trame A Envoyer ####
        /* Recuperrer une trame à envoyer */
        public TrameProtocole PopTrameToSend()
        {
            return _ListTramesToSend.Find(TrameProtocole.TrameAFaire());
        }
        /* Recupere les trames qui ont été envoyés mais pas encore Ackités */
        public List<TrameProtocole> FetchTrameSentNoAck()
        {
            return _ListTramesToSend.FindAll(TrameProtocole.TrameSentNoAck());
        }
        /* Recupere la liste des trames qui n'ont pas encore été envoyées */
        public List<TrameProtocole> FetchTrameToSend()
        {
            return _ListTramesToSend.FindAll(TrameProtocole.TrameAFaire());
        }
        /* Ajoute une trame à envoyer dans la liste */
        public void PushTrameToSend(TrameProtocole trame)
        {
            trame.state = 0;
            trame.countRejeu = 0;
            _ListTramesToSend.Add(trame);
        }
        /* Des trames à envoyer disponnible ? */
        public bool TrameToSendDisponible()
        {
            return _ListTramesToSend.Exists(TrameProtocole.TrameAFaire());
        }
        public void updateTrame(TrameProtocole trame)
        {
            int pos = _ListTramesToSend.FindIndex(TrameProtocole.TrameByNum(trame.num));
            _ListTramesToSend[pos] = trame;
        }
        /* Marquer la trame comme faite */
        public void TrameSend(ushort num)
        {
            int pos = _ListTramesToSend.FindIndex(TrameProtocole.TrameByNum(num));
            TrameProtocole t = _ListTramesToSend[pos];           
            t.state = 1; // La marque comme faite
            t.time = DateTime.Now;
            _ListTramesToSend[pos] = t;
        }
        /* Supprimer la trame une fois ackité */
        public void DeleteTrame(ushort num)
        {
            _ListTramesToSend.Remove(_ListTramesToSend.Find(TrameProtocole.TrameByNum(num)));
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
                /* Envoi */
                List<TrameProtocole> TrameWaitSend = FetchTrameToSend();
                for (int i = 0; i < TrameWaitSend.Count; i++)
                {
                    TrameProtocole trame = TrameWaitSend[i];
                    
                      //TrameProtocole trame = PopTrameToSend(); // recuperer une trame
                        _XbeeAPI.sendApiFrame(trame.dst, _TrameEncoder.MakeTrameBinaryWithEscape(trame));
                        TrameSend(trame.num); // La marquer comme faite
                   
                }

                /* Rejeu */
                List<TrameProtocole> TrameWaitingAck = FetchTrameSentNoAck();
                for (int i = 0; i < TrameWaitingAck.Count; i++)
                {
                    if (( DateTime.Now - TrameWaitingAck[i].time) > TimeSpan.FromSeconds(_DelayRejeu))
                    {
                        if (TrameWaitingAck[i].countRejeu < _MaxRejeu)
                        {
                            TrameProtocole tmp = TrameWaitingAck[i];
                            tmp.countRejeu++;
                            tmp.state = 0; // Declenche l'envoi 

                            TrameWaitingAck[i] = tmp;
                            updateTrame(TrameWaitingAck[i]);
                        }
                        else // Supprimer le message et deconnecter l'arduino
                        {
                            Logger.GlobalLogger.info("Pas de réponses de l'arduino, suppression !");

                            List<TrameProtocole> TrameArduino = FetchTrameToSend();
                            for (int j = 0; j < TrameArduino.Count; j++)
                            {
                                // Suppression des trames en attentes
                                if(TrameArduino[j].dst == TrameWaitingAck[i].dst)
                                    DeleteTrame(TrameArduino[j].num);
                            }
                  
                            DeleteTrame(TrameWaitingAck[i].num);
                            // Envoi a la couche suppérieur pour passer l'arduino en non connecté
                            NewArduinoTimeoutEventArgs e = new NewArduinoTimeoutEventArgs(TrameWaitingAck[i].dst);
                            OnArduinoTimeout(this, e);
                        }
                        // Si on trop attendu : Renvoyer
                    }
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
