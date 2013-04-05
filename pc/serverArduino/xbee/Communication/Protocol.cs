using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using utils;

namespace Communication.Arduino.protocol
{
    /* Structure de la trame */
    public struct TrameProtocole
    {
        public byte src;
        public byte dst;
        public ushort num;
        public byte length;
        public byte[] data;
        public ushort crc;

        public int state;

        // Override the ToString method:
        public override string ToString()
        {
            String datas = "";
            for(int i = 0 ; i < length ; i++)
            {
                datas = String.Format("{0}\t ,{1:X2}", datas, data[i]);
            }
            return (String.Format("(Source :{0}, Destination :{1}, Numéro :{2}, Longeur :{3}, crc :{4:X4} , \r\ndata :{5})", src, dst, num, length, crc, datas));
        }

        public void setSrc(byte src)
        {
            this.src = src;
        }
        public void setDst(byte dst)
        {
            this.dst = dst;
        }
    };


    class Protocol
    {
        private const int BUFFER_DATA_IN = 50; // Nombre d'octets de data que le protocole peut envoyé en meme temps

        private ushort _cpt = 0;
        /** Definition du protocole **/
        enum ProtocoleChar : byte
        {
            /* Octet d'échappement */
            PROTOCOL_ESCAPE = 0xEC,
            /* Octets de start */
            PROTOCOL_START_1 = 0xAF,
            PROTOCOL_START_2 = 0xC9,
            /* Octets de stop */
            PROTOCOL_STOP = 0x8C
        };


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
        private int ProtocolState = 0;

        /* Stockage de la trame temporairement */
        private TrameProtocole m_TrameReceive;

        private int tmp;

        private int CurrentLengthDatas = 0;

        private SerialPort m_Serial = null;
        public SerialPort PortSerie
        {
            get{return m_Serial;}
            set{m_Serial = value;}
        }

        /* Parse la trame en fonction des données reçues et du state */
        private Boolean parseTrame(byte data)
        {
            switch (ProtocolState)
            {
                case 2: // SOURCE (Copie directe)
                    m_TrameReceive.src = data;
                    return true;
                case 3: // DEST (Copie directe)
                    m_TrameReceive.dst = data;
                    return true;
                case 4: // NUMERO DE PACKET (Besoin de deux bytes)
                    CurrentLengthDatas++;
                    if (CurrentLengthDatas == 2) // On a besoin de deux bytes pour la longeur et le numéro 
                    {
                        m_TrameReceive.num = (ushort)(tmp << 8 | data);
                        return true;
                    }
                    else
                    {
                        tmp = data;
                        return false;
                    }
                case 5: // LONGEUR (Copie directe)
                    m_TrameReceive.length = data;
                    if (m_TrameReceive.length > BUFFER_DATA_IN) // On envoi un trame trop longue 
                    {
                        ProtocolState = -1;
                        return false;
                    }
                    m_TrameReceive.data = new byte[m_TrameReceive.length]; // Initialisation du tableau
                    return true;
                case 6: // DATAS (Besoin de m_TrameReceive.length bytes)
                    m_TrameReceive.data[CurrentLengthDatas] = data; // remplis le tableau des données
                    CurrentLengthDatas++;

                    if (CurrentLengthDatas == m_TrameReceive.length)
                    {
                        return true;
                    }
                    else
                        return false;
                case 7: //  CRC RECU (Besoin de deux bytes)
                    CurrentLengthDatas++;
                    if (CurrentLengthDatas == 2) // On a besoin de deux bytes pour le crc 
                    {
                        m_TrameReceive.crc = (ushort)(tmp << 8 | data);
                        return true;
                    }
                    else
                    {
                        tmp = data;
                        return false;
                    }
            }
            return false;
        }


