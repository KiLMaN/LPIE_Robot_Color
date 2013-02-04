using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugProtocolArduino
{
    class Messages
    {
        #region #### Enumeration Messages Headers ####
        enum PCtoEMBmess : byte
        {
            TURN =          0x51,
            MOVE =          0x52,
            CLOSE_CLAW =    0x61,
            OPEN_CLAW =     0x62,
            ASK_SENSOR =    0x31,
            ASK_PING =      0x21,
            RESP_CONN =     0x11
        };

        enum EMBtoPCmess : byte
        {
            ASK_CONN =      0x12,
            RESP_PING =     0x22,
            RESP_SENSOR =   0x32,
            ACK =           0x41
        };
        #endregion

        #region #### PCtoEMB Structures ####
        /* Connection Managment */
        public struct PCtoEMBMessageRespConn
        {
            public byte headerMess;
            public byte state;
        };
        public struct PCtoEMBMessagePing
        {
            public byte headerMess;
        };

        /* Deplacement Managment */
        public struct PCtoEMBMessageTurn
        {
            public byte headerMess;
            public byte direction;
            public byte angle;
        };
        public struct PCtoEMBMessageMove
        {
            public byte headerMess;
            public byte sens;
            public byte speed;
            public byte distance;
        };

        /* Claw Managment */
        public struct PCtoEMBMessageOpenClaw
        {
            public byte headerMess;
        };
        public struct PCtoEMBMessageCloseClaw
        {
            public byte headerMess;
        };

        /* Sensor Managment */
        public struct PCtoEMBMessageAskSensor
        {
            public byte headerMess;
            public byte idSensor;
        };

        #endregion

        #region #### EMBtoPC Structures ####
        /* Connection Managment */
        public struct EMBtoPCMessageAskConn
        {
            public byte headerMess;
        };
        public struct EMBtoPCMessageRespPing
        {
            public byte headerMess;
        };

        /* Sensor Managment */
        public struct EMBtoPCMessageRespSensor
        {
            public byte headerMess;
            public byte idSensor;
            public byte valueSensor;
        };

        /* Global Ack */
        public struct EMBtoPCMessageGlobalAck
        {
            public byte headerMess;
            public byte idCommand;
            public byte valueAck;
        };
        #endregion
    }
}
