using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils;

namespace xbee.Communication
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


        public const int BUFFER_DATA_IN = 50; // Nombre d'octets de data que le protocole peut envoyé en meme temps
        


        // Override the ToString method:
        public override string ToString()
        {
            String datas = "";
            for (int i = 0; i < length; i++)
            {
                datas = String.Format("{0} , {1:X2}", datas, data[i]);
            }
            return (String.Format("(Source :{0}, Destination :{1}, Numéro :{2}, Longeur :{3}, crc :{4:X4} , data :{5})", src, dst, num, length, crc, datas));
        }

        /*public void setSrc(byte src)
        {
            this.src = src;
        }
        public void setDst(byte dst)
        {
            this.dst = dst;
        }*/

        
    };
    /** Definition du protocole **/
    public enum ProtocoleCharEnum : byte
    {
        /* Octet d'échappement */
        PROTOCOL_ESCAPE = 0xEC,
        /* Octets de start */
        PROTOCOL_START_1 = 0xAF,
        PROTOCOL_START_2 = 0xC9,
        /* Octets de stop */
        PROTOCOL_STOP = 0x8C
    };


    /*class MessagesCommunication
    {*/

    #region #### Globales Robots ####
    public enum IDSensorsArduino : byte
    {
        IR = 0x01,
        UltraSon = 0x02
    };
    #endregion

    #region #### Enumeration Messages Headers ####
    public enum PCtoEMBmessHeads : byte
    {
        TURN = 0x51,
        MOVE = 0x52,
        CLOSE_CLAW = 0x61,
        OPEN_CLAW = 0x62,
        ASK_SENSOR = 0x31,
        ASK_PING = 0x21,
        RESP_CONN = 0x11,
        AUTO_MODE = 0x71,
    };
    public enum EMBtoPCmessHeads : byte
    {
        ASK_CONN = 0x12,
        RESP_PING = 0x22,
        RESP_SENSOR = 0x32,
        ACK = 0x41, 
        AUTO_MODE_OFF = 0x72,

    };
    #endregion

    #region #### Global Objects ####
    /** Message de Base :
        *  Contient un Header et une fonction getBytes() qui retourne la liste des bytes de la classe
        **/

    public class MessageProtocol
    {
        public int stateMessage; // Etat du message (en attente : 0, envoyé : 1)
        public DateTime time; // date de reception ou d'envoi 
        public int countRejeu; // Nombre de rejeu du message 

        public byte headerMess; // header du message

        public ushort numMessEnvoye = 0; // Numéro de trame attribuée lors de l'envoi

        // Recuperation des valeurs a envoyer
        public virtual byte[] getBytes()
        {
            return new byte[] { this.headerMess };
        }


        #region #### Predicate ####
        // Recuperation des Message A Envoyer
        public static Predicate<MessageProtocol> MessageAEnvoyer()
        {
            return delegate(MessageProtocol o)
            {
                return o.stateMessage == 0;
            };
        }
        // Recuperation des Message Envoyés en attente des ACK
        public static Predicate<MessageProtocol> MessageAttenteAck()
        {
            return delegate(MessageProtocol o)
            {
                return o.stateMessage == 1;
            };
        }
        /*public static Predicate<MessageProtocol> TrameByNum(ushort num)
        {
            return delegate(MessageProtocol o)
            {
                return num == o.num;
            };
        }*/
        #endregion
    }

    public abstract class PCtoEMBmess : MessageProtocol
    {
    }
    public abstract class EMBtoPCmess : MessageProtocol
    {
    }
    #endregion

    #region #### PCtoEMB Structures ####
    /* Message En provenance du serveur vers les Arduinos */


    /* Connection Managment */
    public class PCtoEMBMessageRespConn : PCtoEMBmess
    {
        public byte state = 0;
        public override byte[] getBytes()
        {
            byte[] data = { this.state };
            return base.getBytes().Concat(data).ToArray();
        }
    }
    public class PCtoEMBMessagePing : PCtoEMBmess
    {

    }

    /* Deplacement Managment */
    public class PCtoEMBMessageTurn : PCtoEMBmess
    {
        public byte direction;
        public byte angle;
        public override byte[] getBytes()
        {
            byte[] data = { this.direction, this.angle };
            return base.getBytes().Concat(data).ToArray();
        }
    }
    public class PCtoEMBMessageMove : PCtoEMBmess
    {
        public byte sens;
        public byte speed;
        public byte distance;
        public override byte[] getBytes()
        {
            byte[] data = { this.sens, this.speed, this.distance };
            return base.getBytes().Concat(data).ToArray();
        }
    }

    /* Claw Managment */
    public class PCtoEMBMessageOpenClaw : PCtoEMBmess
    {
    }
    public class PCtoEMBMessageCloseClaw : PCtoEMBmess
    {
    }

    /* Sensor Managment */
    public class PCtoEMBMessageAskSensor : PCtoEMBmess
    {
        public byte idSensor = 0;
        public override byte[] getBytes()
        {
            byte[] data = { this.idSensor };
            return base.getBytes().Concat(data).ToArray();
        }
    }
    /* Mode Autonome */
    public class PCtoEMBMessageAutoMode : PCtoEMBmess
    {
    }


    #endregion

    #region #### EMBtoPC Structures ####
    /* Message en provenance des Arduino à destination du PC */

    /* Connection Managment */
    public class EMBtoPCMessageAskConn : EMBtoPCmess
    {
        public static explicit operator EMBtoPCMessageAskConn(byte[] data) // cast Explicite
        {
            EMBtoPCMessageAskConn ret = new EMBtoPCMessageAskConn();
            ret.headerMess = data[0];
            return ret;
        }
    }
    public class EMBtoPCMessageRespPing : EMBtoPCmess
    {
        public static explicit operator EMBtoPCMessageRespPing(byte[] data) // cast Explicite
        {
            EMBtoPCMessageRespPing ret = new EMBtoPCMessageRespPing();
            ret.headerMess = data[0];
            return ret;
        }
    }

    /* Sensor Managment */
    public class EMBtoPCMessageRespSensor : EMBtoPCmess
    {
        public byte idSensor = 0;
        public byte valueSensor = 0;
        public override byte[] getBytes()
        {
            byte[] data = { this.idSensor, this.valueSensor };
            return base.getBytes().Concat(data).ToArray();
        }
        public static explicit operator EMBtoPCMessageRespSensor(byte[] data) // cast Explicite
        {
            EMBtoPCMessageRespSensor ret = new EMBtoPCMessageRespSensor();
            ret.headerMess = data[0];
            ret.idSensor = data[1];
            ret.valueSensor = data[2];
            return ret;
        }
    }

    /* Global Ack */
    public class EMBtoPCMessageGlobalAck : EMBtoPCmess
    {
        public ushort idTrame = 0;
        public byte valueAck = 0;
        public override byte[] getBytes()
        {
            byte[] data = { (byte)(this.idTrame >> 8), (byte)(this.idTrame | 0xFF), this.valueAck };
            return base.getBytes().Concat(data).ToArray();
        }
        public static explicit operator EMBtoPCMessageGlobalAck(byte[] data) // cast Explicite
        {
            EMBtoPCMessageGlobalAck ret = new EMBtoPCMessageGlobalAck();
            ret.headerMess = data[0];
            ret.idTrame = (ushort)(data[1] << 8);
            ret.idTrame += (ushort)data[2];
            ret.valueAck = data[3];
            return ret;
        }
    }

    public class EMBtoPCMessageAutoModeOff : EMBtoPCmess
    {
        public static explicit operator EMBtoPCMessageAutoModeOff(byte[] data) // cast Explicite
        {
            EMBtoPCMessageAutoModeOff ret = new EMBtoPCMessageAutoModeOff();
            ret.headerMess = data[0];
            return ret;
        }
    }
    #endregion

    class MessagesUtils
    {
        /* Recuperre les Bytes */
        public static byte[] getBytes(TrameProtocole trame)
        {
            try
            {
                byte[] retVal = new byte[trame.length + 10];
                int index = 0;
                retVal[index++] = (byte)ProtocoleCharEnum.PROTOCOL_START_1;
                retVal[index++] = (byte)ProtocoleCharEnum.PROTOCOL_START_2;
                retVal[index++] = trame.src;
                retVal[index++] = trame.dst;
                retVal[index++] = (byte)(trame.num >> 8);
                retVal[index++] = (byte)(trame.num & 0xFF);
                retVal[index++] = trame.length;

                foreach (byte data in trame.data)
                    retVal[index++] = data;

                retVal[index++] = (byte)(trame.crc >> 8);
                retVal[index++] = (byte)(trame.crc & 0xFF);
                retVal[index++] = (byte)ProtocoleCharEnum.PROTOCOL_STOP;

                return retVal;
            }
            catch (Exception e)
            {
                Logger.GlobalLogger.error(e.ToString());
                return new byte[] { 0 };
            }
        }
        /* Recuperes les bytes pour le calcul du CRC */
        private static byte[] getBytesCrc(TrameProtocole trame)
        {
            try
            {
                byte[] retVal = new byte[trame.length + 5];
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
            catch (Exception e)
            {
                Logger.GlobalLogger.error(e.ToString());
                return new byte[] { 0 };
            }
        }
        /* Retourne la valeur du CRC-16 de la trame */
        public static ushort crc16_protocole(TrameProtocole trame)
        {
            byte[] d = getBytesCrc(trame);
            return crc16.calc_crc16(d, d.Length);
        }
    }
}
