using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;

namespace IA
{
    class IntelArt
    {
        #region #### Evenements ####
        // Evenement pour le dessins sur l'image 
        public event DrawPolylineEventHandler DrawPolylineEvent;

        public void OnPositionUpdateRobots(object sender,UpdatePositionRobotEventArgs args)
        {
        }
        public void OnPositionUpdateCubes(object sender, UpdatePositionCubesEventArgs args)
        {
        }
        public void OnPositionUpdateZones(object sender, UpdatePositionZonesEventArgs args)
        {
        }
        public void OnPositionUpdateZoneTravail(object sender, UpdatePositionZoneTravailEventArgs args)
        {
        }

        #endregion
    }
}
