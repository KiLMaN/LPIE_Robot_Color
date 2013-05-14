using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbee.Communication;

namespace xbee.Communication.Events
{
    class NewArduinoTimeoutEventArgs : EventArgs
    {
        private byte _id;

        public NewArduinoTimeoutEventArgs(byte id)
		{
            _id = id;
		}

        public byte Id
		{
			get {
                return _id;
			}
		}
    }
}
