using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbee.Communication;

namespace IA.Algo
{
    public class TrackMaker
    {
        // Emplacement des cubes sur le terrain
         private List<Objectif> _Cubes;

        // Emplacement des zones
         private List<Zone> _ZonesDepose;

        // Liste des arduino
         private ArduinoManagerComm _ArduinoManager;

         public TrackMaker(ArduinoManagerComm AM)
         {
             this._ArduinoManager = AM;
         }
        
    }
}
