using System;
using utils;

namespace xbee.Communication
{
    class TrameDecoder
    {
        /* ProtocolState :
         0 -> Recherche du premier octet de start 
         1 -> Premier trouvé -> Recherche du second octet se start 
         2 -> Second trouvé  -> 1 octet de source 
         3 -> Source passée  -> 1 octet de destination
         4 -> Dest passée    -> 1 octet de numéro
         5 -> Numéro Ok      -> 1 octet de longeur
         6 -> Longeur OK     -> n octets de données
         7 -> Données OK     -> X octets de CRC
         8 -> CRC recu       -> Recherche de l'octet de stop
         9 -> Stop recu     -> Tramme recu :)
 
         Si > 100 alors le prochain byte est échapé (si c'est un flag, il deviens une data)
         */
        private int _protocolState = 0;

        private bool _bTrameCompleted = false;
        public bool bIsCompleted
        {
            get { return _bTrameCompleted; }
        }
        /* Stockage de la trame temporairement */
        private TrameProtocole _trameReceived;

        private int _tmp;

        private int _CurrentLengthDatas = 0;



        /* Remplis la structure */
        private Boolean _parseData(byte data)
        {
            switch (_protocolState)
            {
                case 2: // SOURCE (Copie directe)
                    _trameReceived.src = data;
                    return true;
                case 3: // DEST (Copie directe)
                    _trameReceived.dst = data;
                    return true;
                case 4: // NUMERO DE PACKET (Besoin de deux bytes)
                    _CurrentLengthDatas++;
                    if (_CurrentLengthDatas == 2) // On a besoin de deux bytes pour la longeur et le numéro 
                    {
                        _trameReceived.num = (ushort)(_tmp << 8 | data);
                        return true;
                    }
                    else
                    {
                        _tmp = data;
                        return false;
                    }
                case 5: // LONGEUR (Copie directe)
                    _trameReceived.length = data;
                    if (_trameReceived.length > TrameProtocole.BUFFER_DATA_IN) // On envoi un trame trop longue 
                    {
                        _protocolState = -1;
                        return false;
                    }
                    _trameReceived.data = new byte[_trameReceived.length]; // Initialisation du tableau
                    return true;
                case 6: // DATAS (Besoin de m_TrameReceive.length bytes)
                    _trameReceived.data[_CurrentLengthDatas] = data; // remplis le tableau des données
                    _CurrentLengthDatas++;

                    if (_CurrentLengthDatas == _trameReceived.length)
                    {
                        return true;
                    }
                    else
                        return false;
                case 7: //  CRC RECU (Besoin de deux bytes)
                    _CurrentLengthDatas++;
                    if (_CurrentLengthDatas == 2) // On a besoin de deux bytes pour le crc 
                    {
                        _trameReceived.crc = (ushort)(_tmp << 8 | data);
                        return true;
                    }
                    else
                    {
                        _tmp = data;
                        return false;
                    }
            }
            return false;
        }

        /* Parse les données en fonction du state du décodeur */
        /* Retourne true si la trame est complette */
        /* Le CRC n'est pas vérifié */
        public bool parseIncomingData(byte data)
        {
           /* if (data == null) // Pas initialisé
                throw new ArgumentNullException();*/

            if (_protocolState < 100) // Pas d'échapement
            {
                switch (data)
                {
                    /* Premier Flag de start */
                    case (int)ProtocoleCharEnum.PROTOCOL_START_1: // Debut du message
                        _bTrameCompleted = false;
                        if (_protocolState == 0) // Si on avait pas commencer un nouveau OK
                        {
                            _protocolState = 1;
                        }
                        else                   // Sinon Erreur dans le protocol mais on prend quand même en compte la nouvelle trame
                        {
                            _protocolState = 1;
                        }
                        break;

                    /* Second Flag de start */
                    case (int)ProtocoleCharEnum.PROTOCOL_START_2:
                        if (_protocolState == 1) // Suit bien le premier flag, tout vas bien 
                        {
                            _protocolState = 2;
                        }
                        else
                        {
                            _protocolState = -1;
                        }
                        break;

                    /* Flag de fin */
                    case (int)ProtocoleCharEnum.PROTOCOL_STOP:
                        if (_protocolState == 8)
                        {
                            _protocolState = 0;
                            //_trameReceived.state = 0;
                            _bTrameCompleted = true;
                        }
                        else
                        {
                            _protocolState = -1;
                        }
                        break;

                    /* Si c'est un escape Alors on ne prends pas en compte le prochain byte comme un flag */
                    case (int)ProtocoleCharEnum.PROTOCOL_ESCAPE: 
                        _protocolState += 100;// On ajoute l'échapement
                        break;


                    /* Ce n'est pas un flag */
                    default:
                        if (_parseData(data)) // Si le _parseData indique de passer a l'état suivant
                        {
                            _CurrentLengthDatas = 0;
                            _protocolState++;
                        }
                        break;
                }
            }
            else // Echapé !
            {
                _protocolState -= 100; // On enlève l'échapement
                if (_parseData(data)) // Si le parseTrame indique de passer a l'état suivant
                {
                    _CurrentLengthDatas = 0;
                    _protocolState++;
                }
            }

            if (_protocolState == -1) // Erreur dans l'ordre des Données
            {
                Logger.GlobalLogger.error("Protocol ERROR");
            }

            
            return _bTrameCompleted;
        }

        public TrameProtocole getDecodedTrame()
        {
            if (_bTrameCompleted)
            {
                TrameProtocole Trame = _trameReceived;
                _protocolState = 0;
                _bTrameCompleted = false;
                _trameReceived = new TrameProtocole();
                return Trame;
            }
            else
                throw new NotSupportedException("Trame pas complete !");
        }

        /* decode une trame en Message */
        public MessageProtocol DecodeTrame(TrameProtocole trame)
        {
            MessageProtocol message = new MessageProtocol();
            switch (trame.data[0]) // Header
            {
                case (byte)EMBtoPCmessHeads.ACK:
                    message = new EMBtoPCMessageGlobalAck();
                    message.headerMess = trame.data[0];
                    ((EMBtoPCMessageGlobalAck)message).idTrame = (ushort)(trame.data[1] << 8);
                    ((EMBtoPCMessageGlobalAck)message).idTrame += trame.data[2];
                    ((EMBtoPCMessageGlobalAck)message).valueAck = trame.data[3];
                    break;
                case (byte)EMBtoPCmessHeads.ASK_CONN:
                    message = new EMBtoPCMessageAskConn();
                    message.headerMess = trame.data[0];
                    break;
                case (byte)EMBtoPCmessHeads.RESP_PING:
                     message = new EMBtoPCMessageRespPing();
                    message.headerMess = trame.data[0];
                    break;
                case (byte)EMBtoPCmessHeads.RESP_SENSOR:
                     message = new EMBtoPCMessageRespSensor();
                    message.headerMess = trame.data[0];
                    ((EMBtoPCMessageRespSensor)message).idSensor = trame.data[1];
                    ((EMBtoPCMessageRespSensor)message).valueSensor = trame.data[2];
                    break;
                default:
                    Logger.GlobalLogger.error("Erreur Message recu inconnu !");
                    break;
            }
            return message;
        }
    }
}
