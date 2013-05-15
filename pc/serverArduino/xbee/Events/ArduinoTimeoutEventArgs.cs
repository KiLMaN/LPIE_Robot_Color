using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbee.Communication;

namespace xbee.Communication.Events
{
    class ArduinoTimeoutEventArgs : EventArgs
    {
        private ArduinoBot _bot;

        public ArduinoTimeoutEventArgs(ArduinoBot bot)
		{
            _bot = bot;
		}

        public ArduinoBot Bot
		{
			get {
                return _bot;
			}
		}
    }
}
