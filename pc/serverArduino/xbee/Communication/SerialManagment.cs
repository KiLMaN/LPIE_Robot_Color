using System;
using System.Collections.Generic;
using System.IO.Ports;
using utils;
using xbee.Communication.Events;


namespace xbee.Communication
{
    class SerialManagment : IDisposable
    {
        #region #### Evenement ####
        //Le délégué pour stocker les références sur les méthodes
        public delegate void NewDataReceivedEventHandler(object sender, NewDataReceveidEventArgs e);
        //L'évènement
        public event NewDataReceivedEventHandler OnNewDataReceived;
        #endregion

        private SerialPort _PortSerie;
        public SerialPort PortSerie
        {
            get { return _PortSerie; }
        }

        private List<byte> _datasReceived;

        /* Modifie le debit du port Série en le fermant avant */
        public int BaudRate
        {
            get { return _PortSerie.BaudRate; }
            set {
                bool bO = _PortSerie.IsOpen;
                if(bO)
                    this.Close();  
                _PortSerie.BaudRate = value;
                if (bO)
                    this.Open();
            }
        }

        public int countData
        {
            get{return _datasReceived.Count;}
        }

        public SerialManagment(string SerialName,int BaudRate)
        {
            _datasReceived = new List<byte>();

            _PortSerie = new SerialPort(SerialName, BaudRate);

            _PortSerie.DataReceived +=new SerialDataReceivedEventHandler(_PortSerie_DataReceived);
            _PortSerie.ErrorReceived +=new SerialErrorReceivedEventHandler(_PortSerie_ErrorReceived);
            //this.Open();
        }

        ~SerialManagment()
        {
            Close();
        }
        public void Dispose()
        {
            Close();
            //throw new NotImplementedException();
        }

        #region #### Management ####
        /* Tente d'ouvrir un port Série*/
        public bool Open()
        {
            try
            {
                _PortSerie.Open();
                return _PortSerie.IsOpen;
            }
            catch (Exception e)
            {
                Logger.GlobalLogger.error(e.Message);
                return false;
            }
        }
        /* Tente de fermer un port Série*/
        public bool Close()
        {
            try
            {
                _PortSerie.Close();
                return !_PortSerie.IsOpen;
            }
            catch (Exception e)
            {
                Logger.GlobalLogger.error(e.Message);
                return false;
            }
        }
        #endregion

        #region #### Enevenements ####
        /* Evenements sur le port Série */
        private void _PortSerie_DataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            // on concerve le nombre de bytes
            int count = _PortSerie.BytesToRead;
            int i = 0;

            Logger.GlobalLogger.debug("Data Lenght :" + count);

            // On les copies dans la liste
            while (i < count)
            {
                _datasReceived.Add((byte)_PortSerie.ReadByte());// Ajout dans la liste
                i++;
            }

            // envoi de l'evenement à l'application
            NewDataReceveidEventArgs e = new NewDataReceveidEventArgs(_datasReceived.Count);
            OnNewDataReceived(this, e);
        }
        private void _PortSerie_ErrorReceived(object sender, SerialErrorReceivedEventArgs args)
        {
            Logger.GlobalLogger.error("Error Serial : " + args.ToString());
        }
        #endregion

        #region #### Données ####
        /* Savoir si une donnée est disponible */
        public bool isDataAvailable()
        {
            return (_datasReceived.Count > 0);
        }
        /* Demander une liste de valeurs dans le buffer sans la retirer */
        public byte[] fetchData(int count)
        {
            /*byte[] d = new byte[count]; 

            for (int i = 0; i < count; i++)
            {
               d[i] = _datasReceived[i];
            }
            return d;*/
            return _datasReceived.GetRange(0, count).ToArray();
        }
        /* Demande la suppression de valeurs dans la liste */
        public void removeData(int count)
        {
            _datasReceived.RemoveRange(0, count);
        }
        /* Recupere des valeurs dans la liste en les retirant */
        public byte[] getData(int count)
        {
            byte[] d = fetchData(count);
            removeData(count);
            return d;
        }

        /* Envoye des données sur le port Série */
        public bool SendBytes(byte[] dataToSend)
        {
            if (!_PortSerie.IsOpen)
                return false;

            try
            {
                _PortSerie.Write(dataToSend, 0, dataToSend.Length);
                return true;
            }
            catch (Exception e)
            {
                Logger.GlobalLogger.error("Impossible d'envoyer les données : " + e.Message);
                return false;
            }
        }
        #endregion


    }
}
