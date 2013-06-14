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
        private PositionElement _position;
        public PositionElement position
        {
            get { return _position; }
        }

        #region #### Constructeurs ####
        public Objectif(int id)
        {
            _id = id;
        }
        public Objectif(int id, PositionElement pos)
            :this(id)
        {
            setPosition(pos);
        }

        public void setPosition(PositionElement pos)
        {
            _position = pos;
        }
        #endregion

    }
}
