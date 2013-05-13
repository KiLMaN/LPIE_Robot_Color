﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils;

namespace xbee.Communication
{
    enum StateArduinoBot : byte
    {
        STATE_ARDUINO_NONE          = 0x00, // Le robot ne fait rien
        STATE_ARDUINO_MOVE          = 0x01, // Le robot bouge
        STATE_ARDUINO_CLAW          = 0x02 // Le robot utilise ça pince

        /*STATE_NONE                  = 0x00,

        STATE_WATTING               = 0x01,

        STATE_MOVING_TO_TARGET      = 0x02,
        STATE_MOVING_TO_BASE        = 0x03,

        STATE_LIFTING_TARGET        = 0x04,
        STATE_LAYING_TARGET         = 0x05,

        STATE_WAIT_ACK              = 0x10,

        STATE_UNCONNECTED           = 0xE0*/
        
    }
    // Etat du protocole de communication du coté PC (Si envoi d'un
    enum StateArduinoComm : byte
    {
        STATE_COMM_NONE             = 0x00, // Pas en attente
        STATE_COMM_WAIT_ACK         = 0x01, // En attente d'un ack
        STATE_COMM_WAIT_PING        = 0x02, // En attente d'un ping
        STATE_COMM_WAIT_SENSOR      = 0x03 // En attente d'une valeur de capteur
    }


    class ArduinoBot
    {
        
    
    
        /* Permet de retrouver le robot dans les listes */
        public static Predicate<ArduinoBot> ById(byte id)
        {
            return delegate(ArduinoBot o)
            {
                return o._id == id;
            };
        }

        private byte _id;
        private bool _connected;
        private DateTime _dateLastMessageReceived;
       // private DateTime _dateLastMessageSend;

        public bool Connected
        {
            set { _connected = value; }
            get { return _connected; }
        }
        #region #### Etats du protocole ####
        private StateArduinoBot _stateArduino;
        private StateArduinoComm _stateCommunication;

        public StateArduinoBot stateBot
        {
            get { return _stateArduino; }
            set { _stateArduino = value; }
        }
        public StateArduinoComm stateComm
        {
            get { return _stateCommunication; }
            set { _stateCommunication = value; }
        }
        #endregion

        public byte id
        {
            get{return _id;}
            set{this._id = value;}
        }
        public DateTime DateLastMessageReceived
        {
            get{return _dateLastMessageReceived;}
            set{this._dateLastMessageReceived = value;}
        }
        /*public DateTime DateLastMessageSend
        {
            get { return _dateLastMessageSend; }
            set { this._dateLastMessageSend = value; }
        }*/


        public ArduinoBot(byte id)
        {
            Logger.GlobalLogger.info("Create new ArduinoBots id:" + id);
            _id = id;

            Disconnect(); // Initialisation 
        }
        public void Disconnect()
        {
            _connected = false;
            _stateArduino = StateArduinoBot.STATE_ARDUINO_NONE;
            _stateCommunication = StateArduinoComm.STATE_COMM_NONE;
        }
    }
}