using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbee.Communication;

namespace xbee.Communication.Events
{
    public class ArduinoTimeoutEventArgs : EventArgs
    {
        private ArduinoBotComm _bot;

        public ArduinoTimeoutEventArgs(ArduinoBotComm bot)
		{
            _bot = bot;
		}

        public ArduinoBotComm Bot
		{
			get {
                return _bot;
			}
		}
    }
}
