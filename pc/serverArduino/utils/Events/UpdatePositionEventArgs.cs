using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils.Events
{
    #region #### Positions Robots ####
    public class UpdatePositionRobotEventArgs : EventArgs
    {
        private List<object> _listeRobot;

        public UpdatePositionRobotEventArgs(List<object> Robots)
        {
            _listeRobot = Robots;
        }

        public List<object> Robots
        {
            get { return _listeRobot; }
        }
    }
    #endregion

    #region #### Positions Cubes ####
    public class UpdatePositionCubesEventArgs : EventArgs
    {
        private List<object> _listeCube;

        public UpdatePositionCubesEventArgs(List<object> Cubes)
        {
            _listeCube = Cubes;
        }

        public List<object> Cubes
        {
            get { return _listeCube; }
        }
    }
    #endregion

    #region #### Positions Zones ####
    public class UpdatePositionZonesEventArgs : EventArgs
    {
        private List<object> _listeZone;

        public UpdatePositionZonesEventArgs(List<object> Zones)
        {
            _listeZone = Zones;
        }

        public List<object> Zones
        {
            get { return _listeZone; }
        }
    }
    #endregion

}
