using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Communication.Arduino.Xbee
{
    class XbeeAPI
    {
        /*
            7E
            Start delimiter
            00 0A
            Length bytes
            01
            API identifier
            01
            API frame ID
            50 01
            Destination address low
            00
            Option byte
            48 65 6C 6C 6F
            Data Packet
            B8
            Checksum
         */
        static byte START_DEL = 0x7E;
        static byte FRAME_TYPE_ID = 0x01; // Envoi avec adresse en 16 bits
        static byte FRAME_ID = 0x00; // Compteur pour les retours 

        static public byte[] buildApiFrame(ushort dstLow,byte[] datas)
        {
            ushort lenght = (ushort)(datas.Length + 5); // Datas + API + API_ID + DST *2
            List<byte> ret = new List<byte>() ;
            List<byte> payload = new List<byte>();
            ret.Add(START_DEL);

            ret.Add((byte)(lenght >> 8));
            ret.Add((byte)(lenght & 0xFF));

            payload.Add(FRAME_TYPE_ID);

            payload.Add(FRAME_ID);

            payload.Add((byte)(dstLow >> 8));
            payload.Add((byte)(dstLow & 0xFF));

            payload.Add(0x00);

            payload.AddRange(datas);

            ret.AddRange(payload.ToArray());
            ret.Add(computeChecksum(payload.ToArray()));
            return ret.ToArray();
        }
        static public byte computeChecksum(byte[] datas)
        {
            int somme = datas.Sum(P => P);
            byte ret = (byte)(Convert.ToByte(0xFF) - Convert.ToByte(somme & 0xFF));
            return ret;
        }

        static public byte[] extractDataFromApiFrame(SerialPort port)
        {
            int length = 0;
            int i = 0;
            List<byte> donnees = new List<byte>();

            while (port.ReadByte() != START_DEL){} // recherche du démarrage

            length = (port.ReadByte() << 8) + port.ReadByte();
            port.ReadByte(); // Skip API Identifier
            port.ReadByte(); //  API frame ID
            port.ReadByte(); port.ReadByte(); //   Destination address low
            port.ReadByte(); //   Option byte

            while (i < (length - 5)) // Parcours des données
            {
                donnees.Add((byte)port.ReadByte());
                i++;
            }
            return donnees.ToArray();

        }
    }
}
