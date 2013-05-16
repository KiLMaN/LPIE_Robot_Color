using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbee.Communication;
using xbee.Communication.Events;
using utils;


namespace xbee.Communication
{
    class XbeeAPI : IDisposable
    {
        #region #### Evenement ####
        //Le délégué pour stocker les références sur les méthodes
        public delegate void NewTrameReceivedEventHandler(object sender, NewTrameReceivedEventArgs e);
        //L'évènement
        public event NewTrameReceivedEventHandler OnNewTrameReceived;
        #endregion

        #region #### XbeeAPI Protocole ####
        /*
         *  http://ftp1.digi.com/support/utilities/digi_apiframes2.htm
         *  Protocole Xbee API pour l'envoi d'un message :
         *  
         * 7E                  Start delimiter
         * 00 0A               Length bytes
         * 01                  API identifier
         * 01                  API frame ID
         * 50 01               Destination address low
         * 00                  Option byte
         * 48 65 6C 6C 6F      Data Packet
         * B8                  Checksum
         *  
         * 
         */
        const byte XBEE_START_DEL            = 0x7E; // DOCUMENT
        const byte XBEE_FRAME_TYPE_ID_S16    = 0x01; // Envoi avec adresse en 16 bits
        const byte XBEE_FRAME_ID             = 0x00; // Compteur pour les retours 

        #endregion
        /* Décodeur Api */
        private bool _bApiTrameCompleted = false; // La trame est complette
        private bool _bApiTrameStarted = false; // On à commencer à traiter la trame 
        private int _ApiTrameLenght = 0; // Taille de la Trame API
        private int _ApiState = 0; // Flag Pour l'état du décodeur
        private List<byte> _DataChecksumApi = new List<byte>(); //Données pour le checksum
        private List<byte> _DataTrameApi = new List<byte>(); //Données pour la trame
        //private TrameProtocole _TrameApi; // derniere Trame
        

        private bool _bApiEnabled = true; // Mode API Activé 

        SerialManagment _SerialManagment; // Interface Série

        public XbeeAPI(string SerialName,bool ApiMode)
        {
            _bApiEnabled = ApiMode;
            _SerialManagment = new SerialManagment(SerialName, 9600);
            _SerialManagment.OnNewDataReceived += new SerialManagment.NewDataReceivedEventHandler(_SerialManagment_OnNewDataReceived);
        }

        public void SetSerialConnexion(bool state)
        {
            if (state)
                _SerialManagment.Open();
            else
                _SerialManagment.Close();
        }
        public bool GetSerialState()
        {
            return _SerialManagment.PortSerie.IsOpen;
        }
        public void SetSerialName(string name)
        {
            _SerialManagment.PortSerie.PortName = name;
        }
        /* Appelé lorsque l'on recois des données */
        private void _SerialManagment_OnNewDataReceived(object sender, NewDataReceveidEventArgs args)
        {
            Logger.GlobalLogger.debug("Données Reçus",0);
            List<byte> dataFrame = new List<byte>();
            if (!_bApiEnabled)
            {
                if (args.DataCount < 11) // Nombre Minimum d'octet d'une trame complette 
                    return;
                dataFrame.AddRange(_SerialManagment.getData(_SerialManagment.countData)); // Copie des octets
            }
            else
            {
                if (args.DataCount < (11 + 9)) // Nombre Minimum d'octet d'une trame complette en mode APÏ
                    return;
                while(_SerialManagment.countData > 0 && !parseReceivedApiData(_SerialManagment.getData(1)[0]))
                {}
                dataFrame = _DataTrameApi;
                //extractDataFromApiFrame(_SerialManagment.fetchData());
            }

            // envoi de l'evenement à la couche suppérieure de l'application
            NewTrameReceivedEventArgs e = new NewTrameReceivedEventArgs(dataFrame.ToArray());
            OnNewTrameReceived(this, e);
            _DataTrameApi.Clear();
            
        }

        /* Envoi d'une trame sur le port Série */
        public bool sendApiFrame(ushort dst , byte[] datas)
        {
            bool ret = false;
            if (_bApiEnabled)
                ret = _SerialManagment.SendBytes(buildXbeeApiFrame(dst, datas));
            else
                ret = _SerialManagment.SendBytes(datas);
            return ret;
        }

