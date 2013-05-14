﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace xbee.Communication
{
    public class MessageBuilder
    {
        static private byte _src = 0xFE;
        static public byte src
        {
            get { return _src; }
            set { _src = src; }
        }

        static public MessageProtocol createMoveMessage(bool Sens,byte vitesse,byte distance)
        {
            PCtoEMBMessageMove Message = new PCtoEMBMessageMove();
            Message.headerMess = (byte)PCtoEMBmessHeads.MOVE;
            Message.sens = (byte)((Sens) ? 0x01 : 0x00);
            Message.speed = vitesse;
            Message.distance = distance;

            return Message;
        }

        static public MessageProtocol createTurnMessage(bool Sens, byte angle)
        {
            PCtoEMBMessageTurn Message = new PCtoEMBMessageTurn();
            Message.headerMess = (byte)PCtoEMBmessHeads.TURN;
            Message.direction = (byte)((Sens) ? 0x01 : 0x00);
            Message.angle = angle;

            return Message;
        }

        static public MessageProtocol createCloseClawMessage()
        {
            PCtoEMBMessageCloseClaw Message = new PCtoEMBMessageCloseClaw();
            Message.headerMess = (byte)PCtoEMBmessHeads.CLOSE_CLAW;

            return Message;
        }
        static public MessageProtocol createOpenClawMessage()
        {
            PCtoEMBMessageOpenClaw Message = new PCtoEMBMessageOpenClaw();
            Message.headerMess = (byte)PCtoEMBmessHeads.OPEN_CLAW;

            return Message;
        }

        static public MessageProtocol createAskPingMessage()
        {
            PCtoEMBMessagePing Message = new PCtoEMBMessagePing();
            Message.headerMess = (byte)PCtoEMBmessHeads.ASK_PING;

            return Message;
        }

        static public MessageProtocol createRespConnMessage(byte state)
        {
            PCtoEMBMessageRespConn Message = new PCtoEMBMessageRespConn();
            Message.headerMess = (byte)PCtoEMBmessHeads.RESP_CONN;
            Message.state = state;

            return Message;
        }

        static public MessageProtocol createAskSensorMessage(byte idSensor)
        {
            PCtoEMBMessageAskSensor Message = new PCtoEMBMessageAskSensor();
            Message.headerMess = (byte)PCtoEMBmessHeads.ASK_SENSOR;
            Message.idSensor = idSensor;

            return Message;
        }
    }
}