using Communication.Arduino.messages;
using Communication.Arduino.protocol;
using utils;

namespace Communication.Arduino
{
    class messageBuilder
    {
        private Protocol _Protocol;
        private byte _src = 0;
        private byte _dst = 0;

        public messageBuilder()
        {
            _Protocol = new Protocol() ;
        }


        private TrameProtocole createMessage(byte[] data)
        {
            TrameProtocole pTrame = _Protocol.MakeTrame(_src, _dst, data);
            pTrame.state = 0; // Non envoyé
            Logger.GlobalLogger.debug(pTrame.ToString());
            return pTrame;
        }

        public TrameProtocole createMoveMessage(bool Sens,byte vitesse,byte distance)
        {
            PCtoEMBMessageMove Message = new PCtoEMBMessageMove();
            Message.headerMess = (byte)PCtoEMBmessHeads.MOVE;
            Message.sens = (byte)((Sens) ? 0x01 : 0x00);
            Message.speed = vitesse;
            Message.distance = distance;

            byte[] data = Message.getBytes();
            return createMessage(data);

        }

        public TrameProtocole createTurnMessage(bool Sens, byte angle)
        {
            PCtoEMBMessageTurn Message = new PCtoEMBMessageTurn();
            Message.headerMess = (byte)PCtoEMBmessHeads.TURN;
            Message.direction = (byte)((Sens) ? 0x01 : 0x00);
            Message.angle = angle;

            byte[] data = Message.getBytes();
            return createMessage(data);
        }

        public TrameProtocole createCloseClawMessage()
        {
            PCtoEMBMessageCloseClaw Message = new PCtoEMBMessageCloseClaw();
            Message.headerMess = (byte)PCtoEMBmessHeads.CLOSE_CLAW;

            byte[] data = Message.getBytes();
            return createMessage(data);
        }
        public TrameProtocole createOpenClawMessage()
        {
            PCtoEMBMessageOpenClaw Message = new PCtoEMBMessageOpenClaw();
            Message.headerMess = (byte)PCtoEMBmessHeads.OPEN_CLAW;

            byte[] data = Message.getBytes();
            return createMessage(data);
        }

        public TrameProtocole createAskPingMessage()
        {
            PCtoEMBMessagePing Message = new PCtoEMBMessagePing();
            Message.headerMess = (byte)PCtoEMBmessHeads.ASK_PING;

            byte[] data = Message.getBytes();
            return createMessage(data);
        }

        public TrameProtocole createRespConnMessage(byte state)
        {
            PCtoEMBMessageRespConn Message = new PCtoEMBMessageRespConn();
            Message.headerMess = (byte)PCtoEMBmessHeads.RESP_CONN;
            Message.state = state;

            byte[] data = Message.getBytes();
            return createMessage(data);
        }

        public TrameProtocole createAskSensorMessage(byte idSensor)
        {
            PCtoEMBMessageAskSensor Message = new PCtoEMBMessageAskSensor();
            Message.headerMess = (byte)PCtoEMBmessHeads.ASK_SENSOR;
            Message.idSensor = idSensor;

            byte[] data = Message.getBytes();
            return createMessage(data);
        }
    }
}
