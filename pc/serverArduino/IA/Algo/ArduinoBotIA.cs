using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbee.Communication;

namespace IA.Algo
{
    public class ArduinoBotIA
    {
        public ArduinoBotComm Communication;
        private Track _Trace = null;

        private byte _id;

        public ArduinoBotIA(byte id)
        {
            this._id = id;
            Communication = new ArduinoBotComm(id);
        }
    }
}
