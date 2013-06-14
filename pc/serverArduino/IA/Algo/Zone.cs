using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;

namespace IA.Algo
{
    class Zone
    {
        // Identifiant de la zone
        private int _id;
        public int id
        {
            get { return _id; }
        }
        // Position de la zone
        private PositionZone _position;
        public PositionZone position
        {
            get { return _position; }
        }

        #region #### Constructeurs ####
        public Zone(int id)
        {
            _id = id;
        }
        public Zone(int id, PositionZone pos)
            :this(id)
        {
            setPosition(pos);
        }
        #endregion
        public void setPosition(PositionZone pos)
        {
            _position = pos;
        }
        
    }
}
