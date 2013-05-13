using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbee.Communication;

namespace xbee.Communication.Events
{
    class NewDataReceveidEventArgs : EventArgs
    {
        private int _count;

        public NewDataReceveidEventArgs(int count)
		{
            _count = count;
		}

        public int DataCount
		{
			get {
                return _count;
			}
		}
    }
}
