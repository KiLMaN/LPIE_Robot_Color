using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;

namespace IA.Algo
{
    public class Objectif
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

        // Identifiant de la zone de dépose ascociée
        private int _idZone;
        public int idZone
        {
            get { return this._idZone; }
        }

        // Le cube a déja eté traité
        public bool Done = false;
        // Robot en charge du cube
        public ArduinoBotIA Robot = null;

        public Objectif(int id, PositionElement pos , int idZone)
        {
            _id = id;
            _idZone = idZone;
            setPosition(pos);
        }

        public void setPosition(PositionElement pos)
        {
            _position = pos;
        }

        /* Permet de retrouver le robot dans les listes */
        public static Predicate<Objectif> ById(int id)
        {
            return delegate(Objectif o)
            {
                return o._id == id;
            };
        }
    }
}
