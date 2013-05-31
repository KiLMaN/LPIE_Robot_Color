using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;

namespace IA.Algo
{
    class Objectif
    {
        // Identifiant de l'objectif
        private int _id;
        public int id
        {
            get { return _id; }
        }
        // Position de l'objectif
        private PositionZone _position;
        public PositionZone position
        {
            get { return _position; }
        }

        #region #### Constructeurs ####
        public Objectif(int id)
        {
            _id = id;
        }
        public Objectif(int id,PositionZone pos)
            :this(id)
        {
            setPosition(pos);
        }

        public void setPosition(PositionZone pos)
        {
            _position = pos;
        }
        #endregion

    }
}
