using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbee.Communication;

namespace xbee.Communication.Events
{
    public class NewTrameArduinoReceveidEventArgs : EventArgs
    {
        private MessageProtocol _message;
        private ArduinoBotComm _source;

        public NewTrameArduinoReceveidEventArgs(MessageProtocol message, ArduinoBotComm Source)
		{
            _message = message;
            _source = Source;
		}

        public MessageProtocol Message
		{
			get {
                return _message;
			}
		}
        public ArduinoBotComm Source
        {
            get { return _source; }
        }
    }
}
