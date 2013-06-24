using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;

namespace IA.Algo
{
    public class Zone
    {
        // Identifiant de la zone (sert aussi pour l'afectaation des cubes pour les zones)
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

        public Zone(int id, PositionZone pos)
        {
            this._id = id;
            setPosition(pos);
        }

        public void setPosition(PositionZone pos)
        {
            _position = pos;
        }

        /* Permet de retrouver le robot dans les listes */
        public static Predicate<Zone> ById(int id)
        {
            return delegate(Zone o)
            {
                return o._id == id;
            };
        }
    }
}
