using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace DebugProtocolArduino
{
    class Messages
    {
        #region #### Globales ####
        public enum IDSensorsArduino : byte
        {
            IR = 0x01,
            UltraSon = 0x02
        };
        #endregion

        #region #### Enumeration Messages Headers ####
       public enum PCtoEMBmess : byte
        {
            TURN =          0x51,
            MOVE =          0x52,
            CLOSE_CLAW =    0x61,
            OPEN_CLAW =     0x62,
            ASK_SENSOR =    0x31,
            ASK_PING =      0x21,
            RESP_CONN =     0x11
        };

        public enum EMBtoPCmess : byte
        {
            ASK_CONN =      0x12,
            RESP_PING =     0x22,
            RESP_SENSOR =   0x32,
            ACK =           0x41
        };
        #endregion

        #region #### PCtoEMB Structures ####
        /* Connection Managment */
        [Serializable]
        public struct PCtoEMBMessageRespConn
        {
            public PCtoEMBmess headerMess;
            public byte state;
        };
        [Serializable]
        public struct PCtoEMBMessagePing
        {
            public PCtoEMBmess headerMess;
        };

        /* Deplacement Managment */
        [Serializable]
        public struct PCtoEMBMessageTurn
        {
            public PCtoEMBmess headerMess;
            public byte direction;
            public byte angle;
        };
        [Serializable]
        public struct PCtoEMBMessageMove
        {
            public PCtoEMBmess headerMess;
            public byte sens;
            public byte speed;
            public byte distance;
        };

        /* Claw Managment */
        [Serializable]
        public struct PCtoEMBMessageOpenClaw
        {
            public PCtoEMBmess headerMess;
        };
        [Serializable]
        public struct PCtoEMBMessageCloseClaw
        {
            public PCtoEMBmess headerMess;
        };

        /* Sensor Managment */
        [Serializable]
        public struct PCtoEMBMessageAskSensor
        {
            public PCtoEMBmess headerMess;
            public byte idSensor;
        };

        #endregion

        #region #### EMBtoPC Structures ####
        /* Connection Managment */
        [Serializable]
        public struct EMBtoPCMessageAskConn
        {
            public EMBtoPCmess headerMess;
        };
        [Serializable]
        public struct EMBtoPCMessageRespPing
        {
            public EMBtoPCmess headerMess;
        };

        /* Sensor Managment */
        [Serializable]
        public struct EMBtoPCMessageRespSensor
        {
            public EMBtoPCmess headerMess;
            public byte idSensor;
            public byte valueSensor;
        };

        /* Global Ack */
        [Serializable]
        public struct EMBtoPCMessageGlobalAck
        {
            public EMBtoPCmess headerMess;
            public byte idCommand;
            public byte valueAck;
        };
        #endregion

        /* Retourne un tableau de bytes a partir d'une structure sérialisable */
        private static byte[] getBytes(object strucutre)
        {
            // Create a memory stream, and serialize.
            using (MemoryStream stream = new MemoryStream())
            {
                // Create a binary formatter.
                IFormatter formatter = new BinaryFormatter();

                // Serialize.
                formatter.Serialize(stream, strucutre);

                // Now return the array.
                return stream.ToArray();
            }
        }
    }
}
