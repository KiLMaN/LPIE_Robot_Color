using System;
using Communication.Arduino.messages;
using Communication.Arduino.protocol;
using utils;


namespace ProtocolArduino
{
    enum StateBot : byte
    {
        STATE_NONE                  = 0x00,

        STATE_WATTING               = 0x01,

        STATE_MOVING_TO_TARGET      = 0x02,
        STATE_MOVING_TO_BASE        = 0x03,

        STATE_LIFTING_TARGET        = 0x04,
        STATE_LAYING_TARGET         = 0x05,

        STATE_WAIT_ACK              = 0x10
        
    }
    
    class ArduinoBots
    {
        /* Permet de retrouver le robot dans les listes */
        public static Predicate<ArduinoBots> ById(byte id)
        {
            return delegate(ArduinoBots o)
            {
                return o._id == id;
            };
        }

        private byte _id;
        private bool _connected;
        private DateTime _dateLastMessage;
        private StateBot _stateBot;
        

        public byte id
        {
            get{return _id;}
            set{this._id = value;}
        }
        public DateTime dateLastMessage
        {
            get{return _dateLastMessage;}
            set{this._dateLastMessage = value;}
        }
        /*
         * Couleur Assignée 
         */


        public ArduinoBots(byte v_id)
        {
            Logger.GlobalLogger.info("Create new ArduinoBots id:" + v_id);
            _stateBot = StateBot.STATE_NONE;
        }
        public void Disconnected()
        {
            _stateBot = StateBot.STATE_NONE;
        }
        public void traiterNouvelleTrameRecu(TrameProtocole trame)
        {
            if(trame.data.Length != trame.length)
            {
                Logger.GlobalLogger.error("traiterNouvelleTrameRecu :  trame.data.Length != trame.length");
                return;
            }

            if (trame.length >= 1 && trame.data.Length == trame.length)
            {
                EMBtoPCmessHeads header = (EMBtoPCmessHeads)trame.data[0]; // Recuperation de l'entete du message
                Logger.GlobalLogger.info("HEADER : " + header.ToString());
                switch (header)
                {
                    case EMBtoPCmessHeads.ASK_CONN:
                        TraiterMessageASK_CONN((EMBtoPCMessageAskConn)trame.data);
                        break;
                    case EMBtoPCmessHeads.ACK:
                        TraiterMessageACK((EMBtoPCMessageGlobalAck)trame.data);
                        break;
                    case EMBtoPCmessHeads.RESP_PING:
                        TraiterMessageRESP_PING((EMBtoPCMessageRespPing)trame.data);
                        break;
                    case EMBtoPCmessHeads.RESP_SENSOR:
                        TraiterMessageRESP_SENSOR((EMBtoPCMessageRespSensor)trame.data);
                        break;
                    default:
                        Logger.GlobalLogger.error("Header Inconnu !");
                        break;
                }

            }
            else
            {
                Logger.GlobalLogger.error("traiterNouvelleTrameRecu :  trame.data.Length == 0");
                return;
            }
        }



        #region #### Incoming Messages Traitement ####
        private void TraiterMessageACK(EMBtoPCMessageGlobalAck message)
        {
        }
        private void TraiterMessageASK_CONN(EMBtoPCMessageAskConn message)
        {
            if (_stateBot == StateBot.STATE_NONE ) //On viens juste de connecter
            {
            }
        }
       
        private void TraiterMessageRESP_PING(EMBtoPCMessageRespPing message)
        {
        }
        private void TraiterMessageRESP_SENSOR(EMBtoPCMessageRespSensor message)
        {
        }
        #endregion

    }
}
