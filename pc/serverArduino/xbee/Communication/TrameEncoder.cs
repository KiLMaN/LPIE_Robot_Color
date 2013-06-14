using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils;

namespace xbee.Communication
{
    class TrameEncoder
    {
        /* Construit la trame à partir des valeurs données en params */
        public TrameProtocole MakeTrame(byte src, byte dst, ushort num, byte[] data)
        {
            if (data.Length > TrameProtocole.BUFFER_DATA_IN)
                throw new Exception("Taille Maximum dépassée");

            TrameProtocole trame = new TrameProtocole();
            trame.src = src;
            trame.dst = dst;
            trame.num = num;
            trame.length = Convert.ToByte(data.Length);
            trame.data = data;
            trame.crc = MessagesUtils.crc16_protocole(trame);

            //trame.state = 0; // Initialise la trame en non envoyée 
            return trame;
        }

        /* Recuperre les données sous forme d'octets de la trame */
        public byte[] MakeTrameBinaryWithEscape(TrameProtocole trame)
        {
            List<byte> trameBinary = new List<byte>();

            trameBinary.Add((byte)ProtocoleCharEnum.PROTOCOL_START_1);
            trameBinary.Add((byte)ProtocoleCharEnum.PROTOCOL_START_2);

            addToTrameBinary(trameBinary, trame.src);
            addToTrameBinary(trameBinary, trame.dst);

            addToTrameBinary(trameBinary, (byte)(trame.num >> 8));
            addToTrameBinary(trameBinary, (byte)(trame.num & 0xFF));

            addToTrameBinary(trameBinary, trame.length);

            foreach (byte data in trame.data)
                addToTrameBinary(trameBinary, data);

            addToTrameBinary(trameBinary, (byte)(trame.crc >> 8));
            addToTrameBinary(trameBinary, (byte)(trame.crc & 0xFF));

            trameBinary.Add((byte)ProtocoleCharEnum.PROTOCOL_STOP);

            return trameBinary.ToArray();
        }

        /* Ajouter une donnée à la trame binaire (prend en compte l'echapement des données */
        private void addToTrameBinary(List<byte> trameSortie, byte Donnee)
        {
            if (Enum.IsDefined(typeof(ProtocoleCharEnum), Donnee)) // Si la donnée existe dans l'enum
                trameSortie.Add((byte)ProtocoleCharEnum.PROTOCOL_ESCAPE); // on l'échape 
            trameSortie.Add(Donnee); // et on l'ajoute
        }

        /* Encode un message En trame pour envoyer au Arduino */
        public TrameProtocole EncodeTrame(MessageProtocol message,byte src,byte dst,ushort num)
        {
            TrameProtocole trame = new TrameProtocole();
            switch(message.headerMess)
            {
                case (byte)PCtoEMBmessHeads.ASK_PING:
                    trame = MakeTrame(src,dst,num,((PCtoEMBMessagePing)message).getBytes());
                    break;
                case (byte)PCtoEMBmessHeads.ASK_SENSOR:
                    trame = MakeTrame(src, dst, num, ((PCtoEMBMessageAskSensor)message).getBytes());
                    break;
                case (byte)PCtoEMBmessHeads.CLOSE_CLAW:
                    trame = MakeTrame(src, dst, num, ((PCtoEMBMessageCloseClaw)message).getBytes());
                    break;
                case (byte)PCtoEMBmessHeads.OPEN_CLAW:
                    trame = MakeTrame(src, dst, num, ((PCtoEMBMessageOpenClaw)message).getBytes());
                    break;
                case (byte)PCtoEMBmessHeads.MOVE:
                    trame = MakeTrame(src, dst, num, ((PCtoEMBMessageMove)message).getBytes());
                    break;
                case (byte)PCtoEMBmessHeads.TURN:
                    trame = MakeTrame(src, dst, num, ((PCtoEMBMessageTurn)message).getBytes());
                    break;
                case (byte)PCtoEMBmessHeads.RESP_CONN:
                    trame = MakeTrame(src, dst, num, ((PCtoEMBMessageRespConn)message).getBytes());
                    break;
                default:
                    Logger.GlobalLogger.error("Erreur à envoyer inconnu !");
                    break;
            }
            return trame;
        }
    }
}
