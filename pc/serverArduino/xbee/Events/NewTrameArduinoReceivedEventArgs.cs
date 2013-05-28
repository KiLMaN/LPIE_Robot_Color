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
        private ArduinoBot _source;

        public NewTrameArduinoReceveidEventArgs(MessageProtocol message, ArduinoBot Source)
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
        public ArduinoBot Source
        {
            get { return _source; }
        }
    }
}
