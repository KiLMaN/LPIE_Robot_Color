using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbee.Communication;
using utils.Events;

namespace IA.Algo
{
    /* Objet contenant un robot Arduino */
    public class ArduinoBotIA
    {
        // Identifiant du robot
        private byte _id;
        public byte ID
        {
            get { return _id; }
        }
        // Position actuelle depuis l'image
        private PositionElement _Position;
        public bool PositionValide = false;
        //Angle orientation par rapport au nord de l'image (en degré)
        public double Angle;
        // Tracé actuel
        private Track _Trace = null;

        // Zone attribuée (depose)
        private Zone _DeposeZone = null;
        // Objectif attribué (cube)
        private Objectif _Objectif = null;
        // Le robot as-til un cube de saisi ?
        private bool _Saisie = false;


        public ArduinoBotIA(byte id)
        {
            this._id = id;
            PositionValide = false;
            //Communication = new ArduinoBotComm(id);
        }

        public PositionElement Position
        {
            get { return _Position; }
            set { _Position = value; PositionValide = true; }
        }
        public Track Trace
        {
            get { return _Trace; }
        }
        public Zone DeposeZone
        {
            get { return _DeposeZone; }
        }
        public bool Saisie
        {
            get { return _Saisie; }
            set { _Saisie = value; }
        }
        public Objectif Cube 
        { 
            get { return this._Objectif;} 
        }

        // Choisi si le deplacement en direction d'une zone ou d'un cube
        public void SetObjectif(Objectif ob)
        {
            _Saisie = false;
            _Objectif = ob;
            _DeposeZone = null;
            _Trace = null;
        }
        public void SetZoneDepose(Zone Depose)
        {
            _Saisie = true;
            _Objectif = null;
            _DeposeZone = Depose;
            _Trace = null;
        }

        // Enregistre le tracé une fois que l'on a determiner celui ci 
        public void SetTrace(Track tr)
        {
            _Trace = tr;
        }

        /* Permet de retrouver le robot dans les listes */
        public static Predicate<ArduinoBotIA> ById(byte id)
        {
            return delegate(ArduinoBotIA o)
            {
                return o._id == id;
            };
        }
    }
}
