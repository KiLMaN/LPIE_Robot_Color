using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbee.Communication
{
    public class ArduinoManager
    {
        // Liste contenant les arduinos
        private List<ArduinoBot> _listArduino;

        public List<ArduinoBot> ListeArduino
        {
            get { return _listArduino; }
            set { _listArduino = value; }
        }

        public ArduinoManager()
        {
            _listArduino = new List<ArduinoBot>();
        }

        public ArduinoBot getArduinoBotById(byte id)
        {
            return _listArduino.Find(ArduinoBot.ById(id));
        }

        public void addArduinoBot(ArduinoBot arduino)
        {
            _listArduino.Add(arduino);
        }

        public void disconnectArduinoBot(byte id)
        {
            _listArduino.Find(ArduinoBot.ById(id)).Disconnect();
        }

    }
}
