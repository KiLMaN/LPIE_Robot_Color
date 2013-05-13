using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbee.Communication;

namespace xbee.Communication.Events
{
    class NewTrameReceivedEventArgs : EventArgs
    {
        private byte[] _trameBytes;

        public NewTrameReceivedEventArgs(byte[] trameBytes)
		{
            _trameBytes = trameBytes;
		}

        public byte[] trameBytes
		{
			get {
                return _trameBytes;
			}
		}
    }
}
