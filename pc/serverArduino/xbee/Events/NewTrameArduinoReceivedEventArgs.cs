using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbee.Communication;

namespace xbee.Communication.Events
{
    class NewTrameArduinoReceveidEventArgs : EventArgs
    {
        private TrameProtocole _trame;

        public NewTrameArduinoReceveidEventArgs(TrameProtocole trame)
		{
            _trame = trame;
		}

        public TrameProtocole Trame
		{
			get {
                return _trame;
			}
		}
    }
}
