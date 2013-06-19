using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbee.Communication
{
    public class ArduinoManagerComm
    {
        // Liste contenant les arduinos
        private List<ArduinoBotComm> _listArduino;

        public List<ArduinoBotComm> ListeArduino
        {
            get { return _listArduino; }
            set { _listArduino = value; }
        }

        public ArduinoManagerComm()
        {
            _listArduino = new List<ArduinoBotComm>();
        }

        public ArduinoBotComm getArduinoBotById(byte id)
        {
            return _listArduino.Find(ArduinoBotComm.ById(id));
        }

        public void addArduinoBot(ArduinoBotComm arduino)
        {
            _listArduino.Add(arduino);
        }

        public void disconnectArduinoBot(byte id)
        {
            _listArduino.Find(ArduinoBotComm.ById(id)).Disconnect();
        }

    }
}