        /* Recupere la trame découpée */
        /* Aucune vérification de CRC ou de destinataire n'est fait */
        /* Pour le CRC : crc16_protocole(Trame); */
        /* ? pour dire que l'on peut renvoyer une valleur null */
        public TrameProtocole getTrame()
        {
            if (PortSerie == null) // Pas initialisé
                return default(TrameProtocole);

            if (PortSerie.BytesToRead == 0) // Si auccune donnée n'est disponible 
                return default(TrameProtocole);

            byte DataSerial;
            Boolean TrameOk = false;

            while (PortSerie.BytesToRead > 0 && !TrameOk)  // Lit les données entrantes du port com
            {
                // Lit un octet du port série
                DataSerial = (byte)PortSerie.ReadByte();

                if (ProtocolState < 100) // Pas d'échapement
                {
                    switch (DataSerial)
                    {
                        /* Premier Flag de start */
                        case (int)ProtocoleChar.PROTOCOL_START_1: // Debut du message

                            if (ProtocolState == 0) // Si on avait pas commencer un nouveau OK
                            {
                                ProtocolState = 1;
                            }
                            else                   // Sinon Erreur dans le protocol mais on prend quand même en compte la nouvelle trame
                            {
                                ProtocolState = 1;
                            }
                            break;

                        /* Second Flag de start */
                        case (int)ProtocoleChar.PROTOCOL_START_2:
                            if (ProtocolState == 1) // Suit bien le premier flag, tout vas bien 
                            {
                                ProtocolState = 2;
                            }
                            else
                            {
                                ProtocolState = -1;
                            }
                            break;

                        /* Flag de fin */
                        case (int)ProtocoleChar.PROTOCOL_STOP:
                            if (ProtocolState == 8)
                            {
                                ProtocolState = 0;
                                TrameOk = true;
                            }
                            else
                            {
                                ProtocolState = -1;
                            }
                            break;

                        /* Si c'est un escape Alors on ne prends pas en compte le prochain byte comme un flag */
                        case (int)ProtocoleChar.PROTOCOL_ESCAPE:
                            ProtocolState += 100;// On ajoute l'échapement
                            break;


                        /* Ce n'est pas un flag */
                        default:
                            if (parseTrame(DataSerial)) // Si le parseTrame indique de passer a l'état suivant
                            {
                                CurrentLengthDatas = 0;
                                ProtocolState++;
                            }
                            break;
                    }
                }
                else // Echapé !
                {
                    ProtocolState -= 100; // On enlève l'échapement
                    if (parseTrame(DataSerial)) // Si le parseTrame indique de passer a l'état suivant
                    {
                        CurrentLengthDatas = 0;
                        ProtocolState++;
                    }
                }

                if (ProtocolState == -1) // Erreur dans l'ordre des Données
                {
                    Logger.GlobalLogger.error("Protocol ERROR");
                }

                if (TrameOk)
                {
                    m_TrameReceive.state = 0; // Non traitée
                    return m_TrameReceive;
                    
                }
                else
                    return default(TrameProtocole);
            }
            return default(TrameProtocole);
        }

        public TrameProtocole MakeTrame(byte src, byte dst, byte[] data)
        {
            if (data.Length > BUFFER_DATA_IN)
                return default(TrameProtocole);

            TrameProtocole trame = new TrameProtocole();
            trame.src = src;
            trame.dst = dst;
            trame.num = _cpt++;
            trame.length = Convert.ToByte(data.Length);
            trame.data = data;
            trame.crc = crc16_protocole(trame);
            return trame;
        }

        private byte[] MakeTrameBinary(TrameProtocole trame)
        {
            List<byte> trameBinary = new List<byte>();

            trameBinary.Add((byte)ProtocoleChar.PROTOCOL_START_1);
            trameBinary.Add((byte)ProtocoleChar.PROTOCOL_START_2);

            addToTrameBinary(trameBinary, trame.src);
            addToTrameBinary(trameBinary, trame.dst);

            addToTrameBinary(trameBinary, (byte)(trame.num >> 8));
            addToTrameBinary(trameBinary, (byte)(trame.num & 0xFF));

            addToTrameBinary(trameBinary, trame.length);

            foreach (byte data in trame.data)
                addToTrameBinary(trameBinary, data);

            addToTrameBinary(trameBinary, (byte)(trame.crc >> 8));
            addToTrameBinary(trameBinary, (byte)(trame.crc & 0xFF));

            trameBinary.Add((byte)ProtocoleChar.PROTOCOL_STOP);

            return trameBinary.ToArray();
        }

        private void addToTrameBinary(List<byte> trameSortie, byte Donnee)
        {
            if (Enum.IsDefined(typeof(ProtocoleChar), Donnee)) // Si la donnée existe dans l'enum
                trameSortie.Add((byte)ProtocoleChar.PROTOCOL_ESCAPE); // on l'échape 
            trameSortie.Add(Donnee); // et on l'ajoute
        }

        public void SendTrame(TrameProtocole trame,bool XbeeAPI)
        {
            if (PortSerie == null)
                return;
            if (!PortSerie.IsOpen)
                return;
            byte[] Bin = MakeTrameBinary(trame);
            Logger.GlobalLogger.debug(String.Format("{0:X}",Bin));

            if (!XbeeAPI)
                SendBytes(Bin);
            else
            {
                byte[] datas= Communication.Arduino.Xbee.XbeeAPI.buildApiFrame(0x5001,new byte[]{0x48,0x65,0x6C,0x6C,0x6F});
                SendBytes(datas);
            }

        }
        public void SendBytes(byte[] datas)
        {
            PortSerie.Write(datas, 0, datas.Length);
        }

        /* Satics */
        public static byte[] getBytes(TrameProtocole trame)
        {
            byte[] retVal = new byte[trame.length + 5];
            /*public byte src;
           public byte dst;
           public ushort num;
           public byte length;
           public byte[] data;*/
            int index = 0;
            retVal[index++] = trame.src;
            retVal[index++] = trame.dst;
            retVal[index++] = (byte)(trame.num >> 8);
            retVal[index++] = (byte)(trame.num & 0xFF);
            retVal[index++] = trame.length;

            foreach (byte data in trame.data)
                retVal[index++] = data;

            return retVal;
        }

        public static ushort crc16_protocole(TrameProtocole trame)
        {
            return crc16.calc_crc16(Protocol.getBytes(trame), trame.length + 5);
        }


    }
}