        /* buildXbeeApiFrame 
         * Construit une trame XbeeApi à partir des données et de la destination
         */
        public byte[] buildXbeeApiFrame(ushort dstLow, byte[] datas)
        {
            ushort lenght = (ushort)(datas.Length + 5); // Datas + API + API_ID + DST *2

            List<byte> ret = new List<byte>();      // Liste finale
            List<byte> payload = new List<byte>();  // données pour calculer le checksum

            /* Construction de la trame */
            ret.Add(XBEE_START_DEL);

            ret.Add((byte)(lenght >> 8));
            ret.Add((byte)(lenght & 0xFF));

            payload.Add(XBEE_FRAME_TYPE_ID_S16);

            payload.Add(XBEE_FRAME_ID);

            payload.Add((byte)(dstLow >> 8));
            payload.Add((byte)(dstLow & 0xFF));

            payload.Add(0x00);

            payload.AddRange(datas);

            ret.AddRange(payload.ToArray()); // Ajout des données dans la liste finale

            ret.Add(computeChecksumXbeeAPI(payload.ToArray())); // Calcul et ajout du CheckSum

            return ret.ToArray(); // Rend la trame complette
        }

        /* computeChecksum 
         * Retroune le Checksum pour la frame XbeeAPI
         */
        public byte computeChecksumXbeeAPI(byte[] datas)
        {
            int somme = datas.Sum(P => P);
            byte checksum = (byte)(Convert.ToByte(0xFF) - Convert.ToByte(somme & 0xFF));
            return checksum;
        }


        /* extractDataFromApiFrame
         * Extrait les données Frames depuis une Frame APIxbee
         */
        /*
        public byte[] extractDataFromApiFrame(byte[] datas)
        {
            int length = 0;
            int i = 0,x = 0;
            List<byte> donnees = new List<byte>();

            while (datas[x] != XBEE_START_DEL) { x++; } // recherche du démarrage

            length = (datas[x++] << 8) + datas[x]++;
            x++; //port.ReadByte(); // Skip API Identifier
            x++; //port.ReadByte(); //  API frame ID
            x++;x++; //port.ReadByte(); port.ReadByte(); //   Destination address low
            x++; //port.ReadByte(); //   Option byte

            while (i < (length - 5)) // Parcours des données
            {
                donnees.Add(datas[x++]);
                i++;
            }
            return donnees.ToArray();
        }
        */

       /* La trame est valide */
        private bool isApiTrameCompleted()
        {
            return _bApiTrameCompleted;
        }

        /* Parse de la donnée API */
        private bool parseReceivedApiData(byte data)
        {
            if (data == XBEE_START_DEL && !_bApiTrameStarted) // Debut de Trame
            {
                _bApiTrameStarted = true;
                _bApiTrameCompleted = false;
                _DataChecksumApi.Clear();
                _ApiState = 1;
            }
            else
            {
                switch (_ApiState)
                {
                    case 1:// Premier Octet de Lenght
                        _ApiTrameLenght = data << 8;
                         _ApiState++; 
                        break;
                    case 2:// Second Octet de Lenght
                        _ApiTrameLenght += data;
                        _ApiState++; 
                        break;
                    case 3: // API TRAME
                        _ApiTrameLenght--;
                        _DataChecksumApi.Add(data);
                        _ApiState++;
                        break;
                    case 4: // Premier Octet de Source
                        _ApiTrameLenght--;
                        _DataChecksumApi.Add(data);
                        _ApiState++;
                        break;
                    case 5: // Second Octet de Source
                        _ApiTrameLenght--;
                        _DataChecksumApi.Add(data);
                        _ApiState++;
                        break;
                    case 6: // RSSI
                        _ApiState++;
                        _DataChecksumApi.Add(data);
                        _ApiTrameLenght--;
                        break;
                    case 7: // Option 2
                        _ApiTrameLenght--;
                        _DataChecksumApi.Add(data);
                        _ApiState++;
                        break;
                    case 8: // Datas
                        _ApiTrameLenght--;
                        _DataChecksumApi.Add(data); // Ajout dans la liste du CRC
                        _DataTrameApi.Add(data); //Ajout dans la liste de la trame
                        if (_ApiTrameLenght == 0)
                            _ApiState++;
                        break;
                    case 9: // Checksum
                        if (computeChecksumXbeeAPI(_DataChecksumApi.ToArray()) == data) // Verification CheckSum
                            Logger.GlobalLogger.debug("CheckSum Api OK !",1);
                   
                        else
                            Logger.GlobalLogger.error("CheckSum Api NOK !");
                        
                        _ApiState = 0; // Fin
                        _bApiTrameStarted = false;
                        _bApiTrameCompleted = true;
                        break;
                    default:
                        Logger.GlobalLogger.error("Error Parsing Api Trame !"); 
                        break;
                }              
            }
            return isApiTrameCompleted();
        }

        public void Dispose()
        {
            SetSerialConnexion(false);
            _SerialManagment.Dispose();
        }
    } 
}
