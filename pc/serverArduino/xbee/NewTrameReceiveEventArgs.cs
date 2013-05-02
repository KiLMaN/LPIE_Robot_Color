using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Communication.Arduino.protocol;

namespace Communication.Events
{
    class NewTrameReceiveEventArgs : EventArgs
    {
        private TrameProtocole _trame;

        public NewTrameReceiveEventArgs(TrameProtocole trame)
		{
            _trame = trame;
		}

        public TrameProtocole trame
		{
			get {
                return _trame;
			}
		}
    }
}
